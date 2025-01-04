using System;
using Data.Repositories.Base;
using E = Domain.Entities;

namespace Data.Repositories.News;

public interface INewsRepo : IBaseRepository<E.News>
{
    Task<List<E.News>> GetAllNewsWithUser();
    Task<E.News?> GetNewsWithUserAndCommentsById(int id);
}
