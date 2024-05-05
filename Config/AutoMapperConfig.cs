using AutoMapper;
using cush.DTO;
using cush.Models;

namespace cush.Config;

using AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Cush, CushDTO>();
    }
}