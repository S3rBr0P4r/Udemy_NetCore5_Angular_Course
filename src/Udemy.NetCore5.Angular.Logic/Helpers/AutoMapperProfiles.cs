using System.Linq;
using AutoMapper;
using Udemy.NetCore5.Angular.Data.Entities;
using Udemy.NetCore5.Angular.Logic.DTOs;
using Udemy.NetCore5.Angular.Logic.Extensions;

namespace Udemy.NetCore5.Angular.Logic.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, AppUserResponse>()
                .ForMember(dest => dest.PhotoUrl, opts => opts.MapFrom(src => src.Photos.FirstOrDefault(p => p.Enabled).Url))
                .ForMember(dest => dest.Age, opts => opts.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, AppUserPhotosResponse>();
            CreateMap<AppUserEditRequest, AppUser>();
            CreateMap<RegisterUserRequest, AppUser>();
        }
    }
}