using System;
using System.IO;
using Acorisoft.FutureGL.Forest.Controls;
using Acorisoft.FutureGL.Forest.Services;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaStudio.Modules;

namespace Acorisoft.FutureGL.MigaStudio
{
    public static class TemplateSystemString
    {
        public static readonly DocumentType[] DocumentTypes = new[]
        {
            DocumentType.CharacterDocument,
            DocumentType.AbilityDocument,
            DocumentType.GeographyDocument,
            DocumentType.ItemDocument,
            DocumentType.OtherDocument,
            DocumentType.MysteryDocument,
        };
        
        public static string GetText(string id) => Language.GetText(id);

        #region ModuleBlock Translate

        // TODO: 翻译
        public static string GetAudioName()
        {
            return Language.Culture switch
            {
                CultureArea.Chinese  => "语音",
                CultureArea.Russian  => "Audio",
                CultureArea.Korean   => "Audio",
                CultureArea.Japanese => "Audio",
                CultureArea.French   => "Audio",
                _                    => "Audio"
            };
        }

        public static string GetUnknownName()
        {
            return Language.Culture switch
            {
                CultureArea.Chinese  => "未知",
                CultureArea.Russian  => "Unknown",
                CultureArea.Korean   => "Unknown",
                CultureArea.Japanese => "Unknown",
                CultureArea.French   => "Unknown",
                _                    => "Unknown"
            };
        }

        public static string GetGroupName()
        {
            return Language.Culture switch
            {
                CultureArea.Chinese  => "分组",
                CultureArea.Russian  => "Group",
                CultureArea.Korean   => "Group",
                CultureArea.Japanese => "Group",
                CultureArea.French   => "Group",
                _                    => "Group"
            };
        }

        public static string GetSequenceName()
        {
            return Language.Culture switch
            {
                CultureArea.Chinese  => "选项",
                CultureArea.Russian  => "Sequence",
                CultureArea.Korean   => "Sequence",
                CultureArea.Japanese => "Sequence",
                CultureArea.French   => "Sequence",
                _                    => "Sequence"
            };
        }

        public static string GetColorName()
        {
            return Language.Culture switch
            {
                CultureArea.Chinese  => "色块",
                CultureArea.Russian  => "Color",
                CultureArea.Korean   => "Color",
                CultureArea.Japanese => "Color",
                CultureArea.French   => "Color",
                _                    => "Color"
            };
        }

        public static string GetNumberName()
        {
            return Language.Culture switch
            {
                CultureArea.Chinese  => "数字块",
                CultureArea.Russian  => "Number",
                CultureArea.Korean   => "Number",
                CultureArea.Japanese => "Number",
                CultureArea.French   => "Number",
                _                    => "Number"
            };
        }

        public static string GetHistogramName()
        {
            return Language.Culture switch
            {
                CultureArea.Chinese  => "直方图",
                CultureArea.Russian  => "Histogram",
                CultureArea.Korean   => "Histogram",
                CultureArea.Japanese => "Histogram",
                CultureArea.French   => "Histogram",
                _                    => "Histogram"
            };
        }

        public static string GetRadarName()
        {
            return Language.Culture switch
            {
                CultureArea.Chinese  => "雷达图",
                CultureArea.Russian  => "Radar",
                CultureArea.Korean   => "Radar",
                CultureArea.Japanese => "Radar",
                CultureArea.French   => "Radar",
                _                    => "Radar"
            };
        }

        public static string GetLikabilityName()
        {
            return Language.Culture switch
            {
                CultureArea.Chinese  => "好感度",
                CultureArea.Russian  => "Likability",
                CultureArea.Korean   => "Likability",
                CultureArea.Japanese => "Likability",
                CultureArea.French   => "Likability",
                _                    => "Likability"
            };
        }

        public static string GetDegreeName()
        {
            return Language.Culture switch
            {
                CultureArea.Chinese  => "程度块",
                CultureArea.Russian  => "Degree",
                CultureArea.Korean   => "Degree",
                CultureArea.Japanese => "Degree",
                CultureArea.French   => "Degree",
                _                    => "Degree"
            };
        }

        public static string GetRateName()
        {
            return Language.Culture switch
            {
                CultureArea.Chinese  => "评分",
                CultureArea.Russian  => "Rate",
                CultureArea.Korean   => "Rate",
                CultureArea.Japanese => "Rate",
                CultureArea.French   => "Rate",
                _                    => "Rate"
            };
        }

        public static string GetSliderName()
        {
            return Language.Culture switch
            {
                CultureArea.Chinese  => "滑块",
                CultureArea.Russian  => "Slider",
                CultureArea.Korean   => "Slider",
                CultureArea.Japanese => "Slider",
                CultureArea.French   => "Slider",
                _                    => "Slider"
            };
        }

