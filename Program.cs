using _2_uzduotis_c_sharp.Models;
using _2_uzduotis_c_sharp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

/*builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();*/

builder.Services.AddControllers();

builder.Services.AddDbContext<UserContext>(opt =>
opt.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SocialAccountsDb;Trusted_Connection=True;"));


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie(opt =>
{
    //opt.LoginPath = "/auth/signin";
})
.AddGoogle(googleOptions =>
{
    googleOptions.ClientId = "17343139863-9a4jhtrcu7f3jvbbl6ubfe62rmv7evrk.apps.googleusercontent.com";
    googleOptions.ClientSecret = "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";
    googleOptions.SaveTokens = true;
})
.AddGitHub(githubOptions =>
{
    githubOptions.ClientId = "f645a9d9990eadaa53f8";
    githubOptions.ClientSecret = "794c1d109dbda8ffeb1fc95c142c1befbb91b8ab";
    githubOptions.CallbackPath = "/github/login/callback";
    githubOptions.SaveTokens = true;
});

var app = builder.Build();


/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
