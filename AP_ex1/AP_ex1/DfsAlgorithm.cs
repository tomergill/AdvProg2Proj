﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_ex1
{
    class DfsAlgorithm<T>
    {
        Stack<State<T>> edges;
        int evaluatedNodes;

        public DfsAlgorithm()
        {
            edges = new Stack<State<T>>();
            evaluatedNodes = 0;
        }

        private Solution<T> backTrace(State<T> s)
        {
            Solution<T> mySol = new Solution<T>();
            State<T> father = s.getFather();
            while (father != default(State<T>))
            {
                mySol.Push(father);
                father = father.getFather();
            }
            return mySol;
        }

        public Solution<T> search(ISearchable<T> serachable)
        {
            HashSet<State<T>> closed = new HashSet<State<T>>();
            edges.Push(serachable.getInitialState());
            while(edges.Count>0)
            {
                State<T> n = edges.Pop();
                evaluatedNodes++;
                if (n.Equals(serachable.getGoalState()))
                    return backTrace(n);
                closed.Add(n);
                List<State<T>> successors = serachable.getAllPossibleStates(n);
                foreach (State<T> s in successors)
                {
                    if (!edges.Contains(s) && !closed.Contains(s))
                        edges.Push(s);

                }
            }
            return default(Solution<T>);
        }

        public int getNumberOfNodesEvaluated()
        {
            return evaluatedNodes;
        }
    }
}
