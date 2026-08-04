namespace FicArchive.Web.Models
{
    public class Work
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Новая строка — список глав этой истории
        public List<Chapter> Chapters { get; set; } = new();
    }
}