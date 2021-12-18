using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.EmailSenders.Interfaces
{
    public interface IEmailSender
    {
        Task Execute(string userEmail, string body, string title);
    }
}
