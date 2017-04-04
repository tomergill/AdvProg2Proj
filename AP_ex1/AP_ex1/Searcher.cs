using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_ex1
{
    public abstract class Searcher<T> : ISearcher<T>
    {
        private MyPriorityQueue<State<T>> openList;
        private int evaluatedNodes;

        public Searcher()
        {
            openList = new MyPriorityQueue<State<T>>();
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

        public void addToOpenList(State<T> val)
        {
            openList.Enqueue(val);
        }

        public bool openContains(State<T> s)
        {
            return openList.Contains(s);
        }

        public abstract Solution<T> search(ISearchable<T> searchable);
    }
}
