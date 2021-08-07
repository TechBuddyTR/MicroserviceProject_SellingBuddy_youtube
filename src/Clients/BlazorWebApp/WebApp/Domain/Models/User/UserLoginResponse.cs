using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Domain.Models.User
{
    public class UserLoginResponse
    {
        public string UserName { get; set; }

        public string UserToken { get; set; }
    }
}
