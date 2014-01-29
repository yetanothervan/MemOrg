using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using DAL.Entity;
using MemOrg.Interfaces;
using Microsoft.Practices.ServiceLocation;

namespace TmpXmlExportImportService
{
    public class TmpXmlExportImportService : ITmpXmlExportImportService
    {
        public void SaveGraph()
        {
            var graphService = (IGraphService)ServiceLocator.Current.GetService(typeof(IGraphService));
            var g = Graph2XmlGraphConverter.Convert(graphService);
            SerializeGraph(g);
        }


        public static void SerializeGraph(XmlGraph s)
        {
            var xmlSerializer = new XmlSerializer(typeof (XmlGraph));
            var sb = new StringBuilder();
            var w = new StringWriter(sb);
            xmlSerializer.Serialize(w, s);

            var doc = new XmlDocument {InnerXml = sb.ToString()};
            doc.Save("graph.xml");
        }

        /*public static Solution XmlDeserializeSolution(string path)
        {
            Solution result = new Solution();
            TextReader tr = new StreamReader(path);
            XmlSerializer xmlDeserializer = new XmlSerializer(typeof (Solution));
            result = (Solution) xmlDeserializer.Deserialize(tr);
            tr.Close();
            return result;
        }*/
    }
}
