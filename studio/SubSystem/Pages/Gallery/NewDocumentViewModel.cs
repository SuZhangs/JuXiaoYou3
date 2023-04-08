using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Acorisoft.FutureGL.Forest.Attributes;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.ViewModels;
using Acorisoft.FutureGL.MigaDB.Core;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaDB.IO;
using Acorisoft.FutureGL.MigaDB.Services;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaStudio.Utilities;
using CommunityToolkit.Mvvm.Input;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Gallery
{
    public class NewDocumentViewModel : ImplicitDialogVM
    {
        private        bool         _selectedAvatar;
        private        string       _name;
        private        ImageSource  _avatar;
        private        MemoryStream _buffer;
        private static DocumentType _type = DocumentType.Character;
        private        Visibility   _visibility;
        public NewDocumentViewModel()
        {
            SetAvatarCommand = new AsyncRelayCommand(SetAvatarImpl);
        }

        protected override void OnStart(RoutingEventArgs parameter)
        {
            var p = parameter.Parameter;
            var a = p.Args;

            if (a[0] is DocumentType type)
            {
                Type       = type;
                Visibility = Visibility.Collapsed;
            }
            else
            {
                Visibility = Visibility.Visible;
            }
        }

        public static Task<Op<DocumentCache>> New()
        {
            return DialogService().Dialog<DocumentCache, NewDocumentViewModel>();
        }
        
        public static Task<Op<DocumentCache>> New(DocumentType type)
        {
            return DialogService().Dialog<DocumentCache, NewDocumentViewModel>(new Parameter
            {
                Args = new object[] { type }
            });
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
            try
            {
                Avatar          = Xaml.FromStream(op.Buffer, 256, 256);
                _buffer         = op.Buffer;
                _selectedAvatar = true;
            }
            catch
            {
                await Xaml.Get<IBuiltinDialogService>().Notify(CriticalLevel.Danger, SubSystemString.Notify, SubSystemString.BadImage);
            }
        }


        protected override bool IsCompleted()
        {
            return !string.IsNullOrEmpty(Name);
        }

        private string ApplyAvatar()
        {
            var dm = Xaml.Get<IDatabaseManager>();

            //
            //
            if (!dm.IsOpen.CurrentValue)
            {
                Xaml.Get<IBuiltinDialogService>().Notify(
                    CriticalLevel.Warning,
                    SubSystemString.Notify,
                    SubSystemString.GetDatabaseResult(DatabaseFailedReason.DatabaseNotOpen));
                return string.Empty;
            }

            var    ie  = dm.GetEngine<ImageEngine>();
            var    raw = Pool.MD5.ComputeHash(_buffer);
            var    md5 = Convert.ToBase64String(raw);
            
            if (ie.HasFile(md5))
            {
                var fr = ie.Records.FindById(md5);
                return fr.Uri;
            }

            var avatar = ImageEngine.GetAvatarUri();
            ie.WriteAvatar(_buffer, avatar);

            var record = new FileRecord
            {
                Id   = md5,
                Uri  = avatar,
                Type = ResourceType.Image
            };

            ie.AddFile(record);
            return avatar;
        }

        protected override void Finish()
        {
            var document = new DocumentCache
            {
                Id             = ID.Get(),
                Avatar         = _selectedAvatar ? ApplyAvatar() : null,
                Keywords       = new ObservableCollection<string>(),
                Name           = _name,
                Removable      = false,
                Type           = _type,
                TimeOfCreated  = DateTime.Now,
                TimeOfModified = DateTime.Now,
                Version        = 1,
                IsDeleted      = false,
            };

            Result = document;
        }

        protected override string Failed()
        {
            return SubSystemString.EmptyName;
        }
        
        /// <summary>
        /// 获取或设置 <see cref="Visibility"/> 属性。
        /// </summary>
        public Visibility Visibility
        {
            get => _visibility;
            set => SetValue(ref _visibility, value);
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