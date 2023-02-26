using System;

namespace ClickerLogic
{
    public class ReactiveProperty<T> where T : IEquatable<T>
    {
        public Action<T> Changed;

        private T _innerValue;

        public T Value
        {
            get { return _innerValue; }
            set
            {
                if (_innerValue.Equals(value)) return;

                _innerValue = value;
                Changed?.Invoke(_innerValue);
            }
        }

        public ReactiveProperty()
        { }

        public ReactiveProperty(T innerValue)
        {
            _innerValue = innerValue;
        }
    }
}
