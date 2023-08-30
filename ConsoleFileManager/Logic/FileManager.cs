using ConsoleFileManager.Models;
using ConsoleFileManager.UI;
using Microsoft.VisualBasic;

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
            if (directory.Name.Contains("System Volume Information") || directory.Name.Contains("$RECYCLE.BIN"))
                continue;
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

    public static void DeleteFile(DirectoryFIleModel directoryFIleModel)
    {
        if (directoryFIleModel.fileType == FileType.File)
        {
            File.SetAttributes(directoryFIleModel.path, FileAttributes.Normal);
            File.Delete(directoryFIleModel.path);
        }
    }

    public static void RenameFile(DirectoryFIleModel directoryFIleModel, string desinationPath)
    {
        if (directoryFIleModel.fileType == FileType.File)
        {
            File.Move(directoryFIleModel.path, desinationPath);
        }
    }

    public static void CreateFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
        }
    }
}