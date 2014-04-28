using System;
using System.Collections.Generic;
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
            eventAggregator.GetEvent<BlockSelected>().Subscribe(OnBlockSelected);
            EditCommand = new DelegateCommand(Edit, () => (CurrentParagpaph != null && CurrentParagpaph.Editible));
            SaveCommand = new DelegateCommand(Save, () => TextChanged);
            DiscardCommand = new DelegateCommand(Discard, () => TextChanged);
            CloseEditingCommand = new DelegateCommand(CloseEditing);
            
            EditWindowVisible = Visibility.Collapsed;
        }
        
        private void OnBlockSelected(Block block)
        {
            var doc = new FlowDocument();
            var captionParagraph = new ParticleParagraph(block.Caption);
            doc.Blocks.Add(captionParagraph);

            doc.MouseDown += (sender, args) =>
            {
                CurrentParagpaph = null;
            };
            foreach (var part in block.Particles.OrderBy(b => b.Order))
            {
                var paragraph = new ParticleParagraph(part);

                paragraph.MouseDown += (sender, args) =>
                {
                    CurrentParagpaph = paragraph;
                    args.Handled = true;
                };

                doc.Blocks.Add(paragraph);
            }
            
            Document = doc;
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
                    SelectedParagpaphText =
                        new TextRange(value.ContentStart, value.ContentEnd).Text;
                }

                _currentParagpaph = value;
                RaisePropertyChangedEvent("CurrentParagpaph");
                EditCommand.RaiseCanExecuteChanged();
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
