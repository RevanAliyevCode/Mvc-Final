using System;
using Data.Repositories.Base;
using E = Domain.Entities;

namespace Data.Repositories.Comment;

public interface ICommentRepo : IBaseRepository<E.Comment>
{
}
