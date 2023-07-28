using Microsoft.EntityFrameworkCore;
using StowageApp.Shared.Entities;

namespace StowageApp.Server.Data
{
    public class FileStowageContext : DbContext
    {
        public FileStowageContext(DbContextOptions<FileStowageContext> options) : base(options) 
        { 

        }

        public DbSet<FileStowage> FileStowages { get; set; }

    }
}
