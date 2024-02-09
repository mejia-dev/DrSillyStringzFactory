using Microsoft.AspNetCore.Mvc;
using Factory.Models;
using System.Collections.Generic;
using System.Linq;

namespace Factory.Controllers
{
  public class HomeController : Controller
  {
    private readonly FactoryContext _db;

    public HomeController(FactoryContext db)
    {
      _db = db;
    }

    [HttpGet("/")]
    public ActionResult Index()
    {
      ViewBag.PageTitle = "Home";
      Dictionary<string, object[]> model = new Dictionary<string, object[]>();
      model.Add("engineers", _db.Engineers.ToArray());
      model.Add("machines", _db.Machines.ToArray());
      return View(model);
    }

  }
}