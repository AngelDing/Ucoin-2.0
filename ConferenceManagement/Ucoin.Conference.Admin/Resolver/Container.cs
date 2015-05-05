using Microsoft.Practices.Unity;
using Ucoin.Conference.EfData;
using Ucoin.Conference.Repositories;
using Ucoin.Conference.Services;
using Ucoin.Framework.Messaging;

namespace Ucoin.Conference.Admin.Resolver
{
    public static class Container
    {
        public static UnityContainer InitUnityContainer()
        {
            var container = new UnityContainer();
            RegisterInstance(container);

            container.RegisterType<IConferenceRepositoryContext, ConferenceRepositoryContext>();
            container.RegisterType<IEventBus, EventBus>();
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

        private static void RegisterInstance(UnityContainer container)
        {
            //container.RegisterInstance<ITextSerializer>(new JsonTextSerializer());
            //EventBus = new EventBus(new MessageSender("SqlBus", "SqlBus.Events"), serializer);

            //container.RegisterInstance<IMetadataProvider>(new StandardMetadataProvider());
        }
    }
}