using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MRA.Jobs.Application.Common.Interfaces;
public interface IFileService
{
    Task<string> UploadAsync(IFormFile file);
    byte[] Download(string key);
    bool FileExists(string key);
}
