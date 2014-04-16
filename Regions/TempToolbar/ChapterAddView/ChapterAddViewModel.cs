using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF;
using MemOrg.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.ServiceLocation;

namespace TempToolbar.ChapterAddView
{
    public class ChapterAddViewModel : ViewModelBase
    {
        public ChapterAddViewModel()
        {
            _books = BlockRepository.BlockSources.Select(s => s.ParamName).Distinct().ToList();
            AddCommand = new DelegateCommand(Add, CanAdd);
            _canClose = false;
        }

        public DelegateCommand AddCommand { get; set; }
        private void Add()
        {
            CanClose = true;
        }

        private bool _canClose;
        public bool CanClose
        {
            get { return _canClose; }
            set
            {
                _canClose = value;
                RaisePropertyChangedEvent("CanClose");
            }
        }

        private bool CanAdd()
        {
            return !string.IsNullOrEmpty(Book) && ChapterNumber != null 
                && string.IsNullOrEmpty(IsChapterNumberCorrect) 
                && !string.IsNullOrEmpty(ChapterCaption);

        }

        private IBlockRepository _blockRepository;
        private IBlockRepository BlockRepository
        {
            get
            {
                if (_blockRepository != null)
                    return _blockRepository;

                _blockRepository =
                    (IBlockRepository)
                        ServiceLocator.Current.GetService(typeof(IBlockRepository));
                return _blockRepository;
            }
        }

        private List<string> _books;
        public List<string> Books 
        {
            get { return _books; }
            set
            {
                _books = value;
                RaisePropertyChangedEvent("Books");
            } 
        }

        private string _chapterCaption;
        public string ChapterCaption
        {
            get { return _chapterCaption; }
            set
            {
                _chapterCaption = value; 
                AddCommand.RaiseCanExecuteChanged();
            }
        }

        private string _book;
        public string Book
        {
            get { return _book; }
            set
            {
                _book = value;
                AddCommand.RaiseCanExecuteChanged();
                RaisePropertyChangedEvent("LastChapterNumber");
                RaisePropertyChangedEvent("Book");
            }
        }

        public string LastChapterNumber
        {
            get
            {
                var books = BlockRepository.BlockSources.Where(b => b.ParamName == Book).ToList();
                return books.Count > 0 
                    ? string.Format("(последний {0})", books.Max(b => b.ParamValue)) 
                    : "";
            }
        }
        
        public string IsChapterNumberCorrect
        {
            get
            {
                var numExist = BlockRepository.BlockSources
                    .Any(b => b.ParamName == Book && b.ParamValue == ChapterNumber);
                return numExist
                    ? string.Format("Глава с таким номером уже есть!")
                    : "";
            }
        }

        private int? _chapterNumber;
        public int? ChapterNumber
        {
            get { return _chapterNumber; }
            set
            {
                _chapterNumber = value;
                AddCommand.RaiseCanExecuteChanged();
                RaisePropertyChangedEvent("IsChapterNumberCorrect");
                RaisePropertyChangedEvent("ChapterNumber");
            }
        }
    }
}
