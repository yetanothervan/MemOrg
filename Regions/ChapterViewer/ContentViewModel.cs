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
            var captionParagraph = new Paragraph(new Bold(new Run(block.Caption)));
            doc.Blocks.Add(captionParagraph);
            foreach (var part in block.Particles.OrderBy(b => b.Order))
            {
                Paragraph paragraph;
                if (part is UserTextParticle)
                    paragraph = new Paragraph(new Run((part as UserTextParticle).Content));
                else if (part is SourceTextParticle)
                    paragraph = new Paragraph(new Run((part as SourceTextParticle).Content));
                else if (part is QuoteSourceParticle)
                    paragraph = new Paragraph(new Run((part as QuoteSourceParticle)
                        .SourceTextParticle.Content));
                else
                    throw new NotImplementedException();

                paragraph.BorderThickness = new Thickness(1);

                paragraph.MouseMove += (sender, args) =>
                {
                    var p = sender as Paragraph;
                    if (p != null) p.BorderBrush = Brushes.LightCoral;
                };

                paragraph.MouseLeave += (sender, args) =>
                {
                    var p = sender as Paragraph;
                    if (p != null) p.BorderBrush = Brushes.Transparent;
                };

                doc.Blocks.Add(paragraph);
            }
            
            Document = doc;
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
