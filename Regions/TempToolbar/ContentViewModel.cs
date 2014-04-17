using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using MemOrg.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.ServiceLocation;
using TempToolbar.ChapterAddView;

namespace TempToolbar
{
    public class ContentViewModel : ViewModelBase
    {
        public ContentViewModel()
        {
            AddNewChapterCommand = new DelegateCommand(AddNewChapter, () => true);
        }
     
        public DelegateCommand AddNewChapterCommand { get; set; }

        private IGraphManagementService _managementService;
        private IGraphManagementService ManagementService 
        {
            get
            {
                if (_managementService != null)
                    return _managementService;

                _managementService =
                    (IGraphManagementService)
                        ServiceLocator.Current.GetService(typeof (IGraphManagementService));
                return _managementService;
            }
        }

        private void AddNewChapter()
        {
            var dlg = new ChapterAddView.ChapterAddView();
            var res = dlg.ShowDialog();
            if (res!= null && res.Value)
            {
                var chapterNumber = dlg.GetChapterNumber();
                if (chapterNumber != null)
                    ManagementService.AddNewChapter
                        (dlg.GetChapterCaption(), dlg.GetBookName(), chapterNumber.Value);
            }
        }
    }
}
