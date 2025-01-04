using System;
using Domain.Entities.Base;
using Domain.Entities.Identity;

namespace Domain.Entities;

public class Basket : BaseEntity
{
    public string UserId { get; set; }
    public AppUser User { get; set; }
    
    public ICollection<BasketItem> Items { get; set; }
}
