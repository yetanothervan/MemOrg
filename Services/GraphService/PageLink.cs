using MemOrg.Interfaces;

namespace GraphService
{
    public class PageLink : IPageLink
    {
        public IPage OppPage { get; set; }
        public PageLinkType LinkType { get; set; }
        public string RelName { get; set; }
    }
}