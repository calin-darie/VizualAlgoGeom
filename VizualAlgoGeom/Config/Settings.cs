using System;
using System.Configuration;
using System.IO;

namespace VizualAlgoGeom.Config
{
  public static class Settings
  {
    public static readonly string AppSaveFolder = Path.Combine(
      Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
      nameof(VizualAlgoGeom));
    public static readonly string DataFolder = Path.Combine(
      AppSaveFolder,
      "data");
    public static readonly string IssueTrackerUrl = ConfigurationManager.AppSettings["issueTrackerUrl"];
  }
}