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
      // if (!ModelState.IsValid)
      // {
      //     ViewBag.PageTitle = "Add an Engineer";
      //     return View(engineer);
      // }
      // else
      // {
      //   _db.Engineers.Add(engineer);
      //   _db.SaveChanges();
      //   List<Engineer> model = _db.Engineers.ToList();
      //   return View("Index", model);
      // }
      if (!ModelState.IsValid)
      {
          // ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
          ViewBag.PageTitle = "Add an Engineer";
          return View(engineer);
      }
      else
      {
        _db.Engineers.Add(engineer);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }

    public ActionResult Details(int id)
    {
      Engineer thisEngineer = _db.Engineers
          .Include(eng => eng.AssignedMachines)
          .ThenInclude(join => join.Machine)
          .FirstOrDefault(eng => eng.EngineerId == id);
      ViewBag.PageTitle = $"Engineer Details - {thisEngineer.EngineerFullName} ";
      ViewBag.MachinesCount = _db.Machines.Count();
      return View(thisEngineer);
    }

    public ActionResult Edit(int id)
    {
      Engineer thisEngineer = _db.Engineers.FirstOrDefault(engr => engr.EngineerId == id);
      ViewBag.PageTitle = $"Edit Engineer - {thisEngineer.EngineerFullName}";

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
      Engineer thisEngineer = _db.Engineers.FirstOrDefault(engr => engr.EngineerId == id);
      ViewBag.PageTitle = $"Delete Engineer - {thisEngineer.EngineerFullName}";
      return View(thisEngineer);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Engineer thisEngineer = _db.Engineers.FirstOrDefault(engr => engr.EngineerId == id);
      _db.Engineers.Remove(thisEngineer);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddMachine(int id)
    {
      Engineer thisEngineer = _db.Engineers
        .Include(engr => engr.AssignedMachines)
        .FirstOrDefault(engr => engr.EngineerId == id);
      ViewBag.MachineId = new SelectList(_db.Machines, "MachineId", "MachineName");
      ViewBag.PageTitle = $"License {thisEngineer.EngineerFullName} For Machine";
      return View(thisEngineer);
    }

    [HttpPost]
    public ActionResult AddMachine(Engineer engr, int machineId)
    {
      #nullable enable
      EngineerMachine? joinEntity = _db.EngineerMachines.FirstOrDefault(join => (join.MachineId == machineId && join.EngineerId == engr.EngineerId));
      #nullable disable
      if (machineId != 0 && joinEntity == null)
      {
        EngineerMachine authorizedMachine = new EngineerMachine() { MachineId = machineId, EngineerId = engr.EngineerId};
        _db.EngineerMachines.Add(authorizedMachine);
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = engr.EngineerId });
    }

    [HttpPost]
    public ActionResult DeleteLicense(int licenseId)
    {
      EngineerMachine licenseEntry = _db.EngineerMachines.FirstOrDefault(entry => entry.EngineerMachineId == licenseId);
      _db.EngineerMachines.Remove(licenseEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}