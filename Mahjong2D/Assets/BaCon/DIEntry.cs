using System;

namespace BaCon
{
    public abstract class DIEntry
    {
        protected DIContainer Container { get; }
        protected bool IsSingleton { get; set; }

        protected DIEntry() { }
        
        protected DIEntry(DIContainer container)
        {
            Container = container;
        }

        public T Resolve<T>()
        {
            return ((DIEntry<T>)this).Resolve();
        }

        public DIEntry AsSingle()
        {
            IsSingleton = true;

            return this;
        }
    }
    
    public class DIEntry<T> : DIEntry
    {
        private Func<DIContainer, T> Factory { get; }
        private T _instance;
        private bool _hasInstance;
        
        public DIEntry(DIContainer container, Func<DIContainer, T> factory) : base(container)
        {
            Factory = factory;
        }

        public DIEntry(T value)
        {
            _instance = value;
            _hasInstance = true;
            IsSingleton = true;
        }

        public T Resolve()
        {
            if (IsSingleton)
            {
                if (!_hasInstance)
                {
                    _instance = Factory(Container);
                    _hasInstance = true;
                }

                return _instance;
            }

            return Factory(Container);
        }
    }
}
