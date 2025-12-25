using System;
using System.Text.Json.Serialization;

namespace API.Entities;

public class Photo
{
    public int Id {get; set;}
    public required string  Url {get; set;}
    public string? PublicId {get; set;}

// NAvigation Property (One to many)
    [JsonIgnore]
    public Member Member {get; set;} =null!;

    public int MemberId { get; set; }
}
