using System.ComponentModel.DataAnnotations;

namespace CleanTemplate.Application.DTOs;

public class ProductPayloadDTO
{
    [Required(ErrorMessage = "Product name is required")]
    [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Stock is required")]
    [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative")]
    public int Stock { get; set; }

    public bool IsActive { get; set; } = true;

    [Required(ErrorMessage = "CreatedBy is required")]
    [StringLength(50, ErrorMessage = "CreatedBy cannot exceed 50 characters")]
    public string CreatedBy { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "UpdatedBy cannot exceed 50 characters")]
    public string? UpdatedBy { get; set; }
}
