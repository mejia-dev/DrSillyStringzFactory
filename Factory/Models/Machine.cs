using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Factory.Models
{
  public class Machine
  {
    public int MachineId { get; set; }
    [Required(ErrorMessage = "The machine must be named.")]
    public string MachineName { get; set; }
    public List<EngineerMachine> MachineEngineers { get; }
  }
}