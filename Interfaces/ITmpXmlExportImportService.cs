﻿namespace MemOrg.Interfaces
{
    public interface ITmpXmlExportImportService
    {
        void SaveGraph();
        void LoadGraph();
        void SaveGraph(string path);
        void LoadGraph(string path);
    }
}
