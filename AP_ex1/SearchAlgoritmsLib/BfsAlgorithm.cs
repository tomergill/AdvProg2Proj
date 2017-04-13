using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_ex1
{
    class BfsAlgorithm<T> : Searcher<T>
    {

        private Solution<T> backTrace(State<T> s)
        {
            Solution<T> mySol = new Solution<T>();
            State<T> father = s.getFather();
            while (father!= default(State<T>))
            {
                mySol.Push(father);
                father = father.getFather();
            }
            return mySol;
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
                    return backTrace(n);
                List<State<T>> successors = serachable.getAllPossibleStates(n);
                foreach (State<T> s in successors)
                {
                    if (!closed.Contains(s) && !openContains(s))
                        //s.setcameFrom(n); //already done by getSuccessors
                        addToOpenList(s);
                    else
                    {
                        if (!closed.Contains(s) && openContains(s))
                        {
                            State<T> tmpState = findAndRerturnState(s);
                            if (tmpState.getCost() > s.getCost())
                                addToOpenList(s);
                            else
                                addToOpenList(tmpState);
                        }
                    }

                }

            }
            return default(Solution<T>);
        }
    }
}
