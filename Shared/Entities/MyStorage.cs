using Stowage;


namespace StowageApp.Shared.Entities;
public class MyInMemoryStorage : MyStorageBase
{
    protected override IFileStorage GetStorage()
    {
        if (fs_internal_do_not_use_directly == null)
        {
            fs_internal_do_not_use_directly = Files.Of.InternalMemory();
        }
        return fs_internal_do_not_use_directly;
    }
}

public class MyLocalDiskStorage : MyStorageBase
{
    private readonly string rootDir;

    public MyLocalDiskStorage(string rootDir)
    {
        this.rootDir = rootDir;
    }

    protected override IFileStorage GetStorage()
    {
        if (fs_internal_do_not_use_directly == null)
        {
            fs_internal_do_not_use_directly = Files.Of.LocalDisk(rootDir);
        }
        return fs_internal_do_not_use_directly;
    }
}

public abstract class MyStorageBase : IDisposable
{
    protected IFileStorage? fs_internal_do_not_use_directly;

    public void Dispose()
    {
        //fechar o 'fs' se ele estiver carregado
        if (fs_internal_do_not_use_directly != null)
        {
            fs_internal_do_not_use_directly.Dispose();
        }
    }


    protected abstract IFileStorage GetStorage();

    public async Task UploadAsync(string localPath, string remotePath)
    {
        var fs = GetStorage();

        //abre o arquivo de origem como um stream
        using (var stream = System.IO.File.OpenRead(localPath))
        {
            await UploadAsync(stream, remotePath);
        }


        //// Converter a string em um array de bytes usando a codificação UTF-8
        //byte[] bytes = Encoding.UTF8.GetBytes(path);
        //// Criar um MemoryStream com os bytes
        //using (MemoryStream stream = new MemoryStream(bytes))
        //{
        //    //converter o path para stream
        //    //...
        //    //chamar o Save 
        //    //..
        //    await SaveAsync(stream);
        //}
    }

    public async Task UploadAsync(Stream localStream, string remotePath)
    {
        //REVER: Nem sempre o stream vai vir do Stowage 
        //salvar o stream no Stowage
        //..
        var fs = GetStorage();
        using (var destStream = await fs.OpenWrite(remotePath))
        {
            localStream.CopyTo(destStream);
            await destStream.FlushAsync();
        }
    }

    public async Task<Stream> OpenAsync(string remotePath)
    {
        var fs = GetStorage();
        var stream = await fs.OpenRead(remotePath);
        return stream;
    }

    public async Task WriteTextAsync(string remotePath, string text, bool checkExists)
    {
        var fs = GetStorage();

        if (checkExists)
        {
            if (await fs.Exists(remotePath))
            {
                throw new Exception("Arquivo já existente");
            }
        }

        using (var stream = await fs.OpenWrite(remotePath))
        {
            using (var sw = new StreamWriter(stream))
            {
                sw.Write(text);
                sw.Flush();
            }
        }
    }

    public async Task DeleteAsync(string remotePath)
    {
        //Deletar arquivo
        var fs = GetStorage();

        if (!await fs.Exists(remotePath))
        {
            throw new Exception("Arquivo não existente");
        }

        await fs.Rm(remotePath);
    }

    public async Task CopyAsync(string remoteSource, string remoteDestination)
    {
        var fs = GetStorage();

        //OpenRead do Stowage
        using (var stream = await OpenAsync(remoteSource))
        {
            //copy (cópia não exclui o arquivo origem!)
            await UploadAsync(stream, remoteDestination);

            //using (var destStream = await fs.OpenWrite(remoteDestination))
            //{
            //    stream.CopyTo(destStream);
            //    await destStream.FlushAsync();
            //    await SaveAsync(destStream, remoteDestination);
            //}
        }

        //obs: ver exemplo do console
    }

    //public async Task UpdateAsync(string remotePath, string text)
    //{
    //    var fs = GetStorage();

    //    using (var stream = await fs.OpenWrite(remotePath))
    //    {
    //        using (var sw = new StreamWriter(stream))
    //        {
    //            sw.Write(text);
    //            await SaveAsync(stream);
    //        }
    //    }

    //}

    public async Task ListAsync(string folder)
    {
        var fs = GetStorage();

        IReadOnlyCollection<IOEntry> fileList = await fs.Ls();
        IEnumerable<string> fileName = fileList.Select(x => x.Name);

        if (fileName.Count() != 0)
        {
            foreach (string file in fileName)
            {
                Console.WriteLine(file);
            }
        }

        //excluir o arquivo a partir do Stowage
    }

}