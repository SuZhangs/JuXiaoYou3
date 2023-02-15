// See https://aka.ms/new-console-template for more information

using Acorisoft.FutureGL.MigaDB.Data.DataParts;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Modules;
using Acorisoft.FutureGL.MigaDB.Utils;

var datas = PartOfModule.CreateSampleData();

Console.WriteLine("PlainText:\n");
File.WriteAllText("E:\\plaintext.txt", datas.ToPlainText());
Console.WriteLine("Markdown:\n");
File.WriteAllText("E:\\plaintext.md", datas.ToMarkdown());
Console.WriteLine("Hello, World!");