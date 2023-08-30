using ConsoleFileManager.Logic;
using ConsoleFileManager.Models;
using ConsoleFileManager.UI;

namespace ConsoleFileManager
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();

            string basePath = "";
            if (args.Length == 0)
                basePath = Directory.GetCurrentDirectory();
            else
                basePath = args[0];

            DirectoryFIleModel directoryFIleModel = null;

            while (true)
            {
                if (directoryFIleModel != null && directoryFIleModel.fileName == "exit")
                {
                    return;
                }
                if (directoryFIleModel != null && directoryFIleModel.fileType == FileType.Directory)
                {
                    directoryFIleModel =
                        FileManager.OpenFolder(directoryFIleModel.path);
                }
                else if (directoryFIleModel != null && directoryFIleModel.fileType == FileType.File)
                {
                    Console.WriteLine($"Selected action on {directoryFIleModel.path}");
                    break;
                }
                else
                {
                    directoryFIleModel =
                        FileManager.OpenFolder(basePath);
                }
            }


            //DisplayConsole.DisplaySecondBox();
        }

    }
}