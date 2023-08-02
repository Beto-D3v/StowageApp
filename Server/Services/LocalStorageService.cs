//namespace StowageApp.Server.Services
//{
//    public class LocalStorageService : IStorageService
//    {
//        //Caminho que vai salvar o arquivo (path)
//        public async Task<string> SaveFile(IFormFile file)
//        {
//            var tempPath = Path.GetTempPath();
//            var fileToSavePath = Path.Combine(tempPath, file.Name);
//            using(var fileStream = System.IO.File.Create(fileToSavePath))
//            {
//                await file.CopyToAsync(fileStream);
//            }

//            return fileToSavePath;

//        }

//    }
//}
