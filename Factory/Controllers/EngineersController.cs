using Factory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Factory.Controllers
{
  public class EngineersController : Controller
  {
    private readonly FactoryContext _db;

    public EngineersController(FactoryContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      ViewBag.PageTitle = "Engineers";
      return View(_db.Engineers.ToList());
    }

    public ActionResult Create()
    {
      ViewBag.PageTitle = "Add an Engineer";
      return View();
    }
    [HttpPost]
    public ActionResult Create(Engineer engineer)
    {
      _db.Engineers.Add(engineer);
      _db.SaveChanges();
      List<Engineer> model = _db.Engineers.ToList();
      return View("Index", model);
    }

    public ActionResult Details(int id)
    {
      Engineer thisEngineer = _db.Engineers
          .Include(eng => eng.AssignedMachines)
          .FirstOrDefault(eng => eng.EngineerId == id);
      ViewBag.PageTitle = $"Engineer Details - {thisEngineer.EngineerFullName} ";
      return View(thisEngineer);
    }

    public ActionResult Edit(int id)
    {
      Engineer thisEngineer = _db.Engineers.FirstOrDefault(engs => engs.EngineerId == id);
      ViewBag.PageTitle = $"Edit Engineer - {thisEngineer.EngineerFullName}";
      ViewBag.MachineId = new SelectList(_db.Machines, "MachineId", "MachineName");
      return View(thisEngineer);
    }

    [HttpPost]
    public ActionResult Edit(Engineer eng)
    {
      _db.Engineers.Update(eng);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Engineer thisEngineer = _db.Engineers.FirstOrDefault(engs => engs.EngineerId == id);
      ViewBag.PageTitle = $"Delete Engineer - {thisEngineer.EngineerFullName}";
      return View(thisEngineer);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Engineer thisEngineer = _db.Engineers.FirstOrDefault(engs => engs.EngineerId == id);
      _db.Engineers.Remove(thisEngineer);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}