using StowageApp.Server.Data;
using StowageApp.Shared.Entities;

namespace StowageApp.Server.Services
{
    public class SeedingServices
    {
        private readonly FileStowageContext _context;

        public SeedingServices(FileStowageContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (_context.FileStowages.Any())
            {
                return;
            }

            FileStowage fileStowage1 = new FileStowage { ID = Guid.NewGuid(), FileName = "File1", FileSize = 200, RemotePath = "Server 1", UploadDate = DateTime.Now };
            FileStowage fileStowage2 = new FileStowage { ID = Guid.NewGuid(), FileName = "File2", FileSize = 200, RemotePath = "Server 2", UploadDate = DateTime.Now };
            FileStowage fileStowage3 = new FileStowage { ID = Guid.NewGuid(), FileName = "File3", FileSize = 200, RemotePath = "Server 3", UploadDate = DateTime.Now };
            FileStowage fileStowage4 = new FileStowage { ID = Guid.NewGuid(), FileName = "File4", FileSize = 200, RemotePath = "Server 4", UploadDate = DateTime.Now };
            FileStowage fileStowage5 = new FileStowage { ID = Guid.NewGuid(), FileName = "File5", FileSize = 200, RemotePath = "Server 5", UploadDate = DateTime.Now };

            _context.FileStowages.AddRange(fileStowage1, fileStowage2, fileStowage3, fileStowage4, fileStowage5);

            _context.SaveChanges();
        }

    }
}
