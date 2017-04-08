using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_ex1
{
    public class State<T>
    {
        private T state;       //the state represented by a string
        private double cost;        //cost to reach this state (set by a setter)
        private State<T> cameFrom;     //the state we came from to this state (setter)

        public State(T state)
        {
            this.state = state;
            this.cost = 0;
            this.cameFrom = null;
        }

        public bool Equals(State<T> s)
        {
            return state.Equals(s.state);
        }

        public void setFatherState(State<T> s)
        {
            this.cameFrom = s;
        }

        public double getCost()
        {
            return this.cost;
        }

        public State<T> getFather()
        {
            return this.cameFrom;
        }

        public void setCost(double val)
        {
            this.cost += val;
        }

        public void setCost(State<T> s)
        {
            this.cost += s.getCost();
        }
        
    }
}
