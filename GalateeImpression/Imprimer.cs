using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Printing;


namespace GalateeImpression
{
    public class Imprimer
    {



    #region ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="PdfFilePrinter"/> class.
    /// </summary>
    /// <param name="pdfFilePath">File Path of the PDF.</param>
    /// <param name="printerName">Name of the printer.</param>
        public Imprimer(string pdfFilePath = "", string printerName = "")
    {
      if (pdfFilePath != String.Empty)
      {
        PdfFullFilePath = pdfFilePath;
      }
      if (printerName != String.Empty)
      {
        PrinterName = printerName;
      }
      _adobeReaderPath = GetAcrobatReaderPath();

      Print(10000);
    }
    #endregion




    #region PDF File Path
    private string _pdfFileName;
    private string _workingDirectory;
    private string _pdfFullFilePath;

    /// <summary>
    /// Gets or sets the PDF file to print
    /// </summary>
    public string PdfFullFilePath
    {
      get { return _pdfFullFilePath; }
      set
      {
        if (!File.Exists(value))
          throw new InvalidOperationException(String.Format("The file {0} does not exist.", value));

        _pdfFileName = Path.GetFileName(value);
        _workingDirectory = Path.GetDirectoryName(value);

        _pdfFullFilePath = value;
      }
    }
    #endregion



    #region Printer Name
    private string _printerName;
    /// <summary>
    /// Gets or sets the name of the printer. 
    /// </summary>
    /// <value>The name of the printer.</value>
    /// <example>'\\myserver\HP LaserJet PCL5'</example>
    public string PrinterName
    {
      get
      {
        if (String.IsNullOrEmpty(_printerName))
        {
          PrinterName = "default";
        }
        return _printerName;
      }
      set
      {
        if (_printerName == value) return;

        using (var printServer = new System.Printing.LocalPrintServer())
        {
          var printer = printServer.GetPrintQueues().FirstOrDefault(p => p.FullName == value);

          _printerName = printer == null ?
            printServer.DefaultPrintQueue.FullName : printer.FullName;
        }
      }
    }
    #endregion



    #region Print
    /// <summary>
    /// Prints the PDF file.
    /// </summary>
    /// <param name="milliseconds">The number of milliseconds to wait for completing the print job.</param>
    public void Print(int milliseconds = -1)
    {
      if (String.IsNullOrEmpty(PdfFullFilePath))
      {
        throw new NullReferenceException("there is no file to print");
      }
      GetAcrobatProcess();
      var args = String.Format("/t \"{0}\" \"{1}\"", _pdfFileName, PrinterName);
      var startInfo = new ProcessStartInfo
                                     {
                                       FileName = _adobeReaderPath,
                                       Arguments = args,
                                       CreateNoWindow = true,
                                       ErrorDialog = false,
                                       UseShellExecute = false,
                                       WorkingDirectory = _workingDirectory

                                     };

      var process = Process.Start(startInfo);
      if (!process.WaitForExit(milliseconds))
      {
        process.Kill();
      }
    }
    #endregion




    #region GetAcrobatProcess
    private static Process _runningAcro;

    private void GetAcrobatProcess()
    {
      if (_runningAcro != null)
      {
        if (!_runningAcro.HasExited) return;

        _runningAcro.Dispose();
        _runningAcro = null;
      }

      var processes = Process.GetProcesses();
      _runningAcro = processes.FirstOrDefault(p => p.Modules[0].FileName == _adobeReaderPath);
      if (_runningAcro != null) return;

      _runningAcro = Process.Start(_adobeReaderPath);
      if (_runningAcro != null) _runningAcro.WaitForInputIdle();
    }
    #endregion




    #region Get Acrobat Reader Path
    private readonly string _adobeReaderPath;
    private static string GetAcrobatReaderPath()
    {
      const string keyBase = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths";
      const string fileName = "AcroRd32.exe";

      var localMachine = Registry.LocalMachine;
      using (var fileKey = localMachine.OpenSubKey(string.Format(@"{0}\{1}", keyBase, fileName)))
      {
        if (fileKey != null)
        {
          return fileKey.GetValue(string.Empty).ToString();
        }
      }
      throw new Exception(string.Format("no installation of {0} found", fileName));
    }
    #endregion



    }
}