        public static string GetMultiLineName()
        {
            return Language.Culture switch
            {
                CultureArea.Chinese  => "多行块",
                CultureArea.Russian  => "MultiLine",
                CultureArea.Korean   => "MultiLine",
                CultureArea.Japanese => "MultiLine",
                CultureArea.French   => "MultiLine",
                _                    => "MultiLine"
            };
        }

        public static string GetSingleLineName()
        {
            return Language.Culture switch
            {
                CultureArea.Chinese  => "单行块",
                CultureArea.Russian  => "SingleLine",
                CultureArea.Korean   => "SingleLine",
                CultureArea.Japanese => "SingleLine",
                CultureArea.French   => "SingleLine",
                _                    => "SingleLine"
            };
        }

        public static string GetReferenceName()
        {
            return Language.Culture switch
            {
                CultureArea.Chinese  => "引用",
                CultureArea.Russian  => "Reference",
                CultureArea.Korean   => "Reference",
                CultureArea.Japanese => "Reference",
                CultureArea.French   => "Reference",
                _                    => "Reference"
            };
        }

        public static string GetVideoName()
        {
            return Language.Culture switch
            {
                CultureArea.Chinese  => "视频",
                CultureArea.Russian  => "Video",
                CultureArea.Korean   => "Video",
                CultureArea.Japanese => "Video",
                CultureArea.French   => "Video",
                _                    => "Video"
            };
        }

        public static string GetMusicName()
        {
            return Language.Culture switch
            {
                CultureArea.Chinese  => "音乐",
                CultureArea.Russian  => "Music",
                CultureArea.Korean   => "Music",
                CultureArea.Japanese => "Music",
                CultureArea.French   => "Music",
                _                    => "Music"
            };
        }

        public static string GetImageName()
        {
            return Language.Culture switch
            {
                CultureArea.Chinese  => "图片",
                CultureArea.Russian  => "Image",
                CultureArea.Korean   => "Image",
                CultureArea.Japanese => "Image",
                CultureArea.French   => "Image",
                _                    => "Image"
            };
        }

        public static string GetFileName()
        {
            return Language.Culture switch
            {
                CultureArea.Chinese  => "文件",
                CultureArea.Russian  => "File",
                CultureArea.Korean   => "File",
                CultureArea.Japanese => "File",
                CultureArea.French   => "File",
                _                    => "File"
            };
        }

        public static string GetBinaryName()
        {
            return Language.Culture switch
            {
                CultureArea.Chinese  => "双选块",
                CultureArea.Russian  => "Binary",
                CultureArea.Korean   => "Binary",
                CultureArea.Japanese => "Binary",
                CultureArea.French   => "Binary",
                _                    => "Binary"
            };
        }

        public static string GetSwitchName()
        {
            return Language.Culture switch
            {
                CultureArea.Chinese  => "开关块",
                CultureArea.Russian  => "Switch",
                CultureArea.Korean   => "Switch",
                CultureArea.Japanese => "Switch",
                CultureArea.French   => "Switch",
                _                    => "Switch"
            };
        }

        public static string GetHeartName()
        {
            return Language.Culture switch
            {
                CultureArea.Chinese  => "心形",
                CultureArea.Russian  => "Heart",
                CultureArea.Korean   => "Heart",
                CultureArea.Japanese => "Heart",
                CultureArea.French   => "Heart",
                _                    => "Heart"
            };
        }

        public static string GetStarName()
        {
            return Language.Culture switch
            {
                CultureArea.Chinese  => "五角星",
                CultureArea.Russian  => "Star",
                CultureArea.Korean   => "Star",
                CultureArea.Japanese => "Star",
                CultureArea.French   => "Star",
                _                    => "Star"
            };
        }


        public static string GetModuleBlockNameByType(object type)
        {
            return type switch
            {
                AudioBlockEditUI      => GetAudioName(),
                FileBlockEditUI       => GetFileName(),
                ImageBlockEditUI      => GetImageName(),
                MusicBlockEditUI      => GetMusicName(),
                VideoBlockEditUI      => GetVideoName(),
                ReferenceBlockEditUI  => GetReferenceName(),
                MultiLineBlockEditUI  => GetMultiLineName(),
                SingleLineBlockEditUI => GetSingleLineName(),
                SliderBlockEditUI     => GetSliderName(),
                NumberBlockEditUI     => GetNumberName(),
                ColorBlockEditUI      => GetColorName(),
                DegreeBlockEditUI     => GetDegreeName(),
                RateBlockEditUI       => GetRateName(),
                LikabilityBlockEditUI => GetLikabilityName(),
                StarBlockEditUI       => GetStarName(),
                HeartBlockEditUI      => GetHeartName(),
                SwitchBlockEditUI     => GetSwitchName(),
                BinaryBlockEditUI     => GetBinaryName(),
                SequenceBlockEditUI   => GetSequenceName(),
                GroupBlockEditUI      => GetGroupName(),
                RadarBlockEditUI      => GetRadarName(),
                HistogramBlockEditUI  => GetHistogramName(),
                _                     => GetUnknownName(),
            };
        }

