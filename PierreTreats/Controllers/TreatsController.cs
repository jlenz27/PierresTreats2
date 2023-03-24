using Microsoft.AspNetCore.Mvc;
using PierresTreats.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;


namespace PierresTreats.Controllers;

[Authorize]
public class TreatsController : Controller
{
  private readonly PierresTreatsContext _db;

  public TreatsController(PierresTreatsContext db)
  {
    _db = db;
  }
}