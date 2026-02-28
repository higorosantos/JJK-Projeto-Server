using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JJK_API.Model;
using Microsoft.IdentityModel.Tokens;

namespace JJK_API.Service
{
  public class TokenService
  {

    private readonly IConfiguration _config;

    public TokenService(IConfiguration config)
    {
      _config = config;
    }


    public string GenerateToken(User user)
    {
      var jwtSettings = _config.GetSection("Jwt");

      var claims = new[]
      {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Nickname)
        };

      var key = new SymmetricSecurityKey(
          Encoding.UTF8.GetBytes(jwtSettings["Key"])
      );

      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(
          issuer: jwtSettings["Issuer"],
          audience: jwtSettings["Audience"],
          claims: claims,
          expires: DateTime.UtcNow.AddMinutes(
              double.Parse(jwtSettings["ExpireMinutes"])
          ),
          signingCredentials: creds
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}
