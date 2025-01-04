using System;

namespace Business.Services.Payment;

public interface IPaymentService
{
    Task<(int code, string message)> CreateCheckoutSessionAsync();
    Task<(int code, string message)> SuccessAsync(Guid trackId);
    Task<(int code, string message)> CancelAsync(Guid trackId);
}
