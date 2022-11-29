using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;

using Trending_News.Models;
using TrendingNews.Authorization;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;



builder.Services.AddControllersWithViews();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
builder.Services.Configure<Trending_News.ApplicationSettings>(
    builder.Configuration.GetSection("ApplicationSettings"));

builder.Services.AddAuthentication(x =>
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
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["ApplicationSettings:Secret"])),
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

builder.Services.AddAuthorization(options =>
{
  options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin", "HR").RequireClaim("id", "Israel"));
  options.AddPolicy("ExclusiveContentPolicy",
      policy => policy.RequireAssertion(context => context.User.HasClaim(claim => claim.Type == "id" && claim.Value == "Israel") ||
      context.User.IsInRole("SuperAdmin")));

  options.AddPolicy("IsOldEnoughWithRole", policy => policy.AddRequirements(new IsOldEnoughWithRoleRequirement(21)));
});

//builder.Services.AddSingleton<IAuthorizationHandler, IsOldEnoughWithRoleHandler>();

builder.Services.AddCors(opt =>
{
  opt.AddPolicy(name: "CorsPolicy", builder =>
  {
    builder.WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
  });
});
builder.Services.AddDbContext<TrendingNewsContext>(option =>
{
  option.UseSqlServer("Data Source=.;Initial Catalog=TrendNewsDb;Integrated Security=true;");
}, ServiceLifetime.Scoped);
    

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
