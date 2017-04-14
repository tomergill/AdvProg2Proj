using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_ex1
{
    /// <summary>
    /// a generic class representing an object that has the ability to search
    /// </summary>
    /// <typeparam name="T"> generic T value </typeparam>
    public abstract class Searcher<T> : ISearcher<T>
    {
        /// <summary>
        /// queue that promotes elements according to thier value
        /// </summary>
        private MyPriorityQueue<T> openList;
        /// <summary>
        /// field that allowes us to check how many states were developed in the search
        /// </summary>
        private int evaluatedNodes;

        /// <summary>
        /// searcher constructor
        /// </summary>
        public Searcher()
        {
            openList = new MyPriorityQueue<T>();
            evaluatedNodes = 0;
        }

        /// <summary>
        /// pops the element with the highest priority, and updates evaluatedNodes
        /// </summary>
        /// <returns> state T </returns>
        protected State<T> PopOpenList()
        {
            evaluatedNodes++;
            return openList.Dequeue();
        }

        /// <summary>
        /// returns size of the priority queue
        /// </summary>
        public int OpenListSize
        {
            get { return openList.count; }
        }

        /// <summary>
        /// returns evaluatedNodes
        /// </summary>
        /// <returns> evaluatedNodes </returns>
        public int GetNumberOfNodesEvaluated()
        {
            return evaluatedNodes;
        }

        /// <summary>
        /// adds element to the priority queue
        /// </summary>
        /// <param name="val"> State T </param>
        protected void AddToOpenList(State<T> val)
        {
            openList.Enqueue(val);
        }

        /// <summary>
        /// checks if priority queue contains a specific state
        /// </summary>
        /// <param name="s"> state T </param>
        /// <returns> true or false </returns>
        protected bool OpenContains(State<T> s)
        {
            return openList.Contains(s);
        }

        /// <summary>
        /// finding element in priority queue
        /// </summary>
        /// <remarks> must check if value is in priority queue first!
        ///           there are no validation checks, this method 
        ///           works in o(1) complexity 
        /// </remarks>
        /// <param name="s"> state T </param>
        /// <returns> state T </returns>
        protected State<T> FindAndRerturnState(State<T> s)
        {
            MyPriorityQueue<T> tmpQueue = new MyPriorityQueue<T>();
            State<T> desiredState = default(State<T>);
            while (OpenListSize!=0)
            {
                State<T> checkState = PopOpenList();
                if (checkState.Equals(s))
                {
                    desiredState = checkState;
                    break;
                }
                tmpQueue.Enqueue(checkState);
            }
            while (tmpQueue.count!=0)
            {
                AddToOpenList(tmpQueue.Dequeue());
            }
            return desiredState;
        }

        /// <summary>
        /// abstract method that would be overriten by the extending searchers
        /// </summary>
        /// <param name="searchable"> a searchable object </param>
        /// <returns> solution, a path to goal from initial state </returns>
        public abstract Solution<T> Search(ISearchable<T> searchable);
    }
}
