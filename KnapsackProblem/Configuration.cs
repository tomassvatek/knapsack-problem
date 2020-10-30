using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace KnapsackProblem
{
    public class Configuration: IEquatable<Configuration>
    {
        public Configuration(int index, int price)
        {
            Index = index;
            Price = price;
        }

        public int Index { get; set; }
        public int Price { get; set; }

        public bool Equals([AllowNull] Configuration other)
        {
            if (Index == other.Index)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return Index;
        }

        public override string ToString()
        {
            return $"{Index} {Price}";
        }
    }

    public class HashSetCustom<T> : ICollection<T>
    {
        private readonly HashSet<T> _hashSet;

        public HashSetCustom()
        {
            _hashSet = new HashSet<T>();
        }

        public int Count => _hashSet.Count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            if (_hashSet.Contains(item))
            {
                _hashSet.Remove(item);
                _hashSet.Add(item);
            }
            else
            {
                _hashSet.Add(item);
            }
        }

        public void Clear()
            => _hashSet.Clear();

        public bool Contains(T item)
            => _hashSet.Contains(item);

        public void CopyTo(T[] array, int arrayIndex)
            => _hashSet.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator()
            => _hashSet.GetEnumerator();

        public bool Remove(T item)
            => _hashSet.Remove(item);

        IEnumerator IEnumerable.GetEnumerator()
            => _hashSet.GetEnumerator();
    }
}
