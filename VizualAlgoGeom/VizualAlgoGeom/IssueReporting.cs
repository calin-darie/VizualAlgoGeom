using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using VizualAlgoGeom.Config;

namespace VizualAlgoGeom
{
  public static class IssueReporting
  {
    public static readonly string Folder = Path.Combine(Settings.AppSaveFolder, nameof(IssueReporting));
    static readonly IFileSystem FileSystem = new FileSystem();

    public static async Task TakeSnapshot()
    {
      await FileSystem.ZipDirectory(Settings.DataFolder, Path.Combine(Folder, $"data-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.zip"));
    }

    public static void OpenSnapshotFolderAndIssueTracker()
    {
      TryOpen(Settings.IssueTrackerUrl);
      TryOpen(Folder);
    }

    static void TryOpen(string fileOrUrl)
    {
      var cursor = Cursor.Current;
      try
      {
        Cursor.Current = Cursors.AppStarting;
        Process.Start(fileOrUrl);
      }
      catch (Exception ex)
      {
        Logger.Error(ex, "could not open {0}", fileOrUrl);
      }
      finally
      {
        Cursor.Current = cursor;
      }
    }
    
    static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    
    public static async Task FatalException(object exception)
    {
      Logger.Fatal(exception);
      await TakeSnapshot();
      OpenSnapshotFolderAndIssueTracker();
    }
  }
}