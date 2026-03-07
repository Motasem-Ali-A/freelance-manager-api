namespace FreelanceManager.Core.DTOs.Client;

public class CreateClientDto
{
    public string Name {get; set;} = string.Empty;
    public string Email {get; set;} = string.Empty;
    public string Phone {get; set;} = string.Empty;
    public string CompanyName {get; set;} = string.Empty;
    public string Address {get; set;} = string.Empty;
    public string Notes {get; set;} = string.Empty;
}
