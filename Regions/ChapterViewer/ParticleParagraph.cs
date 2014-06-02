using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using DAL.Entity;
using MemOrg.Interfaces;
using Microsoft.Practices.Prism.Commands;

namespace ChapterViewer
{
    public class ParticleParagraph : Paragraph
    {
        private readonly Particle _particle;
        private readonly ParagraphType _type;
        private readonly string _text;

        public ParticleParagraph(Particle particle, ParagraphType type)
        {
            _particle = particle;
            _type = type;

            Over = false;
            _isSelected = false;

            if (particle is UserTextParticle)
                _text = (particle as UserTextParticle).Content;
            else if (particle is SourceTextParticle)
                _text = (particle as SourceTextParticle).Content;
            else if (particle is QuoteSourceParticle)
                _text = (particle as QuoteSourceParticle).SourceTextParticle.Content;
            else
                throw new NotImplementedException();

            Editible = (particle is SourceTextParticle) || (particle is UserTextParticle);

            switch (type)
            {
                case ParagraphType.SourceMixedQuotes:
                    Background = Brushes.Tan;
                    break;
                case ParagraphType.SourceBlockTagQuotes:
                    Background = Brushes.Gray;
                    break;
                case ParagraphType.SourceNoQuotes:
                    Background = Brushes.White;
                    break;
                case ParagraphType.SourceBlockQuotes:
                    Background = Brushes.LightGray;
                    break;
                case ParagraphType.SourceBlockRelQuotes:
                    Background = Brushes.Yellow;
                    break;
            }

            Init();
        }
        
        public ParticleParagraph(string caption)
        {
            Editible = false;
            _text = caption;
            Over = false;
            _isSelected = false;
            Init();
        }

        public bool Editible { get; private set; }

        private void Init()
        {
            Inlines.Add(new Run(_text));
            UpdateView();

            this.BorderThickness = new Thickness(1);

            this.MouseEnter += (sender, args) =>
            {
                if (Over) return;
                Over = true;
                UpdateView();
            };

            this.MouseLeave += (sender, args) =>
            {
                if (Over == false) return;
                Over = false;
                UpdateView();
            };
        }

        public Particle MyParticle
        {
            get
            {
                return _particle;
            }
        }

        public string GetText()
        {
            return _text;
        }

        public bool Over { get; private set; }

        void UpdateView()
        {
            if (IsSelected)
            {
                this.BorderBrush = Brushes.Red;
                this.BorderThickness = new Thickness(2);
            }
            else
            {
                this.BorderThickness = new Thickness(1);
                this.BorderBrush = Over
                    ? Brushes.Red
                    : Brushes.Transparent;
            }
        }
        
        private bool _isSelected;
        
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                UpdateView();
            }
        }
    }
}
