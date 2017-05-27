using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class ChooseMazeModel
    {
        /// <summary>
        /// getter for maze colomns
        /// </summary>
        public int MazeCols
        {
            get { return Properties.Settings.Default.MazeCols; }
        }

        /// <summary>
        /// getter for maze rows
        /// </summary>
        public int MazeRows
        {
            get { return Properties.Settings.Default.MazeRows; }
        }

        /// <summary>
        /// model save changes function
        /// </summary>
        public void SaveSettings()
        {
            Properties.Settings.Default.Save();
        }
    }
}
