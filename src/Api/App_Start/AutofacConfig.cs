using System;
using System.Reflection;
using System.Web.Http;
using Api.App_Start.Extensions;
using Autofac;
using Autofac.Integration.WebApi;

namespace Api.App_Start
{
    public static class AutofacConfig
    {
        #region Private Read-Only Fields

        private static IContainer container;
        private static ContainerBuilder builder;

        #endregion

        #region Public Static Methods

        public static void Register()
        {
            AutofacConfig.builder = new ContainerBuilder();
            AutofacConfig.builder.RegisterDependencies();
            AutofacConfig.builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            AutofacConfig.container = AutofacConfig.builder.Build();

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        public static TObject Resolve<TObject>()
        {
            return AutofacConfig.container.Resolve<TObject>();
        }

        public static object Resolve(Type type)
        {
            if (AutofacConfig.container.IsRegistered(type))
                return AutofacConfig.container.Resolve(type);

            return null;
        }

        #endregion
    }
}