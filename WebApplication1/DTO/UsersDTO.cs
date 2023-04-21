using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.DTO
{
    public class UsersDTO
    {
        public string UserEmail { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserType { get; set; }
        public string UserImg { get; set; }
        public string UserPassword { get; set; }
        public int UserBuget { get; set; }
    }
}