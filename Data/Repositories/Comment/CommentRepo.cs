using System;
using Data.Contexts;
using Data.Repositories.Base;
using E = Domain.Entities;

namespace Data.Repositories.Comment;

public class CommentRepo : BaseRepo<E.Comment>, ICommentRepo
{
    public CommentRepo(AppDbContext context) : base(context)
    {
    }
}
