using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class ChooseMazeModel
    {
        public int MazeCols
        {
            get { return Properties.Settings.Default.MazeCols; }
        }

        public int MazeRows
        {
            get { return Properties.Settings.Default.MazeRows; }
        }

        public void SaveSettings()
        {
            Properties.Settings.Default.Save();
        }
    }
}
