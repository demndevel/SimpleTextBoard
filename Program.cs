using Microsoft.EntityFrameworkCore;
using DbContext = Demnoboard.DbContext;

var builder = WebApplication.CreateBuilder(args);

string connection = builder.Configuration.GetConnectionString("MySQLConnection");
int versionMinor = Convert.ToInt16(builder.Configuration.GetConnectionString("Minor"));
int versionMajor = Convert.ToInt16(builder.Configuration.GetConnectionString("Major"));
int versionBuild = Convert.ToInt16(builder.Configuration.GetConnectionString("Build"));

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DbContext>(options => options.UseMySql(connection, new MySqlServerVersion(new Version(versionMajor,versionMinor,versionBuild))));
builder.Services.Configure<CaptchaConfig>(builder.Configuration.GetSection("CaptchaConfig"));
builder.Services.Configure<BotConfig>(builder.Configuration.GetSection("BotConfig"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Posts/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Posts}/{action=Index}/");

app.Run();

public class CaptchaConfig
{
    public string ServerKey { get; set; } = "";
}

public class BotConfig
{
    public string Token { get; set; } = "";
    public string ChatId { get; set; } = "";
}