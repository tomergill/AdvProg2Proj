using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_ex1
{
    public abstract class Searcher<T> : ISearcher<T>
    {
        private MyPriorityQueue<T> openList;
        private int evaluatedNodes;

        public Searcher()
        {
            openList = new MyPriorityQueue<T>();
            evaluatedNodes = 0;
        }

        protected State<T> popOpenList()
        {
            evaluatedNodes++;
            return openList.Dequeue();
        }

        public int OpenListSize
        {
            get { return openList.count; }
        }

        public int getNumberOfNodesEvaluated()
        {
            return evaluatedNodes;
        }

        protected void addToOpenList(State<T> val)
        {
            openList.Enqueue(val);
        }

        protected bool openContains(State<T> s)
        {
            return openList.Contains(s);
        }

        protected State<T> findAndRerturnState(State<T> s)
        {
            MyPriorityQueue<T> tmpQueue = new MyPriorityQueue<T>();
            State<T> desiredState = default(State<T>);
            while (tmpQueue.count!=0)
            {
                State<T> checkState = popOpenList();
                if (checkState.Equals(s))
                {
                    desiredState = checkState;
                    break;
                }
                tmpQueue.Enqueue(checkState);
            }
            while (tmpQueue.count!=0)
            {
                addToOpenList(tmpQueue.Dequeue());
            }
            return desiredState;
        }

        public abstract Solution<T> search(ISearchable<T> searchable);
    }
}
