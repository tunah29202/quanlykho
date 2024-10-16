using AutoMapper;
using Services.Core.Contracts;
using Database.Entities;
using Common;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserRequest, User>();
        CreateMap<User, UserResponse>();
        CreateMap<PagedList<User>, PagedList<UserResponse>>();

        CreateMap<ResourceRequest, Resource>();
        CreateMap<Resource, ResourceResponse>();
        CreateMap<PagedList<Resource>, PagedList<ResourceResponse>>();

    }
}