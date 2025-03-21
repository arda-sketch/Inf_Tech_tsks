using System.Collections.Generic;

namespace LinearListApp.Models
{
    public class LinearList<T>
    {
        private List<T> _items;
        private int _currentIndex;

        public LinearList()
        {
            _items = new List<T>();
            _currentIndex = -1;
        }

        public T? CurrentElement
        {
            get => _currentIndex >= 0 && _currentIndex < _items.Count ? _items[_currentIndex] : default;
        }

        public int Count
        {
            get => _items.Count;
        }

        public bool IsEmpty
        {
            get => _items.Count == 0;
        }

        public IReadOnlyList<T> Items
        {
            get => _items.AsReadOnly();
        }

        public void Add(T item)
        {
            _items.Add(item);
            if (_currentIndex == -1 && _items.Count > 0)
            {
                _currentIndex = 0;
            }
        }

        public void Remove(T item)
        {
            int index = _items.IndexOf(item);
            if (index != -1)
            {
                _items.RemoveAt(index);
                if (_items.Count == 0)
                {
                    _currentIndex = -1;
                }
                else if (index <= _currentIndex)
                {
                    _currentIndex = _currentIndex > 0 ? _currentIndex - 1 : 0;
                }
            }
        }

        public bool MoveNext()
        {
            if (_currentIndex < _items.Count - 1)
            {
                _currentIndex++;
                return true;
            }
            return false;
        }

        public void MoveToStart()
        {
            _currentIndex = _items.Count > 0 ? 0 : -1;
        }
    }
}