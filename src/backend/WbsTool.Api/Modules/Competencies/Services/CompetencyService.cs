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

    public IEnumerable<CompetencyDto> GetAll()
    {
        return _dbContext.Competencies
            .AsNoTracking()
            .Where(c => c.IsActive)
            .OrderBy(c => c.Code)
            .Select(c => new CompetencyDto
            {
                Id = c.Id,
                Code = c.Code,
                Name = c.Name,
                Description = c.Description,
                IsActive = c.IsActive
            })
            .ToList();
    }

    public IEnumerable<PersonCompetencyDto> GetPersonCompetencies(Guid personId)
    {
        return _dbContext.PersonCompetencies
            .AsNoTracking()
            .Include(pc => pc.Competency)
            .Where(pc => pc.PersonId == personId && pc.IsActive)
            .OrderBy(pc => pc.Competency.Code)
            .Select(pc => new PersonCompetencyDto
            {
                Id = pc.Id,
                PersonId = pc.PersonId,
                CompetencyId = pc.CompetencyId,
                CompetencyCode = pc.Competency.Code,
                CompetencyName = pc.Competency.Name,
                ProficiencyLevel = pc.ProficiencyLevel,
                Comment = pc.Comment,
                IsActive = pc.IsActive
            })
            .ToList();
    }

    public PersonCompetencyDto AddPersonCompetency(Guid personId, AddPersonCompetencyRequest request)
    {
        var entry = new PersonCompetency
        {
            Id = Guid.NewGuid(),
            PersonId = personId,
            CompetencyId = request.CompetencyId,
            ProficiencyLevel = request.ProficiencyLevel,
            Comment = request.Comment,
            IsActive = true,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };

        _dbContext.PersonCompetencies.Add(entry);
        _dbContext.SaveChanges();

        var competency = _dbContext.Competencies
            .AsNoTracking()
            .First(c => c.Id == request.CompetencyId);

        return new PersonCompetencyDto
        {
            Id = entry.Id,
            PersonId = entry.PersonId,
            CompetencyId = entry.CompetencyId,
            CompetencyCode = competency.Code,
            CompetencyName = competency.Name,
            ProficiencyLevel = entry.ProficiencyLevel,
            Comment = entry.Comment,
            IsActive = entry.IsActive
        };
    }

    public IEnumerable<WbsRequiredCompetencyDto> GetWbsRequiredCompetencies(Guid wbsNodeId)
    {
        return _dbContext.WbsRequiredCompetencies
            .AsNoTracking()
            .Include(w => w.Competency)
            .Where(w => w.WbsNodeId == wbsNodeId)
            .OrderBy(w => w.Competency.Code)
            .Select(w => new WbsRequiredCompetencyDto
            {
                Id = w.Id,
                ProjectId = w.ProjectId,
                WbsNodeId = w.WbsNodeId,
                CompetencyId = w.CompetencyId,
                CompetencyCode = w.Competency.Code,
                CompetencyName = w.Competency.Name,
                RequiredLevel = w.RequiredLevel,
                Comment = w.Comment
            })
            .ToList();
    }

    public WbsRequiredCompetencyDto AddWbsRequiredCompetency(Guid wbsNodeId, AddWbsRequiredCompetencyRequest request)
    {
        var entry = new WbsRequiredCompetency
        {
            Id = Guid.NewGuid(),
            ProjectId = request.ProjectId,
            WbsNodeId = wbsNodeId,
            CompetencyId = request.CompetencyId,
            RequiredLevel = request.RequiredLevel,
            Comment = request.Comment,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };

        _dbContext.WbsRequiredCompetencies.Add(entry);
        _dbContext.SaveChanges();

        var competency = _dbContext.Competencies
            .AsNoTracking()
            .First(c => c.Id == request.CompetencyId);

        return new WbsRequiredCompetencyDto
        {
            Id = entry.Id,
            ProjectId = entry.ProjectId,
            WbsNodeId = entry.WbsNodeId,
            CompetencyId = entry.CompetencyId,
            CompetencyCode = competency.Code,
            CompetencyName = competency.Name,
            RequiredLevel = entry.RequiredLevel,
            Comment = entry.Comment
        };
    }
}
