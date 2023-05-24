// See https://aka.ms/new-console-template for more information

using System.Drawing;
using Acorisoft.FutureGL.MigaStudio.Models;
using Colorful;
using Console = Colorful.Console;

namespace Acorisoft.FutureGL.MigaStudio.Tools.Headerless
{
    public class Program
    {
        private static readonly Color PrimaryColor   = Color.FromArgb( 0x98, 0xa1, 0x2b);
        private static readonly Color ObsoletedColor = Color.FromArgb( 0xff, 0x66, 0x00);
        private static readonly Color WarningColor   = Color.FromArgb( 0xff, 0xb3, 0x14);
        private static readonly Color DangerColor    = Color.FromArgb( 0xd9, 0x08, 0x0c);

        private static bool FormatArgs(string[] args, out BugLevel level,out string dir, out string log)
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
                "欢迎使用{0},此工具可以帮助您反馈BUG！\n",
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
                    "此工具只支持由{2}启动!",
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
            var output = Path.Combine(Path.GetDirectoryName(log), "Feedbacks", "output.zip");
            var formatter = new []
            {
                GetBugFormatter(bug),
                new Formatter(dir, Color.Peru),
                new Formatter(log, Color.Peru),
                new Formatter(output, Color.Peru),
            };
            
            Console.WriteLineFormatted("BUG等级：{0}\n数据位置：{1}\n日志位置：{2}\n输出位置：{3}\n", Color.LightGray, formatter);
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