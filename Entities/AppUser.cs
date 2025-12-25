using System.ComponentModel.DataAnnotations;
// using API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Entities;

public class AppUser
{
    [Key]
    public int Id { get; set; }
    public required string DisplayName { get; set; }
    public required string Email { get; set; }
    public  string ?ImageUrl { get; set; }
    public required byte[] PasswordHash {get; set;}
    public required byte[] PasswordSalt {get; set;}
    // Nav Property
    public Member Member {get; set;} = null!;

    
    // internal ActionResult<UserDto> ToDto()
    // {
    //     throw new NotImplementedException();
    // }
}
