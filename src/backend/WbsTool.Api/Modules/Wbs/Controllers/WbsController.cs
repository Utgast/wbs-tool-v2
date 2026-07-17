using Microsoft.AspNetCore.Mvc;
using WbsTool.Api.Modules.Wbs.Contracts;
using WbsTool.Api.Modules.Wbs.Services;

namespace WbsTool.Api.Modules.Wbs.Controllers;

[ApiController]
[Route("api/projects/{projectId:guid}/wbs")]
public class WbsController : ControllerBase
{
    private readonly IWbsService _wbsService;

    public WbsController(IWbsService wbsService)
    {
        _wbsService = wbsService;
    }

    /// <summary>
    /// Liefert alle WBS-Knoten eines Projekts als flache Liste.
    /// </summary>
    // Fachlicher Zweck: Die flache Liste wird genutzt, wenn Arbeitspakete
    // in Tabellen oder einfachen Listenansichten ohne Hierarchie angezeigt werden.
    [HttpGet]
    public ActionResult<IEnumerable<WbsNodeDto>> GetByProjectId(Guid projectId)
    {
        var nodes = _wbsService.GetByProjectId(projectId);
        return Ok(nodes);
    }

    /// <summary>
    /// Liefert die WBS eines Projekts als hierarchischen Baum.
    /// </summary>
    // Fachlicher Zweck: Die Baumstruktur wird im Frontend verwendet,
    // um Arbeitspakete in ihrer Parent/Child-Struktur darzustellen.
    [HttpGet("tree")]
    public ActionResult<IEnumerable<WbsTreeNodeDto>> GetTreeByProjectId(Guid projectId)
    {
        var nodes = _wbsService.GetTreeByProjectId(projectId);
        return Ok(nodes);
    }

    /// <summary>
    /// Legt einen neuen WBS-Knoten fuer ein Projekt an.
    /// </summary>
    // Fachlicher Zweck: Der Endpunkt ermoeglicht das schrittweise Aufbauen
    // der Arbeitspaketstruktur direkt im Projektkontext.
    [HttpPost]
    public ActionResult<WbsNodeDto> Create(Guid projectId, CreateWbsNodeRequest request)
    {
        var createdNode = _wbsService.Create(projectId, request);
        return Ok(createdNode);
    }
}