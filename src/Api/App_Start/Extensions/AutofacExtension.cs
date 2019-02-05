using Autofac;
using Autofac.Integration.WebApi;
using System.Linq;
using System.Reflection;

namespace Api.App_Start.Extensions
{
    public static class AutofacExtension
    {
        #region [ Extensions Methods ]

        public static void RegisterDependencies(this ContainerBuilder container)
        {
            AutofacExtension.RegisterServices(container);
            AutofacExtension.RegisterRepositories(container);
            AutofacExtension.RegisterApiControllers(container);
        }

        #endregion

        #region [ Private Methods ]

        private static void RegisterApiControllers(this ContainerBuilder container)
        {
            container.RegisterApiControllers(Assembly.GetExecutingAssembly());
        }

        private static void RegisterServices(ContainerBuilder container)
        {
            var list = container.RegisterAssemblyTypes(Assembly.Load("Service"));
            list = list.Where(t => t.Name.EndsWith("Service"));

            list.AsImplementedInterfaces().AsSelf().InstancePerLifetimeScope();
        }

        private static void RegisterRepositories(ContainerBuilder container)
        {
            var list = container.RegisterAssemblyTypes(Assembly.Load("Repository"));
            list = list.Where(t => t.Name.EndsWith("Repository"));

            list.AsImplementedInterfaces().AsSelf().InstancePerLifetimeScope();
        }


        #endregion
    }
}