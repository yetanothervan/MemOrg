using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;
using MemOrg.Interfaces;

namespace ChapterViewer.CreatePointBlock
{
    public class CreatePointBlockViewModel: ViewModelBase
    {
        private string _captionView;
        private string _captionBlock;
        private bool _isCreateNew;
        private Block _myBlock;

        public string CaptionView
        {
            get { return _captionView; }
            set
            {
                _captionView = value;
                RaisePropertyChangedEvent("CaptionView");
            }
        }

        public string CaptionBlock
        {
            get
            {
                if (!IsCreateNew && MyBlock != null)
                    return MyBlock.Caption;
                return _captionBlock;
            }
            set
            {
                _captionBlock = value;
                RaisePropertyChangedEvent("CaptionBlock");
            }
        }

        public bool IsCreateNew
        {
            get { return _isCreateNew; }
            set
            {
                _isCreateNew = value;
                RaisePropertyChangedEvent("IsCreateNew");
                RaisePropertyChangedEvent("IsCreateNewReversed");
                RaisePropertyChangedEvent("CaptionBlock");
            }
        }
        
        public bool IsCreateNewReversed
        {
            get { return !_isCreateNew; }
        }

        public Block MyBlock
        {
            get { return _myBlock; }
            set
            {
                _myBlock = value;
                if (_myBlock != null)
                    CaptionBlock = _myBlock.Caption;
                RaisePropertyChangedEvent("MyBlock");
            }
        }
    }
}
