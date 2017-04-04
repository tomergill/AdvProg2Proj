using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_ex1
{
    class BfsAlgorithm<T> : Searcher<T>
    {

        private Solution<T> backTrace()
        {
            
        }


        public override Solution<T> search(ISearchable<T> serachable)
        {
            addToOpenList(serachable.getInitialState());
            HashSet<State<T>> closed = new HashSet<State<T>>();
            while (OpenListSize > 0)
            {
                State<T> n = popOpenList();
                closed.Add(n);
                if (n.Equals(serachable.getGoalState()))
                    return backTrace();
                List<State<T>> succerssors = serachable.getAllPossibleStates(n);
                foreach (State<T> s in succerssors)
                {
                    if (!closed.Contains(s) && !openContains(s))
                        addToOpenList(s);
                }

            }
        }
    }
}
