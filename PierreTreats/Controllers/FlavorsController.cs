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
  public class FlavorsController : Controller
  {
    private readonly PierresTreatsContext _db;
    private readonly UserManager<ApplicationUser> _userManager; 

    public FlavorsController(UserManager<ApplicationUser> userManager, PierresTreatsContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    public ActionResult Index()
    {
      return View(_db.Flavors.ToList());
    }
    public async Task<ActionResult> MyFlavors()
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      List<Flavor> userItems = _db.Flavors
                          .Where(entry => entry.User.Id == currentUser.Id)
                          .Include(item => item.Treat)
                          .ToList();
      return View(userItems);
    }


    public ActionResult Create()
    {
      return View();
    }

      [HttpPost]
    public async Task<ActionResult> Create(Flavor flavor)
    {
      if (!ModelState.IsValid)
      {
        
        ViewBag.FlavorId = new SelectList(_db.Treats, "TreatId", "TreatName");
        return View(flavor);
      }
      else
      {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
        flavor.User = currentUser;
        
        _db.Flavors.Add(flavor);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }
    
    public ActionResult Details(int id)
    {
      Flavor ThisFlavor = _db.Flavors
        .Include(recipe => recipe.JoinEntities)
        .ThenInclude(recipe => recipe.Treat)
        .FirstOrDefault(recipe => recipe.FlavorId == id);

      return View(ThisFlavor);
    }

    public ActionResult Edit(int id)
    {
      Flavor thisFlavor = _db.Flavors.FirstOrDefault(recipe => recipe.FlavorId == id);
      return View(thisFlavor);

    }

    [HttpPost]
    public ActionResult Edit(Flavor flavor)
    {
      _db.Flavors.Update(flavor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

      public ActionResult Delete(int id)
    {
      Flavor thisFlavor = _db.Flavors.FirstOrDefault(recipe => recipe.FlavorId == id);
      return View(thisFlavor);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Flavor thisFlavor= _db.Flavors.FirstOrDefault(recipe => recipe.FlavorId == id);
      _db.Flavors.Remove(thisFlavor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddTreat(int id)
    {
        Flavor thisFlavor = _db.Flavors
                              .Include(fish => fish.JoinEntities)
                              .ThenInclude(join => join.Treat)
                              .FirstOrDefault(recipes => recipes.FlavorId == id);
        ViewBag.TagId = new SelectList(_db.Treats, "TreatId", "TreatName");
        return View(thisFlavor);
    }

    [HttpPost]
    public ActionResult AddTreat (Flavor flavor, int TreatId)
    {
    #nullable enable
        FlavorTreat? joinEntity = _db.FlavorTreats
                                  .FirstOrDefault(join => (join.TreatId == TreatId 
                                  && join.FlavorId == flavor.FlavorId));
    #nullable disable
        if(joinEntity == null && TreatId != 0)
        {
            _db.FlavorTreats.Add(new FlavorTreat() {TreatId = TreatId, FlavorId = flavor.FlavorId});
            _db.SaveChanges();
        }  
        return RedirectToAction("Details", new { id = flavor.FlavorId});

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
