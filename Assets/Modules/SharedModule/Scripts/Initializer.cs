using System;

namespace Modules.SharedModule.Scripts
{ 
    public class Initializer // Сделал для случая, если надо будет обратиться к другому компоненту в течение кадра
    {
        public bool Initialized { get; private set; }
        
        private readonly Action _initializationAction;

        public Initializer(Action initializationAction)
        {
            _initializationAction = initializationAction;
        }
        
        public bool TryInitialize()
        {
            if (Initialized)
            {
                return false;
            }

            _initializationAction?.Invoke();
            Initialized = true;
            return true;
        }
        
    }
}