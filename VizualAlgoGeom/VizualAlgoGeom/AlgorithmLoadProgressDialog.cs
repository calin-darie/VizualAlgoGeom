using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using NLog;
using VizualAlgoGeom.AssemblyLoading;

namespace VizualAlgoGeom
{
  public partial class AlgorithmLoadProgressDialog : Form
  {
    ComponentResourceManager _resources;
    readonly AlgorithmLoader _algorithmLoader;
    readonly string _fileName;
    static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public AlgorithmLoadProgressDialog()
      : this(null, null)
    {
    }

    public AlgorithmLoadProgressDialog(AlgorithmLoader algorithmLoader, string fileName)
    {
      _algorithmLoader = algorithmLoader;
      _fileName = fileName;
      InitializeComponent();
      _InitializeComponent();
    }

    void _InitializeComponent()
    {
      _resources = new ComponentResourceManager(
        typeof (AlgorithmLoadProgressDialog));

      _algorithmExecuter.RunWorkerCompleted +=
        _algorithmExecuter_RunWorkerCompleted;
    }

    void _cancelButton_Click(object sender, EventArgs e)
    {
      _algorithmExecuter.Abort();
    }

    void _algorithmExecuter_DoWork(object sender, DoWorkEventArgs e)
    {
      if (_algorithmLoader != null)
        _algorithmLoader.RunAlgorithm(_fileName);
    }
    void _algorithmExecuter_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs args)
    {
      Exception exception = args.Error;
      if (exception != null)
      {
        MessageBox.Show(
          _resources.GetString("please contact author"),
          _resources.GetString("algorithm execution problem"),
          MessageBoxButtons.OK,
          MessageBoxIcon.Error);
        
        Logger.Error(exception, $"error in algorithm {AlgorithmName}");
      }
      else
        DialogResult = DialogResult.OK;
      Close();
    }

    public string AlgorithmName => Path.GetFileName(_fileName)?.Replace(".dll", "") ?? string.Empty;

    public new DialogResult ShowDialog()
    {
      _algorithmExecuter.RunWorkerAsync();
      return base.ShowDialog();
    }
  }
}