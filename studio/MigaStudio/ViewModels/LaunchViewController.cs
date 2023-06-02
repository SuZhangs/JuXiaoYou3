﻿using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Data.Keywords;
using Acorisoft.FutureGL.MigaStudio.Core;
using Acorisoft.FutureGL.MigaUtils;
using NLog;

namespace Acorisoft.FutureGL.MigaStudio.ViewModels
{
    public class LaunchViewController : LaunchViewControllerBase
    {
        private readonly AssemblyLoadContext _alc;

        public LaunchViewController()
        {
            _alc = AssemblyLoadContext.Default;

            // 加载设置
            Job("text.launch.loadSetting", _ => { });

            // 检查更新
            Job("text.launch.checkVersion", x => { });

            // 加载插件
            Job("text.launch.loadModules", x => LoadModuleImpl((GlobalStudioContext)x));

            // 打开数据库
            Job("text.launch.openDatabase", x => OpenDatabaseImpl((GlobalStudioContext)x));
        }

        private static string GetPluginDirectory()
        {
            var pluginDir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
            if (!System.IO
                       .Directory
                       .Exists(pluginDir))
            {
                System.IO
                      .Directory
                      .CreateDirectory(pluginDir);
            }

            return pluginDir;
        }

        private void LoadModuleImpl(GlobalStudioContext context)
        {
            var pluginDir = GetPluginDirectory();
            var maybePluginFiles = System.IO
                                         .Directory
                                         .GetFiles(pluginDir, "*.dll");
            foreach (var maybePluginFile in maybePluginFiles)
            {
                try
                {
                    var assembly = _alc.LoadFromAssemblyPath(maybePluginFile);
                    var ssmType  = Classes.FindInterfaceImplementation<ISubSystemModule>(assembly);

                    if (ssmType is null)
                    {
                        Xaml.Get<ILogger>()
                            .Warn($"文件：{maybePluginFile}是可识别的插件，但缺少默认的插件实现！");
                        continue;
                    }

                    var ssm = (ISubSystemModule)Activator.CreateInstance(ssmType);

                    if (ssm is null)
                    {
                        Xaml.Get<ILogger>()
                            .Warn($"文件：{maybePluginFile}是可识别的插件，但无法创建插件实现！");
                        continue;
                    }

                    //
                    // 注册
                    ssm.Initialize(Language.Culture, Xaml.Container);

                    var nameItem = new NamedItem<string>
                    {
                        Name  = ssm.FriendlyName,
                        Value = ssm.ModuleId
                    };

                    //
                    // 添加控制器列表
                    context.ControllerList.Add(nameItem);

                    //
                    // 添加控制器映射
                    var map = (Dictionary<string, ITabViewController>)context.ControllerMaps;
                    if (map.TryAdd(ssm.ModuleId, ssm.GetController()))
                    {
                        Xaml.Get<ILogger>()
                            .Warn($"已加载插件，插件ID:{ssm.ModuleId}, 插件名:{ssm.FriendlyName}, 路径:{maybePluginFile}！");
                    }
                    else
                    {
                        Xaml.Get<ILogger>()
                            .Warn($"无法加载插件，已有同名插件，插件ID:{ssm.ModuleId}, 插件名:{ssm.FriendlyName}, 路径:{maybePluginFile}！");
                    }
                }
                catch
                {
                    Xaml.Get<ILogger>()
                        .Warn($"文件：{maybePluginFile}并不是可识别的插件！");
                }
            }
        }

        private static void OpenDatabaseImpl(GlobalStudioContext context)
        {
            var setting = Xaml.Get<SystemSetting>()
                              .RepositorySetting;

            if (string.IsNullOrEmpty(setting.LastRepository))
            {
                return;
            }

            var dr = Studio.DatabaseManager()
                           .LoadAsync(setting.LastRepository)
                           .GetAwaiter()
                           .GetResult();

            if (dr.IsFinished)
            {
                context.IsDatabaseOpen = true;
            }
            else
            {
                Xaml.Get<IDialogService>()
                    .Notify(
                        CriticalLevel.Warning,
                        Language.NotifyText,
                        SubSystemString.GetDatabaseResult(dr.Reason));
            }
        }

        protected override void OnStartup(RoutingEventArgs arg)
        {
            Context = arg.Parameter.Args[0] as GlobalStudioContext;
            Init();
        }

        protected override object GetExecuteContext() => Context;

        protected override void OnJobCompleted()
        {
            var opening = Context.IsDatabaseOpen;

#if DEBUG

            var controller = opening ? 
                Context.Controllers.First(x => x is TabShell) :
                Context.Controllers.First(x => x is QuickStartController);
#else
            var controller = opening ? 
                Context.Controllers.First(x => x is TabShell) : 
                Context.Controllers.First(x => x is QuickStartController) ;
#endif

            //
            //
            Context.SwitchController(controller);
        }

        public GlobalStudioContext Context { get; private set; }
    }
}