using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using VizualAlgoGeom.Config;

namespace VizualAlgoGeom
{
  public static class IssueReporting
  {
    static readonly string Folder = Path.Combine(Settings.AppSaveFolder, nameof(IssueReporting));
    static readonly IFileSystem FileSystem = new FileSystem();

    public static async Task TakeSnapshot()
    {
      await FileSystem.ZipDirectory(Settings.DataFolder, Path.Combine(Folder, $"data-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.zip"));
    }

    public static void OpenSnapshotFolderAndIssueTracker()
    {
      var cursor = Cursor.Current;
      try
      {
        Cursor.Current = Cursors.AppStarting;
        Process.Start(Settings.IssueTrackerUrl);
      }
      catch (Exception)
      {
      }
      finally
      {
        Cursor.Current = cursor;
      }
      Process.Start(Folder);
    }
  }
}