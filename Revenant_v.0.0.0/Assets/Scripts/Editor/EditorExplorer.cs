#if UNITY_EDITOR

using System.Diagnostics;
using System.IO;
using UnityEditor;

[InitializeOnLoad]
public class EditorExplorer
{
   [MenuItem("Explorer/Project Root Folder", priority = 10)]
   public static void OpenProjectRootFolder()
   {
      string currentDirectory = Directory.GetCurrentDirectory();
      string patcherPath = Path.Combine(currentDirectory, "..");
      openExplorer(patcherPath);
   }

   [MenuItem("Explorer/Project Folder", priority = 11)]
   public static void OpenProjectFolder()
   {
      openExplorer(Directory.GetCurrentDirectory());
   }

   private static void openExplorer(string path)
   {
      var processInfo = new ProcessStartInfo();
      processInfo.FileName = "explorer.exe";
      processInfo.Arguments = path;
      Process.Start(processInfo);
   }
}

#endif