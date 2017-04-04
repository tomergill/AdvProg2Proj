using System.Collections.Generic;

namespace AP_ex1
{
    internal class MyPriorityQueue<T>
    {
        private Queue<T> myElems;
        public int count; //counts the number of elemnts

        public MyPriorityQueue()
        {
            this.count = 0;
        }

        public T Dequeue()
        {
            if (myElems.Count == 0)
                return default(T); //returns null or 0 depending on the type of T
            else
                return myElems.Dequeue();
        }

        public void Enqueue(T elem)
        {
            myElems.Enqueue(elem);
        }

        public bool Contains(T elem)
        {
            return myElems.Contains(elem);
        }

    }
}