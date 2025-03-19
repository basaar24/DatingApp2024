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
        CreateMap<Message, MessageResponse>()
            .ForMember(d => d.SenderPhotoUrl,
                o => o.MapFrom(s => s.Sender.Photos.FirstOrDefault(p => p.IsMain)!.Url))
            .ForMember(d => d.RecipientPhotoUrl,
                o => o.MapFrom(s => s.Recipient.Photos.FirstOrDefault(p => p.IsMain)!.Url));
    }
}