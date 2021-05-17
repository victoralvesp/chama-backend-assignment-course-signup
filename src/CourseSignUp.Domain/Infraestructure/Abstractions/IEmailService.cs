using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseSignUp.Infraestructure.Abstractions
{
    public interface IEmailService
    {
        void SendEmail(string email, string v);
    }
}
