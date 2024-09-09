using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public  string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        [Display(Name = "List Price")]
        [Range(0, int.MaxValue)]
        public double ListPrice { get; set; }

        [Required]
        [Display(Name = "Price for 1-50")]
        [Range(0, int.MaxValue)]
        public double Price { get; set; }

        [Required]
        [Display(Name = "Price for 50+")]
        [Range(0, int.MaxValue)]
        public double Price50 { get; set; }

        [Required]
        [Display(Name = "Price for 100+")]
        [Range(0, int.MaxValue)]
        public double Price100 { get; set; }
    }
}
