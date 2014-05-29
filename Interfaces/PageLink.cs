namespace MemOrg.Interfaces
{
    public class PageLink
    {
        public IPage OppPage { get; set; }
        public PageLinkType LinkType { get; set; }
        public string RelName { get; set; }
    }
}