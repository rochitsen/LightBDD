using System;
using System.Diagnostics;
using LightBDD.Core.Extensibility;

namespace LightBDD.Core.Execution.Implementation
{
    [DebuggerStepThrough]
    internal class ContextProvider : IContextProvider
    {
        private readonly ExecutionContextDescriptor _contextDescriptor;
        private bool _disposed;
        private object _context;
        public static readonly IContextProvider NoContext = new NoContextProvider();

        public ContextProvider(ExecutionContextDescriptor contextDescriptor)
        {
            _contextDescriptor = contextDescriptor ?? throw new ArgumentNullException(nameof(contextDescriptor));
        }

        public object GetContext()
        {
            if (_disposed)
                throw new ObjectDisposedException("Context is already disposed");

            return _context ?? (_context = _contextDescriptor.ContextProvider.Invoke());
        }

        public void Dispose()
        {
            if (_disposed)
                return;
            _disposed = true;

            var disposable = _context as IDisposable;
            _context = null;

            if (_contextDescriptor.TakeOwnership && disposable != null)
                DisposeContext(disposable);
        }

        private static void DisposeContext(IDisposable disposable)
        {
            try
            {
                disposable.Dispose();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Failed to dispose context '{disposable.GetType().Name}': " + e.Message, e);
            }
        }

        [DebuggerStepThrough]
        private class NoContextProvider : IContextProvider
        {
            public void Dispose() { }
            public object GetContext() => null;
        }
    }
}