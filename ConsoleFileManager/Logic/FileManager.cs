using ConsoleFileManager.Models;
using ConsoleFileManager.UI;

namespace ConsoleFileManager.Logic;

public class FileManager
{
    public static DirectoryFIleModel OpenFolder(string baseFolderPath)
    {
        List<DirectoryFIleModel> items = new List<DirectoryFIleModel>();
        
        items.AddRange(ReadDirectory(baseFolderPath));
        
        return DisplayConsole.DisplayFirstBox(items);
    }

    public static DirectoryFIleModel[] ReadDirectory(string baseFolderPath)
    {
        List<DirectoryFIleModel> items = new List<DirectoryFIleModel>();
        DirectoryInfo directoryInfo = new DirectoryInfo(baseFolderPath);

        foreach (var directory in directoryInfo.GetDirectories())
        {
            var dirObj = new DirectoryFIleModel(directory.Name, directory.FullName, 0, directory.LastWriteTime,
                FileType.Directory);
            items.Add(dirObj);
        }

        foreach (var file in directoryInfo.GetFiles())
        {
            var fileObj = new DirectoryFIleModel(file.Name, file.FullName, 0, file.LastWriteTime,
                FileType.File);
            items.Add(fileObj);
        }

        return items.ToArray();
    } 
}