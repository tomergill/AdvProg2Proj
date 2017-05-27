namespace WpfApplication1
{
    /// <summary>
    /// settings view model
    /// </summary>
    class SettingsViewModel : ViewModel
    {
        //having a model
        private ISettingsModel model;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="model"></param>
        public SettingsViewModel(ISettingsModel model)
        {
            this.model = model;
        }

        /// <summary>
        /// getter and setter for serverIP, requesting and updating by notifing model
        /// </summary>
        public string ServerIP
        {
            get { return model.ServerIP; }
            set
            {
                model.ServerIP = value;
                NotifyPropertyChanged("ServerIP");
            }
        }

        /// <summary>
        /// getter and setter for server port, requesting and updating by notifing model
        /// </summary>
        public int ServerPort
        {
            get { return model.ServerPort; }
            set
            {
                model.ServerPort = value;
                NotifyPropertyChanged("ServerPort");
            }
        }

        /// <summary>
        /// getter and setter for maze rows, requesting and updating by notifing model
        /// </summary>
        public int MazeRows
        {
            get { return model.MazeRows; }
            set
            {
                model.MazeRows = value;
                NotifyPropertyChanged("MazeRows");
            }
        }

        /// <summary>
        /// getter and setter for maze colomns, requesting and updating by notifing model
        /// </summary>
        public int MazeCols
        {
            get { return model.MazeCols; }
            set
            {
                model.MazeCols = value;
                NotifyPropertyChanged("MazeCols");
            }
        }

        /// <summary>
        /// getter and setter for search algorithm number, requesting and updating by notifing model
        /// </summary>
        public int SearchAlgorithm
        {
            get { return model.SearchAlgorithm; }
            set
            {
                if (value>=0 && value<2)
                {
                    model.SearchAlgorithm = value;
                    NotifyPropertyChanged("SearchAlgorithm");

                }
            }
        }

        /// <summary>
        /// save setting function, notifying model
        /// </summary>
        public void SaveSettings()
        {
            model.SaveSettings();
        }
    }
}