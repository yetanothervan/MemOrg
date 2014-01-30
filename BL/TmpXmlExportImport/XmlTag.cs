using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Entity;

namespace TmpXmlExportImportService
{
    public class XmlTag
    {
        public Int32 TagId;
        public Int32 ParentTagId;
        public Int32 TagBlockId;
        public String Caption;

        public static List<XmlTag> Convert(IEnumerable<Tag> tags)
        {
            return tags.Select(tag => new XmlTag
            {
                TagId = tag.TagId,
                ParentTagId = tag.Parent.TagId,
                TagBlockId = tag.TagBlock.BlockId,
                Caption = tag.Caption
            }).ToList();
        }
    }
}