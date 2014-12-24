using System;
using System.Drawing;
using System.Windows.Forms;

namespace VizualAlgoGeom.Config
{
  public class FormStatePreserver
  {
    protected FormStateConfigSection _Config;
    readonly IPreservableForm _form;

    public FormStatePreserver(IPreservableForm form, string sectionName)
    {
      form.Visible = false;
      form.Load += RestoreFromConfigFile;
      form.FormClosing += SaveToConfigFile;

      _form = form;
      GetSection(sectionName);
    }

    protected virtual void GetSection(string sectionName)
    {
      _Config = CommonConfig.GetSection(sectionName)
        as FormStateConfigSection;
    }

    protected virtual void RestoreFromConfigFile(object sender, EventArgs e)
    {
      _form.Height = _Config.Height;
      _form.Width = _Config.Width;
      _form.Top = _Config.Top;
      _form.Left = _Config.Left;
      _form.WindowState = _Config.WindowState;
      _form.Visible = true;
    }

    protected virtual void SaveToConfigFile(object sender, FormClosingEventArgs eArgs)
    {
      switch (_form.WindowState)
      {
        case FormWindowState.Minimized:
          Rectangle m_RestoreBounds = _form.RestoreBounds;
          _Config.WindowState = FormStateConfigSection.DefaultWindowState;
          _Config.Top = m_RestoreBounds.Top;
          _Config.Left = m_RestoreBounds.Left;
          _Config.Height = m_RestoreBounds.Height;
          _Config.Width = m_RestoreBounds.Width;
          break;
        case FormWindowState.Maximized:
          _Config.WindowState = _form.WindowState;
          _Config.Top = _form.RestoreBounds.Top;
          _Config.Left = _form.RestoreBounds.Left;
          _Config.Height = _form.RestoreBounds.Height;
          _Config.Width = _form.RestoreBounds.Width;
          break;
        case FormWindowState.Normal:
          _Config.WindowState = _form.WindowState;
          _Config.Top = _form.Top;
          _Config.Left = _form.Left;
          _Config.Height = _form.Height;
          _Config.Width = _form.Width;
          break;
      }
    }
  }
}