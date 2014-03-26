using System.Collections;
using System.Collections.Generic;

namespace MemOrg.Interfaces
{
    public class OrgGridElem : GridElem, IOrgGridElem
    {
        public OrgGridElem(IGrid myGrid) : base(myGrid)
        {
            HorizontalContentAligment = HorizontalAligment.Left;
            VerticalContentAligment = VerticalAligment.Top;
        }

        public HorizontalAligment HorizontalContentAligment { get; set; }
        public VerticalAligment VerticalContentAligment { get; set; }
    }
}