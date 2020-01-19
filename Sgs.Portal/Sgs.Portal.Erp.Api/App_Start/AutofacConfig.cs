using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using SAP.Middleware.Connector;
using Sgs.Portal.Erp.Api.Managers.SAP;
using Sgs.Portal.Erp.Api.Services;
using System.Reflection;
using System.Web.Http;

namespace Sgs.Portal.Erp.Api
{
    public class AutofacConfig
    {
        public static void Register()
        {
            var bldr = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;
            bldr.RegisterApiControllers(Assembly.GetExecutingAssembly());
            RegisterServices(bldr);
            bldr.RegisterWebApiFilterProvider(config);
            bldr.RegisterWebApiModelBinderProvider();
            var container = bldr.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static void RegisterServices(ContainerBuilder bldr)
        {
            //Auto Mapping
            var mappingConfigs = new MapperConfiguration(cfg =>
            {
                //Add profiles here
                //cfg.AddProfile(new CampMaooingProfile());
            });

            bldr.RegisterInstance(mappingConfigs.CreateMapper())
                .As<IMapper>()
                .SingleInstance();

            //Sap Services

            bldr.RegisterType<SapConfiguration>()
                .As<ISapConfiguration>()
                .WithParameter("configurationType", "Default")
                .InstancePerRequest();

            bldr.RegisterType<SapDestinationConfiguration>()
                .As<IDestinationConfiguration>()
                .InstancePerRequest();

            bldr.RegisterType<SapCountriesManager>()
                .As<IErpCountriesManager>()
                .InstancePerRequest();

            //bldr.RegisterType<CampContext>()
            //  .InstancePerRequest();

            //bldr.RegisterType<CampRepository>()
            //  .As<ICampRepository>()
            //  .InstancePerRequest();
        }
    }
}