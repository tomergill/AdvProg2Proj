using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class ChooseMazeViewModel : ViewModel
    {
        private ChooseMazeModel CMM;

        public ChooseMazeViewModel(ChooseMazeModel CMM1)
        {
            this.CMM = CMM1;
        }

        public int MazeRows
        {
            get { return CMM.MazeRows; }
        }

        public int MazeCols
        {
            get { return CMM.MazeCols; }
        }


        public void SaveSettings()
        {
            CMM.SaveSettings();
        }
    }
}
