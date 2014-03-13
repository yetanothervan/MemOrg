using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using GraphVizualizeService.VisualElems;
using MemOrg.Interfaces;
using MemOrg.Interfaces.OrgUnits;

namespace GraphVizualizeService
{
    public class VisualTree : IVisual, ITree, IComponent
    {
        private readonly ITree _tree;
        private IComponent _mySelf;

        public VisualTree(ITree tree)
        {
            _tree = tree;
        }

        public IComponent Visualize(IDrawer drawer, IVisualizeOptions options)
        {
            _mySelf = drawer.DrawTree();
            
            var rootComp = new VisualGridElemTag(_tree.MyElem).Visualize(drawer, options);
            _mySelf.Childs = new List<IComponent> {rootComp};

            if (_tree.Subtrees != null)
                foreach (var subtree in _tree.Subtrees)
                {
                    var vst = new VisualTree(subtree);
                    IComponent st = vst.Visualize(drawer, options);
                    _mySelf.Childs.Add(st);
                }

            return _mySelf;
        }

        public List<IComponent> Childs
        {
            get { return _mySelf.Childs; }
            set { _mySelf.Childs = value; }
        }

        public List<DrawingVisual> Render(Point p)
        {
            return _mySelf.Render(p);
        }

        public Size GetSize()
        {
            return _mySelf.GetSize();
        }

        public double RenderWidth { get; set; }
        public double RenderHeight { get; set; }
        public HorizontalAligment HorizontalAligment { get; set; }
        public VerticalAligment VerticalAligment { get; set; }

        public IOrgTag MyElem { get; set; }
        public ICollection<ITree> Subtrees { get; set; }
    }
}