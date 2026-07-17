using Microsoft.EntityFrameworkCore;
using WbsTool.Api.Data;
using WbsTool.Api.Modules.Capacity.Contracts;
using WbsTool.Api.Modules.Capacity.Models;
using WbsTool.Api.Modules.Competencies.Contracts;

namespace WbsTool.Api.Modules.Capacity.Services;

public class CapacityService : ICapacityService
{
    private readonly AppDbContext _dbContext;

    public CapacityService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Liefert alle Personen als bearbeitbare Basis fuer Auslastung und Kompetenzzuordnung.
    /// </summary>
    public async Task<IEnumerable<PersonDto>> GetPersonsAsync()
    {
        return await _dbContext.Persons
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .Select(p => new PersonDto
            {
                Id = p.Id,
                Name = p.Name
            })
            .ToListAsync();
    }

    /// <summary>
    /// Legt eine Person als bearbeitbare Stammdatenbasis an.
    /// </summary>
    public async Task<PersonDto> CreatePersonAsync(CreatePersonRequest request)
    {
        var normalizedName = request.Name.Trim();

        // Verhindert doppelte Personeneintraege, damit Kompetenzen und Auslastung
        // spaeter eindeutig einer realen Person zugeordnet werden koennen.
        var exists = await _dbContext.Persons.AnyAsync(p => p.Name == normalizedName);

        if (exists)
        {
            throw new ArgumentException($"Person '{normalizedName}' already exists.");
        }

        var person = new WbsTool.Api.Modules.Competencies.Models.Person
        {
            Id = Guid.NewGuid(),
            Name = normalizedName
        };

        _dbContext.Persons.Add(person);
        await _dbContext.SaveChangesAsync();

        return new PersonDto
        {
            Id = person.Id,
            Name = person.Name
        };
    }

