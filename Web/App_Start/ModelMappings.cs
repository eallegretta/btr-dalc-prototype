using Autofac;
using AutoMapper;
using Web.Backend.DomainModel.Entities;
using Web.Models;

namespace Web.App_Start
{
    public class ModelMappings: IStartable
    {
        private readonly IConfiguration _mapper;

        public ModelMappings(IConfiguration mapper)
        {
            _mapper = mapper;
        }

        public void Start()
        {
            _mapper.CreateMap<GenreEntity, CategoryModel>()
                 .ForMember(destination => destination.Id, option => option.MapFrom(source => source.GenreId))
                 .ForMember(destination => destination.Description, option => option.MapFrom(source => source.GenreDescription))
                 .ForMember(destination => destination.ChildCategories, option => option.MapFrom(source => source.ChildGenres));
        }
    }
}