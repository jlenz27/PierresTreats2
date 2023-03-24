using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PierresTreats.Models;


public class Flavor
{
  public int FlavorId { get; set; }
  [Required(ErrorMessage = "Please enter a name for the flavor.")]
  public string FlavorName { get; set; }
  public List<FlavorTreat> JoinEntities { get; set; }

      public ApplicationUser User { get; set; }

      public Treat Treat {get; set;}


}