        public static string GetModuleBlockNameByKind(MetadataKind type)
        {
            return type switch
            {
                MetadataKind.Audio      => GetAudioName(),
                MetadataKind.File       => GetFileName(),
                MetadataKind.Image      => GetImageName(),
                MetadataKind.Music      => GetMusicName(),
                MetadataKind.Video      => GetVideoName(),
                MetadataKind.Reference  => GetReferenceName(),
                MetadataKind.MultiLine  => GetMultiLineName(),
                MetadataKind.SingleLine => GetSingleLineName(),
                MetadataKind.Slider     => GetSliderName(),
                MetadataKind.Number     => GetNumberName(),
                MetadataKind.Color      => GetColorName(),
                MetadataKind.Degree     => GetDegreeName(),
                MetadataKind.Rate       => GetRateName(),
                MetadataKind.Likability => GetLikabilityName(),
                MetadataKind.Star       => GetStarName(),
                MetadataKind.Heart      => GetHeartName(),
                MetadataKind.Switch     => GetSwitchName(),
                MetadataKind.Binary     => GetBinaryName(),
                MetadataKind.Sequence   => GetSequenceName(),
                MetadataKind.Group      => GetGroupName(),
                MetadataKind.Radar      => GetRadarName(),
                MetadataKind.Histogram  => GetHistogramName(),
                _                       => GetUnknownName(),
            };
        }

        #endregion

        #region Property Translate

        public static string GetNameField()
        {
            return Language.Culture switch
            {
                CultureArea.Chinese  => "名字",
                CultureArea.Russian  => "Name",
                CultureArea.Korean   => "Name",
                CultureArea.Japanese => "Name",
                CultureArea.French   => "Name",
                _                    => "Name"
            };
        }


        public static string GetMaximumField()
        {
            return Language.Culture switch
            {
                CultureArea.Chinese  => "最大值",
                CultureArea.Russian  => "Maximum",
                CultureArea.Korean   => "Maximum",
                CultureArea.Japanese => "Maximum",
                CultureArea.French   => "Maximum",
                _                    => "Maximum"
            };
        }


        public static string GetMinimumField()
        {
            return Language.Culture switch
            {
                CultureArea.Chinese  => "最小值",
                CultureArea.Russian  => "Minimum",
                CultureArea.Korean   => "Minimum",
                CultureArea.Japanese => "Minimum",
                CultureArea.French   => "Minimum",
                _                    => "Minimum"
            };
        }


        public static string GetDivideLineField()
        {
            return Language.Culture switch
            {
                CultureArea.Chinese  => "分割线",
                CultureArea.Russian  => "DivideLine",
                CultureArea.Korean   => "DivideLine",
                CultureArea.Japanese => "DivideLine",
                CultureArea.French   => "DivideLine",
                _                    => "DivideLine"
            };
        }

        public static string GetFallbackField()
        {
            return Language.Culture switch
            {
                CultureArea.Chinese  => "默认值",
                CultureArea.Russian  => "Fallback",
                CultureArea.Korean   => "Fallback",
                CultureArea.Japanese => "Fallback",
                CultureArea.French   => "Fallback",
                _                    => "Fallback"
            };
        }


        public static string GetToolTipsField()
        {
            return Language.Culture switch
            {
                CultureArea.Chinese  => "提示",
                CultureArea.Russian  => "ToolTips",
                CultureArea.Korean   => "ToolTips",
                CultureArea.Japanese => "ToolTips",
                CultureArea.French   => "ToolTips",
                _                    => "ToolTips"
            };
        }


        public static string GetMetadataField()
        {
            return Language.Culture switch
            {
                CultureArea.Chinese  => "喵喵咒语",
                CultureArea.Russian  => "Kitty Spell",
                CultureArea.Korean   => "Kitty Spell",
                CultureArea.Japanese => "Kitty Spell",
                CultureArea.French   => "Kitty Spell",
                _                    => "Kitty Spell"
            };
        }

