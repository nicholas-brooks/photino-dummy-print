This is a Photino.NET based application that demonstrates the ability to
request the Printer Name from the user using the WinForm's PrintDialog and
pretends to print.

In a real application, the process might be:

1. The web-client would send a command to the native-client (Photino.NET based app) using
`window.external.sendMessage(...)` to print a document at a file-path or url.
2. The native-client would fetch the document, prompt the user which printer to print to,
then send the document to the printer perhaps using a third-party (e.g. Spire.PDF for a
PDF document).
3. The native-client could then notify the web-client when the document was printed using
`mainWindow.SendWebMessage(...)`.