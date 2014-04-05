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
            IComponent rootComp;
            if (_tree.MyElem is IOrgBlockTag)
                rootComp = new VisualGridElemBlockTag(_tree.MyElem as IOrgBlockTag)
                    .Visualize(drawer, options);
            else if (_tree.MyElem is IOrgBlockRel)
                rootComp = new VisualGridElemBlockRel(_tree.MyElem as IOrgBlockRel)
                    .Visualize(drawer, options);
            else if (_tree.MyElem is IOrgBlockUserText)
                rootComp = new VisualGridElemBlockUserText(_tree.MyElem as IOrgBlockUserText)
                    .Visualize(drawer, options);
            else if (_tree.MyElem is IOrgBlockOthers)
                rootComp = new VisualGridElemBlock(_tree.MyElem as IOrgBlockOthers)
                    .Visualize(drawer, options);
            else if (_tree.MyElem is IOrgTag)
                rootComp = new VisualGridElemTag(_tree.MyElem as IOrgTag)
                    .Visualize(drawer, options);
            else 
                throw new NotImplementedException();
            
            _mySelf.AddChild(rootComp);

            if (_tree.Subtrees != null)
                foreach (var subtree in _tree.Subtrees)
                {
                    var vst = new VisualTree(subtree);
                    IComponent st = vst.Visualize(drawer, options);
                    _mySelf.AddChild(st);
                }

            return _mySelf;
        }

        public IEnumerable<IComponent> Childs
        {
            get { return _mySelf.Childs; }
        }

        public void AddChild(IComponent child)
        {
            _mySelf.AddChild(child);
        }

        public List<Visual> Render(Point p1)
        {
            return _mySelf.Render(p1);
        }

        public Size GetActualSize()
        {
            return _mySelf.GetActualSize();
        }

        public Size? PreferSize { get; set; }
        public object Logical { get; set; }

        public IOrg MyElem { get; set; }
        public ICollection<ITree> Subtrees { get; set; }
    }
}