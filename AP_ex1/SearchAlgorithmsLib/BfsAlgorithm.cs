using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAlgorithmsLib
{
    /// <summary>
    /// a searcher that uses the Breadth first search algorithm
    /// </summary>
    /// <typeparam name="T"> generic T type </typeparam>
    public class BfsAlgorithm<T> : Searcher<T>
    {

        /// <summary>
        /// backtraces the states and returning a stack 
        /// representing the solution of the searchable object
        /// </summary>
        /// <param name="s"></param>
        /// <returns> solution </returns>
        private Solution<T> BackTrace(State<T> s)
        {
            Solution<T> mySolution = new Solution<T>();
            State<T> fatherState = s.GetFatherState();
            while (fatherState!= default(State<T>))
            {
                mySolution.Push(fatherState);
                fatherState = fatherState.GetFatherState();
            }
            return mySolution;
        }

        /// <summary>
        /// searches the searchable object and returns the solution to it
        /// according to the BFS algorithm
        /// </summary>
        /// <param name="serachable"></param>
        /// <returns> a solution by calling BackTrace </returns>
        public override Solution<T> Search(ISearchable<T> serachable)
        {
            AddToOpenList(serachable.GetInitialState());
            HashSet<State<T>> closed = new HashSet<State<T>>();
            while (OpenListSize > 0)
            {
                State<T> n = PopOpenList();
                closed.Add(n);
                if (n.Equals(serachable.GetGoalState()))
                    return BackTrace(n);
                List<State<T>> successors = serachable.GetAllPossibleStates(n);
                foreach (State<T> s in successors)
                {
                    if (!closed.Contains(s) && !OpenContains(s))
                        //s.setcameFrom(n); //already done by getSuccessors
                        AddToOpenList(s);
                    else
                    {
                        if (!closed.Contains(s) && OpenContains(s))
                        {
                            State<T> tmpState = FindAndRerturnState(s);
                            if (tmpState.GetCost() >= s.GetCost())
                                AddToOpenList(s);
                            else
                                AddToOpenList(tmpState);
                        }
                    }

                }

            }
            return default(Solution<T>);
        }
    }
}
