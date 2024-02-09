using System.Collections.Generic;

namespace Factory.Models
{
  public class Engineer
  {
    public int EngineerId { get; set; }
    public string EngineerFirstName { get; set; }
    public string EngineerLastName { get; set; }
    public List<string> EngineerLicenses { get; set; }
    public string EngineerFullName => $"{EngineerFirstName} {EngineerLastName}";
  }
}
