using System.Collections.Generic;
using System.Threading;

namespace Sudoku_Solver
{
    class LockerList<T>
    {
        private static readonly List<T> list = new List<T>();
        private static readonly object locker = new object();

        public bool Acquire(T t)
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

        public bool Release(T t)
        {
            lock (locker)
            {
                bool removed = list.Remove(t);
                Monitor.PulseAll(locker);
                return removed;
            }
        }

        public void WaitingAcquire(T t)
        {
            while (!Acquire(t))
            {
                lock (locker)
                {
                    Monitor.Wait(locker);
                }
            }
        }
    }
}
