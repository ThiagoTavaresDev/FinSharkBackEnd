using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinSharkBackEnd.Dtos.Account
{
    public class UserDTO
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }
    
    }
}