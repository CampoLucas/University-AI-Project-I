using System.Collections.Generic;
using System.Linq;


    public class Pool<T> where T: new()
    {
        private List<T> used;
        private List<T> unused;

        public List<T> Used { get => used; }
        public List<T> UnUsed { get => unused; }

        public Pool()
        {
            used = new List<T>();
            unused = new List<T>();
        }
        public T Get()
        {
            T t;
            if (unused.Count > 0)
            {
                t = unused.First();
                unused.Remove(t);
            }
            else
            {
                t = new T();
            }
            used.Add(t);
            return t;
        }
        public void Recicle (T t)
        {
            used.Remove(t);
            unused.Add(t);
        }
        public void Add(T t)
        {
            used.Add(t);
        }
    }