namespace ConsoleFileManager.Models;

public class DirectoryFIleModel
{
    public string fileName;
    public string path;
    public int size;
    public DateTime lastEditDate;
    public FileType fileType;

    public DirectoryFIleModel(string fileName, string path, int size, DateTime lastEditDate, FileType fileType)
    {
        this.fileName = fileName;
        this.path = path;
        this.size = size;
        this.lastEditDate = lastEditDate;
        this.fileType = fileType;
    }

    public override string ToString()
    {
        return fileName;
    }
}

public enum FileType
{
    File = 0,
    Directory = 1,
}