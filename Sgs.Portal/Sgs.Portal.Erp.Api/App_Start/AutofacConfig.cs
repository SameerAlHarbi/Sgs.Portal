using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
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
            var config = new MapperConfiguration(cfg =>
            {
                //Add profiles here
                //cfg.AddProfile(new CampMaooingProfile());
            });

            bldr.RegisterInstance(new SapConfiguration("Default"))
                .SingleInstance();

            bldr.RegisterInstance(config.CreateMapper())
                .As<IMapper>()
                .SingleInstance();

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