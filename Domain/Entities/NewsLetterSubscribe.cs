using Domain.Entities.Base;

namespace Domain.Entities;

public class NewsLetterSubscribe : BaseEntity
{
    public string Email { get; set; }
    public string? Token { get; set; }
    public bool IsSubscribed { get; set; }
}
