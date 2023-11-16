namespace Core
{
    public class FileMessageModel
    {
        public Guid SequenceId { get; set; }
        public string Content { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }

        public static FileMessageModel operator +(FileMessageModel messageA, FileMessageModel messageB) => new FileMessageModel()
        {
            SequenceId = messageA.SequenceId,
            Content = messageA.Content + messageB.Content,
            FileName = messageA.FileName,
            Extension = messageA.Extension,
        };
    }
}