    /// <summary>
    /// Entfernt eine Person aus der bearbeitbaren Stammdatenbasis.
    /// </summary>
    public async Task<bool> DeletePersonAsync(Guid id)
    {
        var person = await _dbContext.Persons.FindAsync(id);

        if (person is null)
        {
            return false;
        }

        _dbContext.Persons.Remove(person);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Liefert alle hinterlegten Wochenkapazitaeten.
    /// </summary>
    public async Task<IEnumerable<PersonCapacityDto>> GetCapacitiesAsync()
    {
        return await _dbContext.PersonCapacities
            .AsNoTracking()
            .Include(c => c.Person)
            .OrderBy(c => c.WeekStartDate)
            .ThenBy(c => c.Person.Name)
            .Select(c => new PersonCapacityDto
            {
                Id = c.Id,
                PersonId = c.PersonId,
                PersonName = c.Person.Name,
                WeekStartDate = c.WeekStartDate,
                AvailableHours = c.AvailableHours
            })
            .ToListAsync();
    }

    /// <summary>
    /// Speichert verfuegbare Stunden pro Person und Woche.
    /// </summary>
    public async Task<PersonCapacityDto> CreateCapacityAsync(CreatePersonCapacityRequest request)
    {
        var person = await _dbContext.Persons.FindAsync(request.PersonId);

        if (person is null)
        {
            throw new ArgumentException($"Person with id '{request.PersonId}' was not found.");
        }

        // Eine eindeutige Wochenkapazitaet pro Person verhindert doppelte Planungsstaende
        // und bildet die Grundlage fuer spaetere Berechnung freier Kapazitaet.
        var existingCapacity = await _dbContext.PersonCapacities.FirstOrDefaultAsync(c =>
            c.PersonId == request.PersonId && c.WeekStartDate == request.WeekStartDate);

        if (existingCapacity is not null)
        {
            existingCapacity.AvailableHours = request.AvailableHours;
            await _dbContext.SaveChangesAsync();

            return new PersonCapacityDto
            {
                Id = existingCapacity.Id,
                PersonId = existingCapacity.PersonId,
                PersonName = person.Name,
                WeekStartDate = existingCapacity.WeekStartDate,
                AvailableHours = existingCapacity.AvailableHours
            };
        }

        var capacity = new PersonCapacity
        {
            Id = Guid.NewGuid(),
            PersonId = request.PersonId,
            WeekStartDate = request.WeekStartDate,
            AvailableHours = request.AvailableHours
        };

        _dbContext.PersonCapacities.Add(capacity);
        await _dbContext.SaveChangesAsync();

        return new PersonCapacityDto
        {
            Id = capacity.Id,
            PersonId = capacity.PersonId,
            PersonName = person.Name,
            WeekStartDate = capacity.WeekStartDate,
            AvailableHours = capacity.AvailableHours
        };
    }

    /// <summary>
    /// Entfernt einen Kapazitaetseintrag.
    /// </summary>
    public async Task<bool> DeleteCapacityAsync(Guid id)
    {
        var capacity = await _dbContext.PersonCapacities.FindAsync(id);

        if (capacity is null)
        {
            return false;
        }

        _dbContext.PersonCapacities.Remove(capacity);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Liefert alle geplanten Stundenallokationen.
    /// </summary>
    public async Task<IEnumerable<PersonAllocationDto>> GetAllocationsAsync()
    {
        return await _dbContext.PersonAllocations
            .AsNoTracking()
            .Include(a => a.Person)
            .OrderBy(a => a.WeekStartDate)
            .ThenBy(a => a.Person.Name)
            .Select(a => new PersonAllocationDto
            {
                Id = a.Id,
                PersonId = a.PersonId,
                PersonName = a.Person.Name,
                ProjectId = a.ProjectId,
                WbsNodeId = a.WbsNodeId,
                WeekStartDate = a.WeekStartDate,
                PlannedHours = a.PlannedHours
            })
            .ToListAsync();
    }

    /// <summary>
    /// Speichert geplante Stunden fuer Person, Projekt und optionales Arbeitspaket.
    /// </summary>
    public async Task<PersonAllocationDto> CreateAllocationAsync(CreatePersonAllocationRequest request)
    {
        var person = await _dbContext.Persons.FindAsync(request.PersonId);

        if (person is null)
        {
            throw new ArgumentException($"Person with id '{request.PersonId}' was not found.");
        }

        var projectExists = await _dbContext.Projects.AnyAsync(p => p.Id == request.ProjectId);

        if (!projectExists)
        {
            throw new ArgumentException($"Project with id '{request.ProjectId}' was not found.");
        }

        if (request.WbsNodeId.HasValue)
        {
            // Stellt sicher, dass optionale Allokationen nur auf existierende Arbeitspakete zeigen,
            // damit spaetere Kapazitaetsauswertungen keinen toten Bezug enthalten.
            var wbsNodeExists = await _dbContext.WbsNodes.AnyAsync(w => w.Id == request.WbsNodeId.Value);

            if (!wbsNodeExists)
            {
                throw new ArgumentException($"WBS node with id '{request.WbsNodeId.Value}' was not found.");
            }
        }

        var allocation = new PersonAllocation
        {
            Id = Guid.NewGuid(),
            PersonId = request.PersonId,
            ProjectId = request.ProjectId,
            WbsNodeId = request.WbsNodeId,
            WeekStartDate = request.WeekStartDate,
            PlannedHours = request.PlannedHours
        };

        _dbContext.PersonAllocations.Add(allocation);
        await _dbContext.SaveChangesAsync();

        return new PersonAllocationDto
        {
            Id = allocation.Id,
            PersonId = allocation.PersonId,
            PersonName = person.Name,
            ProjectId = allocation.ProjectId,
            WbsNodeId = allocation.WbsNodeId,
            WeekStartDate = allocation.WeekStartDate,
            PlannedHours = allocation.PlannedHours
        };
    }

    /// <summary>
    /// Entfernt eine geplante Stundenallokation.
    /// </summary>
    public async Task<bool> DeleteAllocationAsync(Guid id)
    {
        var allocation = await _dbContext.PersonAllocations.FindAsync(id);

        if (allocation is null)
        {
            return false;
        }

        _dbContext.PersonAllocations.Remove(allocation);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}