using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class MemberController(IMemberRepository  memberRepository, IPhotoService photoService) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Member>>> GetMembres()
        {
            return Ok(await memberRepository.GetMembersAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(int id)
        {
            var member = await memberRepository.GetMemberByIdAsync(id);

            if(member == null) return NotFound();

            return member;
        }

        [HttpGet("{id}/photos")]
        public async Task<ActionResult<IReadOnlyList<Photo>>> GetMemberPhotos(int id)
        {
            return Ok(await memberRepository.GetPhotosForMemberAsync(id));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateMember(MemberUpdateDto memberUpdateDto)
        {
            var memberId = User.GetMemberId();

            var member = await memberRepository.GetMemberForUpdate(memberId);

            if (member == null)
                return NotFound("Member not found");

            member.DisplayName = memberUpdateDto.DisplayName ?? member.DisplayName;
            member.Description = memberUpdateDto.Description ?? member.Description;
            member.City = memberUpdateDto.City ?? member.City;
            member.Country = memberUpdateDto.Country ?? member.Country;

            member.User.DisplayName = memberUpdateDto.DisplayName ?? member.User.DisplayName;

            // Optional if using EF Core change tracking
            memberRepository.Update(member);

            if (await memberRepository.SaveAllAsync())
                return NoContent();

            return BadRequest("Failed to update member");
        }

        [HttpPost("add-photo")]

        public async Task<ActionResult<Photo>> AddPhoto([FromForm] IFormFile file)
        {
            var member = await memberRepository.GetMemberForUpdate(User.GetMemberId());
            if(member == null) return BadRequest("Connot Update Member");

            var result = await photoService.UploadPhotoAsync(file);
            if(result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                MemberId = User.GetMemberId()
            };

            if(member.ImageUrl == null)
            {
                member.ImageUrl = photo.Url;
                member.User.ImageUrl = photo.Url;
            };

            member.Photos.Add(photo);

            if(await memberRepository.SaveAllAsync()) return photo;

            return BadRequest("Problem adding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<IActionResult> SetMainPhoto(int photoId)
        {
           var member = await memberRepository.GetMemberForUpdate(User.GetMemberId());
           if(member == null) return BadRequest("Connot get member from token");

           var photo = member.Photos.SingleOrDefault(x=> x.Id == photoId);

            if(member.ImageUrl == photo?.Url || photo == null)
            {
                return BadRequest("Connot set this as Main image");
            }

            member.ImageUrl = photo.Url;
            member.User.ImageUrl = photo.Url;

            if(await memberRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Problem setting main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<IActionResult> DeletePhoto(int photoId)
        {
            var member = await memberRepository.GetMemberForUpdate(User.GetMemberId());
            if(member == null) return BadRequest("Connot get member from token");

            var photo = member.Photos.SingleOrDefault(x=> x.Id == photoId);

            if(photo == null || photo.Url == member.ImageUrl)
            {
                return BadRequest("This photo cannot be deleted");
            }

            member.Photos.Remove(photo);
            if(await memberRepository.SaveAllAsync()) return Ok();

            return BadRequest("Problem deleting the photo");
        }
    }
}
