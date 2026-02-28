using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JJK_API.Data;
using JJK_API.DTO.User;
using JJK_API.Model;
using JJK_API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JJK_API.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class UserController : ControllerBase
  {

    private AppDbContext _dbContext;


    private readonly TokenService _tokenService;

    public UserController(AppDbContext appDbContext, TokenService tokenService)
    {
      this._dbContext = appDbContext;
      _tokenService = tokenService;
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

        var token = this._tokenService.GenerateToken(user);

        UserAuthResponse response = new UserAuthResponse(token, !string.IsNullOrEmpty(user.Nickname));

        return Ok(response);

      }
      catch(Exception e)
      {
        return BadRequest();
      }
     
    }


    //CRIAR UM ENUM DE STATUS DE ERRO
    [Authorize]
    [HttpPost("nickname")]
    public async Task<IActionResult> NickName(ChangeNickRequestDTO userNickname)
    {

      try
      {

        User nickExist = await this._dbContext.Usuario.FirstOrDefaultAsync(user => user.Nickname == userNickname.NewNickName);

        if (nickExist != null)
        {
          return BadRequest(new { message = "Apelido já cadastrado." });
        }

        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        User user = await _dbContext.Usuario.FirstAsync(u => u.Id == userId);

        if (user == null)
        {
          return NotFound(new { message = "Usuário não encontrado"});
        }

        user.Nickname = userNickname.NewNickName;

        this._dbContext.Usuario.Update(user);
        await this._dbContext.SaveChangesAsync();
        return Ok();

      }
      catch (Exception e)
      {
        Console.WriteLine(e);
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

        var token = this._tokenService.GenerateToken(newUser);

        UserAuthResponse response = new UserAuthResponse(token, false);

        return Ok(response);

      }catch(Exception e)
      {
        return BadRequest();
      }
    }
  }
}
