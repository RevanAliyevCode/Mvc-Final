using System;

namespace Business.Services.Admin.SendNews;

public interface ISendNewsService
{
    Task<List<(string email, bool isSubscirbe)>> GetUsersVMAsync();
    Task<bool> SendMailAsync(string subject, string message);
}
