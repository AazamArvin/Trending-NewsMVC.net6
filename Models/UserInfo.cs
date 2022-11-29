using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trending_News.Models
{
  public class UserInfo
  {

    public int id { get; set; }

    public string Name { get; set; }
    public string Mobile { get; set; }

    public string Long { get; set; }

    public string Lang { get; set; }
    [Required]
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Picture { get; set; }

    [Required]
    public byte[] PasswordSalt { get; set; }
    [Required]
    public byte[] PasswordHash { get; set; }
  }
}
