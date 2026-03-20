using System.ComponentModel.DataAnnotations;

namespace StockApp.Models;

public class GameViewModel
{
    public string Id { get; set; } = string.Empty;
    [Required(ErrorMessage = "Giv venligst dysten et navn")]
    [StringLength(50, ErrorMessage = "Navnet er for langt")]
    public string Name { get; set; } = string.Empty;

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Range(100, 10000000, ErrorMessage = "Startkapital skal være mellem 100 og 10.000.000")]
    public decimal StartCapital { get; set; }
}
