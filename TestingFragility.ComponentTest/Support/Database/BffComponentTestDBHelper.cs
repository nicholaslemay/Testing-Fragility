using System.IO;

namespace BFF.Component.Tests.Support.Database;

public static class BffComponentTestDbHelper
{ 
    public static string DatabaseFolderLocation => $"{Directory.GetCurrentDirectory()}/../../../Support/Database/";
}