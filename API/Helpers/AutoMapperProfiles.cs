namespace API.Helpers;

using System.Globalization;
using API.DataEntities;
using API.DTOs;
using API.Extensions;
using AutoMapper;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, MemberResponse>()
            .ForMember(d => d.Age,
                o => o.MapFrom(s => s.BirthDay.CalculateAge()))
            .ForMember(d => d.PhotoUrl,
                o => o.MapFrom(s => s.Photos.FirstOrDefault(p => p.IsMain)!.Url));
        CreateMap<Photo, PhotoResponse>();
        CreateMap<MemberUpdateRequest, AppUser>();
        CreateMap<RegisterRequest, AppUser>();
        CreateMap<string, DateOnly>().ConvertUsing(s => DateOnly.Parse(s, CultureInfo.InvariantCulture));
    }
}