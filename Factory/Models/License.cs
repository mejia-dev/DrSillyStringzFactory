using System.Collections.Generic;

namespace Factory.Models
{
  public class License 
  {
    public int LicenseId { get; set; }
    public string LicenseName { get; set; }
    public List<EngineerLicense> LicensedEngineers { get; }
  }
}