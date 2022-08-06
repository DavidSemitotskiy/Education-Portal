using Portal.Domain.DTOs;
using Portal.Domain.Interfaces;
using Portal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Application.Interfaces
{
    public interface IUserManager
    {
        static User? CurrentUser { get; set; }

        IUserRepository UserRepository { get; set; }

        void LogIn(UserLoginDTO userLogin);

        void Register(UserRegisterDTO userRegister);

        void LogOff();
    }
}
