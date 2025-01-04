using System;
using Data.Repositories.Base;
using E = Domain.Entities;

namespace Data.Repositories.NewsLetterSubscribe;

public interface INewsLetterSubscribeRepo : IBaseRepository<E.NewsLetterSubscribe>
{
    Task<E.NewsLetterSubscribe?> GetNewsLetterByEmailAsync(string email);
    Task<E.NewsLetterSubscribe?> GetNewsLetterByEmailAndTokenAsync(string email, string token);
    Task<List<E.NewsLetterSubscribe>> GetSubscribedUsersAsync();
}
