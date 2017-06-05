using System;
using System.Collections.Generic;
using System.Threading;


namespace SuiBao_v1.Utility
{
    public class BlockQueue<T>
    {
        private readonly Queue<T> _inner_queue = null;

        private readonly ManualResetEvent _dequeue_wait = null;

        public int Count => _inner_queue.Count;

        public BlockQueue(int capacity = -1)
        {
            this._inner_queue = capacity == -1 ? new Queue<T>() : new Queue<T>(capacity);
            this._dequeue_wait = new ManualResetEvent(false);
        }

        public void EnQueue(T item)
        {
            if (this._isShutdown == true) throw new InvalidOperationException("服务未开启.[EnQueue]");

            lock (this._inner_queue)
            {
                this._inner_queue.Enqueue(item);
                this._dequeue_wait.Set();
            }
        }

        public T DeQueue(int waitTime)
        {
            bool _queueEmpty = false;
            T item = default(T);
            while (true)
            {
                lock (this._inner_queue)
                {
                    if (this._inner_queue.Count > 0)
                    {
                        item = this._inner_queue.Dequeue();
                        this._dequeue_wait.Reset();
                        //break;
                    }
                    else
                    {
                        if (this._isShutdown == true)
                        {
                            throw new InvalidOperationException("服务未开启[DeQueue].");
                        }
                        else
                        {
                            _queueEmpty = true;
                        }
                    }
                }
                if (item != null)
                {
                    return item;
                }
                if (_queueEmpty)
                {
                    this._dequeue_wait.WaitOne(waitTime);
                }
            }

        }

        private bool _isShutdown = false;

        public void Shutdown()
        {
            this._isShutdown = true;
            this._dequeue_wait.Set();
        }

        public void Clear()
        {
            this._inner_queue.Clear();
        }
    }
}
