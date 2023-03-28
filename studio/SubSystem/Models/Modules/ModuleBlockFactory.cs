using System;
using System.Collections.Generic;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Modules;
using Acorisoft.FutureGL.MigaDB.Utils;
using Acorisoft.FutureGL.MigaUtils.Collections;

namespace Acorisoft.FutureGL.MigaStudio.Models.Modules.ViewModels
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
                Name     = SubSystemString.GetSingleLineName(),
                ToolTips = SubSystemString.GetToolTipsField(),

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
                Name     = SubSystemString.GetSequenceName(),
                Fallback = "Item1",
                Value    = null,
                Items    = new List<OptionItem>(),
                ToolTips = SubSystemString.GetToolTipsField(),
            };
            b.Items.Add(new OptionItem { Name = "Item1", Value = "Item1" });
            b.Items.Add(new OptionItem { Name = "Item2", Value = "Item2" });
            b.Items.Add(new OptionItem { Name = "Item3", Value = "Item3" });
            b.Items.Add(new OptionItem { Name = "Item4", Value = "ItemA" });
            return b;
        }


        private static SingleLineBlock CreateSingleLine() => new SingleLineBlock
        {
            Id       = ID.Get(),
            Value    = null,
            Name     = SubSystemString.GetSingleLineName(),
            Suffix   = SubSystemString.GetSuffixField(),
            Fallback = SubSystemString.GetFallbackField(),
            ToolTips = SubSystemString.GetToolTipsField(),
        };

        private static MultiLineBlock CreateMultiLine() => new MultiLineBlock
        {
            Id       = ID.Get(),
            Value    = null,
            Name     = SubSystemString.GetMultiLineName(),
            Fallback = SubSystemString.GetFallbackField(),
            ToolTips = SubSystemString.GetToolTipsField(),
        };

        private static ColorBlock CreateColor() => new ColorBlock
        {
            Id       = ID.Get(),
            Value    = null,
            Name     = SubSystemString.GetColorName(),
            Fallback = "#007ACC",
            ToolTips = SubSystemString.GetToolTipsField(),
        };


        private static NumberBlock CreateNumber() => new NumberBlock
        {
            Id       = ID.Get(),
            Name     = SubSystemString.GetNumberName(),
            Fallback = 12,
            Maximum  = 30,
            Minimum  = 0,
            Value    = -1,
            Suffix   = SubSystemString.GetNumberName(),
            ToolTips = SubSystemString.GetToolTipsField(),
        };

        private static SliderBlock CreateSlider() => new SliderBlock
        {
            Id       = ID.Get(),
            Name     = SubSystemString.GetSliderName(),
            Fallback = 12,
            Maximum  = 30,
            Minimum  = 0,
            Value    = -1,
            Suffix   = SubSystemString.GetSliderName(),
            ToolTips = SubSystemString.GetToolTipsField(),
        };

        private static DegreeBlock CreateDegree() => new DegreeBlock
        {
            Id         = ID.Get(),
            Name       = SubSystemString.GetDegreeName(),
            Fallback   = 4,
            Maximum    = 10,
            Minimum    = 0,
            DivideLine = 10,
            Value      = -1,
            Negative   = SubSystemString.GetNegativeField(),
            Positive   = SubSystemString.GetPositiveField(),
            ToolTips   = SubSystemString.GetToolTipsField(),
        };


        private static SwitchBlock CreateSwitch() => new SwitchBlock
        {
            Id       = ID.Get(),
            Name     = SubSystemString.GetSwitchName(),
            Fallback = false,
            ToolTips = SubSystemString.GetToolTipsField(),
        };


        private static BinaryBlock CreateBinary() => new BinaryBlock
        {
            Id       = ID.Get(),
            Name     = SubSystemString.GetBinaryName(),
            Fallback = false,
            ToolTips = SubSystemString.GetToolTipsField(),

            Negative = SubSystemString.GetNegativeField(),
            Positive = SubSystemString.GetPositiveField(),
        };


        private static HeartBlock CreateHeart() => new HeartBlock
        {
            Id       = ID.Get(),
            Name     = SubSystemString.GetHeartName(),
            Fallback = false,
            ToolTips = SubSystemString.GetToolTipsField(),
        };


        private static StarBlock CreateStar() => new StarBlock
        {
            Id       = ID.Get(),
            Name     = SubSystemString.GetStarName(),
            Fallback = false,
            ToolTips = SubSystemString.GetToolTipsField(),
        };

        private static LikabilityBlock CreateLikability() => new LikabilityBlock
        {
            Id       = ID.Get(),
            Name     = SubSystemString.GetLikabilityName(),
            Fallback = 6,
            Maximum  = 10,
            Minimum  = 0,
            Value    = -1,
            ToolTips = SubSystemString.GetToolTipsField(),
        };

        private static RateBlock CreateRate() => new RateBlock
        {
            Id       = ID.Get(),
            Name     = SubSystemString.GetRateName(),
            Fallback = 6,
            Maximum  = 10,
            Value    = -1,
            Minimum  = 0,
            ToolTips = SubSystemString.GetToolTipsField(),
        };

        private static RadarBlock CreateRadar() => new RadarBlock
        {
            Id       = ID.Get(),
            Name     = SubSystemString.GetRadarName(),
            Maximum  = 10,
            Value    = null,
            Minimum  = 0,
            ToolTips = SubSystemString.GetToolTipsField(),

            Axis     = new[] { "Axis1", "Axis2", "Axis3", "Axis4" },
            Fallback = new[] { 3, 4, 5, 6 },
            Color    = "#007ACC"
        };


        private static HistogramBlock CreateHistogram() => new HistogramBlock
        {
            Id       = ID.Get(),
            Name     = SubSystemString.GetHistogramName(),
            Maximum  = 10,
            Value    = null,
            Minimum  = 0,
            ToolTips = SubSystemString.GetToolTipsField(),

            Axis     = new[] { "Axis1", "Axis2", "Axis3", "Axis4" },
            Fallback = new[] { 3, 4, 5, 6 },
            Color    = "#007ACC"
        };

        private static AudioBlock CreateAudio() => new AudioBlock
        {
            Id              = ID.Get(),
            Name            = SubSystemString.GetAudioName(),
            TargetName      = SubSystemString.GetAudioName(),
            TargetSource    = SubSystemString.GetAudioName(),
            TargetThumbnail = SubSystemString.GetAudioName(),
            ToolTips        = SubSystemString.GetToolTipsField(),
        };

        private static FileBlock CreateFile() => new FileBlock
        {
            Id              = ID.Get(),
            Name            = SubSystemString.GetFileName(),
            TargetName      = SubSystemString.GetFileName(),
            TargetSource    = SubSystemString.GetFileName(),
            TargetThumbnail = SubSystemString.GetFileName(),
            ToolTips        = SubSystemString.GetToolTipsField(),
        };

        private static VideoBlock CreateVideo() => new VideoBlock
        {
            Id              = ID.Get(),
            Name            = SubSystemString.GetVideoName(),
            TargetName      = SubSystemString.GetVideoName(),
            TargetSource    = SubSystemString.GetVideoName(),
            TargetThumbnail = SubSystemString.GetVideoName(),
            ToolTips        = SubSystemString.GetToolTipsField(),
        };

        private static MusicBlock CreateMusic() => new MusicBlock
        {
            Id              = ID.Get(),
            Name            = SubSystemString.GetMusicName(),
            TargetName      = SubSystemString.GetMusicName(),
            TargetSource    = SubSystemString.GetMusicName(),
            TargetThumbnail = SubSystemString.GetMusicName(),
            ToolTips        = SubSystemString.GetToolTipsField(),
        };

        private static ImageBlock CreateImage() => new ImageBlock
        {
            Id              = ID.Get(),
            Name            = SubSystemString.GetImageName(),
            TargetName      = SubSystemString.GetImageName(),
            TargetSource    = SubSystemString.GetImageName(),
            TargetThumbnail = SubSystemString.GetImageName(),
            ToolTips        = SubSystemString.GetToolTipsField(),
        };

        private static ReferenceBlock CreateReference() => new ReferenceBlock
        {
            Id              = ID.Get(),
            Name            = SubSystemString.GetReferenceName(),
            DataSource      = ReferenceSource.Compose,
            TargetName      = SubSystemString.GetReferenceName(),
            TargetSource    = SubSystemString.GetReferenceName(),
            TargetThumbnail = SubSystemString.GetReferenceName(),
            ToolTips        = SubSystemString.GetToolTipsField(),
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

        public static ModuleBlockDataUI GetDataUI(ModuleBlock block, Action<ModuleBlockDataUI, ModuleBlock> handler)
        {
            return block switch
            {
                SingleLineBlock SingleLineBlock => new SingleLineBlockDataUI(SingleLineBlock, handler),
                MultiLineBlock MultiLineBlock   => new MultiLineBlockDataUI(MultiLineBlock, handler),
                ColorBlock ColorBlock           => new ColorBlockDataUI(ColorBlock, handler),
                NumberBlock NumberBlock         => new NumberBlockDataUI(NumberBlock, handler),
                SliderBlock SliderBlock         => new SliderBlockDataUI(SliderBlock, handler),

                DegreeBlock DegreeBlock => new DegreeBlockDataUI(DegreeBlock, handler),
                SwitchBlock SwitchBlock => new SwitchBlockDataUI(SwitchBlock, handler),
                BinaryBlock BinaryBlock => new BinaryBlockDataUI(BinaryBlock, handler),
                HeartBlock HeartBlock   => new HeartBlockDataUI(HeartBlock, handler),
                StarBlock StarBlock     => new StarBlockDataUI(StarBlock, handler),

                SequenceBlock SequenceBlock => new SequenceBlockDataUI(SequenceBlock, handler),
                GroupBlock GroupBlock       => new GroupBlockDataUI(GroupBlock, handler),

                LikabilityBlock LikabilityBlock => new LikabilityBlockDataUI(LikabilityBlock, handler),
                RateBlock RateBlock             => new RateBlockDataUI(RateBlock, handler),

                RadarBlock RadarBlock         => new RadarBlockDataUI(RadarBlock, handler),
                HistogramBlock HistogramBlock => new HistogramBlockDataUI(HistogramBlock, handler),

                AudioBlock AudioBlock         => new AudioBlockDataUI(AudioBlock, handler),
                FileBlock FileBlock           => new FileBlockDataUI(FileBlock, handler),
                VideoBlock VideoBlock         => new VideoBlockDataUI(VideoBlock, handler),
                MusicBlock MusicBlock         => new MusicBlockDataUI(MusicBlock, handler),
                ImageBlock ImageBlock         => new ImageBlockDataUI(ImageBlock, handler),
                ReferenceBlock ReferenceBlock => new ReferenceBlockDataUI(ReferenceBlock, handler),
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


        internal static void EmptyHandler(ModuleBlockDataUI ui, ModuleBlock block)
        {
        }
    }
}