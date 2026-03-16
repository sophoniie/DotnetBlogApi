namespace BlogApi.DTOs.Shared;

public class CategorySummaryResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
}
