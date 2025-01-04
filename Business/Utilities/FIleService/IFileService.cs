using System;
using Microsoft.AspNetCore.Http;

namespace Business.Utilities.FIleService;

public interface IFileService
{
    string Upload(IFormFile file, string folderName);
    void Delete(string fileName, string folderName);
    bool IsImage(string contentType);

    bool IsAvailableSize(long size, long max = 100);
}
