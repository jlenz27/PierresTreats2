using Microsoft.AspNetCore.Mvc;
using PierresTreats.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;


namespace PierresTreats.Controllers;

[Authorize]
public class FlavorsController : Controller
{
  private readonly PierresTreatsContext _db;

  public FlavorsController(PierresTreatsContext db)
  {
    _db = db;
  }

}