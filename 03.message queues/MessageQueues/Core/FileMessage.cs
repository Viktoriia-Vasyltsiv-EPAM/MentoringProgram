namespace Core
{
    public class FileMessage
    {
        public Guid SequenceId { get; set; }
        public string Content { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
    }
}