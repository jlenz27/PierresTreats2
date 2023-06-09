using System.Net.Mail;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;


using PierresTreats.Models;

namespace PierresTreats.Controllers
{ 
  [Authorize]
  public class TreatsController : Controller
  {
    private readonly PierresTreatsContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public TreatsController(UserManager<ApplicationUser> userManager, PierresTreatsContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    public ActionResult Index()
    {
      return View(_db.Treats.ToList());
    }

        public async Task<ActionResult> MyTreat()
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      List<Treat> userItems = _db.Treats
                          .Where(entry => entry.User.Id == currentUser.Id)
                          .Include(item => item.TreatName)
                          .ToList();
      return View(userItems);
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
  public ActionResult  Create(Treat treat)
      {
    
        _db.Treats.Add(treat);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    

    public ActionResult Details(int id)
    {
      Treat thisTag = _db.Treats
          .Include(fish => fish.JoinEntities)
          .ThenInclude(fish => fish.Flavor)
          .FirstOrDefault(tag => tag.TreatId == id);
      return View(thisTag);
    }

    public ActionResult Edit(int id)
    {
      Treat thisTag = _db.Treats.FirstOrDefault(tag => tag.TreatId == id);
      ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "FlavorName");
      return View(thisTag);

    }

    [HttpPost]
    public ActionResult Edit(Treat treat)
    {
      _db.Treats.Update(treat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Treat thisTag = _db.Treats.FirstOrDefault(tag => tag.TreatId == id);
      return View(thisTag);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Treat thisTag = _db.Treats.FirstOrDefault(tag => tag.TreatId == id);
      _db.Treats.Remove(thisTag);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddTreat(int id)
    {
      Treat thisTag = _db.Treats
                          .FirstOrDefault(tags => tags.TreatId == id);
      ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "FlavorName");
      return View(thisTag);
    }

    [HttpPost]
    public ActionResult AddTreat(Treat treat, int flavorId)
    {
      #nullable enable
      FlavorTreat? joinEntity = _db.FlavorTreats.FirstOrDefault(join => (join.FlavorId == flavorId && join.TreatId == treat.TreatId));
      #nullable disable
      if (joinEntity == null && flavorId != 0)
      {
        _db.FlavorTreats.Add(new FlavorTreat() { FlavorId = flavorId, TreatId = treat.TreatId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = treat.TreatId });
    }

    [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    {
      FlavorTreat joinEntry = _db.FlavorTreats.FirstOrDefault(entry => entry.FlavorTreatId == joinId);
      _db.FlavorTreats.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }


  }
}