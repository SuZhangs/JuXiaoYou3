using System;
using System.Collections.Generic;
using Acorisoft.FutureGL.MigaDB.Data.Metadatas;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Modules;
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
                Name     = nameof(SingleLineBlock),
                ToolTips = nameof(SingleLineBlock),
                Metadata = nameof(SingleLineBlock),
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
                Name     = nameof(SequenceBlock),
                Fallback = "Item1",
                Items    = new List<OptionItem>(),
                ToolTips = nameof(SequenceBlock),
                Metadata = nameof(SequenceBlock),
            };
            b.Items.Add(new OptionItem { Name = "Item1", Value = "Item1" });
            b.Items.Add(new OptionItem { Name = "Item2", Value = "Item2" });
            b.Items.Add(new OptionItem { Name = "Item3", Value = "Item3" });
            b.Items.Add(new OptionItem { Name = "Item4", Value = "ItemA" });
            return b;
        }

        public static RadioBlock CreateRadio()
        {
            var b = new RadioBlock
            {
                Name     = nameof(RadioBlock),
                Fallback = "Item1",
                Items    = new List<OptionItem>(),
                ToolTips = nameof(RadioBlock),
                Metadata = nameof(RadioBlock),
            };
            b.Items.Add(new OptionItem { Name = "Item1", Value = "Item1" });
            b.Items.Add(new OptionItem { Name = "Item2", Value = "Item2" });
            b.Items.Add(new OptionItem { Name = "Item3", Value = "Item3" });
            b.Items.Add(new OptionItem { Name = "Item4", Value = "ItemA" });
            return b;
        }

        private static SingleLineBlock CreateSingleLine() => new SingleLineBlock
        {
            Name     = nameof(SingleLineBlock),
            Suffix   = nameof(SingleLineBlock),
            Fallback = nameof(SingleLineBlock),
            ToolTips = nameof(SingleLineBlock),
            Metadata = nameof(SingleLineBlock),
        };

        private static MultiLineBlock CreateMultiLine() => new MultiLineBlock
        {
            Name     = nameof(MultiLineBlock),
            Fallback = nameof(MultiLineBlock),
            ToolTips = nameof(MultiLineBlock),
            Metadata = nameof(MultiLineBlock),
        };

        private static ColorBlock CreateColor() => new ColorBlock
        {
            Name     = nameof(ColorBlock),
            Fallback = "#007ACC",
            ToolTips = nameof(ColorBlock),
            Metadata = nameof(ColorBlock),
        };


        private static NumberBlock CreateNumber() => new NumberBlock
        {
            Name     = nameof(NumberBlock),
            Fallback = 12,
            Maximum  = 30,
            Minimum  = 0,
            Suffix   = nameof(NumberBlock),
            ToolTips = nameof(NumberBlock),
            Metadata = nameof(NumberBlock),
        };

        private static SliderBlock CreateSlider() => new SliderBlock
        {
            Name     = nameof(SliderBlock),
            Fallback = 12,
            Maximum  = 30,
            Minimum  = 0,
            Suffix   = nameof(SliderBlock),
            ToolTips = nameof(SliderBlock),
            Metadata = nameof(SliderBlock),
        };

        private static DegreeBlock CreateDegree() => new DegreeBlock
        {
            Name       = nameof(DegreeBlock),
            Fallback   = 4,
            Maximum    = 10,
            Minimum    = 0,
            DivideLine = 10,
            Negative   = nameof(DegreeBlock.Negative),
            Positive   = nameof(DegreeBlock.Positive),
            ToolTips   = nameof(DegreeBlock),
            Metadata   = nameof(DegreeBlock),
        };


        private static SwitchBlock CreateSwitch() => new SwitchBlock
        {
            Name     = nameof(SwitchBlock),
            Fallback = false,
            ToolTips = nameof(SwitchBlock),
            Metadata = nameof(SwitchBlock),
        };


        private static BinaryBlock CreateBinary() => new BinaryBlock
        {
            Name     = nameof(BinaryBlock),
            Fallback = false,
            ToolTips = nameof(BinaryBlock),
            Metadata = nameof(BinaryBlock),
            Negative = nameof(DegreeBlock.Negative),
            Positive = nameof(DegreeBlock.Positive),
        };


        private static HeartBlock CreateHeart() => new HeartBlock
        {
            Name     = nameof(HeartBlock),
            Fallback = false,
            ToolTips = nameof(HeartBlock),
            Metadata = nameof(HeartBlock),
        };


        private static StarBlock CreateStar() => new StarBlock
        {
            Name     = nameof(StarBlock),
            Fallback = false,
            ToolTips = nameof(StarBlock),
            Metadata = nameof(StarBlock),
        };

        private static LikabilityBlock CreateLikability() => new LikabilityBlock
        {
            Name     = nameof(LikabilityBlock),
            Fallback = 6,
            Maximum  = 10,
            Minimum  = 0,
            ToolTips = nameof(LikabilityBlock),
            Metadata = nameof(LikabilityBlock),
        };

        private static RateBlock CreateRate() => new RateBlock
        {
            Name     = nameof(RateBlock),
            Fallback = 6,
            Maximum  = 10,
            Minimum  = 0,
            ToolTips = nameof(RateBlock),
            Metadata = nameof(RateBlock),
        };

        private static RadarBlock CreateRadar() => new RadarBlock
        {
            Name     = nameof(RadarBlock),
            Maximum  = 10,
            Minimum  = 0,
            ToolTips = nameof(RadarBlock),
            Metadata = nameof(RadarBlock),
            Axis     = new[] { "Axis1", "Axis2", "Axis3", "Axis4" },
            Fallback = new[] { 3, 4, 5, 6 },
            Color    = "#007ACC"
        };


        private static HistogramBlock CreateHistogram() => new HistogramBlock
        {
            Name     = nameof(HistogramBlock),
            Maximum  = 10,
            Minimum  = 0,
            ToolTips = nameof(HistogramBlock),
            Metadata = nameof(HistogramBlock),
            Axis     = new[] { "Axis1", "Axis2", "Axis3", "Axis4" },
            Fallback = new[] { 3, 4, 5, 6 },
            Color    = "#007ACC"
        };

        private static AudioBlock CreateAudio() => new AudioBlock
        {
            Name            = nameof(AudioBlock),
            TargetName      = nameof(AudioBlock),
            TargetSource    = nameof(AudioBlock),
            TargetThumbnail = nameof(AudioBlock),
            ToolTips        = nameof(AudioBlock),
            Metadata        = nameof(AudioBlock),
        };

        private static FileBlock CreateFile() => new FileBlock
        {
            Name            = nameof(FileBlock),
            TargetName      = nameof(FileBlock),
            TargetSource    = nameof(FileBlock),
            TargetThumbnail = nameof(FileBlock),
            ToolTips        = nameof(FileBlock),
            Metadata        = nameof(FileBlock),
        };

        private static VideoBlock CreateVideo() => new VideoBlock
        {
            Name            = nameof(VideoBlock),
            TargetName      = nameof(VideoBlock),
            TargetSource    = nameof(VideoBlock),
            TargetThumbnail = nameof(VideoBlock),
            ToolTips        = nameof(VideoBlock),
            Metadata        = nameof(VideoBlock),
        };

        private static MusicBlock CreateMusic() => new MusicBlock
        {
            Name            = nameof(MusicBlock),
            TargetName      = nameof(MusicBlock),
            TargetSource    = nameof(MusicBlock),
            TargetThumbnail = nameof(MusicBlock),
            ToolTips        = nameof(MusicBlock),
            Metadata        = nameof(MusicBlock),
        };

        private static ImageBlock CreateImage() => new ImageBlock
        {
            Name            = nameof(ImageBlock),
            TargetName      = nameof(ImageBlock),
            TargetSource    = nameof(ImageBlock),
            TargetThumbnail = nameof(ImageBlock),
            ToolTips        = nameof(ImageBlock),
            Metadata        = nameof(ImageBlock),
        };

        private static ReferenceBlock CreateReference() => new ReferenceBlock
        {
            Name            = nameof(ReferenceBlock),
            DataSource      = ReferenceSource.Compose,
            TargetName      = nameof(ReferenceBlock),
            TargetSource    = nameof(ReferenceBlock),
            TargetThumbnail = nameof(ReferenceBlock),
            ToolTips        = nameof(ReferenceBlock),
            Metadata        = nameof(ReferenceBlock),
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


                CreateRadio(),
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


                RadioBlock RadioBlock       => new RadioBlockDataUI(RadioBlock),
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


                MetadataKind.Radio    => CreateRadio(),
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


                RadioBlock RadioBlock       => new RadioBlockEditUI(RadioBlock),
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

        // (\MetadataKind.w,
        // MetadataKind.$1,
        public static readonly MetadataKind[] BasicBlockKinds = new[]
        {
            MetadataKind.Color,
            MetadataKind.Degree,
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
            MetadataKind.Radio,
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