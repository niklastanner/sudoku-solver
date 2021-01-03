using System.Collections.Generic;

namespace Sudoku_Solver
{
    class LockerList<T>
    {
        private static readonly List<T> list = new List<T>();
        private static readonly object locker = new object();

        public bool Add(T t)
        {
            lock (locker)
            {
                if (!list.Contains(t))
                {
                    list.Add(t);
                    return true;
                }
                return false;
            }
        }

        public bool Remove(T t)
        {
            lock (locker)
            {
                return list.Remove(t);
            }
        }
    }
}
