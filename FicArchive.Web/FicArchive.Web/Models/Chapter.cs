namespace FicArchive.Web.Models
{
    public class Chapter
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int ChapterNumber { get; set; }

        public int WorkId { get; set; }
        public Work? Work { get; set; }
    }
}