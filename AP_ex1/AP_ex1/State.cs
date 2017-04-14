using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_ex1
{
    /// <summary>
    /// a generic State class
    /// </summary>
    /// <typeparam name="T"> generic T value </typeparam>
    public class State<T>
    {
        /// <summary>
        /// the member 
        /// </summary>
        /// <remarks> : the state itself, the cost to reach it 
        ///             and what state did it came from 
        /// </remarks>
        private T state;            
        private double cost;            
        private State<T> cameFrom;

        /// <summary>
        /// constructor of the class
        /// </summary>
        /// <param name="state"> generic T value </param>
        public State(T state)
        {
            this.state = state;
            this.cost = 0;
            this.cameFrom = default(State<T>);
        }

        /// <summary>
        /// returns the state of the current class
        /// </summary>
        /// <returns> T state </returns>
        public T GetState()
        {
            return this.state;
        }

        /// <summary>
        /// returns the cost of this current class
        /// </summary>
        /// <returns> double cost </returns>
        public double GetCost()
        {
            return this.cost;
        }

        /// <summary>
        /// returns the state we came from
        /// </summary>
        /// <returns> the state T we came from </returns>
        public State<T> GetFatherState()
        {
            return this.cameFrom;
        }

        /// <summary>
        /// allowes to set cost by a given 'double' value
        /// </summary>
        /// <param name="val"> 'double' value </param>
        public void SetCost(double val)
        {
            this.cost += val;
        }

        /// <summary>
        /// allowes to set cost by a given state
        /// </summary>
        /// <param name="s"> desiged for father state </param>
        public void SetCost(State<T> s)
        {
            this.cost += s.GetCost();
        }

        /// <summary>
        /// allowes to set state we came from
        /// </summary>
        /// <param name="s"> father state </param>
        public void SetFatherState(State<T> s)
        {
            this.cameFrom = s;
        }

        /// <summary>
        /// allowes to compare between states
        /// </summary>
        /// <param name="s"> some state </param>
        /// <returns></returns>
        public bool Equals(State<T> s)
        {
            return state.Equals(s.state);
        }
        
        /// <summary>
        /// allowes to compare state to itself or other classes
        /// </summary>
        /// <param name="obj"> any class </param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as State<T>);
        }

        /// <summary>
        /// allowes find and insert to hash lists in o(1)
        /// </summary>
        /// <returns> hash code according to ToString method </returns>
        public override int GetHashCode()
        {
            return state.ToString().GetHashCode();
        }


    }
}
