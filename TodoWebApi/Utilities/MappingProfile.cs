using AutoMapper;
using Entity;
using TodoWebApi.DtoModels;

namespace TodoWebApi.Utilities
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TodoDbModel, TodoDtoModel>().ReverseMap();
        }
    }
}
