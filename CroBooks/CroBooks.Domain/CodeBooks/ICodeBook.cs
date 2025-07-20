namespace CroBooks.Domain.CodeBooks;

public interface ICodeBook
{
    int Id { get; set; }
    string Name { get; set; }
    bool IsSystemGenerated { get; set; }
}