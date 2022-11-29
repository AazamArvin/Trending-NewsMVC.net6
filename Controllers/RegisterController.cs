
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trending_News.Models;

namespace Trending_News.Controllers
{
  [Route("api/[Controller]")]
  public class RegisterController : Controller
  {
    public TrendingNewsContext Context { get; set; }

    public RegisterController(TrendingNewsContext _Context)
    {
      this.Context = _Context;

    }

    [HttpGet]
    public IActionResult Index()
    {
      // return new ObjectResult(Context.Login.Select(p => new { p.UserName, p.Password, p.id }).ToList());
      return new ObjectResult(Context.Registers.ToList());
    }


    [HttpPost]
    public IActionResult RegisterNew([FromBody] Register Model)
    {

      if (Model.id == 0)
        Context.Registers.Add(Model);
      else
        Context.Entry(Model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

      Context.SaveChanges();

      return new OkObjectResult(Model);


    }


    [HttpDelete("{id}")]
    public IActionResult Remove(int id)
    {
      var Model = Context.Registers.Find(id);
      Context.Registers.Remove(Model);
      Context.SaveChanges();
      return new OkObjectResult(Model);
    }

  }


 

}
