namespace MemOrg.Interfaces
{
    public interface IOrgGridElem : IGridElem
    {
        HorizontalAligment HorizontalContentAligment { get; set; }
        VerticalAligment VerticalContentAligment { get; set; }
    }

    public enum HorizontalAligment
    {
        Left, Right, Center, Stretch
    }

    public enum VerticalAligment
    {
        Top, Bottom, Center, Stretch
    }
}