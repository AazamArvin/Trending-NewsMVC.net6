using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trending_News.Models
{
  public class Register
  {
    public int id { get; set; }

    [Required]
    public string UserName { get; set; }
    [Required]
    public string Role { get; set; }
    [Required]
    public string BirthDay { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string ConfirmPassword { get; set; }


    /// <summary>
    /// Navigation Property
    /// </summary>
    public ICollection<Login> Logins { get; set; }

    /////image





  }
}
