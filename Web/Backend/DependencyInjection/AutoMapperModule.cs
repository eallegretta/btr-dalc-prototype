using AutoMapper;
using AutoMapper.Mappers;
using Autofac;

namespace BlogTalkRadio.Common.Data.DependencyInjection
{
    public class AutoMapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                var autoMapperConfig = new ConfigurationStore(new TypeMapFactory(), MapperRegistry.AllMappers());
                autoMapperConfig.ConstructServicesUsing(c.Resolve);
                return autoMapperConfig;
            }).As<ConfigurationStore>()
              .As<IConfigurationProvider>()
              .As<IConfiguration>()
              .SingleInstance();

            builder.RegisterType<MappingEngine>().As<IMappingEngine>().SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                    .AsClosedTypesOf(typeof(ITypeConverter<,>))
                    .AsSelf();
        }
    }
}