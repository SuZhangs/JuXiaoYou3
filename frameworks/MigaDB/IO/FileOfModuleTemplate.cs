using System.Runtime.InteropServices;
using Acorisoft.FutureGL.MigaDB.Data.DataParts;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaUtils;
using Koyashiro.PngChunkUtil;
using Newtonsoft.Json;

namespace Acorisoft.FutureGL.MigaDB.IO
{
    public static class FileOfModuleTemplate
    {
        public static Task<Result<IOReason>> WriteAsync(PartOfModule module, byte[] pngImageData, string fileName)
        {
            return Task.Run(async () =>
            {
                if (pngImageData is null || pngImageData.Length <= 0)
                {
                    return Result<IOReason>.Failed(IOReason.DataNOE);
                }

                if (module is null)
                {
                    return Result<IOReason>.Failed(IOReason.PayloadNOE);
                }

                if (string.IsNullOrEmpty(fileName))
                {
                    return Result<IOReason>.Failed(IOReason.OutputFileNameNOE);
                }

                try
                {
                    module.Clear();
                    var payload = JSON.Serialize(module);
                    await PNG.Write(fileName, payload, pngImageData);
                    return Result<IOReason>.Successful;
                }
                catch (JsonSerializationException)
                {
                    return Result<IOReason>.Failed(IOReason.PayloadError);
                }
                catch (UnauthorizedAccessException)
                {
                    return Result<IOReason>.Failed(IOReason.WriteUnauthorized);
                }
                catch
                {
                    return Result<IOReason>.Failed(IOReason.Unknown);
                }

            });
        }

        public static Task<Result<IOReason, ImagePayload<PartOfModule>>> ReadAsync(string fileName)
        {
            return Task.Run(() =>
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    return Result<IOReason, ImagePayload<PartOfModule>>.Failed(IOReason.InputFileNameNOE);
                }

                try
                {
                    var buffer =  File.ReadAllBytes(fileName);
                    var imgData = PNG.ReadImage(buffer);
                    var payload = PNG.ReadData(buffer);
                    
                    return Result<IOReason, ImagePayload<PartOfModule>>.Success(new ImagePayload<PartOfModule>
                    {
                        ImageData = imgData,
                        Value = JSON.FromJson<PartOfModule>(payload)
                    });
                }
                catch (JsonSerializationException)
                {
                    return Result<IOReason, ImagePayload<PartOfModule>>.Failed(IOReason.PayloadError);
                }
                catch (UnauthorizedAccessException)
                {
                    return Result<IOReason, ImagePayload<PartOfModule>>.Failed(IOReason.WriteUnauthorized);
                }
                catch
                {
                    return Result<IOReason, ImagePayload<PartOfModule>>.Failed(IOReason.Unknown);
                }
            });
        }
    }
}