using PhotinoNET;

namespace PhotinoPrint;

static class Program
{
    private static PhotinoWindow? mainWindow;

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        
        mainWindow = new PhotinoWindow();
        mainWindow.SetTitle("Dummy Printing")
            .RegisterWebMessageReceivedHandler(HandleMessageFromClientDelegate)
            .SetUseOsDefaultSize(false)
            .SetResizable(true)
            .SetWidth(800)
            .SetHeight(600)
            .Center()
            .LoadRawString(GetWebClient())
            .WaitForClose();
    }

    private static string GetWebClient()
    {
        var assembly = typeof(Program).Assembly;
        if (assembly is null) throw new Exception("WTF!");
        
        using var resource = assembly.GetManifestResourceStream($"{nameof(PhotinoPrint)}.webclient.html");
        if (resource is null) throw new Exception("Unable to get embedded resource webclient.html");

        using var reader = new StreamReader(resource);
        return reader.ReadToEnd();
    }

    private static void HandleMessageFromClientDelegate(object? sender, string message)
    {
        if (message == "print")
        {
            HandlePrintRequest();
        }   
    }

    private static void HandlePrintRequest()
    {
        /*
         * In a real application, we would be retrieving a document,
         * then using some third-party library, we would print the
         * document to the selected printer.
         */
        var printerName = CapturePrinterNameFromUser();
        if (printerName is null)
        {
            SendMessageToClient("No printer selected");
            return;
        }
        
        SendMessageToClient($"Printing to {printerName} ...");

        // todo: do the actual printing...
        Thread.Sleep(1000);
        
        SendMessageToClient($"Document printed");
    }

    private static string? CapturePrinterNameFromUser()
    {
        using var dialog = new PrintDialog();
        dialog.AllowSelection = false;
        dialog.AllowCurrentPage = false;
        dialog.AllowSomePages = false;
        return dialog.ShowDialog() == DialogResult.OK ? dialog.PrinterSettings.PrinterName : null;
    }

    private static void SendMessageToClient(string message)
    {
        mainWindow?.SendWebMessage(message);
    }
}