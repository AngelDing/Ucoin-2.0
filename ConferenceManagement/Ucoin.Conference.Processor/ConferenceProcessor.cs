using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using Ucoin.Framework.Processes;
using Ucoin.Framework.Serialization;
using Ucoin.Framework.Metadata;

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

            OnCreating();

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

            //// infrastructure
            container.RegisterInstance<ITextSerializer>(new JsonTextSerializer());
            container.RegisterInstance<IMetadataProvider>(new StandardMetadataProvider());

            //container.RegisterType<DbContext, RegistrationProcessManagerDbContext>("registration", new TransientLifetimeManager(), new InjectionConstructor("ConferenceRegistrationProcesses"));
            //container.RegisterType<IProcessManagerDataContext<RegistrationProcessManager>, SqlProcessManagerDataContext<RegistrationProcessManager>>(
            //    new TransientLifetimeManager(),
            //    new InjectionConstructor(new ResolvedParameter<Func<DbContext>>("registration"), typeof(ICommandBus), typeof(ITextSerializer)));

            //container.RegisterType<DbContext, PaymentsDbContext>("payments", new TransientLifetimeManager(), new InjectionConstructor("Payments"));
            //container.RegisterType<IDataContext<ThirdPartyProcessorPayment>, SqlDataContext<ThirdPartyProcessorPayment>>(
            //    new TransientLifetimeManager(),
            //    new InjectionConstructor(new ResolvedParameter<Func<DbContext>>("payments"), typeof(IEventBus)));

            //container.RegisterType<ConferenceRegistrationDbContext>(new TransientLifetimeManager(), new InjectionConstructor("ConferenceRegistration"));

            //container.RegisterType<IConferenceDao, ConferenceDao>(new ContainerControlledLifetimeManager());
            //container.RegisterType<IOrderDao, OrderDao>(new ContainerControlledLifetimeManager());

            //container.RegisterType<IPricingService, PricingService>(new ContainerControlledLifetimeManager());

            //// handlers
            //container.RegisterType<ICommandHandler, RegistrationProcessManagerRouter>("RegistrationProcessManagerRouter");
            //container.RegisterType<ICommandHandler, OrderCommandHandler>("OrderCommandHandler");
            //container.RegisterType<ICommandHandler, SeatsAvailabilityHandler>("SeatsAvailabilityHandler");
            //container.RegisterType<ICommandHandler, ThirdPartyProcessorPaymentCommandHandler>("ThirdPartyProcessorPaymentCommandHandler");
            //container.RegisterType<ICommandHandler, SeatAssignmentsHandler>("SeatAssignmentsHandler");

            //// Conference management integration
            //container.RegisterType<global::Conference.ConferenceContext>(new TransientLifetimeManager(), new InjectionConstructor("ConferenceManagement"));

            //OnCreateContainer(container);

            return container;
        }

        partial void OnCreating();
        partial void OnCreateContainer(UnityContainer container);
    }
}
