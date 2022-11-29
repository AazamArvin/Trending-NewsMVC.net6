using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Trending_News.Controllers
{
    [Authorize]
    [ApiController]
    public class ItemsController : Controller
    {
        public List<string> colorList = new List<string>() { "blue", "red", "green", "yellow", "pink" };

        [HttpGet("GetColorList")]
        public List<string> GetColorList()
        {
            try
            {
                var ff = User.Claims;
                return colorList;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
