using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemOrg.Interfaces;

namespace ChapterViewer.AddRefDlg
{
    public class AddRefViewModel : ViewModelBase
    {
        private bool _createUserTextCheckBoxEnabled;
        private bool _isCreateUserText;

        public AddRefViewModel()
        {
            RefTypes = new List<string>
            {
                "->", "<-", "<->"
            };
            RefType = "->";
        }
        public List<string> RefTypes { get; set; }
        public string RefType { get; set; }

        public bool IsCreateUserText
        {
            get { return _isCreateUserText; }
            set
            {
                _isCreateUserText = value;
                RaisePropertyChangedEvent("IsCreateUserText");
            }
        }

        public bool CreateUserTextCheckBoxEnabled
        {
            get { return _createUserTextCheckBoxEnabled; }
            set
            {
                _createUserTextCheckBoxEnabled = value;
                RaisePropertyChangedEvent("CreateUserTextCheckBoxEnabled");
                if (!value)
                    IsCreateUserText = false;
            }
        }
    }
}