        public static string GetNegativeField()
        {
            return Language.Culture switch
            {
                CultureArea.Chinese  => "负面值",
                CultureArea.Russian  => "Negative",
                CultureArea.Korean   => "Negative",
                CultureArea.Japanese => "Negative",
                CultureArea.French   => "Negative",
                _                    => "Negative"
            };
        }

        public static string GetColorField()
        {
            return Language.Culture switch
            {
                CultureArea.Chinese  => "颜色",
                CultureArea.Russian  => "Color",
                CultureArea.Korean   => "Color",
                CultureArea.Japanese => "Color",
                CultureArea.French   => "Color",
                _                    => "Color"
            };
        }

        public static string GetPositiveField()
        {
            return Language.Culture switch
            {
                CultureArea.Chinese  => "正面值",
                CultureArea.Russian  => "Positive",
                CultureArea.Korean   => "Positive",
                CultureArea.Japanese => "Positive",
                CultureArea.French   => "Positive",
                _                    => "Positive"
            };
        }

        public static string GetSuffixField()
        {
            return Language.Culture switch
            {
                CultureArea.Chinese  => "Kg",
                CultureArea.Russian  => "Kg",
                CultureArea.Korean   => "Kg",
                CultureArea.Japanese => "Kg",
                CultureArea.French   => "Kg",
                _                    => "Kg"
            };
        }

        #endregion


        public static string GetDatabaseResult(DatabaseFailedReason reason)
        {
            // TODO: 翻译
            return reason switch
            {
                DatabaseFailedReason.DatabaseNotOpen => Language.Culture switch
                {
                    CultureArea.English  => "Database Not open!",
                    CultureArea.French   => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                    CultureArea.Japanese => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                    CultureArea.Korean   => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                    CultureArea.Russian  => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                    _                    => "还没有打开任何世界观设定！",
                },
                _ => Language.Culture switch
                {
                    CultureArea.English  => "Unknown error, can you feedback for us?",
                    CultureArea.French   => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                    CultureArea.Japanese => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                    CultureArea.Korean   => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                    CultureArea.Russian  => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                    _                    => "未知的错误，能否联系开发者反馈Bug？谢谢！",
                }
            };
        }

        public static string GetEngineResult(EngineFailedReason reason)
        {
            // TODO: 翻译
            return reason switch
            {
                EngineFailedReason.Duplicated => GetText("enum.EngineFailedReason.Duplicated"),
                _                             => GetText("enum.EngineFailedReason.Unknown")
            };
        }

        public static string Notify => GetText("dialog.title.notify");
        public static string BadModule => GetText("text.notModule");
        public static string ImageTooSmall => GetText("text.ImageTooSmall");
        public static string ImageTooBig => GetText("text.ImageTooBig");

        public static string OperationOfSaveIsSuccessful
        {
            // TODO: 翻译
            get => Language.Culture switch
            {
                CultureArea.English  => "Processing...",
                CultureArea.French   => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.Japanese => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.Korean   => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.Russian  => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                _                    => "保存成功！",
            };
        }

        public static string OperationOfAddIsSuccessful => GetText("text.OperationOfAddIsSuccessful");
        public static string OperationOfRemoveIsSuccessful => GetText("text.OperationOfRemoveIsSuccessful");
        public static string AreYouSureCreateNew => GetText("text.AreYouSureCreateNew");
        public static string AreYouSureRemoveIt => GetText("text.AreYouSureRemoveIt");

        public static string ImageProcessing
        {
            // TODO: 翻译
            get => Language.Culture switch
            {
                CultureArea.English  => "Processing...",
                CultureArea.French   => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.Japanese => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.Korean   => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.Russian  => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                _                    => "正在处理图片....",
            };
        }

        public static string ImageFilter
        {
            // TODO: 翻译
            get => Language.Culture switch
            {
                CultureArea.English  => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.French   => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.Japanese => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.Korean   => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                CultureArea.Russian  => "Image File|*.png;*.jpg;*.bmp;*.jpeg",
                _                    => "图片文件|*.png;*.jpg;*.bmp;*.jpeg",
            };
        }

        public static string ModuleFilter
        {
            // TODO: 翻译
            get => Language.Culture switch
            {
                CultureArea.English  => "Module File|*.png",
                CultureArea.French   => "Module File|*.png",
                CultureArea.Japanese => "Module File|*.png",
                CultureArea.Korean   => "Module File|*.png",
                CultureArea.Russian  => "Module File|*.png",
                _                    => "模组文件|*.png",
            };
        }

        public static string EmptyName => GetText("text.EmptyName");
    }
}