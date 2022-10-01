using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WCore.Web.Models.Users
{
    public class LoginOrRegisterModel
    {
        public LoginModel Login { get; set; }
        public RegisterModel Register { get; set; }
    }
}
