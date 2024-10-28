using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Movie.Models;

public class OrderDetail
{
    public int Id { get; set; }
    [Required] public int OrderHearderId { get; set; }

    [ForeignKey("OrderHearderId")]
    [ValidateNever]
    public OrderHeader OrderHearder { get; set; }

    [Required] public int ProductId { get; set; }

    [ForeignKey("ProductId")]
    [ValidateNever]
    public Product Product { get; set; }

    public int Count { get; set; }
    public double Price { get; set; }
    
}