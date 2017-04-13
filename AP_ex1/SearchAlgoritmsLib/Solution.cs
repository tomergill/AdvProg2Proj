using System.Collections.Generic;

namespace AP_ex1
{
    public class Solution<T>
    {
        private Stack<State<T>> path;

        public Solution()
        {
            this.path = new Stack<State<T>>();
        }

        public void Push(State<T> val)
        {
            path.Push(val);
        }

        public State<T> Pop()
        {
            return path.Pop();
        }
    }
}