using System.Windows.Media;

namespace GraphDrawService
{
    public class GridElemStyle
    {
        public const double TextMaxWidth = 200;

        public static readonly Brush SourceBlockBrush;
        public static readonly Brush BlockBrush;
        public static readonly Brush TagBrush;
        public static readonly Brush RelationBrush;
        public static readonly Brush UserBlockBrush;

        //private static readonly Pen Pen;


        static GridElemStyle()
        {
            (SourceBlockBrush = new SolidColorBrush(Color.FromRgb(100, 100, 100))).Freeze();
            (BlockBrush = new SolidColorBrush(Color.FromRgb(100, 100, 100))).Freeze();
            (TagBrush = new SolidColorBrush(Color.FromRgb(100, 100, 100))).Freeze();
            (RelationBrush = new SolidColorBrush(Color.FromRgb(100, 100, 100))).Freeze();
            (UserBlockBrush = new SolidColorBrush(Color.FromRgb(100, 100, 100))).Freeze();
            
          //  Pen = new Pen(Brush, 2.0);
            //Brush.Freeze();
            //Pen.Freeze();
        }
    }
}
