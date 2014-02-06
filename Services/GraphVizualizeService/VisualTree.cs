using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using GraphOrganizeService.VisualElems;
using GraphVizualizeService.VisualElems;
using MemOrg.Interfaces;
using MemOrg.Interfaces.GridElems;

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
            
            if (!(_tree.MyElem is IGridElemTag))
                throw new NotImplementedException();
            
            var rootComp = new VisualGridElemTag(_tree.MyElem as IGridElemTag).Visualize(drawer, options);
            _mySelf.Childs = new List<IComponent> {rootComp};

            if (_tree.Subtrees != null)
                foreach (var subtree in Subtrees)
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

        public IGridElem MyElem { get; set; }
        public ICollection<ITree> Subtrees { get; set; }
    }
}