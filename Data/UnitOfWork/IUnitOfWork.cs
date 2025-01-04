using System;

namespace Data.UnitOfWork;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}
