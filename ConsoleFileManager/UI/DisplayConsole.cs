using System;
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
        int displayOffset = 3;
        while (true)
        {
            Console.Clear();

            for (int i = selectedIndex - displayOffset; i < items.Count; i++)
            {
                if (i < 0)
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
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    selectedIndex = Math.Max(0, selectedIndex - 1);
                    break;

                case ConsoleKey.DownArrow:
                    selectedIndex = Math.Min(items.Count - 1, selectedIndex + 1);
                    break;

                case ConsoleKey.Enter:
                    Console.Clear();
                    Console.WriteLine($"Selected: {items[selectedIndex]}");
                    Console.CursorVisible = true;
                    return items[selectedIndex];

                default:
                    break;
            }
        }
    }

    public static void DisplaySecondBox(DirectoryFIleModel directoryFIleModel)
    {
        var items = FileManager.ReadDirectory(directoryFIleModel.path);

        Console.SetCursorPosition(Console.WindowWidth / 2, 0);
        for (int i = 0; i < Console.WindowHeight; i++)
        {
            Console.SetCursorPosition(Console.WindowWidth / 2, i);
            Console.Write("|");
        }

        for (int i = 0; i < items.Length; i++)
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