﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Users.Models
{
    //public class UserViewModels
    //{
    //}

    public class CreateModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }


    public class LoginModel
    {
        [Required]
        public String Name { get; set; }

        [Required]
        public String Password { get; set; }

    }

}