﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using MemOrg.Interfaces;

namespace GraphDrawService.Draw
{
    class Text : Component
    {
        private readonly FormattedText _text;

        public Text(string text, IDrawStyle style)
        {
            _text = new FormattedText(text, CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight, style.TextTypeface, style.TextEmSize, style.TextBrush);
            _text.MaxTextWidth = style.MaxTextBlockWidth;
            _text.MaxTextHeight = style.MaxTextBlockHeight;
        }

        public override List<Visual> Render(Point p)
        {
            var dv = new DrawingVisual();
            using (var dc = dv.RenderOpen())
            {
                dc.DrawText(_text, p);
            }
            return new List<Visual> { dv };
        }

        public override Size GetActualSize()
        {
            return new Size(_text.Width, _text.Height);
        }
    }
}

/*
 *   private enum FormatType
        {
            Bold,
            Italic
        };

        private struct FormatUnit
        {
            public FormatType Type;
            public int StartIndex;
            public int Count;
        };

        private static FormattedText CreateFormattedText(Block block, double desiredTextWidth)
        {
            var funits = new List<FormatUnit>();
            var pos = 0;
            var sb = new StringBuilder();
            sb.Append(block.Caption);
            sb.Append("\r\n");
            pos += block.Caption.Length + 2;
            funits.Add(new FormatUnit{StartIndex = 0, Count = pos, Type = FormatType.Bold});
            
            foreach (var particle in block.Particles.OrderBy(t => t.Order))
            {
                if (particle is SourceTextParticle)
                {
                    var content = (particle as SourceTextParticle).Content;
                    sb.Append(content);
                    sb.Append("\r\n");
                    pos += content.Length + 2;
                }
                else if (particle is QuoteSourceParticle)
                {
                    var content = (particle as QuoteSourceParticle).SourceTextParticle.Content;
                    sb.Append(content);
                    sb.Append("\r\n");
                    funits.Add(new FormatUnit {StartIndex = pos, Count = content.Length + 2, Type = FormatType.Italic});
                    pos += content.Length + 2;
                }
            }

            var ft = new FormattedText(sb.ToString(),
                CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Times New Roman"), 12, Brush);
            ft.MaxTextWidth = desiredTextWidth;
            foreach (var formatUnit in funits)
            {
                if (formatUnit.Type == FormatType.Bold)
                    ft.SetFontWeight(FontWeights.Bold, formatUnit.StartIndex, formatUnit.Count);
                if (formatUnit.Type == FormatType.Italic)
                    ft.SetFontStyle(FontStyles.Italic, formatUnit.StartIndex, formatUnit.Count);
            }

            return ft;
        }
 */
