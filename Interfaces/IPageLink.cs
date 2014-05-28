namespace MemOrg.Interfaces
{
    public interface IPageLink
    {
        IPage OppPage { get; set; }
        PageLinkType LinkType { get; set; }
        string RelName { get; set; }
    }

    public enum PageLinkType
    {
        RelationNoBlockToObject,
        RelationNoBlockToSubject,
        ToRelationBlock,
        ReferenceTo
    }
}