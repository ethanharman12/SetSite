using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Owin;
using Ninject;
using Owin;
using SetSite.Models;
using SetSite.Repositories;

[assembly: OwinStartupAttribute(typeof(SetSite.Startup))]
namespace SetSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var kernel = new StandardKernel();
            var resolver = new NinjectSignalRDependencyResolver(kernel);

            //kernel.Bind<ApplicationDbContext>().ToSelf();
            //kernel.Bind(typeof(UserManager<ApplicationUser, int>))
            //      .To(typeof(ApplicationUserManager))
            //      .InSingletonScope();
            //kernel.Bind(typeof(UserManager<>))
            //      .ToSelf()
            //      .InSingletonScope();
            //kernel.Bind<IUserStore<ApplicationUser, int>>()
            //      .To<UserStore<ApplicationUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>>();

            //kernel.Bind<UserManager<ApplicationUser, int>>()
            //      .To<ApplicationUserManager>();

            kernel.Bind<ISetRepository>()
                  .To<SetRepository>()
                  .InSingletonScope();  // Make it a singleton object.

            //kernel.Bind<MultiplayerGameHub>()
            //      .To<MultiplayerGameHub>();

            //kernel.Bind(typeof(IHubConnectionContext<dynamic>)).ToMethod(context =>
            //        resolver.Resolve<IConnectionManager>().GetHubContext<MultiplayerGameHub>().Clients
            //         ).WhenInjectedInto(typeof(ISetRepository), typeof(UserManager<ApplicationUser,int>));

            //GlobalHost.DependencyResolver.Register(
            //typeof(MultiplayerGameHub),
            //() => new MultiplayerGameHub(new SetRepository()));

            var config = new HubConfiguration();
            config.Resolver = resolver;
            //GlobalHost.DependencyResolver = resolver;

            ConfigureAuth(app);            

            app.MapSignalR(config);
        }
    }
}
