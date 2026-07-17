namespace WbsTool.Api.Modules.Competencies.Contracts;

/// <summary>
/// Beschreibt eine Person als bearbeitbare Stammdatenbasis fuer Kompetenzen und Auslastung.
/// </summary>
public class PersonDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}