using Factory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Factory.Controllers
{
  public class MachinesController : Controller
  {
    private readonly FactoryContext _db;

    public MachinesController(FactoryContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      ViewBag.PageTitle = "Machines";
      return View(_db.Machines.ToList());
    }

    public ActionResult Create()
    {
      ViewBag.PageTitle = "Add a Machine";
      return View();
    }
    [HttpPost]
    public ActionResult Create(Machine machine)
    {
      _db.Machines.Add(machine);
      _db.SaveChanges();
      List<Machine> model = _db.Machines.ToList();
      return View("Index", model);
    }

    public ActionResult Details(int id)
    {
      Machine thisMachine = _db.Machines
          .Include(mcn => mcn.MachineEngineers)
          .FirstOrDefault(mcn => mcn.MachineId == id);
      ViewBag.PageTitle = $"Machine Details - {thisMachine.MachineName} ";
      ViewBag.EngineersCount = _db.Engineers.Count();
      return View(thisMachine);
    }

    public ActionResult Edit(int id)
    {
      Machine thisMachine = _db.Machines.FirstOrDefault(mcn => mcn.MachineId == id);
      ViewBag.PageTitle = $"Edit Machine - {thisMachine.MachineName}";
      return View(thisMachine);
    }

    [HttpPost]
    public ActionResult Edit(Machine mcn)
    {
      _db.Machines.Update(mcn);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Machine thisMachine = _db.Machines.FirstOrDefault(mcn => mcn.MachineId == id);
      ViewBag.PageTitle = $"Delete Machine - {thisMachine.MachineName}";
      return View(thisMachine);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Machine thisMachine = _db.Machines.FirstOrDefault(mcn => mcn.MachineId == id);
      _db.Machines.Remove(thisMachine);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddEngineer(int id)
    {
      Machine thisMachine = _db.Machines
        .Include(mcn => mcn.MachineEngineers)
        .FirstOrDefault(mcn => mcn.MachineId == id);
      ViewBag.EngineerId = new SelectList(_db.Engineers, "EngineerId", "EngineerFullName");
      ViewBag.PageTitle = $"License {thisMachine.MachineName} To Engineer";
      return View(thisMachine);
    }

    [HttpPost]
    public ActionResult AddEngineer(Machine mcn, int engineerId)
    {
      #nullable enable
      EngineerMachine? joinEntity = _db.EngineerMachines.FirstOrDefault(join => (join.EngineerId == engineerId && join.MachineId == mcn.MachineId));
      #nullable disable
      if (engineerId != 0 && joinEntity == null)
      {
        EngineerMachine licensedEngineer = new EngineerMachine() { EngineerId = engineerId, MachineId = mcn.MachineId};
        _db.EngineerMachines.Add(licensedEngineer);
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = mcn.MachineId });
    }
  }
}