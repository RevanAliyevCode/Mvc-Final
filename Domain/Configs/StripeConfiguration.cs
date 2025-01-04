using System;

namespace Domain.Configs;

public class StripeConfiguration
{
    public string SecretKey { get; set; }
    public string PublishableKey { get; set; }
}
