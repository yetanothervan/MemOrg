using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphOrganizeService.Elems;
using MemOrg.Interfaces;
using MemOrg.Interfaces.GridElems;

namespace GraphOrganizeService
{
    public class LayoutCamomile : IGridLayout
    {
        private readonly IGraph _graph;

        public LayoutCamomile(IGraph graph)
        {
            _graph = graph;
        }

        public IGrid CreateGrid()
        {
            var grid = new Grid();

            foreach (var book in _graph.Books)
            {
                int ci = 0;
                foreach (var chapter in book.Chapters)
                {
                    int pi = 0;
                    var selem = new GridElemBlockSource(chapter.ChapterBlock, grid);
                    selem.PlaceOn(pi--, ci);
                    foreach (var page in chapter.PagesBlocks)
                    {

                        GridElemBlock elem;
                        if (page.IsBlockTag)
                            elem = new GridElemBlockTag(page.Block, page.Tag, grid);
                        else
                            elem = new GridElemBlockOthers(page.Block, grid);
                            
                        elem.PlaceOn(pi, ci);
                        --pi;
                    }
                    ++ci;
                }
            }

            return grid;
        }
    }
}
