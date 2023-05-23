// See https://aka.ms/new-console-template for more information

using System.Text;
using ICSharpCode.SharpZipLib.Zip;

Console.WriteLine("Hello, World!");
using var fs     = new FileStream(@"E:\\zip.zip", FileMode.Create);
using var zip    = new ZipOutputStream(fs, StringCodec.FromEncoding(Encoding.UTF8));
var       entry  = new ZipEntry(@"folder/");
var       entry1 = new ZipEntry(@"folder/1.txt");
zip.SetLevel(3);
zip.PutNextEntry(entry);
zip.CloseEntry();
zip.PutNextEntry(entry1);
zip.Write(new byte[] { 0x69, 0x91, 0x71 });
zip.CloseEntry();
zip.Flush();
zip.Close();