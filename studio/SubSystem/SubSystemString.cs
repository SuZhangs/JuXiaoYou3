﻿namespace Acorisoft.FutureGL.MigaStudio
{
    public static partial class SubSystemString
    {
        public static readonly DocumentType[] DocumentTypes = new[]
        {
            DocumentType.Character,
            DocumentType.Skill,
            DocumentType.Geography,
            DocumentType.Item,
            DocumentType.Other,
        };

        public static readonly DocumentType[] DocumentGalleryTypes = new[]
        {
            DocumentType.None,
            DocumentType.Character,
            DocumentType.Skill,
            DocumentType.Geography,
            DocumentType.Item,
            DocumentType.Other,
        };

        public static string GetText(string id) => Language.GetText(id);

        #region ModuleBlock Translate

        // TODO: 翻译
        public static string GetAudioName() => ModuleBlock.IdOfAudio;

        public static string GetUnknownName() => "__Unknown";

        public static string GetGroupName() => ModuleBlock.IdOfGroup;

        public static string GetSequenceName() => ModuleBlock.IdOfSequence;

        public static string GetColorName() => ModuleBlock.IdOfColor;

        public static string GetNumberName() => ModuleBlock.IdOfNumber;

        public static string GetHistogramName() => ModuleBlock.IdOfHistogram;

        public static string GetRadarName() => ModuleBlock.IdOfRadar;

        public static string GetLikabilityName() => ModuleBlock.IdOfLikability;

        public static string GetDegreeName() => ModuleBlock.IdOfDegree;

        public static string GetRateName() => ModuleBlock.IdOfRate;

        public static string GetSliderName() => ModuleBlock.IdOfSlider;

        public static string GetMultiLineName() => ModuleBlock.IdOfMultiLine;

        public static string GetSingleLineName() => ModuleBlock.IdOfSingleLine;

        public static string GetReferenceName() => ModuleBlock.IdOfReference;

        public static string GetVideoName() => ModuleBlock.IdOfVideo;
        public static string GetMusicName() => ModuleBlock.IdOfMusic;

        public static string GetImageName() => ModuleBlock.IdOfImage;

        public static string GetFileName() => ModuleBlock.IdOfFile;

        public static string GetBinaryName() => ModuleBlock.IdOfBinary;

        public static string GetSwitchName() => ModuleBlock.IdOfSwitch;

        public static string GetHeartName() => ModuleBlock.IdOfHeart;

        public static string GetStarName() => ModuleBlock.IdOfStar;


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

        public static string GetModuleBlockNameByKind(BlockType type)
        {
            return type switch
            {
                BlockType.Audio      => GetAudioName(),
                BlockType.File       => GetFileName(),
                BlockType.Image      => GetImageName(),
                BlockType.Music      => GetMusicName(),
                BlockType.Video      => GetVideoName(),
                BlockType.Reference  => GetReferenceName(),
                BlockType.MultiLine  => GetMultiLineName(),
                BlockType.SingleLine => GetSingleLineName(),
                BlockType.Slider     => GetSliderName(),
                BlockType.Number     => GetNumberName(),
                BlockType.Color      => GetColorName(),
                BlockType.Degree     => GetDegreeName(),
                BlockType.Rate       => GetRateName(),
                BlockType.Likability => GetLikabilityName(),
                BlockType.Star       => GetStarName(),
                BlockType.Heart      => GetHeartName(),
                BlockType.Switch     => GetSwitchName(),
                BlockType.Binary     => GetBinaryName(),
                BlockType.Sequence   => GetSequenceName(),
                BlockType.Group      => GetGroupName(),
                BlockType.Radar      => GetRadarName(),
                BlockType.Histogram  => GetHistogramName(),
                _                    => GetUnknownName(),
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


        #region ModuleBlockPresentation Translate

        // TODO: 翻译
        internal static string GetName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Language.Culture switch
                {
                    _ => "世界观：未知"
                };
            }

            return value;
        }

        internal static string GetFor(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Language.Culture switch
                {
                    _ => "世界观名字"
                };
            }

            var pattern = Language.Culture switch
            {
                _ => "世界观:{0}",
            };

            return string.Format(pattern, value);
        }

        internal static string GetAuthor(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Language.Culture switch
                {
                    _ => "作者：佚名"
                };
            }

            var pattern = Language.Culture switch
            {
                _ => "作者：{0}",
            };

            return string.Format(pattern, value);
        }


        internal static string GetContractList(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Language.Culture switch
                {
                    _ => "联系方式：暂无"
                };
            }

            var pattern = Language.Culture switch
            {
                _ => "联系方式：{0}",
            };

            return string.Format(pattern, value);
        }


        internal static string GetIntro(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Language.Culture switch
                {
                    _ => "简介：暂无"
                };
            }

            return value;
        }

        #endregion


        public static string GetDatabaseResult(DatabaseFailedReason reason) => Language.GetEnum(reason);
        public static string GetEngineResult(EngineFailedReason reason) => Language.GetEnum(reason);

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

        public static string ApplyWhenRestart => GetText("text.applyWhenRestart");
        public static string OperationOfAutoSaveIsSuccessful => GetText("text.OperationOfAutoSaveIsSuccessful");
        public static string OperationOfAddIsSuccessful => GetText("text.OperationOfAddIsSuccessful");
        public static string OperationOfRemoveIsSuccessful => GetText("text.OperationOfRemoveIsSuccessful");
        public static string AreYouSureOverrideIt => GetText("text.AreYouSureOverrideIt");
        public static string AreYouSureSynchronizeIt => GetText("text.AreYouSureSynchronizeIt");
        public static string AreYouSureUnOverrideIt => GetText("text.AreYouSureUnOverrideIt");
        public static string AreYouSureCreateNew => GetText("text.AreYouSureCreateNew");
        public static string AreYouSureRemoveIt => GetText("text.AreYouSureRemoveIt");
        public static string AreYouSureClearIt => GetText("text.AreYouSureClearIt");
        public static string Notify => GetText("text.notify");
        public static string NoDataToSave => GetText("text.NoDataToSave");
        public static string NoChange => GetText("text.noChange");
        public static string BadImage => GetText("text.notimage");
        public static string BadFormat => GetText("text.BadFormat");
        public static string BadModule => GetText("text.notModule");
        public static string ImageTooSmall => GetText("text.ImageTooSmall");
        public static string ImageTooBig => GetText("text.ImageTooBig");
        public static string EmptyName => GetText("text.EmptyName");
        public static string EmptyIntro => GetText("text.EmptyIntro");
        public static string EmptyAuthor => GetText("text.EmptyAuthor");
        public static string EmptyValue => GetText("text.EmptyValue");
        public static string EmptyFor => GetText("text.EmptyFor");
        public static string KeywordTooMany => GetText("text.KeywordTooMany");
        public static string AddKeywordTitle => GetText("text.AddKeyword");
        public static string EditNameTitle => GetText("text.editName");
        public static string EditValueTitle => GetText("text.editValue");
        public static string SelectTitle => GetText("global.select");
        public static string Unknown => GetText("text.UnknownException");
        public static string CannotDelete => GetText("text.CannotDelete");

        public static string Processing
        {
            // TODO: 翻译
            get => Language.Culture switch
            {
                CultureArea.English  => "Processing...",
                CultureArea.French   => "Processing...",
                CultureArea.Japanese => "Processing...",
                CultureArea.Korean   => "Processing...",
                CultureArea.Russian  => "Processing...",
                _                    => "正在处理图片....",
            };
        }

        public static string NoMoreData => GetText("text.noMoreData");
    }
}