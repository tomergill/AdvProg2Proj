using System;
using System.Collections.Generic;

namespace SearchAlgorithmsLib
{
    /// <summary>
    /// generic class representing a solution to a puzzle
    /// </summary>
    /// <typeparam name="T"> generic T type </typeparam>
    public class Solution<T>
    {
        /// <summary>
        /// the solution is represented by a stack, first pop is initial state, last is the goal
        /// </summary>
        private Stack<State<T>> path;

        /// <summary>
        /// solution constructor
        /// </summary>
        public Solution()
        {
            this.path = new Stack<State<T>>();
        }

        /// <summary>
        /// pushing state to the solution
        /// </summary>
        /// <param name="val"> generic state T </param>
        public void Push(State<T> val)
        {
            path.Push(val);
        }

        /// <summary>
        /// poping a state from the solution
        /// </summary>
        /// <returns> returns a state T </returns>
        public State<T> Pop()
        {
            try
            {
                return path.Pop();
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}