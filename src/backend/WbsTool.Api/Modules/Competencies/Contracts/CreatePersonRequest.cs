using System.ComponentModel.DataAnnotations;

namespace WbsTool.Api.Modules.Competencies.Contracts;

/// <summary>
/// Beschreibt die minimale Eingabe zum Anlegen einer bearbeitbaren Person.
/// </summary>
public class CreatePersonRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
}