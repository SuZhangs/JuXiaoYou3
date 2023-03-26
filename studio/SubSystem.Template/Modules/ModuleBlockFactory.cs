using System;
using System.Collections.Generic;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Modules;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaStudio.Modules.ViewModels
{
    public static class ModuleBlockFactory
    {
        public static IEnumerable<ModuleBlock> CreateBlocks()
        {
            var b = CreateBlocksImpl();
            b.ForEach(x => x.ClearValue());
            return b;
        }

        #region BlockFactory

        public static GroupBlock CreateGroup()
        {
            return new GroupBlock
            {
                Id       = ID.Get(),
                Name     = TemplateSystemString.GetSingleLineName(),
                ToolTips  = TemplateSystemString.GetToolTipsField(),
                
                Items = new List<ModuleBlock>
                {
                    CreateDegree(),
                    CreateDegree(),
                    CreateDegree(),
                    CreateBinary(),
                    CreateBinary(),
                    CreateBinary(),
                }
            };
        }

        public static SequenceBlock CreateSequence()
        {
            var b = new SequenceBlock
            {
                Id       = ID.Get(),
                Name     = TemplateSystemString.GetSequenceName(),
                Fallback = "Item1",
                Value = null,
                Items    = new List<OptionItem>(),
                ToolTips  = TemplateSystemString.GetToolTipsField(),
                
            };
            b.Items.Add(new OptionItem { Name = "Item1", Value = "Item1" });
            b.Items.Add(new OptionItem { Name = "Item2", Value = "Item2" });
            b.Items.Add(new OptionItem { Name = "Item3", Value = "Item3" });
            b.Items.Add(new OptionItem { Name = "Item4", Value = "ItemA" });
            return b;
        }


        private static SingleLineBlock CreateSingleLine() => new SingleLineBlock
        {
            Id = ID.Get(),
            Value = null,
            Name     = TemplateSystemString.GetSingleLineName(),
            Suffix   = TemplateSystemString.GetSuffixField(),
            Fallback   = TemplateSystemString.GetFallbackField(),
            ToolTips  = TemplateSystemString.GetToolTipsField(),
            
        };

        private static MultiLineBlock CreateMultiLine() => new MultiLineBlock
        {
            Id       = ID.Get(),
            Value    = null,
            Name     = TemplateSystemString.GetMultiLineName(),
            Fallback  = TemplateSystemString.GetFallbackField(),
            ToolTips  = TemplateSystemString.GetToolTipsField(),
            
        };

        private static ColorBlock CreateColor() => new ColorBlock
        {
            Id       = ID.Get(),
            Value    = null,
            Name     = TemplateSystemString.GetColorName(),
            Fallback = "#007ACC",
            ToolTips  = TemplateSystemString.GetToolTipsField(),
            
        };


        private static NumberBlock CreateNumber() => new NumberBlock
        {
            Id       = ID.Get(),
            Name     = TemplateSystemString.GetNumberName(),
            Fallback = 12,
            Maximum  = 30,
            Minimum  = 0,
            Value    = -1,
            Suffix   = TemplateSystemString.GetNumberName(),
            ToolTips  = TemplateSystemString.GetToolTipsField(),
            
        };

        private static SliderBlock CreateSlider() => new SliderBlock
        {
            Id       = ID.Get(),
            Name     = TemplateSystemString.GetSliderName(),
            Fallback = 12,
            Maximum  = 30,
            Minimum  = 0,
            Value    = -1,
            Suffix   = TemplateSystemString.GetSliderName(),
            ToolTips  = TemplateSystemString.GetToolTipsField(),
            
        };

        private static DegreeBlock CreateDegree() => new DegreeBlock
        {
            Id         = ID.Get(),
            Name       = TemplateSystemString.GetDegreeName(),
            Fallback   = 4,
            Maximum    = 10,
            Minimum    = 0,
            DivideLine = 10,
            Value      = -1,
            Negative  = TemplateSystemString.GetNegativeField(),
            Positive  = TemplateSystemString.GetPositiveField(),
            ToolTips  = TemplateSystemString.GetToolTipsField(),
            
        };


        private static SwitchBlock CreateSwitch() => new SwitchBlock
        {
            Id       = ID.Get(),
            Name     = TemplateSystemString.GetSwitchName(),
            Fallback = false,
            ToolTips  = TemplateSystemString.GetToolTipsField(),
            
        };


        private static BinaryBlock CreateBinary() => new BinaryBlock
        {
            Id       = ID.Get(),
            Name     = TemplateSystemString.GetBinaryName(),
            Fallback = false,
            ToolTips  = TemplateSystemString.GetToolTipsField(),
            
            Negative  = TemplateSystemString.GetNegativeField(),
            Positive  = TemplateSystemString.GetPositiveField(),
        };


        private static HeartBlock CreateHeart() => new HeartBlock
        {
            Id       = ID.Get(),
            Name     = TemplateSystemString.GetHeartName(),
            Fallback = false,
            ToolTips  = TemplateSystemString.GetToolTipsField(),
            
        };


        private static StarBlock CreateStar() => new StarBlock
        {
            Id       = ID.Get(),
            Name     = TemplateSystemString.GetStarName(),
            Fallback = false,
            ToolTips  = TemplateSystemString.GetToolTipsField(),
            
        };

        private static LikabilityBlock CreateLikability() => new LikabilityBlock
        {
            Id       = ID.Get(),
            Name     = TemplateSystemString.GetLikabilityName(),
            Fallback = 6,
            Maximum  = 10,
            Minimum  = 0,
            Value    = -1,
            ToolTips  = TemplateSystemString.GetToolTipsField(),
            
        };

        private static RateBlock CreateRate() => new RateBlock
        {
            Id       = ID.Get(),
            Name     = TemplateSystemString.GetRateName(),
            Fallback = 6,
            Maximum  = 10,
            Value    = -1,
            Minimum  = 0,
            ToolTips  = TemplateSystemString.GetToolTipsField(),
            
        };

        private static RadarBlock CreateRadar() => new RadarBlock
        {
            Id       = ID.Get(),
            Name     = TemplateSystemString.GetRadarName(),
            Maximum  = 10,
            Value    = null,
            Minimum  = 0,
            ToolTips  = TemplateSystemString.GetToolTipsField(),
            
            Axis     = new[] { "Axis1", "Axis2", "Axis3", "Axis4" },
            Fallback = new[] { 3, 4, 5, 6 },
            Color    = "#007ACC"
        };


        private static HistogramBlock CreateHistogram() => new HistogramBlock
        {
            Id       = ID.Get(),
            Name     = TemplateSystemString.GetHistogramName(),
            Maximum  = 10,
            Value    = null,
            Minimum  = 0,
            ToolTips  = TemplateSystemString.GetToolTipsField(),
            
            Axis     = new[] { "Axis1", "Axis2", "Axis3", "Axis4" },
            Fallback = new[] { 3, 4, 5, 6 },
            Color    = "#007ACC"
        };

        private static AudioBlock CreateAudio() => new AudioBlock
        {
            Id              = ID.Get(),
            Name            = TemplateSystemString.GetAudioName(),
            TargetName      = TemplateSystemString.GetAudioName(),
            TargetSource    = TemplateSystemString.GetAudioName(),
            TargetThumbnail = TemplateSystemString.GetAudioName(),
            ToolTips  = TemplateSystemString.GetToolTipsField(),
        };

        private static FileBlock CreateFile() => new FileBlock
        {
            Id              = ID.Get(),
            Name            = TemplateSystemString.GetFileName(),
            TargetName      = TemplateSystemString.GetFileName(),
            TargetSource    = TemplateSystemString.GetFileName(),
            TargetThumbnail = TemplateSystemString.GetFileName(),
            ToolTips  = TemplateSystemString.GetToolTipsField(),
            
        };

        private static VideoBlock CreateVideo() => new VideoBlock
        {
            Id              = ID.Get(),
            Name            = TemplateSystemString.GetVideoName(),
            TargetName      = TemplateSystemString.GetVideoName(),
            TargetSource    = TemplateSystemString.GetVideoName(),
            TargetThumbnail = TemplateSystemString.GetVideoName(),
            ToolTips  = TemplateSystemString.GetToolTipsField(),
            
        };

        private static MusicBlock CreateMusic() => new MusicBlock
        {
            Id              = ID.Get(),
            Name            = TemplateSystemString.GetMusicName(),
            TargetName      = TemplateSystemString.GetMusicName(),
            TargetSource    = TemplateSystemString.GetMusicName(),
            TargetThumbnail = TemplateSystemString.GetMusicName(),
            ToolTips  = TemplateSystemString.GetToolTipsField(),
            
        };

        private static ImageBlock CreateImage() => new ImageBlock
        {
            Id              = ID.Get(),
            Name            = TemplateSystemString.GetImageName(),
            TargetName      = TemplateSystemString.GetImageName(),
            TargetSource    = TemplateSystemString.GetImageName(),
            TargetThumbnail = TemplateSystemString.GetImageName(),
            ToolTips  = TemplateSystemString.GetToolTipsField(),
            
        };

        private static ReferenceBlock CreateReference() => new ReferenceBlock
        {
            Id              = ID.Get(),
            Name            = TemplateSystemString.GetReferenceName(),
            DataSource      = ReferenceSource.Compose,
            TargetName      = TemplateSystemString.GetReferenceName(),
            TargetSource    = TemplateSystemString.GetReferenceName(),
            TargetThumbnail = TemplateSystemString.GetReferenceName(),
            ToolTips  = TemplateSystemString.GetToolTipsField(),
            
        };

        #endregion

        public static ModuleBlock[] CreateBlocksImpl()
        {
            return new ModuleBlock[]
            {
                CreateSingleLine(),
                CreateMultiLine(),
                CreateColor(),
                CreateNumber(),
                CreateSlider(),
                CreateDegree(),
                CreateSwitch(),
                CreateBinary(),
                CreateHeart(),
                CreateStar(),


                CreateSequence(),
                CreateGroup(),

                CreateLikability(),
                CreateRate(),

                CreateRadar(),
                CreateHistogram(),

                CreateAudio(),
                CreateFile(),
                CreateVideo(),
                CreateMusic(),
                CreateImage(),
                CreateReference(),
            };
        }


        public static ModuleBlockDataUI GetDataUI(ModuleBlock block)
        {
            return block switch
            {
                SingleLineBlock SingleLineBlock => new SingleLineBlockDataUI(SingleLineBlock),
                MultiLineBlock MultiLineBlock   => new MultiLineBlockDataUI(MultiLineBlock),
                ColorBlock ColorBlock           => new ColorBlockDataUI(ColorBlock),
                NumberBlock NumberBlock         => new NumberBlockDataUI(NumberBlock),
                SliderBlock SliderBlock         => new SliderBlockDataUI(SliderBlock),

                DegreeBlock DegreeBlock => new DegreeBlockDataUI(DegreeBlock),
                SwitchBlock SwitchBlock => new SwitchBlockDataUI(SwitchBlock),
                BinaryBlock BinaryBlock => new BinaryBlockDataUI(BinaryBlock),
                HeartBlock HeartBlock   => new HeartBlockDataUI(HeartBlock),
                StarBlock StarBlock     => new StarBlockDataUI(StarBlock),

                SequenceBlock SequenceBlock => new SequenceBlockDataUI(SequenceBlock),
                GroupBlock GroupBlock       => new GroupBlockDataUI(GroupBlock),

                LikabilityBlock LikabilityBlock => new LikabilityBlockDataUI(LikabilityBlock),
                RateBlock RateBlock             => new RateBlockDataUI(RateBlock),

                RadarBlock RadarBlock         => new RadarBlockDataUI(RadarBlock),
                HistogramBlock HistogramBlock => new HistogramBlockDataUI(HistogramBlock),

                AudioBlock AudioBlock         => new AudioBlockDataUI(AudioBlock),
                FileBlock FileBlock           => new FileBlockDataUI(FileBlock),
                VideoBlock VideoBlock         => new VideoBlockDataUI(VideoBlock),
                MusicBlock MusicBlock         => new MusicBlockDataUI(MusicBlock),
                ImageBlock ImageBlock         => new ImageBlockDataUI(ImageBlock),
                ReferenceBlock ReferenceBlock => new ReferenceBlockDataUI(ReferenceBlock),
                _                             => throw new ArgumentOutOfRangeException(nameof(block), block, null)
            };
        }

        public static ModuleBlock GetBlock(MetadataKind kind)
        {
            return kind switch
            {
                MetadataKind.SingleLine => CreateSingleLine(),
                MetadataKind.MultiLine  => CreateMultiLine(),
                MetadataKind.Color      => CreateColor(),
                MetadataKind.Number     => CreateNumber(),
                MetadataKind.Slider     => CreateSlider(),

                MetadataKind.Degree => CreateDegree(),
                MetadataKind.Switch => CreateSwitch(),
                MetadataKind.Binary => CreateBinary(),
                MetadataKind.Heart  => CreateHeart(),
                MetadataKind.Star   => CreateStar(),


                MetadataKind.Sequence => CreateSequence(),
                MetadataKind.Group    => CreateGroup(),

                MetadataKind.Likability => CreateLikability(),
                MetadataKind.Rate       => CreateRate(),

                MetadataKind.Radar     => CreateRadar(),
                MetadataKind.Histogram => CreateHistogram(),

                MetadataKind.Audio     => CreateAudio(),
                MetadataKind.File      => CreateFile(),
                MetadataKind.Video     => CreateVideo(),
                MetadataKind.Music     => CreateMusic(),
                MetadataKind.Image     => CreateImage(),
                MetadataKind.Reference => CreateReference(),
                _                      => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };
        }
        
        public static ModuleBlockEditUI GetEditUI(MetadataKind kind)
        {
            ModuleBlock b = kind switch
            {
                MetadataKind.SingleLine => CreateSingleLine(),
                MetadataKind.MultiLine  => CreateMultiLine(),
                MetadataKind.Color      => CreateColor(),
                MetadataKind.Number     => CreateNumber(),
                MetadataKind.Slider     => CreateSlider(),

                MetadataKind.Degree => CreateDegree(),
                MetadataKind.Switch => CreateSwitch(),
                MetadataKind.Binary => CreateBinary(),
                MetadataKind.Heart  => CreateHeart(),
                MetadataKind.Star   => CreateStar(),


                MetadataKind.Sequence => CreateSequence(),
                MetadataKind.Group    => CreateGroup(),

                MetadataKind.Likability => CreateLikability(),
                MetadataKind.Rate       => CreateRate(),

                MetadataKind.Radar     => CreateRadar(),
                MetadataKind.Histogram => CreateHistogram(),

                MetadataKind.Audio     => CreateAudio(),
                MetadataKind.File      => CreateFile(),
                MetadataKind.Video     => CreateVideo(),
                MetadataKind.Music     => CreateMusic(),
                MetadataKind.Image     => CreateImage(),
                MetadataKind.Reference => CreateReference(),
                _                      => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };
            return GetEditUI(b);
        }

        public static ModuleBlockEditUI GetEditUI(ModuleBlock block)
        {
            return block switch
            {
                SingleLineBlock SingleLineBlock => new SingleLineBlockEditUI(SingleLineBlock),
                MultiLineBlock MultiLineBlock   => new MultiLineBlockEditUI(MultiLineBlock),
                ColorBlock ColorBlock           => new ColorBlockEditUI(ColorBlock),
                NumberBlock NumberBlock         => new NumberBlockEditUI(NumberBlock),
                SliderBlock SliderBlock         => new SliderBlockEditUI(SliderBlock),

                DegreeBlock DegreeBlock => new DegreeBlockEditUI(DegreeBlock),
                SwitchBlock SwitchBlock => new SwitchBlockEditUI(SwitchBlock),
                BinaryBlock BinaryBlock => new BinaryBlockEditUI(BinaryBlock),
                HeartBlock HeartBlock   => new HeartBlockEditUI(HeartBlock),
                StarBlock StarBlock     => new StarBlockEditUI(StarBlock),


                SequenceBlock SequenceBlock => new SequenceBlockEditUI(SequenceBlock),
                GroupBlock GroupBlock       => new GroupBlockEditUI(GroupBlock),

                LikabilityBlock LikabilityBlock => new LikabilityBlockEditUI(LikabilityBlock),
                RateBlock RateBlock             => new RateBlockEditUI(RateBlock),

                RadarBlock RadarBlock         => new RadarBlockEditUI(RadarBlock),
                HistogramBlock HistogramBlock => new HistogramBlockEditUI(HistogramBlock),

                AudioBlock AudioBlock         => new AudioBlockEditUI(AudioBlock),
                FileBlock FileBlock           => new FileBlockEditUI(FileBlock),
                VideoBlock VideoBlock         => new VideoBlockEditUI(VideoBlock),
                MusicBlock MusicBlock         => new MusicBlockEditUI(MusicBlock),
                ImageBlock ImageBlock         => new ImageBlockEditUI(ImageBlock),
                ReferenceBlock ReferenceBlock => new ReferenceBlockEditUI(ReferenceBlock),
                _                             => throw new ArgumentOutOfRangeException(nameof(block), block, null)
            };
        }

        public static readonly MetadataKind[] GroupElementKinds = new[]
        {
            MetadataKind.Degree,
            MetadataKind.Switch,
            MetadataKind.Star,
            MetadataKind.Heart,
            MetadataKind.Binary,
            MetadataKind.Likability,
            MetadataKind.Rate,
        };

        // (\MetadataKind.w,
        // MetadataKind.$1,
        public static readonly MetadataKind[] BasicBlockKinds = new[]
        {
            MetadataKind.Color,
            MetadataKind.Number,
            MetadataKind.Slider,
            MetadataKind.SingleLine,
            MetadataKind.MultiLine,
            MetadataKind.Likability,
            MetadataKind.Rate,
        };

        public static readonly MetadataKind[] AdvancedBlockKinds = new[]
        {
            MetadataKind.Reference,
            MetadataKind.Image,
            MetadataKind.Video,
            MetadataKind.Music,
            MetadataKind.Audio,
            MetadataKind.File,
        };

        public static readonly MetadataKind[] OptionBlockKinds = new[]
        {
            MetadataKind.Switch,
            MetadataKind.Star,
            MetadataKind.Heart,
            MetadataKind.Binary,
            MetadataKind.Sequence,
            MetadataKind.Group,
        };

        public static readonly MetadataKind[] ChartBlockKinds = new[]
        {
            MetadataKind.Histogram,
            MetadataKind.Radar,
        };
    }
}