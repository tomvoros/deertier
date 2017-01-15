using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Mvc;
using DeerTier.Web.Data;
using DeerTier.Web.Services;
using System.Web.Mvc;

namespace DeerTier.Web
{
    public class IoCConfig
    {
        private static IWindsorContainer _container;

        public static void RegisterComponents()
        {
            _container = new WindsorContainer();

            // Register ASP.NET MVC controller factory
            var controllerFactory = new WindsorControllerFactory(_container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);

            // Register controllers
            _container.Register(Classes.FromThisAssembly().BasedOn<IController>().LifestyleTransient());

            // Register repositories
            _container.Register(Component.For<IDbConnectionProvider>().ImplementedBy<DbConnectionProvider>().LifestyleSingleton());
            _container.Register(Component.For<LeaderboardRepository>().LifestyleSingleton());
            _container.Register(Component.For<AccountRepository>().LifestyleSingleton());
            _container.Register(Component.For<ModerationRepository>().LifestyleSingleton());

            // Register services
            _container.Register(Component.For<CategoryService>().LifestyleSingleton());
            _container.Register(Component.For<AccountService>().LifestyleSingleton());
            _container.Register(Component.For<LeaderboardService>().LifestyleSingleton());
            _container.Register(Component.For<ModerationService>().LifestyleSingleton());
        } 

        public static void Dispose()
        {
            _container.Dispose();
        }
    }
}