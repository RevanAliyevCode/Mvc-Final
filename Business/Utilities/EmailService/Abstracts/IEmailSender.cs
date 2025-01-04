using System;
using Business.Utilities.EmailService.Concrets;

namespace Business.Utilities.EmailService.Abstracts;

public interface IEmailSender
{
    void SendEmail(Message message);

}
