
using System.Collections.Generic;
using Priority_Queue;
namespace AP_ex1
{
    /// <summary>
    /// the priority queue used in searcher
    /// </summary>
    /// <typeparam name="T"> generic T value </typeparam>
    internal class MyPriorityQueue<T>
    {
        /// <summary>
        /// members
        /// </summary>
        /// <remarks> using a nuget priority queue, SimplePriorityQueue
        ///           count member to keep track with the queue size
        /// </remarks>
        private SimplePriorityQueue<State<T>> myElementsQueue;
        public int count; //counts the number of elemnts

        /// <summary>
        /// constructor
        /// </summary>
        public MyPriorityQueue()
        {
            this.count = 0;
            myElementsQueue = new SimplePriorityQueue<State<T>>();
        }

        /// <summary>
        /// dequeuing an elem from the queue
        /// </summary>
        /// <returns> state T </returns>
        public State<T> Dequeue()
        {
            if (myElementsQueue.Count == 0)
                return default(State<T>); //returns null or 0 depending on the type of T
            else
            {
                count -= 1;
                return myElementsQueue.Dequeue();
            }
        }

        /// <summary>
        /// enqueuing an elem to the queue
        /// </summary>
        /// <param name="elem"> state T </param>
        public void Enqueue(State<T> elem)
        {
            this.count += 1;
            myElementsQueue.Enqueue(elem,(float) elem.GetCost());
        }

        /// <summary>
        /// checks if the queue contains an element
        /// </summary>
        /// <param name="elem"> generic state T </param>
        /// <returns> true or false </returns>
        public bool Contains(State<T> elem)
        {
            return myElementsQueue.Contains(elem);
        }

    }
}