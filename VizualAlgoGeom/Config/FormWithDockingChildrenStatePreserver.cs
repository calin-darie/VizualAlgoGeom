using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Darwen.Windows.Forms.Controls.Docking;

namespace VizualAlgoGeom.Config
{
  public class FormWithDockingChildrenStatePreserver : FormStatePreserver
  {
    FormWithDockingChildrenStateConfigSection _config;
    readonly IPreservableFormWithDockingChildren _form;

    public FormWithDockingChildrenStatePreserver(
      IPreservableFormWithDockingChildren form, string sectionName)
      : base(form, sectionName)
    {
      _form = form;
    }

    protected override void GetSection(string sectionName)
    {
      _config = CommonConfig.GetSection(sectionName)
        as FormWithDockingChildrenStateConfigSection;
      _Config = _config;
    }

    protected override void RestoreFromConfigFile(object sender, EventArgs eargs)
    {
      base.RestoreFromConfigFile(sender, eargs);
      foreach (KeyValuePair<string, IDockingControl> entry in _form.PreservableDockingControls)
      {
        DockingControlStateConfigElement cfgElement =
          _config.DockingControlConfigElementCollection[entry.Key];
        IDockingControl targetControl = _form.PreservableDockingControls[entry.Key];

        targetControl.AutoHide = cfgElement.AutoHide;
        targetControl.DockIndex = cfgElement.DockIndex;


        bool cancelled = cfgElement.Cancelled;
        DockingType dockingType = cfgElement.DockingType;

        if (dockingType == DockingType.Floating)
        {
          if (false == cancelled)
          {
            if (
              (cfgElement.LeftIfFloating.HasValue) &&
              (cfgElement.TopIfFloating.HasValue) &&
              (cfgElement.WidthIfFloating.HasValue) &&
              (cfgElement.HeightIfFloating.HasValue)
              )
              targetControl.FloatControl(new Rectangle(
                cfgElement.LeftIfFloating.Value,
                cfgElement.TopIfFloating.Value,
                cfgElement.WidthIfFloating.Value,
                cfgElement.HeightIfFloating.Value
                ));
            /*XXX else float like when unpinning by mouse click
                         * ... but how ? */
          }
        }
        else
        {
          targetControl.DockControl(
            cfgElement.PanelIndex,
            cfgElement.DockIndex,
            cfgElement.DockingType
            );
          targetControl.DockedDimension = cfgElement.DockDimension;
        }
        targetControl.Cancelled = cancelled;
      }
    }

    protected override void SaveToConfigFile(object sender, FormClosingEventArgs eArgs)
    {
      foreach (KeyValuePair<string, IDockingControl> entry in _form.PreservableDockingControls)
      {
        DockingControlStateConfigElement cfgElement =
          _config.DockingControlConfigElementCollection[entry.Key];
        IDockingControl control = _form.PreservableDockingControls[entry.Key];

        cfgElement.AutoHide = control.AutoHide;
        cfgElement.Cancelled = control.Cancelled;
        DockingType dockingType = control.DockingType;
        cfgElement.DockingType = dockingType;
        if (dockingType == DockingType.Floating)
        {
          Rectangle controlBounds = control.FloatingBounds;
          cfgElement.TopIfFloating = controlBounds.Top;
          cfgElement.LeftIfFloating = controlBounds.Left;
          cfgElement.WidthIfFloating = controlBounds.Width;
          cfgElement.HeightIfFloating = controlBounds.Height;
        }
        else
        {
          cfgElement.DockIndex = control.DockIndex;
          cfgElement.DockDimension = control.DockedDimension;
        }
      }
      base.SaveToConfigFile(sender, eArgs);
    }
  }
}