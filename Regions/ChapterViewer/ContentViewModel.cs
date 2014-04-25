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
using Microsoft.Practices.Prism.Events;
using Block = DAL.Entity.Block;

namespace ChapterViewer
{
    public class ContentViewModel : ViewModelBase
    {
        public ContentViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<BlockSelected>().Subscribe(OnBlockSelected);
        }

        private void OnBlockSelected(Block block)
        {
            var doc = new FlowDocument();
            var captionParagraph = new ParticleParagraph(new Bold(new Run(block.Caption)));
            doc.Blocks.Add(captionParagraph);
            foreach (var part in block.Particles.OrderBy(b => b.Order))
            {
                ParticleParagraph paragraph;
                if (part is UserTextParticle)
                    paragraph = new ParticleParagraph(new Run((part as UserTextParticle).Content));
                else if (part is SourceTextParticle)
                    paragraph = new ParticleParagraph(new Run((part as SourceTextParticle).Content));
                else if (part is QuoteSourceParticle)
                    paragraph = new ParticleParagraph(new Run((part as QuoteSourceParticle)
                        .SourceTextParticle.Content));
                else
                    throw new NotImplementedException();

                paragraph.MouseDown += (sender, args) =>
                {
                    ParagraphBlur();
                    paragraph.IsSelected = true;
                    CurrentParagpaph = paragraph;
                };

                doc.Blocks.Add(paragraph);
            }
            
            Document = doc;
        }

        public void ParagraphBlur()
        {
            foreach (var b in Document.Blocks.OfType<ParticleParagraph>())
                b.IsSelected = false;
        }

        private ParticleParagraph _currentParagpaph;
        public ParticleParagraph CurrentParagpaph
        {
            get { return _currentParagpaph; }
            set
            {
                _currentParagpaph = value;
                RaisePropertyChangedEvent("CurrentParagpaph");
                RaisePropertyChangedEvent("SelectedParagpaphText");
            }
        }

        public string SelectedParagpaphText
        {
            get
            {
                if (CurrentParagpaph == null) return null;
                
                var text = 
                    new TextRange(CurrentParagpaph.ContentStart, CurrentParagpaph.ContentEnd).Text;
                return text;
            }
            set { return; }
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
    }
}
