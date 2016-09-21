using System;
using System.Collections.Generic;

namespace PX.SurveyMonkeyReader.Models
{
    public class LastNQueue
    {
        private readonly Queue<DateTime> _queue;
        private readonly int _maxCount;

        private DateTime newest;

        public LastNQueue(int maxCount)
        {
            _maxCount = maxCount;
            _queue = new Queue<DateTime>(maxCount);
        }

        public int Count()
        {
            return _queue.Count;
        }

        public void Add(DateTime item)
        {
            if (_queue.Count == _maxCount)
            {
                 _queue.Dequeue();
            }
            newest = item;
            _queue.Enqueue(item);
        }

        public DateTime GetOldest()
        {
            return _queue.Peek();
        }

        public DateTime GetNewest()
        {
            return newest;
        }
    }
}
