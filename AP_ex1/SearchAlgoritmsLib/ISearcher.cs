﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_ex1
{
    interface ISearcher<T>
    {
        Solution<T> search(ISearchable<T> searchable);
        int getNumberOfNodesEvaluated();
    }
}