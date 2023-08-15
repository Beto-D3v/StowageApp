using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using Stowage;
using StowageApp.Server.Data;
using StowageApp.Server.Services;
using StowageApp.Shared.Entities;
using System.Diagnostics.CodeAnalysis;

namespace StowageApp.Server.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class FileStowageController : ControllerBase
    {
        private readonly FileStowageContext _context;
        private readonly MyStorageBase _storageService;
        private string rootDir = @"C:\Users\windows\Desktop\C#\Stowage\StowageApp\Client\wwwroot\Folder";
        public FileStowageController(FileStowageContext context, MyStorageBase storageService)
        {
            _context = context;
            _storageService = storageService;
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

        //[HttpPost]
        //public async Task<ActionResult<FileStowage>> Post(FileStowage fileStowage)
        //{
        //    _context.Add(fileStowage);
        //    await _context.SaveChangesAsync();
        //    return new CreatedAtRouteResult("GetFileStowage", new { id = fileStowage.ID }, fileStowage);
        //}

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
            //var remotePath = Path.Combine(rootDir, file.FileName);
            //if (!string.IsNullOrEmpty(remotePath))
            //{
            //    await _storageService.DeleteAsync(remotePath);
            //}
            await _context.SaveChangesAsync();
            return Ok(file);
        }

        [HttpPost]
        public async Task<ActionResult<FileStowage>> UploadFile([FromForm] IFormFile file)
        {
            try
            {
              
                var remotePath = Path.Combine(rootDir, file.FileName);

                await _storageService.UploadAsync(file.OpenReadStream(), remotePath);


                var fileStowage = new FileStowage
                {
                    FileName = file.FileName,
                    FileSize = (int)file.Length,
                    RemotePath = rootDir,
                    UploadDate = DateTime.Now
                };

                _context.FileStowages.Add(fileStowage);
                await _context.SaveChangesAsync();

                // Retorna a entidade criada (ou pode retornar qualquer outro resultado desejado)
                return Ok(fileStowage);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao fazer o upload do arquivo: {ex.Message}");
            }
        
        }

        //[HttpPost]
        //public async Task<ActionResult<FileStowage>> DownloadFile(string file)
        //{
        //    try
        //    {

        //        var remotePath = Path.Combine(rootDir, file);

        //        await _storageService.DownloadAsync(remotePath);

        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"Erro ao fazer o download do arquivo: {ex.Message}");
        //    }

        //}

        [HttpGet("Download/{fileName}")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            try
            {
                //Abro o fluxo de bytes
                var fileStream = await _storageService.OpenAsync(fileName);
                
               //Aqui utilizo o método File para passar o fluxo de bytes e o nome do arquivo que será baixado. Ao invés de usar um endereço, utilizou-se o fluxo de bytes
                return File(fileStream, "application/octet-stream", fileName);

                
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an appropriate response
                return StatusCode(500, $"Error downloading file: {ex.Message}");
            }
        }

    }


}




