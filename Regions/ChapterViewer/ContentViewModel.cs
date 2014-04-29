using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using DAL.Entity;
using MemOrg.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using Block = DAL.Entity.Block;

namespace ChapterViewer
{
    public class ContentViewModel : ViewModelBase
    {
        public ContentViewModel(IEventAggregator eventAggregator)
        {
            _curPage = null;
            eventAggregator.GetEvent<PageSelected>().Subscribe(OnPageSelected);
            eventAggregator.GetEvent<ParticleChanged>().Subscribe(OnParticleChanged);
            eventAggregator.GetEvent<ParticleDeleted>().Subscribe(OnParticleDeleted);
            EditCommand = new DelegateCommand(Edit, () => (CurrentParagpaph != null && CurrentParagpaph.Editible));
            SaveCommand = new DelegateCommand(Save, () => TextChanged);
            DiscardCommand = new DelegateCommand(Discard, () => TextChanged);
            CloseEditingCommand = new DelegateCommand(CloseEditing);
            AddSourceCommand = new DelegateCommand(AddSource, () => _curPage != null && _curPage.IsBlockSource);
            DeleteCommand = new DelegateCommand(Delete, () => (CurrentParagpaph != null && CurrentParagpaph.Editible));
            
            EditWindowVisible = Visibility.Collapsed;
        }

        private void OnParticleDeleted(Particle obj)
        {
            var part =
                Document.Blocks.OfType<ParticleParagraph>()
                    .FirstOrDefault(p => p.MyParticle != null && p.MyParticle.ParticleId == obj.ParticleId);

            if (part != null)
                Document.Blocks.Remove(part);
        }

        private void OnParticleChanged(Particle obj)
        {
            var source = obj as SourceTextParticle;
            var user = obj as UserTextParticle;
            if (source == null && user == null) return;
            
            var part =
                Document.Blocks.OfType<ParticleParagraph>()
                    .FirstOrDefault(p => p.MyParticle != null && p.MyParticle.ParticleId == obj.ParticleId);
            
            if (part != null)
            {
                var paragraph = CreateParagraph(obj);
                Document.Blocks.InsertAfter(part, paragraph);
                Document.Blocks.Remove(part);
            }
            else
            {
                var paragraph = CreateParagraph(obj);
                Document.Blocks.Add(paragraph);
            }
        }

        private IPage _curPage;
        private void OnPageSelected(IPage curPage)
        {
            _curPage = curPage;
            Discard();
            CloseEditing();
            CurrentParagpaph = null;
            
            var doc = new FlowDocument();
            var captionParagraph = new ParticleParagraph(curPage.Block.Caption);
            doc.Blocks.Add(captionParagraph);
            
            foreach (var part in curPage.Block.Particles.OrderBy(b => b.Order))
            {
                var paragraph = CreateParagraph(part);
                doc.Blocks.Add(paragraph);
            }
            
            Document = doc;
            AddSourceCommand.RaiseCanExecuteChanged();
        }

        private ParticleParagraph CreateParagraph(Particle p)
        {
            var paragraph = new ParticleParagraph(p);

            paragraph.MouseDown += (sender, args) =>
            {
                CurrentParagpaph = paragraph;
            };

            return paragraph;
        }


        public DelegateCommand EditCommand { get; set; }
        private void Edit()
        {
            EditWindowVisible = Visibility.Visible;
        }

        public DelegateCommand SaveCommand { get; set; }
        private void Save()
        {
            if (CurrentParagpaph == null || CurrentParagpaph.MyParticle == null) return;
            ManagementService.UpdateParticleText(CurrentParagpaph.MyParticle.ParticleId, SelectedParagpaphText);
            TextChanged = false;
        }

        public DelegateCommand DiscardCommand { get; set; }
        private void Discard()
        {
            RaisePropertyChangedEvent("SelectedParagpaphText");
            TextChanged = false;
        }
        
        public DelegateCommand CloseEditingCommand { get; set; }
        private void CloseEditing()
        {
            EditWindowVisible = Visibility.Collapsed;
        }

        public DelegateCommand AddSourceCommand { get; set; }
        private void AddSource()
        {
            if (_curPage != null && _curPage.Block != null && _curPage.IsBlockSource)
                ManagementService.AddSourceParticle(_curPage.Block);
        }

        public DelegateCommand DeleteCommand { get; set; }
        private void Delete()
        {
            if (CurrentParagpaph != null && CurrentParagpaph.MyParticle != null)
                ManagementService.RemoveSourceParticle(CurrentParagpaph.MyParticle);
        }



        public void ParagraphBlur()
        {
            foreach (var b in Document.Blocks.OfType<ParticleParagraph>())
            {
                b.IsSelected = false;
            }
        }

        private ParticleParagraph _currentParagpaph;
        public ParticleParagraph CurrentParagpaph
        {
            get { return _currentParagpaph; }
            set
            {
                if (Equals(_currentParagpaph, value))
                    return;
                
                ParagraphBlur();

                SelectedParagpaphText = null;

                if (value != null)
                {
                    value.IsSelected = true;
                    SelectedParagpaphText = new TextRange(value.ContentStart, value.ContentEnd).Text;
                }

                _currentParagpaph = value;
                RaisePropertyChangedEvent("CurrentParagpaph");
                EditCommand.RaiseCanExecuteChanged();
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }

        private string _particleEditText;
        public string SelectedParagpaphText
        {
            get
            {
                return _particleEditText;
            }
            set
            {
                _particleEditText = value;
                RaisePropertyChangedEvent("SelectedParagpaphText");
            }
        }

        private FlowDocument _document;
        public FlowDocument Document
        {
            get { return _document; }
            set
            {
                _document = value;
                RaisePropertyChangedEvent("Document");
            }
        }

        private bool _textChanged;
        public bool TextChanged
        {
            get { return _textChanged; }
            set
            {
                _textChanged = value;
                SaveCommand.RaiseCanExecuteChanged();
                DiscardCommand.RaiseCanExecuteChanged();
            }
        }

        private IGraphManagementService _managementService;
        private IGraphManagementService ManagementService
        {
            get
            {
                if (_managementService != null)
                    return _managementService;

                _managementService =
                    (IGraphManagementService)
                        ServiceLocator.Current.GetService(typeof(IGraphManagementService));
                return _managementService;
            }
        }

        private Visibility _editWindowVisible;
        public Visibility EditWindowVisible
        {
            get
            {
                return _editWindowVisible;
            }
            set
            {
                _editWindowVisible = value;
                TextChanged = false;
                RaisePropertyChangedEvent("EditWindowVisible");
            }
        }
    }
}
