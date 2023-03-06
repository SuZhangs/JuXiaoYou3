using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Media;
using Acorisoft.FutureGL.Forest.Attributes;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.ViewModels;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.IO;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Gallery
{
    public class NewDocumentWizardViewModel : InputViewModel
    {
        private        string       _name;
        private        ImageSource  _avatar;
        private        Resource     _resource;
        private        byte[]       _buffer;
        private static DocumentType _type = DocumentType.CharacterConstraint;

        public NewDocumentWizardViewModel()
        {
            SetAvatarCommand = new AsyncRelayCommand(SetAvatarImpl);
        }

        private async Task SetAvatarImpl()
        {
            //
            //
            var op = await ImageUtilities.Avatar();

            //
            //
            if (!op.IsFinished)
            {
                return;
            }

            //
            //
            Avatar    = Xaml.FromStream(op.Buffer, 256, 256);
            _resource = op.Resource;
            _buffer   = op.Buffer;
        }
        
        

        protected override bool IsCompleted()
        {
            return !string.IsNullOrEmpty(Name);
        }

        protected override void Finish()
        {
            var dm = Xaml.Get<IDatabaseManager>();

            //
            //
            if (!dm.IsOpen.CurrentValue)
            {
                Xaml.Get<IBuiltinDialogService>().Notify(
                    CriticalLevel.Warning, 
                    StringFromCode.Notify, 
                    StringFromCode.GetDatabaseResult(DatabaseFailedReason.DatabaseNotOpen));
                return;
            }

            var ie  = dm.GetEngine<ImageEngine>();
            var de  = dm.GetEngine<DocumentEngine>();
            
            //
            // 写入图片，不做去重处理
            ie.Write(_buffer, _resource);
            
            var document = new DocumentCache
            {
                Id             = ID.Get(),
                Avatar         = _resource.GetUri(),
                Keywords       = new ObservableCollection<string>(),
                Name           = _name,
                Removable      = false,
                Type           = _type,
                TimeOfCreated  = DateTime.Now,
                TimeOfModified = DateTime.Now,
                Version        = 1,
                IsDeleted      = false,
            };
            
            var r = de.AddDocument(document);

            if (r.IsFinished)
            {                
                Xaml.Get<IBuiltinDialogService>().Notify(
                    CriticalLevel.Success, 
                    StringFromCode.Notify, 
                    StringFromCode.OperationOfAddIsSuccess);
            }
            else
            {
                Xaml.Get<IBuiltinDialogService>().Notify(
                    CriticalLevel.Warning, 
                    StringFromCode.Notify, 
                    StringFromCode.GetEngineResult(r.Reason));
            }
        }

        protected override string Failed()
        {
            return "名字为空";
        }

        /// <summary>
        /// 获取或设置 <see cref="Type"/> 属性。
        /// </summary>
        public DocumentType Type
        {
            get => _type;
            set => SetValue(ref _type, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Avatar"/> 属性。
        /// </summary>
        public ImageSource Avatar
        {
            get => _avatar;
            set => SetValue(ref _avatar, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Name"/> 属性。
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }
        
        public AsyncRelayCommand SetAvatarCommand { get; }
    }
}