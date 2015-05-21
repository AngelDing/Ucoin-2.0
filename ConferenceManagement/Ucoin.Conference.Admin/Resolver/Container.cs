using Microsoft.Practices.Unity;
using Ucoin.Conference.EfData;
using Ucoin.Conference.Repositories;
using Ucoin.Conference.Services;
using Ucoin.Framework.Messaging;
using Ucoin.Framework.Metadata;
using Ucoin.Framework.Serialization;
using Ucoin.Framework.SqlDb.Messaging;
using Ucoin.Framework.SqlDb.Messaging.Implementation;

namespace Ucoin.Conference.Admin.Resolver
{
    public static class Container
    {
        public static UnityContainer InitUnityContainer()
        {
            var container = new UnityContainer();
            var serializer = new JsonTextSerializer();
            container.RegisterInstance<ITextSerializer>(serializer);
            container.RegisterInstance<IMetadataProvider>(new StandardMetadataProvider());

            //container.RegisterType<IMessageSender, MessageSender>(
            //    "Commands",
            //    new TransientLifetimeManager(),
            //    new InjectionConstructor("SqlBus", "SqlBus.Commands")
            //);

            container.RegisterType<IMessageSender, MessageSender>(
               "Events",
               new TransientLifetimeManager(),
               new InjectionConstructor("SqlBus", "SqlBus.Events")
            );

            //container.RegisterType<ICommandBus, CommandBus>(
            //    new ContainerControlledLifetimeManager(),
            //    new InjectionConstructor(new ResolvedParameter<IMessageSender>("Commands"), serializer)
            //);


            container.RegisterType<IEventBus, EventBus>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(new ResolvedParameter<IMessageSender>("Events"), typeof(ITextSerializer))
            );

            container.RegisterType<IConferenceRepositoryContext, ConferenceRepositoryContext>();
            container.RegisterType<IConferenceRepository, ConferenceRepository>(
                new TransientLifetimeManager(),
                new InjectionConstructor(typeof(IConferenceRepositoryContext)));

            container.RegisterType<IOrderRepository, OrderRepository>(
                new TransientLifetimeManager(),
                new InjectionConstructor(typeof(IConferenceRepositoryContext)));
            container.RegisterType<ISeatTypeRepository, SeatTypeRepository>(
                new TransientLifetimeManager(),
                new InjectionConstructor(typeof(IConferenceRepositoryContext)));

            var injectionConstructor = new InjectionConstructor(
                typeof(IEventBus),
                typeof(IConferenceRepositoryContext),
                typeof(IConferenceRepository),
                typeof(IOrderRepository),
                typeof(ISeatTypeRepository)
            );

            container.RegisterType<IConferenceService, ConferenceService>(
                new HierarchicalLifetimeManager(),
                injectionConstructor);

            return container;
        }
    }
}