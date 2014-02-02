using MemOrg.Interfaces;

namespace GraphVizualizeService
{
    public class VisualizeOptions : IVisualizeOptions
    {
        public VisualizeOptions()
        {
            HeadersOnly = true;
        }
        public bool HeadersOnly { get; set; }
    }
}