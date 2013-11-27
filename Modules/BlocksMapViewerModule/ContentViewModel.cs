using Interfaces;

namespace BlocksMapViewerModule
{
    public class ContentViewModel : ViewModelBase
    {
        public ContentViewModel()
        {
            MyText = "Some of my texts";
        }

        private string _myText;
        public string MyText
        {
            get
            {
                return _myText;
            }
            set
            {
                if (_myText != value)
                {
                    _myText = value;
                    RaisePropertyChangedEvent("MyText");
                }
            }
        }
    }
}
