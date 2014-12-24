using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using VizualAlgoGeom.Config;

namespace VizualAlgoGeom
{
  internal static class Program
  {
    /// <summary>
    ///   The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      CultureInfo cultureInfo = CommonConfigSection.DefaultSection.Culture.Info;
      Thread.CurrentThread.CurrentUICulture = cultureInfo;
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new MainForm());
      CommonConfig.SaveConfig();
    }
  }
}