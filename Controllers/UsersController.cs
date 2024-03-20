using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _2_uzduotis_c_sharp.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using _2_uzduotis_c_sharp.Services;
using Microsoft.AspNetCore.Authorization;
using Google.Apis.Auth.OAuth2;
using Google.Apis.PeopleService.v1;
using Google.Apis.Services;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using _2_uzduotis_c_sharp.Repo;
using NuGet.Protocol;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using AspNet.Security.OAuth.GitHub;
using Google.Apis.PeopleService.v1.Data;
using System.Xml;

namespace _2_uzduotis_c_sharp.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserContext _context;

        public UsersController(UserContext context)
        {
            _context = context;
        }

        [HttpGet("google/profile/save")]
        public async Task<IActionResult> GetAndSaveGoogleUser()
        {
            var service = new PeopleServiceService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = GoogleCredential.FromAccessToken(TokenRepo.GetToken("Google")),
                ApplicationName = "Your Application Name",
            });

            var personRequest = service.People.Get("people/me");
            personRequest.PersonFields = "names,emailAddresses,photos";
            var person = await personRequest.ExecuteAsync();

            var userGoogle = new GoogleUser
            {
                Id = "17343139863-9a4jhtrcu7f3jvbbl6ubfe62rmv7evrk.apps.googleusercontent.com",
                Email = person.EmailAddresses[0].Value,
                GivenName = person.Names[0].GivenName,
                FamilyName = person.Names[0].FamilyName,
                Picture = person.Photos[0].Url,
            };
            _context.GoogleUsers.Add(userGoogle);
            await _context.SaveChangesAsync();
            return Ok("Saved");
        }

        [HttpGet("google/profile/get")]
        public async Task<ActionResult<IEnumerable<GoogleUser>>> GetDBGoogleUser()
        {
            var users = await _context.GoogleUsers.ToListAsync();
            return Ok(users);

        }

        [HttpGet("google/profile/remove")]
        public async Task<IActionResult> RemoveGoogleUsers()
        {
            var users = _context.GoogleUsers;
            if (users.Any())
            {
                _context.GoogleUsers.RemoveRange(users);
                await _context.SaveChangesAsync();
                return Ok("Removed");
            }
            else
            {
                return NotFound("Not removed");
            }
        
        }

        [HttpGet("google/profile")]
        public async Task<GoogleUser> GetGoogleUser()
        {
            var service = new PeopleServiceService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = GoogleCredential.FromAccessToken(TokenRepo.GetToken("Google")),
                ApplicationName = "Your Application Name",
            });
            
            var personRequest = service.People.Get("people/me");
            personRequest.PersonFields = "names,emailAddresses,photos";
            var person = await personRequest.ExecuteAsync();

            Console.WriteLine(person);

            var userGoogle = new GoogleUser
            {
                Id = "17343139863-9a4jhtrcu7f3jvbbl6ubfe62rmv7evrk.apps.googleusercontent.com",
                Email = person.EmailAddresses[0].Value,
                GivenName = person.Names[0].GivenName,
                FamilyName = person.Names[0].FamilyName,
                Picture = person.Photos[0].Url,
            };
            return userGoogle;
        }

        [HttpGet("google/login")]
        public IActionResult GoogleLogin()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "/google/login/callback" }, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google/login/callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
                return BadRequest("Fail");

            var accessToken = authenticateResult.Ticket.Properties.GetTokenValue("access_token");

            TokenRepo.SetToken("Google", accessToken);

            return Redirect("/");
        }


        [HttpGet("google/logout")]
        public async Task<IActionResult> GoogleLogout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TokenRepo.RemoveToken("Google");

            return Ok("Logged out");
        }


        [HttpGet("github/profile/save")]
        public async Task<IActionResult> GetAndStoreGithubUser()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenRepo.GetToken("GitHub"));
            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppName", "1.0"));
            var response = await httpClient.GetAsync("https://api.github.com/user");

            var content = await response.Content.ReadAsStringAsync();

            JsonDocument doc = JsonDocument.Parse(content);
            JsonElement root = doc.RootElement;

            var Id = root.GetProperty("id").GetInt32();
            var Login = root.GetProperty("login").GetString();
            var Picture = root.GetProperty("avatar_url").GetString();
            var Type = root.GetProperty("type").GetString();
            var PublicRepos = root.GetProperty("public_repos").GetInt32();
            var Created = root.GetProperty("created_at").GetString();

            var userGithub = new GitHubUser
            {
                Login = Login,
                Picture = Picture,
                Type = Type,
                PublicRepos = PublicRepos,
                Created = Created,
            };
            _context.GitHubUsers.Add(userGithub);
            await _context.SaveChangesAsync();
            return Ok("Saved");

        }

        [HttpGet("github/profile/get")]
        public async Task<ActionResult<IEnumerable<GitHubUser>>> GetDBGithubUser()
        {
            var users = await _context.GitHubUsers.ToListAsync();
            return Ok(users);

        }

        [HttpGet("github/profile/remove")]
        public async Task<IActionResult> RemoveGitHubUsers()
        {
            var users = _context.GitHubUsers;
            if (users.Any())
            {
                _context.GitHubUsers.RemoveRange(users);
                await _context.SaveChangesAsync();
                return Ok("Removed");
            }
            else
            {
                return NotFound("Not removed");
            }

        }


        [HttpGet("github/profile")]
        public async Task<GitHubUser> GetGithubUser()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenRepo.GetToken("GitHub"));
            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppName", "1.0"));
            var response = await httpClient.GetAsync("https://api.github.com/user");

            var content = await response.Content.ReadAsStringAsync();

            JsonDocument doc = JsonDocument.Parse(content);
            JsonElement root = doc.RootElement;

            var Id = root.GetProperty("id").GetInt32();
            var Login = root.GetProperty("login").GetString();
            var Picture = root.GetProperty("avatar_url").GetString();
            var Type = root.GetProperty("type").GetString();
            var PublicRepos = root.GetProperty("public_repos").GetInt32();
            var Created = root.GetProperty("created_at").GetString();

            var userGithub = new GitHubUser
            {
                Id = Id,
                Login = Login,
                Picture = Picture,
                Type = Type,
                PublicRepos = PublicRepos,
                Created = Created,
            };
            return userGithub;

        }


        [HttpGet("github/login")]
        public IActionResult GithubLogin()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "/callback" }, "GitHub");
        }

        [HttpGet("callback")]
        public async Task<IActionResult> GithubCallback()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
                return BadRequest("Fail");

            var accessToken = authenticateResult.Ticket.Properties.GetTokenValue("access_token");

            TokenRepo.SetToken("GitHub", accessToken);

            return Redirect("/");
        }


        [HttpGet("github/logout")]
        public async Task<IActionResult> GithubLogout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TokenRepo.RemoveToken("GitHub");

            return Ok("Logged out");
        }

    }
}
