using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StowageApp.Server.Data;
using StowageApp.Shared.Entities;

namespace StowageApp.Server.Controllers;

    [Route("api/[controller]")]
    [ApiController]
    public class FileStowageController : ControllerBase
    {
        private readonly FileStowageContext _context;
        public FileStowageController(FileStowageContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<FileStowage>>> Get()
        {
            return await _context.FileStowages.ToListAsync();
        }
        
        [HttpGet("{id}" , Name= "GetFileStowage")]

        public async Task<ActionResult<FileStowage>> GetFileStowage(Guid id)
        {
            return await _context.FileStowages.FirstOrDefaultAsync(item => item.ID == id);
        }

        [HttpPost]
        public async Task<ActionResult<FileStowage>> Post(FileStowage fileStowage)
        {
            _context.Add(fileStowage);
            await _context.SaveChangesAsync();
            return new CreatedAtRouteResult("GetFileStowage", new { id = fileStowage.ID }, fileStowage);
        }

        [HttpPut]
        public async Task<ActionResult<FileStowage>> Put(FileStowage fileStowage)
        {
            _context.Entry(fileStowage).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(fileStowage);
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult<FileStowage>> DeleteFileStowage(Guid id)
        {
            var file = await _context.FileStowages.FirstOrDefaultAsync(item => item.ID == id);
            _context.Remove(file);
            await _context.SaveChangesAsync();
            return Ok(file);
        }

}

