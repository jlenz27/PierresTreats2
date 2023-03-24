using Microsoft.AspNetCore.Mvc;
using PierresTreats.Models;
using System.Collections.Generic;
using System.Linq;

namespace PierresTreats.Controllers;

public class HomeController : Controller
{
  private readonly PierresTreatsContext _db;

  public HomeController(PierresTreatsContext db)
  {
    _db = db;
  }

  [HttpGet("/")]
  public ActionResult Index()
  {
    Treat[] treats = _db.Treats.ToArray();
    Flavor[] flavors = _db.Flavors.ToArray();
    Dictionary<string, object[]> model = new Dictionary<string, object[]>();
    model.Add("treats", treats);
    model.Add("flavors", flavors); 
    return View(model);
  }
}