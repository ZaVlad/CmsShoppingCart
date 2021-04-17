using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Models
{
    public class User
    {
        [Required,MinLength(4,ErrorMessage ="Length of your name is too small, minimum is 4")]
        [Display(Name ="UserName")]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password),Required, MinLength(4, ErrorMessage = "Length of your password is too small, minimum is 4")]
        public string Password{ get; set; }
    }
}
