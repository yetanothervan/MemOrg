using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using DAL.Entity;
using MemOrg.Interfaces;

namespace GraphOrganizeService
{
    public class VisualGrid : IVisualGrid, IComponent
    {
        private readonly IGrid _grid;

        private readonly List<List<IGridElem>> _elems;

        public VisualGrid(IGrid grid)
        {
            _grid = grid;
            _elems = new List<List<IGridElem>>(RowCount);
            for (int index = 0; index < _elems.Count; ++index)
                _elems[index] = new List<IGridElem>(RowLength);
        }
        
        public void Prerender(IDrawer drawer)
        {
            _mySelf = drawer.DrawGrid();
            foreach (var gridElem in _grid)
            {
                if (gridElem is GridElemBasedOnBlock)
                {
                    var elem = gridElem as GridElemBasedOnBlock;

                    IComponent visElem;
                    switch (elem.Type)
                    {
                        case GridElemBasedOnBlockType.BlockOther:
                        case GridElemBasedOnBlockType.BlockSource:
                        case GridElemBasedOnBlockType.BlockTag:
                        case GridElemBasedOnBlockType.BlockRel:
                        {
                            visElem = drawer.DrawBox();
                            IComponent caption = drawer.DrawCaption(elem.Block.Caption);
                            visElem.Childs.Add(caption);
                            foreach (var part in elem.Block.Particles.OrderBy(o => o.Order))
                            {
                                if (part is SourceTextParticle)
                                {
                                    var t = drawer.DrawQuoteText((part as SourceTextParticle).Content);
                                    visElem.Childs.Add(t);
                                }
                                else if (part is QuoteSourceParticle)
                                {
                                    var t = drawer.DrawQuoteText((part as QuoteSourceParticle).SourceTextParticle.Content);
                                    var qb = drawer.DrawQuoteBox();
                                    qb.Childs.Add(t);
                                    visElem.Childs.Add(qb);
                                }
                                else
                                {
                                    throw new NotImplementedException();
                                }
                            }
                        }
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    _mySelf.Childs.Add(visElem);
                }
                else if (gridElem is GridElemBasedOnTag)
                {
                    
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        public IEnumerator<IGridElem> GetEnumerator()
        {
            return new GridEnumerator(_elems);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int RowCount { get { return _grid.RowCount; } }
        public int RowLength { get { return _grid.RowLength; } }

        private IComponent _mySelf;
        public List<IComponent> Childs
        {
            get { return _mySelf.Childs; }
            set { _mySelf.Childs = value; }
        }
        
        public List<DrawingVisual> Render()
        {
            throw new NotImplementedException();
        }

        public Size GetSize()
        {
            throw new NotImplementedException();
        }
    }
}