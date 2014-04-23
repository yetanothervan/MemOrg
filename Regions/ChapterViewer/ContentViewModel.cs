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
                
                doc.Blocks.Add(paragraph);
            }
            
            Document = doc;
            Document.MouseDown += (sender, args) =>
            {
                if (Equals(args.Source, Document))
                    foreach (var b in Document.Blocks.OfType<ParticleParagraph>())
                        b.IsSelected = false;
                else
                    foreach (var b in Document.Blocks.OfType<ParticleParagraph>())
                    {
                        b.IsSelected = b.Over;
                        if (b.Over)
                        {
                            CurrentParagpaph = b;
                            CurrentParagraphContent = b.MyContent;
                        }
                    }
            };
        }

        public void ParagraphBlur()
        {
            foreach (var b in Document.Blocks.OfType<ParticleParagraph>())
                b.IsSelected = false;
        }

        public ParticleParagraph CurrentParagpaph { get; set; }
        public Paragraph CurrentParagraphContent { get; set; }


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
