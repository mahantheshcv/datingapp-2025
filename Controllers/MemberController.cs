using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")] // localhost:5001/api/members
    [ApiController]
    public class MemberController(AppDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AppUser>>> GetMembres()
        {
            var membres = await context.Users.ToListAsync();

            return membres;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetMember(int id)
        {
            var member = await context.Users.FindAsync(id);

            if(member == null) return NotFound();

            return member;
        }
    }
}
