using System.ComponentModel.DataAnnotations;
using CroBooks.Domain.Base;

namespace CroBooks.Domain.CodeBooks;

public sealed class AddressType : AuditEntity<int>, ICodeBook
{
    [StringLength(50)] 
    public string Name { get; set; } = string.Empty;
    public bool IsSystemGenerated { get; set; } = false;
}