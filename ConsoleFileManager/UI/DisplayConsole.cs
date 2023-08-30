using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using ConsoleFileManager.Logic;
using ConsoleFileManager.Models;

namespace ConsoleFileManager.UI;

public class DisplayConsole
{
    public static DirectoryFIleModel DisplayFirstBox(List<DirectoryFIleModel> items)
    {
        Console.SetCursorPosition(0, 0);
        Console.CursorVisible = false;

        int selectedIndex = 0;
        int displayOffset = 5;

        while (true)
        {
            Console.Clear();

            for (int i = selectedIndex - displayOffset; i < items.Count && i - selectedIndex + displayOffset < Console.WindowHeight - 1; i++)
            {
                if (i < 0 || i - selectedIndex + displayOffset >= Console.WindowHeight)
                {
                    continue;
                }

                var selectedItem = items[i];
                Console.SetCursorPosition(0, i - selectedIndex + displayOffset);
                if (selectedItem.fileType == FileType.Directory)
                    Console.ForegroundColor = ConsoleColor.Green;
                else
                    Console.ForegroundColor = ConsoleColor.White;

                if (i == selectedIndex)
                {
                    Console.Write($"> {selectedItem}");
                    if (items[selectedIndex].fileType == FileType.Directory)
                    {
                        DisplaySecondBox(items[selectedIndex]);
                    }
                }
                else
                {
                    Console.Write($"{selectedItem}");
                }
                if (items[selectedIndex].fileType != FileType.Directory)
                {
                    Console.SetCursorPosition(Console.WindowWidth - selectedItem.lastEditDate.ToString().Length, i - selectedIndex + displayOffset);
                    Console.Write(selectedItem.lastEditDate.ToString());
                }
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            string displayedPath = new DirectoryInfo(items[selectedIndex].path).Parent.FullName;
            Console.Write($"{displayedPath}");

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (keyInfo.Key != ConsoleKey.VolumeMute)
            {

                if (keyInfo.Key == ConsoleKey.UpArrow && (keyInfo.Modifiers & ConsoleModifiers.Control) != 0)
                {
                    selectedIndex = 0;
                }

                else if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    selectedIndex = Math.Max(0, selectedIndex - 1);
                }

                else if (keyInfo.Key == ConsoleKey.Q)
                {
                    return new DirectoryFIleModel(fileName: "exit");
                }

                else if (keyInfo.Key == ConsoleKey.DownArrow && (keyInfo.Modifiers & ConsoleModifiers.Control) != 0)
                {
                    selectedIndex = items.Count - 1;
                }

                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    selectedIndex = Math.Min(items.Count - 1, selectedIndex + 1);
                }

                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    Console.Clear();
                    Console.WriteLine($"Selected: {items[selectedIndex]}");
                    if (items[selectedIndex].fileType == FileType.File)
                        OpenFile(items[selectedIndex]);
                    else
                    {
                        Console.CursorVisible = true;
                        return items[selectedIndex];
                    }
                }

                else if (keyInfo.Key == ConsoleKey.Escape || keyInfo.Key == ConsoleKey.LeftArrow)
                {
                    Console.Clear();
                    Console.WriteLine($"Selected: {items[selectedIndex]}");
                    return GetParentFolder(items[selectedIndex]);
                }

                else if (keyInfo.Key == ConsoleKey.C)
                {
                    Console.Clear();
                    Console.WriteLine($"Copied: {items[selectedIndex]}");
                }

                else if (keyInfo.Key == ConsoleKey.V)
                {
                    Console.Clear();
                    Console.WriteLine($"Paste: {items[selectedIndex]}");
                }

                else if (keyInfo.Key == ConsoleKey.Delete)
                {
                    Console.Clear();
                    Console.WriteLine($"Delete: {items[selectedIndex]}");
                    FileManager.DeleteFile(items[selectedIndex]);
                    //items.RemoveAt(selectedIndex);
                    return RefreshView(items[selectedIndex]);
                }

                else if (keyInfo.Key == ConsoleKey.R)
                {
                    Rename(items[selectedIndex]);
                    return RefreshView(items[selectedIndex]);
                }

                else if (keyInfo.Key == ConsoleKey.N)
                {
                    CreateNewFile(items[selectedIndex]);
                    return RefreshView(items[selectedIndex]);
                }

            }
        }
    }

    static void CreateNewFile(DirectoryFIleModel directoryFIleModel)
    {
        var filePath = Path.Combine(RefreshView(directoryFIleModel).path, CommandBar("Enter new file name:"));
        FileManager.CreateFile(filePath);
    }

    static DirectoryFIleModel RefreshView(DirectoryFIleModel directoryFIleModel)
    {
        var s = directoryFIleModel.path.Split(@"\");
        if (s.Length <= 1)
            s = directoryFIleModel.path.Split(@"/");
        var dir = new DirectoryInfo(string.Join("/", s)).Parent;
        return new DirectoryFIleModel(dir.Name, dir.FullName, 0, dir.LastWriteTime, FileType.Directory);
    }

    static void ClearCurrentConsoleLine()
    {
        int currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, currentLineCursor);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, currentLineCursor);
    }

    static string CommandBar(string hintString)
    {
        Console.CursorVisible = true;
        Console.SetCursorPosition(0, Console.WindowHeight - 1);
        ClearCurrentConsoleLine();
        Console.WriteLine(hintString);
        var output = Console.ReadLine();
        Console.CursorVisible = false;
        return output;
    }

    static void Rename(DirectoryFIleModel directoryFIleModel)
    {

        if (directoryFIleModel.fileType == FileType.File)
        {
            var newName = CommandBar(directoryFIleModel.fileName);

            if (newName == null)
                return;

            string regex = @"\.[a-zA-Z]{1,9}";
            MatchCollection matchCollection = Regex.Matches(directoryFIleModel.fileName, regex);
            MatchCollection matchCollection2 = Regex.Matches(newName, regex);
            if(matchCollection.Count < 1 || matchCollection2.Count < 1 || matchCollection[0].ToString() != matchCollection2[0].ToString())
                return;

            var newFullName = directoryFIleModel.path.Split(@"\");
            if (newFullName.Length <= 1) 
                newFullName = directoryFIleModel.path.Split(@"/");
            newFullName[newFullName.Length - 1] = newName;

            FileManager.RenameFile(directoryFIleModel, string.Join("/", newFullName));

        }
        Console.CursorVisible = false;
    }

    private static void OpenFile(DirectoryFIleModel directoryFIleModel)
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = directoryFIleModel.path,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private static DirectoryFIleModel GetParentFolder(DirectoryFIleModel child)
    {
        var parentDirectory = new DirectoryInfo(child.path).Parent.Parent;
        if (parentDirectory == null)
            return child;

        return new DirectoryFIleModel(parentDirectory.Name, parentDirectory.FullName, 0, parentDirectory.LastWriteTime, FileType.Directory);
    }

    public static void DisplaySecondBox(DirectoryFIleModel directoryFIleModel)
    {
        var items = FileManager.ReadDirectory(directoryFIleModel.path);

        Console.SetCursorPosition(Console.WindowWidth / 2, 0);
        for (int i = 0; i < Console.WindowHeight - 1; i++)
        {
            Console.SetCursorPosition(Console.WindowWidth / 2, i);
            Console.Write("|");
        }

        for (int i = 0; i < items.Length && i < Console.WindowHeight - 1; i++)
        {
            if (items[i].fileType == FileType.Directory)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.SetCursorPosition(Console.WindowWidth / 2 + 1, i);
            Console.Write($"{items[i]}");
        }
    }
}