using System;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IMemberRepository
{

    void Update(Member member);
    Task<bool> SaveAllAsync();
    Task<PaginatedResult<Member>> GetMembersAsync(MemberParams memberParams);
    Task<Member?> GetMemberByIdAsync(int id);
    Task<IReadOnlyList<Photo>> GetPhotosForMemberAsync(int memberId);
    Task<Member?> GetMemberForUpdate(int id);
}
