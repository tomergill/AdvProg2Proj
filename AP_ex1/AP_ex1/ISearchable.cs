
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_ex1
{
    /// <summary>
    /// interface for searchable objects
    /// </summary>
    /// <typeparam name="T"> generic T type </typeparam>
    public interface ISearchable<T>
    {
        /// <summary>
        /// extending classes must be able to return thier initial state
        /// </summary>
        /// <returns> state T </returns>
        State<T> GetInitialState();
        /// <summary>
        /// extending classes must be able to return thier goal state
        /// </summary>
        /// <returns> state T </returns>
        State<T> GetGoalState();
        /// <summary>
        /// extending classes must be able to return a list of neighbouring states
        /// according to a given one
        /// </summary>
        /// <param name="s"> state in searchable </param>
        /// <returns> list of neighbouring states </returns>
        List<State<T>> GetAllPossibleStates(State<T> s);
    }
}
