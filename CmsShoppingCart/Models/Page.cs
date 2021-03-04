using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Models
{
    public class Page
    {
        public int Id { get; set; }
        [Required,]
        [MaxLength(10),MinLength(2, ErrorMessage = "Minimum Length is 2")]
        public string Title { get; set; }
        public string Slug { get; set; }
        [Required]
        [MaxLength(100), MinLength(4, ErrorMessage = "Minimum Length is 4")]
        public string Content { get; set; }
        public int Sorting { get; set; }

    }
}
