using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Trending_News.Models
{
  public class Login
  {
    public int id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }

    [ForeignKey("ProductGroup")]
    public int RegisterId { get; set; }
    public Register Register { get; set; }


  }
}
