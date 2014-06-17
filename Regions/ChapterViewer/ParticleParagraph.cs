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
        private string _text;

        public ParticleParagraph(Particle particle, ParagraphType type, bool deletable)
        {
            _particle = particle;
            _type = type;

            Over = false;
            _isSelected = false;

            _text = GetTextFromParticle(particle);

            Editible = (particle is SourceTextParticle) || (particle is UserTextParticle);

            Deletable = deletable;

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

        private string GetTextFromParticle(Particle particle)
        {
            if (particle is UserTextParticle)
                return (particle as UserTextParticle).Content;
            if (particle is SourceTextParticle)
                return (particle as SourceTextParticle).Content;
            if (particle is QuoteSourceParticle)
                return (particle as QuoteSourceParticle).SourceTextParticle.Content;
            throw new NotImplementedException();
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

        public void SetText(Particle part)
        {
            if (!(Inlines.FirstInline is Run)) return;
            
            _text = GetTextFromParticle(part);
            (Inlines.FirstInline as Run).Text = _text;
        }

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

        public bool Deletable { get; private set; }
    }
}
