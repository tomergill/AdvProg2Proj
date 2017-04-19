using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchAlgorithmsLib;

namespace Server
{
    /// <summary>
    /// Holds a solution and the number of nodes evaluated in it's search.
    /// </summary>
    /// <typeparam name="T">Type of state the solution will hold.</typeparam>
    public class SolutionWithNodesEvaluated<T>
    {
        /// <summary>
        /// The solution.
        /// </summary>
        private Solution<T> solution;

        /// <summary>
        /// The number of nodes evaluated.
        /// </summary>
        private int nodesEvaluated;

        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionWithNodesEvaluated{T}"/> class.
        /// </summary>
        /// <param name="sol">The solution.</param>
        /// <param name="nodes">The number of nodes evaulated.</param>
        public SolutionWithNodesEvaluated(Solution<T> sol, int nodes)
        {
            solution = sol;
            nodesEvaluated = nodes;
        }

        /// <summary>
        /// Gets the solution.
        /// </summary>
        /// <value>
        /// The solution.
        /// </value>
        public Solution<T> Solution { get => solution;}

        /// <summary>
        /// Gets the number of nodes evaluated.
        /// </summary>
        /// <value>
        /// The  number of nodes evaluated.
        /// </value>
        public int NodesEvaluated { get => nodesEvaluated;}
    }
}
