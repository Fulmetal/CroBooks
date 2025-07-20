using System.ComponentModel.DataAnnotations;

namespace CroBooks.Shared.Dto;

public class CodeBookDto
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;

    public bool IsSystemGenerated { get; set; } = false;
}