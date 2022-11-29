using System.IO;
namespace BFF.Tests.Support.Database;
public static class BffTestDbHelper
{
    public static string DatabaseFolderLocation => $"{Directory.GetCurrentDirectory()}/../../../Support/Database/";
}
