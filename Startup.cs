using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Trending_News
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllersWithViews();

      services.AddControllersWithViews();
      services.AddSwaggerGen();
      //services.AddEndpointsApiExplorer();
      services.Configure<Trending_News.AppSettings>(
      Configuration.GetSection("ApplicationSettings"));
      services.AddAuthentication(x =>
      {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddCookie(x =>
      {
        x.Cookie.Name = "token";

      }).AddJwtBearer(x =>
      {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["ApplicationSettings:Secret"].ToString())),
          ValidateIssuer = false,
          ValidateAudience = false
        };
        x.Events = new JwtBearerEvents
        {
          OnMessageReceived = context =>
          {
            context.Token = context.Request.Cookies["token"];
            return Task.CompletedTask;
          }
        };

      });


      services.AddAuthorization(options =>
      {
        options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin", "HR").RequireClaim("id", "Israel"));
        options.AddPolicy("ExclusiveContentPolicy",
            policy => policy.RequireAssertion(context => context.User.HasClaim(claim => claim.Type == "id" && claim.Value == "Israel") ||
            context.User.IsInRole("SuperAdmin")));

       // options.AddPolicy("IsOldEnoughWithRole", policy => policy.AddRequirements(new IsOldEnoughWithRoleRequirement(21)));
      });
      services.AddCors(opt =>
      {
        opt.AddPolicy(name: "CorsPolicy", builder =>
        {
          builder.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
        });
      });
      services.AddDbContext<Models.TrendingNewsContext>(option =>
      {
        option.UseSqlServer("Data Source=.;Initial Catalog=TrendNewsDb;Integrated Security=true;");
      }, ServiceLifetime.Scoped);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }
      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

     // app.UseCors(p => p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
      app.UseCors("CorsPolicy");
     
      app.UseDefaultFiles();
      app.UseStaticFiles();


      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}");
      });

      //app.UseMvcWithDefaultRoute();
    }
  }
}
