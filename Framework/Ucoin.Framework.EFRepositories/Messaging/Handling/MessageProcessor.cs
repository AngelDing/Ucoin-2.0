
namespace Ucoin.Framework.SqlDb.Messaging.Handling
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using Ucoin.Framework.Processes;
    using Ucoin.Framework.Serialization;

    /// <summary>
    /// Provides basic common processing code for components that handle 
    /// incoming messages from a receiver.
    /// </summary>
    public abstract class MessageProcessor : DisposableObject, IProcessor
    {
        private readonly IMessageReceiver receiver;
        private readonly ISerializer serializer;
        private readonly object lockObject = new object();
        private bool disposed;
        private bool started = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessor"/> class.
        /// </summary>
        protected MessageProcessor(IMessageReceiver receiver, ISerializer serializer)
        {
            this.receiver = receiver;
            this.serializer = serializer;
        }

        /// <summary>
        /// Starts the listener.
        /// </summary>
        public virtual void Start()
        {
            ThrowIfDisposed();
            lock (this.lockObject)
            {
                if (!this.started)
                {
                    this.receiver.MessageReceived += OnMessageReceived;
                    this.receiver.Start();
                    this.started = true;
                }
            }
        }

        /// <summary>
        /// Stops the listener.
        /// </summary>
        public virtual void Stop()
        {
            lock (this.lockObject)
            {
                if (this.started)
                {
                    this.receiver.Stop();
                    this.receiver.MessageReceived -= OnMessageReceived;
                    this.started = false;
                }
            }
        }


        protected abstract void ProcessMessage(object payload, string correlationId);

        private void OnMessageReceived(object sender, MessageReceivedEventArgs args)
        {
            Trace.WriteLine(new string('-', 100));

            try
            {
                var body = Deserialize(args.Message.Body);

                TracePayload(body);
                Trace.WriteLine("");

                ProcessMessage(body, args.Message.CorrelationId);

                Trace.WriteLine(new string('-', 100));
            }
            catch (Exception e)
            {
                // NOTE: we catch ANY exceptions as this is for local 
                // development/debugging. The Windows Azure implementation 
                // supports retries and dead-lettering, which would 
                // be totally overkill for this alternative debug-only implementation.
                Trace.TraceError("An exception happened while processing message through handler/s:\r\n{0}", e);
                Trace.TraceWarning("Error will be ignored and message receiving will continue.");
            }
        }

        protected object Deserialize(string serializedPayload)
        {
            return this.serializer.Deserialize(serializedPayload, typeof(object));
        }

        protected string Serialize(object payload)
        {
            return this.serializer.SerializeToString(payload);
        }

        private void ThrowIfDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("MessageProcessor");
            }
        }


        [Conditional("TRACE")]
        private void TracePayload(object payload)
        {
            Trace.WriteLine(this.Serialize(payload));
        }

        protected override void OnDispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.Stop();
                    this.disposed = true;

                    using (this.receiver as IDisposable)
                    {
                        // Dispose receiver if it's disposable.
                    }
                }
            }
        }
    }
}
