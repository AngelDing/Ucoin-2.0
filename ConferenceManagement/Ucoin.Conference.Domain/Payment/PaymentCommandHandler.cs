namespace Ucoin.Payments.Domain
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using Ucoin.Conference.Contracts.Commands.Payments;
    using Ucoin.Conference.Entities.Payments;
    using Ucoin.Conference.Repositories;
    using Ucoin.Framework.Messaging.Handling;
    using Ucoin.Framework.Repositories;

    public class PaymentCommandHandler :
        ICommandHandler<InitiateThirdPartyProcessorPayment>,
        ICommandHandler<CompleteThirdPartyProcessorPayment>,
        ICommandHandler<CancelThirdPartyProcessorPayment>
    {
        private IPaymentRepository repository;
        private IRepositoryContext context;

        public PaymentCommandHandler(IRepositoryContext context, IPaymentRepository paymentRepository)
        {
            this.context = context;
            this.repository = paymentRepository;
        }

        public void Handle(InitiateThirdPartyProcessorPayment command)
        {
            var items = command.Items.Select(t => new PaymentItem(t.Description, t.Amount)).ToList();
            var processor = new PaymentProcessor(command.PaymentId, command.PaymentSourceId, command.Description, command.TotalAmount, items);

            context.RegisterNew(processor.Payment);
            context.Commit();
        }

        public void Handle(CompleteThirdPartyProcessorPayment command)
        {
            var payment = repository.GetByKey(command.PaymentId);

            if (payment != null)
            {
                var processor = new PaymentProcessor(payment);
                processor.Complete();
                context.RegisterModified(processor.Payment);
                context.Commit();
            }
            else
            {
                Trace.TraceError("Failed to locate the payment entity with id {0} for the completed third party payment.", command.PaymentId);
            }
        }

        public void Handle(CancelThirdPartyProcessorPayment command)
        {
            var payment = repository.GetByKey(command.PaymentId);

            if (payment != null)
            {
                var processor = new PaymentProcessor(payment);
                processor.Cancel();
                context.RegisterModified(processor.Payment);
                context.Commit();
            }
            else
            {
                Trace.TraceError("Failed to locate the payment entity with id {0} for the cancelled third party payment.", command.PaymentId);
            }
        }
    }
}
