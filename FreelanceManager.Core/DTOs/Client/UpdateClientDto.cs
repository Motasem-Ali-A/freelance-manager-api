using System.ComponentModel.DataAnnotations;

namespace FreelanceManager.Core.DTOs.Client;

public class UpdateClientDto
{
    [Required, MaxLength(100)]
    public string Name {get; set;} = string.Empty;
    [MaxLength(20)]
    public string Phone {get; set;} = string.Empty;
    [MaxLength(100)]
    public string CompanyName {get; set;} = string.Empty;
    [MaxLength(100)]
    public string Address {get; set;} = string.Empty;
    [MaxLength(500)]
    public string Notes {get; set;} = string.Empty;
}
