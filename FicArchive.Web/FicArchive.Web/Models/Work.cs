namespace FicArchive.Web.Models
{
    public class Work
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Summary { get; set; }

        // --- Метаданные в стиле AO3 ---
        public string Rating { get; set; } = "Not Rated";
        public string? Fandoms { get; set; }
        public string? Category { get; set; }
        public string? Relationships { get; set; }
        public string? Characters { get; set; }
        public string? AdditionalTags { get; set; }

        public string AuthorName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<Chapter> Chapters { get; set; } = new();
    }
}