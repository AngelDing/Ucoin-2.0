using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using Ucoin.Framework.Processes;
using Ucoin.Framework.Serialization;
using Ucoin.Framework.Metadata;
using Ucoin.Conference.Domain;
using Ucoin.Framework.SqlDb.Processes;
using System.Data.Entity;
using Ucoin.Framework.Messaging;
using Ucoin.Conference.EfData;
using Ucoin.Conference.Services;
using Ucoin.Conference.Domain.Services;
using Ucoin.Conference.Repositories;
using Ucoin.Conference.IRepositories;
using Ucoin.Framework.Messaging.Handling;
using Ucoin.Conference.Domain.Handlers;
using Ucoin.Framework.SqlDb.Messaging.Handling;
using Ucoin.Framework.SqlDb.Messaging;
using Ucoin.Framework.SqlDb.Messaging.Implementation;
using Ucoin.Framework.SqlDb.MessageLog;
using Ucoin.Framework.SqlDb.EventSourcing;
using Ucoin.Framework.EventSourcing;

namespace Ucoin.Conference.Processor
{
    public sealed partial class ConferenceProcessor : IDisposable
    {
        private IUnityContainer container;
        private CancellationTokenSource cancellationTokenSource;
        private List<IProcessor> processors;
        private bool instrumentationEnabled;

        public ConferenceProcessor(bool instrumentationEnabled = false)
        {
            this.instrumentationEnabled = instrumentationEnabled;

            this.cancellationTokenSource = new CancellationTokenSource();
            this.container = CreateContainer();

            this.processors = this.container.ResolveAll<IProcessor>().ToList();
        }

        public void Start()
        {
            this.processors.ForEach(p => p.Start());
        }

        public void Stop()
        {
            this.cancellationTokenSource.Cancel();

            this.processors.ForEach(p => p.Stop());
        }

        public void Dispose()
        {
            this.container.Dispose();
            this.cancellationTokenSource.Dispose();
        }

        private UnityContainer CreateContainer()
        {
            var container = new UnityContainer();

            // infrastructure
            container.RegisterInstance<ITextSerializer>(new JsonTextSerializer());
            container.RegisterInstance<IMetadataProvider>(new StandardMetadataProvider());

            container.RegisterType<DbContext, ConferenceContext>(new TransientLifetimeManager());
            container.RegisterType<IProcessManagerDataContext<RegistrationProcessManager>, SqlProcessManagerDataContext<RegistrationProcessManager>>(
                new TransientLifetimeManager(),
                new InjectionConstructor(new ResolvedParameter<Func<DbContext>>(), typeof(ICommandBus), typeof(ITextSerializer)));

            container.RegisterType<IConferenceViewService, ConferenceViewService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IOrderViewService, OrderViewService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISeatTypeViewRepository, SeatTypeViewRepository>();
            container.RegisterType<IPricingService, PricingService>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(typeof(ISeatTypeViewRepository))
            );

            container.RegisterType<IConferenceRepositoryContext, ConferenceRepositoryContext>();
            container.RegisterType<IRegistrationProcessRepository, RegistrationProcessRepository>(
                new TransientLifetimeManager(),
                new InjectionConstructor(typeof(IConferenceRepositoryContext)));
            container.RegisterType<ISeatTypeRepository, SeatTypeRepository>(
                new TransientLifetimeManager(),
                new InjectionConstructor(typeof(IConferenceRepositoryContext)));
            container.RegisterType<IOrderRepository, OrderRepository>(
                 new TransientLifetimeManager(),
                 new InjectionConstructor(typeof(IConferenceRepositoryContext)));

            // handlers
            container.RegisterType<ICommandHandler, RegistrationProcessManagerRouter>(
                new InjectionConstructor(typeof(IConferenceRepositoryContext), typeof(IRegistrationProcessRepository))
            );
            container.RegisterType<ICommandHandler, OrderCommandHandler>("OrderCommandHandler");
            container.RegisterType<ICommandHandler, SeatsAvailabilityHandler>("SeatsAvailabilityHandler");
            //container.RegisterType<ICommandHandler, ThirdPartyProcessorPaymentCommandHandler>("ThirdPartyProcessorPaymentCommandHandler");
            container.RegisterType<ICommandHandler, SeatAssignmentsHandler>("SeatAssignmentsHandler");

            // Conference management integration
            container.RegisterType<global::Ucoin.Conference.EfData.ConferenceContext>(new TransientLifetimeManager());

            OnCreateContainer(container);

            return container;
        }

        private void OnCreateContainer(UnityContainer container)
        {
            var serializer = container.Resolve<ITextSerializer>();
            var metadata = container.Resolve<IMetadataProvider>();


            var commandBus = new CommandBus(new MessageSender("SqlBus", "SqlBus.Commands"), serializer);
            var eventBus = new EventBus(new MessageSender("SqlBus", "SqlBus.Events"), serializer);

            var commandProcessor = new CommandProcessor(new MessageReceiver("SqlBus", "SqlBus.Commands"), serializer);
            var eventProcessor = new EventProcessor(new MessageReceiver("SqlBus", "SqlBus.Events"), serializer);

            container.RegisterInstance<ICommandBus>(commandBus);
            container.RegisterInstance<IEventBus>(eventBus);
            container.RegisterInstance<ICommandHandlerRegistry>(commandProcessor);
            container.RegisterInstance<IProcessor>("CommandProcessor", commandProcessor);
            container.RegisterInstance<IEventHandlerRegistry>(eventProcessor);
            container.RegisterInstance<IProcessor>("EventProcessor", eventProcessor);

            // Event log database and handler.
            container.RegisterType<SqlMessageLog>(new InjectionConstructor(serializer, metadata));
            container.RegisterType<IEventHandler, SqlMessageLogHandler>("SqlMessageLogHandler");
            container.RegisterType<ICommandHandler, SqlMessageLogHandler>("SqlMessageLogHandler");

            RegisterRepository(container);
            RegisterEventHandlers(container, eventProcessor);
            RegisterCommandHandlers(container);
        }

        private void RegisterEventHandlers(UnityContainer container, EventProcessor eventProcessor)
        {
            eventProcessor.Register(container.Resolve<RegistrationProcessManagerRouter>());
            eventProcessor.Register(container.Resolve<DraftOrderViewModelGenerator>());
            eventProcessor.Register(container.Resolve<PricedOrderViewModelGenerator>());
            eventProcessor.Register(container.Resolve<ConferenceViewModelGenerator>());
            eventProcessor.Register(container.Resolve<SeatAssignmentsViewModelGenerator>());
            eventProcessor.Register(container.Resolve<SeatAssignmentsHandler>());
            eventProcessor.Register(container.Resolve<OrderEventHandler>());
            eventProcessor.Register(container.Resolve<SqlMessageLogHandler>());
        }

        private void RegisterRepository(UnityContainer container)
        {
            // repository
            container.RegisterType<EventStoreDbContext>(new TransientLifetimeManager());
            container.RegisterType(typeof(IEventSourcedRepository<>), typeof(SqlEventSourcedRepository<>), new ContainerControlledLifetimeManager());
        }

        private static void RegisterCommandHandlers(IUnityContainer unityContainer)
        {
            var commandHandlerRegistry = unityContainer.Resolve<ICommandHandlerRegistry>();

            foreach (var commandHandler in unityContainer.ResolveAll<ICommandHandler>())
            {
                commandHandlerRegistry.Register(commandHandler);
            }
        }
    }
}
