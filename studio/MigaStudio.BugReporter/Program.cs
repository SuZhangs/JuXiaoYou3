// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Drawing;
using Acorisoft.FutureGL.MigaStudio.Models;
using Acorisoft.FutureGL.MigaUtils;
using Acorisoft.FutureGL.MigaUtils.Collections;
using Colorful;
using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;
using Console = Colorful.Console;

namespace Acorisoft.FutureGL.MigaStudio.Tools.BugReporter
{
    public class Program
    {
        private static readonly Color PrimaryColor            = Color.FromArgb(0x98, 0xa1, 0x2b);
        private static readonly Color ObsoletedColor          = Color.FromArgb(0xff, 0x66, 0x00);
        private static readonly Color WarningColor            = Color.FromArgb(0xff, 0xb3, 0x14);
        private static readonly Color DangerColor             = Color.FromArgb(0xd9, 0x08, 0x0c);
        private const           char  ZipEntryFolderCharacter = '/';

        private static bool FormatArgs(string[] args, out BugLevel level, out string dir, out string log)
        {
            if (args is null ||
                args.Length < 3)
            {
                level = BugLevel.Bug;
                dir   = string.Empty;
                log   = string.Empty;
                return false;
            }

            if (!int.TryParse(args[1], out var n))
            {
                level = BugLevel.Bug;
                dir   = string.Empty;
                log   = string.Empty;
                return false;
            }

            level = (BugLevel)n;


            if (string.IsNullOrEmpty(args[0]) ||
                string.IsNullOrEmpty(args[2]))
            {
                level = BugLevel.Bug;
                dir   = string.Empty;
                log   = string.Empty;
                return false;
            }

            dir = args[0];
            log = args[2];
            return true;
        }

        static void Main(string[] args)
        {
            var formatter = new Formatter[]
            {
                new Formatter("橘小柚-修复工具", PrimaryColor),
                new Formatter("任意键", Color.Green),
                new Formatter("橘小柚", PrimaryColor)
            };

            //-------------------------------------
            // 打印开头
            //-------------------------------------
            Console.WriteAscii("JuXiaoYou", PrimaryColor);
            Console.WriteLineFormatted(
                "欢迎使用{0},此工具可以帮助您反馈BUG！",
                Color.LightSlateGray,
                formatter);

            WriteEmptyLine();

            //-------------------------------------
            // 检测任务
            //-------------------------------------
            if (FormatArgs(args, out var bug, out var dir, out var log))
            {
                Console.WriteLineFormatted(
                    "此工具由{2}启动，正在执行自动化操作！\n这个过程比较耗费时间,请耐心等待！\n",
                    Color.LightSlateGray,
                    formatter);
                Report(bug, dir, log);
            }
            else
            {
                Console.WriteLineFormatted(
                    "如果您遇到!",
                    Color.LightSlateGray,
                    formatter);
                // Console.WriteLineFormatted(
                //     "根据工具命令提示，即可完成反馈BUG的操作！\n那接下来让我们开始吧！",
                //     Color.LightSlateGray,
                //     formatter);
                Manual();
            }

            WriteEmptyLine();
            Console.WriteLineFormatted(
                "按下{1}即可退出",
                Color.LightSlateGray,
                formatter);
            Console.ReadLine();
        }

        private static void WriteEmptyLine()
        {
            Console.WriteLine();
            Console.WriteLine();
        }

        private static void Report(BugLevel bug, string dir, string log)
        {
            var parent           = Path.GetDirectoryName(log);
            var feedback         = Path.Combine(parent, "Feedbacks");
            var crashes          = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Crashes");
            var user             = Path.Combine(parent, "UserData");
            var settingFile      = Path.Combine(user, "juxiaoyou-main.json");
            var outputLogZipFile = Path.Combine(feedback, "日志.zip");
            var outputDbZipFile  = Path.Combine(feedback, "世界观.zip");
            var outputReadmeFile  = Path.Combine(feedback, "[readme]看这里.txt");
            var setting          = JSON.OpenSetting<Setting>(settingFile, () => new Setting { Language = CultureArea.Chinese });

            if (!Directory.Exists(feedback))
            {
                Directory.CreateDirectory(feedback);
            }

            var formatter = new[]
            {
                GetBugFormatter(bug),
                new Formatter(dir, Color.Peru),
                new Formatter(log, Color.Peru),
                new Formatter(user, Color.Peru),
                new Formatter(outputLogZipFile, Color.Peru),
                new Formatter(outputDbZipFile, Color.Peru),
                new Formatter(settingFile, Color.Peru),
                new Formatter(Setting.GetName(setting.Language), Color.Peru),
            };

            Console.WriteLineFormatted("BUG等级：{0}\n数据位置：{1}\n日志位置：{2}\n", Color.LightGray, formatter);
            Console.WriteLineFormatted("用户数据目录:{3}\n设置位置：{6}\n", Color.LightGray, formatter);
            Console.WriteLineFormatted("日志压缩包输出位置:{4}\n数据库压缩包输出位置：{5}\n语言:{6}\n", Color.LightGray, formatter);

            if (bug == BugLevel.Bug)
            {
                Pack(log, outputLogZipFile);
            }
            else if (bug == BugLevel.NotImplemented)
            {
                Pack(log, outputLogZipFile);
                Pack(dir, outputDbZipFile);
            }
            else
            {
                Pack(log, outputLogZipFile);
                Pack(dir, outputDbZipFile);
            }
            
            File.Copy(Setting.GetFileName(crashes, setting.Language), outputReadmeFile, true);

            Process.Start(new ProcessStartInfo
            {
                FileName  = "explorer.exe",
                Arguments = feedback
            });
            
            Process.Start(new ProcessStartInfo
            {
                FileName  = "explorer.exe",
                Arguments = outputReadmeFile
            });
        }

        private static void Pack(string dir, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            if (string.IsNullOrEmpty(dir))
            {
                throw new ArgumentNullException(nameof(dir));
            }

            var zip = new FastZip();
            var formatter = new[]
            {
                new Formatter(DateTime.Now, Color.Peru),
                new Formatter(fileName, Color.Peru),
            };
            zip.CreateZip(fileName, dir, true, "");
            Console.WriteLineFormatted("{0}压缩完毕：{1}\t\n数据位置：{1}\n", Color.LightGray, formatter);
        }


        private static Formatter GetBugFormatter(BugLevel level)
        {
            return level switch
            {
                BugLevel.Bug            => new Formatter("普通", ObsoletedColor),
                BugLevel.NotImplemented => new Formatter("严重", WarningColor),
                _                       => new Formatter("危险", DangerColor),
            };
        }

        private static void Manual()
        {
        }
    }
}