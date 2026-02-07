using JJK_API.Data;
using JJK_API.DTO.User;
using JJK_API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JJK_API.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class UserController : ControllerBase
  {

    private AppDbContext _dbContext;

    public UserController(AppDbContext appDbContext)
    {
      this._dbContext = appDbContext;
    }

    [HttpPost("auth")]
    public async Task<IActionResult> Auth(UserAuthRequest userRequest) {

      try
      {

        User user = await this._dbContext.Usuario.FirstAsync(user => user.Email == userRequest.email);

        if (user == null)
        {
          return Unauthorized(new { message = "Usuário não existe" });
        }

        if (userRequest.password != user.Password)
        {
          return Unauthorized(new { message = "Senha incorreta" });
        }

        UserAuthResponse response = new UserAuthResponse("32180I3U0218371280");

        return Ok(response);

      }
      catch(Exception e)
      {
        return BadRequest();
      }
     
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserRegisterRequest user)
    {
 
      try
      {

        if (this._dbContext.Usuario.FirstOrDefault(u => u.Email == user.email) != null)
        {
          return BadRequest(new { message = "Email já cadastrado" });
        }

        if (user.password.Length < 8)
        {
          return BadRequest(new { message = "Senha muito fraca" });
        }

        User newUser = new User(user.email, user.password);

        this._dbContext.Usuario.Add(newUser);
        await this._dbContext.SaveChangesAsync();


        UserAuthResponse response = new UserAuthResponse("230184192-0412-9321=");

        return Ok(response);

      }catch(Exception e)
      {
        return BadRequest();
      }
    }
  }
}
