using System.Collections.Generic;

namespace AP_ex1
{
    public class Solution<T>
    {
        private Stack<T> path;

        public Solution()
        {

        }

        public void Push(T val)
        {
            path.Push(val);
        }

        public T Pop(T val)
        {
            return path.Pop();
        }
    }
}