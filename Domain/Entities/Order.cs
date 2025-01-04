using System;
using Domain.Entities.Base;
using Domain.Entities.Identity;
using Domain.Enums;

namespace Domain.Entities;

public class Order : BaseEntity
{
    public StatusEnum Status { get; set; }
    public Guid TrackingNumber { get; set; }
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public List<OrderItem> Items { get; set; }
}
