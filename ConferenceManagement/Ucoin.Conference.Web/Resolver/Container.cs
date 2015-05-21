using Microsoft.Practices.Unity;
using Ucoin.Conference.EfData;
using Ucoin.Conference.Repositories;
using Ucoin.Conference.Services;
using Ucoin.Framework.Cache;
using Ucoin.Framework.Messaging;
using Ucoin.Framework.Metadata;
using Ucoin.Framework.Serialization;
using Ucoin.Framework.SqlDb.Messaging;
using Ucoin.Framework.SqlDb.Messaging.Implementation;

namespace Ucoin.Conference.Web.Resolver
{
    public static class Container
    {
        public static UnityContainer InitUnityContainer()
        {
            var container = new UnityContainer();

            var serializer = new JsonTextSerializer();
            container.RegisterInstance<ITextSerializer>(serializer);

            container.RegisterInstance<ICacheManager>(
                new CacheManager<StaticCache>(t => { return new StaticCache(); })
            );

            container.RegisterType<IMessageSender, MessageSender>(
                "Commands",
                new TransientLifetimeManager(),
                new InjectionConstructor("SqlBus", "SqlBus.Commands")
            );

            container.RegisterType<ICommandBus, CommandBus>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(new ResolvedParameter<IMessageSender>("Commands"), serializer)
            );

            //container.RegisterType<IMessageSender, MessageSender>(
            //   "Events",
            //   new TransientLifetimeManager(),
            //   new InjectionConstructor("SqlBus", "SqlBus.Events")
            //);      

            //container.RegisterType<IEventBus, EventBus>(
            //    new ContainerControlledLifetimeManager(),
            //    new InjectionConstructor(new ResolvedParameter<IMessageSender>("Events"), typeof(ITextSerializer))
            //);

            container.RegisterType<IConferenceViewService, ConferenceViewService>(
                new ContainerControlledLifetimeManager()
            );
            container.RegisterType<IOrderViewService, OrderViewService>(
                new ContainerControlledLifetimeManager()
            );           

            return container;
        }
    }
}