using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    /// <summary>
    /// choose maze view model
    /// </summary>
    class ChooseMazeViewModel : ViewModel
    {
        //having a model
        private ChooseMazeModel CMM;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="CMM1"></param>
        public ChooseMazeViewModel(ChooseMazeModel CMM1)
        {
            this.CMM = CMM1;
        }

        /// <summary>
        /// requesting model for maze rows
        /// </summary>
        public int MazeRows
        {
            get { return CMM.MazeRows; }
        }

        /// <summary>
        /// requesting model for maze columns
        /// </summary>
        public int MazeCols
        {
            get { return CMM.MazeCols; }
        }

        /// <summary>
        /// requesting model to save changes
        /// </summary>
        public void SaveSettings()
        {
            CMM.SaveSettings();
        }
    }
}
