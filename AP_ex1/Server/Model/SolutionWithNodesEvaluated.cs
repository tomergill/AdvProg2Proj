using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchAlgorithmsLib;

namespace Server
{
    public class SolutionWithNodesEvaluated<T>
    {
        private Solution<T> solution;
        private int nodesEvaluated;

        public SolutionWithNodesEvaluated(Solution<T> sol, int nodes)
        {
            solution = sol;
            nodesEvaluated = nodes;
        }

        public Solution<T> Solution { get => solution;}
        public int NodesEvaluated { get => nodesEvaluated;}
    }
}
