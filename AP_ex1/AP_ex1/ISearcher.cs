using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_ex1
{
    /// <summary>
    /// interface for searchers
    /// </summary>
    /// <typeparam name="T"> generic T type </typeparam>
    interface ISearcher<T>
    {
        /// <summary>
        /// extending classes must have the ability to return a solution
        /// </summary>
        /// <param name="searchable"> a searchable object </param>
        /// <returns> solution </returns>
        Solution<T> Search(ISearchable<T> searchable);
        /// <summary>
        /// extending classes must be able to count the amount of states developed
        /// </summary>
        /// <returns></returns>
        int GetNumberOfNodesEvaluated();
    }
}
