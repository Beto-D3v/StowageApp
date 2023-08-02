using System.ComponentModel.DataAnnotations;


namespace StowageApp.Shared.Entities
{
    public class FileStowage
    {
        [Key]
        public Guid ID { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public string RemotePath { get; set; }
        public DateTime UploadDate { get; set; }


        public FileStowage()
        {

        }

        public FileStowage(Guid id, string fileName, int fileSize, string remotePath, DateTime uploadDate)
        {
            ID = id;
            FileName = fileName;
            FileSize = fileSize;
            RemotePath = remotePath;
            UploadDate = uploadDate;
        }
    }
}
