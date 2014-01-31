using System;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Media;

namespace MemOrg.Interfaces
{
    public interface IDrawStyle
    {
        double MaxTextBlockWidth { get; set; }
        double MaxTextBlockHeight { get; set; }
        Typeface TextTypeface { get; set; }
        double TextEmSize { get; set; }
        Brush TextBrush { get; set; }
        
        Typeface CaptionTypeface { get; set; }
        double CaptionEmSize { get; set; }
        Brush CaptionBrush { get; set; }

        Brush QuoteBlockBrush { get; set; }
        Pen QuoteBlockPen { get; set; }
    }
}