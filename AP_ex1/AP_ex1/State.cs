﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_ex1
{
    public class State<T>
    {
        private T state;       //
        private double cost;        //cost to reach this state (set by a setter)
        private State<T> cameFrom;     //the state we came from to this state (setter)

        public State(T state)
        {
            this.state = state;
            this.cost = 0;
            this.cameFrom = default(State<T>);
        }

        public T getState()
        {
            return this.state;
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

        public void setFatherState(State<T> s)
        {
            this.cameFrom = s;
        }

        public bool Equals(State<T> s)
        {
            return state.Equals(s.state);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as State<T>);
        }

        public override int GetHashCode()
        {
            return state.ToString().GetHashCode();
        }


    }
}
