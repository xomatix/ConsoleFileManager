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

            List<string> items2 = new List<string>
            {
                "Item 1",
                "Item 2",
                "Item 3",
                "Item 4",
                "Item 5"
            };

            string basePath = @"/home/thinkpad/code/testEnv";
            DirectoryFIleModel directoryFIleModel = null;

            while (true)
            {
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