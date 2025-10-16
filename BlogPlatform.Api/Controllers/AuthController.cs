using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlogPlatform.Api.Models;
using BlogPlatform.Api.Models.Requests;
using BlogPlatform.Api.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BlogPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    public AuthController(IConfiguration config) => _config = config;

    // Simple demo endpoint to issue a JWT
    [HttpPost("token")]
    [AllowAnonymous]
    public ActionResult<Result<ResponseTokenResponse>> Token([FromBody] AuthRequest request)
    {
        // Basic demo-only check. Replace with real user store.
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return Ok(Result<ResponseTokenResponse>.Fail("Invalid credentials."));

        // Here you could check username/password against a DB/IdP
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, request.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var issuer = _config["Jwt:Issuer"] ?? "BlogPlatform";
        var audience = _config["Jwt:Audience"] ?? "BlogPlatformAudience";
        var key = _config["Jwt:Key"] ?? "dev-key-change-me";
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var expires = DateTime.UtcNow.AddHours(2);
        var token = new JwtSecurityToken(issuer, audience, claims, expires: expires, signingCredentials: creds);
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(Result<ResponseTokenResponse>.Ok(new ResponseTokenResponse
        {
            Token = tokenString,
            ExpiresAtUtc = expires
        }));
    }
}
