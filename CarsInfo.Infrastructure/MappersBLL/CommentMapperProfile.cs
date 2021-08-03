using AutoMapper;
using CarsInfo.BLL.Models.Dtos;
using CarsInfo.DAL.Entities;

namespace CarsInfo.Infrastructure.MappersBLL
{
    public class CommentMapperProfile : Profile
    {
        public CommentMapperProfile()
        {
            CreateMap<Comment, CommentDto>()
                .ReverseMap();
        }
    }
}
