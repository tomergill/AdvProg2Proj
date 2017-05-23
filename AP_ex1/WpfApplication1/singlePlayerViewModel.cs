using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class singlePlayerViewModel : ViewModel
    {
        private singlePlayerModel SPM;
        
        public singlePlayerViewModel(String mazeName, int rows, int cols)
        {
            SPM = new singlePlayerModel(mazeName, rows, cols);
        }
    }
}
