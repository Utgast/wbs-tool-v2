using Microsoft.EntityFrameworkCore;
using WbsTool.Api.Data;
using WbsTool.Api.Modules.Competencies.Contracts;
using WbsTool.Api.Modules.Competencies.Models;

namespace WbsTool.Api.Modules.Competencies.Services;

public class CompetencyService : ICompetencyService
{
    private readonly AppDbContext _dbContext;

    public CompetencyService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Liefert alle bearbeitbaren Kompetenzen ohne Bewertungslogik.
    /// </summary>
    public async Task<IEnumerable<CompetencyDto>> GetCompetenciesAsync()
    {
        return await _dbContext.Competencies
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .Select(c => new CompetencyDto
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToListAsync();
    }

    /// <summary>
    /// Legt eine neue Kompetenz als neutralen Stammdateneintrag an.
    /// </summary>
    public async Task<CompetencyDto> CreateCompetencyAsync(CreateCompetencyRequest request)
    {
        var normalizedName = request.Name.Trim();

        // Verhindert doppelte Kompetenznamen, damit dieselbe Faehigkeit spaeter
        // eindeutig zugeordnet und gefiltert werden kann.
        var exists = await _dbContext.Competencies.AnyAsync(c => c.Name == normalizedName);

        if (exists)
        {
            throw new ArgumentException($"Competency '{normalizedName}' already exists.");
        }

        var competency = new Competency
        {
            Id = Guid.NewGuid(),
            Name = normalizedName
        };

        _dbContext.Competencies.Add(competency);
        await _dbContext.SaveChangesAsync();

        return new CompetencyDto
        {
            Id = competency.Id,
            Name = competency.Name
        };
    }

    /// <summary>
    /// Entfernt eine Kompetenz, wenn sie nicht mehr benoetigt wird.
    /// </summary>
    public async Task<bool> DeleteCompetencyAsync(Guid id)
    {
        var competency = await _dbContext.Competencies.FindAsync(id);

        if (competency is null)
        {
            return false;
        }

        _dbContext.Competencies.Remove(competency);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Liefert alle vorhandenen Zuordnungen zwischen Personen und Kompetenzen.
    /// </summary>
    public async Task<IEnumerable<PersonCompetencyDto>> GetPersonCompetenciesAsync()
    {
        return await _dbContext.PersonCompetencies
            .AsNoTracking()
            .Include(pc => pc.Person)
            .Include(pc => pc.Competency)
            .OrderBy(pc => pc.Person.Name)
            .ThenBy(pc => pc.Competency.Name)
            .Select(pc => new PersonCompetencyDto
            {
                Id = pc.Id,
                PersonId = pc.PersonId,
                PersonName = pc.Person.Name,
                CompetencyId = pc.CompetencyId,
                CompetencyName = pc.Competency.Name
            })
            .ToListAsync();
    }

    /// <summary>
    /// Erstellt eine belegbare Zuordnung Person zu Kompetenz.
    /// </summary>
    public async Task<PersonCompetencyDto> CreatePersonCompetencyAsync(CreatePersonCompetencyRequest request)
    {
        var personExists = await _dbContext.Persons.AnyAsync(p => p.Id == request.PersonId);
        var competencyExists = await _dbContext.Competencies.AnyAsync(c => c.Id == request.CompetencyId);

        if (!personExists)
        {
            throw new ArgumentException($"Person with id '{request.PersonId}' was not found.");
        }

        if (!competencyExists)
        {
            throw new ArgumentException($"Competency with id '{request.CompetencyId}' was not found.");
        }

        // Sichert, dass eine Kompetenz pro Person nur einmal zugeordnet wird,
        // damit spaetere Auswertungen keine kuenstlichen Mehrfachtreffer enthalten.
        var alreadyExists = await _dbContext.PersonCompetencies.AnyAsync(pc =>
            pc.PersonId == request.PersonId && pc.CompetencyId == request.CompetencyId);

        if (alreadyExists)
        {
            throw new ArgumentException("The person-competency assignment already exists.");
        }

        var assignment = new PersonCompetency
        {
            Id = Guid.NewGuid(),
            PersonId = request.PersonId,
            CompetencyId = request.CompetencyId
        };

        _dbContext.PersonCompetencies.Add(assignment);
        await _dbContext.SaveChangesAsync();

        return await _dbContext.PersonCompetencies
            .AsNoTracking()
            .Include(pc => pc.Person)
            .Include(pc => pc.Competency)
            .Where(pc => pc.Id == assignment.Id)
            .Select(pc => new PersonCompetencyDto
            {
                Id = pc.Id,
                PersonId = pc.PersonId,
                PersonName = pc.Person.Name,
                CompetencyId = pc.CompetencyId,
                CompetencyName = pc.Competency.Name
            })
            .SingleAsync();
    }

    /// <summary>
    /// Loescht eine bestehende Zuordnung zwischen Person und Kompetenz.
    /// </summary>
    public async Task<bool> DeletePersonCompetencyAsync(Guid id)
    {
        var assignment = await _dbContext.PersonCompetencies.FindAsync(id);

        if (assignment is null)
        {
            return false;
        }

        _dbContext.PersonCompetencies.Remove(assignment);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}