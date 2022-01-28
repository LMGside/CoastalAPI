using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Web.Http;
using System.Reflection;
using ContourApiAuthentication;
using CoastalAPIDataLayer;
using CoastalAPIBusinessLayer;

[assembly: OwinStartup(typeof(CoastalAPI.Startup))]

namespace CoastalAPI
{
    public class Startup
    {
        //IAppBuilder object uses Owin. ContainBuilder uses Autofac
        public virtual void RegisterDependencies(IAppBuilder app, ContainerBuilder builder)
        {
            builder.Register(c => new CoastalAPISettings()).As<ICoastalAPISettings>().SingleInstance();
            builder.RegisterType<CoastalAPIBL>().UsingConstructor(typeof(ICoastalAPISettings)).As<CoastalAPIBL>().SingleInstance();

            // Initialising API Authentication
            builder.Register(c => new AuthApiInitialize()).As<IAuthApiInitialise>().SingleInstance();
            builder.RegisterType<AuthorizationServerProvider>().As<AuthorizationServerProvider>().SingleInstance();
        }

        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            /* Enable for additional test troubleshooting
            config.EnableSystemDiagnosticsTracing();
            SystemDiagnosticsTraceWriter traceWriter = config.EnableSystemDiagnosticsTracing();
            traceWriter.IsVerbose = false;
            traceWriter.MinimumLevel = (System.Web.Http.Tracing.TraceLevel)TraceLevel.Verbose;
            */

            var builder = new ContainerBuilder();

            // Register Web API controller in executing assembly.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            this.RegisterDependencies(app, builder);
            builder.RegisterWebApiFilterProvider(config);
            // Create and assign a dependency resolver for Web API to use.
            IContainer container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // Register the Autofac middleware FIRST. This also adds
            // Autofac-injected middleware registered with the container.
            app.UseAutofacMiddleware(container);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            //The following 2 lines are needed for Initialising Api Authentication
            var authApiInitialize = container.Resolve<IAuthApiInitialise>();
            var vendSettings = container.Resolve<ICoastalAPISettings>();
            authApiInitialize.InitialiseAuthTable(vendSettings.GetCoastalAPIDBConnectionString());

            var authProvider = container.Resolve<AuthorizationServerProvider>();

            var options = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(2),
                Provider = authProvider
            };

            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Make sure the Autofac lifetime scope is passed to Web API.
            app.UseAutofacWebApi(config);

            app.UseWebApi(config);
        }
    }
}