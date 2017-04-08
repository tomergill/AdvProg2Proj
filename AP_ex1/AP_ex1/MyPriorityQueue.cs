
using System.Collections.Generic;
using Priority_Queue;
namespace AP_ex1
{
    internal class MyPriorityQueue<T>
    {
        private SimplePriorityQueue<State<T>> myElems;
        public int count; //counts the number of elemnts

        public MyPriorityQueue()
        {
            this.count = 0;
        }

        public State<T> Dequeue()
        {
            if (myElems.Count == 0)
                return default(State<T>); //returns null or 0 depending on the type of T
            else
            {
                count -= 1;
                return myElems.Dequeue();
            }
        }

        public void Enqueue(State<T> elem)
        {
            this.count += 1;
            myElems.Enqueue(elem,(float)elem.getCost());
        }

        public bool Contains(State<T> elem)
        {
            return myElems.Contains(elem);
        }

    }
}