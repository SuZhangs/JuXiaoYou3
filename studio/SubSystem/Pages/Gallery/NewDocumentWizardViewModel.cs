using System;
using System.Collections.ObjectModel;
using System.IO;
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
        private                 string       _name;
        private                 ImageSource  _avatar;
        private                 MemoryStream _buffer;
        private static          DocumentType _type = DocumentType.CharacterConstraint;

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
            try
            {
                Avatar  = Xaml.FromStream(op.Buffer, 256, 256);
                _buffer = op.Buffer;
            }
            catch
            {
                await Xaml.Get<IBuiltinDialogService>().Notify(CriticalLevel.Danger, StringFromCode.Notify, StringFromCode.BadImage);
            }
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

            var ie           = dm.GetEngine<ImageEngine>();
            var originLength = (int)_buffer.Length;
            var originBuffer = _buffer.GetBuffer();
            var saltedStream = new MemoryStream(originBuffer.Length + 4);
            
            saltedStream.Write(BitConverter.GetBytes(originLength));
            saltedStream.Write(originBuffer);

            var    raw = Pool.MD5.ComputeHash(saltedStream);
            var    md5 = Convert.ToBase64String(raw);
            string avatar;
            
            if (ie.HasFile(md5))
            {
                var fr = ie.Records.FindById(md5);
                avatar = fr.Uri;
            }
            else
            {
                avatar = $"avatar_{ID.Get()}.png";
                ie.SetAvatar(_buffer, avatar);

                var record = new FileRecord
                {
                    Id   = md5,
                    Uri  = avatar,
                    Type = ResourceType.Image
                };

                ie.AddFile(record);
            }
            

            //
            // 写入图片，不做去重处理

            var document = new DocumentCache
            {
                Id             = ID.Get(),
                Avatar         = avatar,
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
            return StringFromCode.EmptyName;
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