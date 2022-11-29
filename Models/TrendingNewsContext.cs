using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trending_News.Models
{
  public class TrendingNewsContext : DbContext
  {
    public TrendingNewsContext(DbContextOptions option) : base(option) { }

    public DbSet<Login> Logins { get; set; }
    public DbSet<Register> Registers { get; set; }
    public DbSet<UserInfo> UserInfos { get; set; }
   
  }
}
