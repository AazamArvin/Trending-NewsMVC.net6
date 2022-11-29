using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Trending_News.Controllers
{
  [Authorize]
  [ApiController]
  public class GeneralInfoController : Controller
  {
    [HttpGet("GetInfo")]
    public IActionResult GetInfo()
    {
      try
      {
        // Get the claims values
        var claims = User.Claims;
        var username = claims.Where(c => c.Type == ClaimTypes.Name)
                           .Select(c => c.Value).SingleOrDefault();

        var name = claims.Where(c => c.Type == ClaimTypes.Surname)
                         .Select(c => c.Value).SingleOrDefault();

        var email = claims.Where(c => c.Type == ClaimTypes.Email)
                           .Select(c => c.Value).SingleOrDefault();

        var profileImg = claims.Where(c => c.Type == ClaimTypes.Uri)
                         .Select(c => c.Value).SingleOrDefault();

        return Ok(new { UserName = username, Title = name, Email = email, img = profileImg });




      }
      catch (Exception ex)
      {

        throw;
      }
    }
  }
}
