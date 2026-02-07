using System.ComponentModel.DataAnnotations.Schema;
using JJK_API.Enum;
using Microsoft.AspNetCore.Identity.Data;

namespace JJK_API.Model
{

  [Table("Usuario")]
  public class User
  {
    public Guid Id { get; init; }
    public string Nickname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime Created_at { get; set; }
    public DateTime Updated_at { get; set; }
    public StatusEnum Status { get; set ; }


    public User(string email, string password)
    {
      this.Id = new Guid();
      this.Nickname = "";
      this.Email = email;
      this.Password = password;
      this.Status = StatusEnum.ACTIVE;
      this.Created_at = DateTime.Now;
      this.Updated_at = DateTime.Now;
    }

  }
}
