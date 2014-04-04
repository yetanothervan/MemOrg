using System.Collections.Generic;
using System.Linq;
using MemOrg.Interfaces;

namespace GraphOrganizeService.LayoutCamomile
{
    public class ChapterLayoutBundle
    {
        private List<ChapterLayoutBundle> _bundles;
        private List<IPage> _ones;
        private IPage _myElem;
        private IPage _rel;
        private BundleDirection _direction;
        
        private ChapterLayoutBundle()
        {
            _direction = BundleDirection.Root;
            _rel = null;
            _myElem = null;
            _ones = new List<IPage>();
            _bundles = new List<ChapterLayoutBundle>();
        }

        //public static ChapterLayoutBundle ExtractBundleFrom(HashSet<IPage> from, IPage by, IChapter chapter)
        //{
        //    var result = new ChapterLayoutBundle
        //    {
        //        _myElem = by
        //    };
            

        //    var toOnes = new List<IPage>();
        //    var toOnesNew = new List<IPage>();
        //    foreach (var page in by.ReferencedBy)
        //    {
                
                
                
        //        @by.ReferencedBy.(r => @from.Contains(r) && r.ReferencedBy.Count == 1);
                
        //    }
                
        //    foreach (var page in toOnes) from.Remove(page);

        //    var toBundles = by.ReferencedBy.Where(@from.Contains).ToList(); //others
        //    toBundles.AddRange(@by.RelatedBy.Where(@from.Contains)); //rels
        //    foreach (var page in toBundles) from.Remove(page);

        //    result._ones = toOnes;
            

        //    return result;
        //}

        //private IEnumerable<IPage> ExtractBundleFrom(HashSet<IPage> @from, int chapterId, IPage blockPage, List<IPage> res)
        //{
        //    if (res == null)
        //        res = new List<IPage> { blockPage };
        //    else
        //        if (!res.Contains(blockPage)) res.Add(blockPage);

        //    if (blockPage.RelatedBy.Count == 0) return res;

        //    var toAdd = blockPage.RelatedBy.Where(r => !res.Contains(r)
        //        && r.MyChapter.ChapterBlock.BlockId == chapterId).ToList();
        //    res.AddRange(toAdd);

        //    foreach (var rel in toAdd)
        //    {
        //        var next = rel.RelationFirst.Block.BlockId == blockPage.Block.BlockId
        //            ? rel.RelationSecond
        //            : rel.RelationFirst;
        //        ExtractRowForBlockInChapter(@from, chapterId, next, res);
        //    }

        //    return res;
        //}


        //private ChapterLayoutBundle YeildChapterLayoutBundle(HashSet<IPage> @from, IEnumerable<IPage> whatList)
        //{
        //    var enumerable = whatList as IList<IPage> ?? whatList.ToList();
        //    @from.RemoveWhere(enumerable.Contains);
        //    return new ChapterLayoutBundle { Pages = enumerable.ToList() };
        //}
    }
}