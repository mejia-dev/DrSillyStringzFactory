using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Factory.Models
{
  public class Engineer
  {
    public int EngineerId { get; set; }
    [Required(ErrorMessage = "The engineer must have a first name.")]
    public string EngineerFirstName { get; set; }
    [Required(ErrorMessage = "The engineer must have a last name.")]
    public string EngineerLastName { get; set; }
    public List<EngineerMachine> AssignedMachines { get; }
    public string EngineerFullName => $"{EngineerFirstName} {EngineerLastName}";
  }
}
