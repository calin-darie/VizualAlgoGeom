using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VizualAlgoGeom.Config;

namespace VizualAlgoGeom
{
  static class Program
  {
    [STAThread]
    static void Main()
    {
      CultureInfo cultureInfo = CommonConfigSection.DefaultSection.Culture.Info;
      Thread.CurrentThread.CurrentUICulture = cultureInfo;
      AppDomain.CurrentDomain.UnhandledException += async (sender1, unhandledExceptionEventArgs) => 
          await HandleFatalExceptionAndExit(unhandledExceptionEventArgs.ExceptionObject);
      Application.ThreadException += async (sender, threadExceptionEventArgs) => 
          await HandleFatalExceptionAndExit(threadExceptionEventArgs.Exception);
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new MainForm());
      CommonConfig.SaveConfig();
    }

    public static async Task HandleFatalExceptionAndExit(object exception)
    {
      MessageBox.Show(
        string.Format(Translations.Program_PleaseSendIssueReport,IssueReporting.Folder), 
        Translations.Program_FatalError, 
        MessageBoxButtons.OK,
        MessageBoxIcon.Error);
      await IssueReporting.FatalException(exception);
      Application.Exit();
    }
  }
}