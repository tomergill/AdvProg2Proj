using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAlgorithmsLib
{
    /// <summary>
    /// a searcher that uses Depth first algorithm
    /// </summary>
    /// <typeparam name="T"> generic T type </typeparam>
    public class DfsAlgorithm<T>
    {
        /// <summary>
        /// members
        /// </summary>
        /// <remarks> a stack of possible neighbouring states
        ///           number of evaluated states along the search
        /// </remarks>
        Stack<State<T>> neighbouringStatesStack;
        int evaluatedNodes;

        /// <summary>
        /// constractor
        /// </summary>
        public DfsAlgorithm()
        {
            neighbouringStatesStack = new Stack<State<T>>();
            evaluatedNodes = 0;
        }

        /// <summary>
        /// backtraces the states and returning a stack 
        /// representing the solution of the searchable object
        /// </summary>
        /// <param name="s"> state T </param>
        /// <returns> solution of the searchable object</returns>
        private Solution<T> BackTrace(State<T> s)
        {
            Solution<T> mySolution = new Solution<T>();
            mySolution.Push(s);
            State<T> fatherState = s.GetFatherState();
            while (fatherState != default(State<T>))
            {
                mySolution.Push(fatherState);
                fatherState = fatherState.GetFatherState();
            }
            return mySolution;
        }

        /// <summary>
        /// searches the searchable object and returns the solution to it
        /// according to the DFS algorithm
        /// </summary>
        /// <param name="serachable"></param>
        /// <returns> solution by calling Backtrace </returns>
        public Solution<T> Search(ISearchable<T> serachable)
        {
            HashSet<State<T>> closed = new HashSet<State<T>>();
            neighbouringStatesStack.Push(serachable.GetInitialState());
            while(neighbouringStatesStack.Count>0)
            {
                State<T> n = neighbouringStatesStack.Pop();
                evaluatedNodes++;
                if (n.Equals(serachable.GetGoalState()))
                    return BackTrace(n);
                closed.Add(n);
                List<State<T>> successors = serachable.GetAllPossibleStates(n);
                foreach (State<T> s in successors)
                {
                    if (!neighbouringStatesStack.Contains(s) && !closed.Contains(s))
                        neighbouringStatesStack.Push(s);

                }
            }
            return default(Solution<T>);
        }

        /// <summary>
        /// returns the number of states developed
        /// </summary>
        /// <returns></returns>
        public int GetNumberOfNodesEvaluated()
        {
            return evaluatedNodes;
        }
    }
}
