﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Models
{
    public class Login
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password), Required, MinLength(4, ErrorMessage = "Length of your password is too small, minimum is 4")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }

    }
}

