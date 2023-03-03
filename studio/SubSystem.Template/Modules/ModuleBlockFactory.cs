using System;
using System.Collections.Generic;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Module;

namespace Acorisoft.FutureGL.MigaStudio.Modules.ViewModels
{
    public class ModuleBlockFactory
    {
        public static GroupBlock CreateGroupBlock()
        {
            return new GroupBlock
            {
                Name     = nameof(SingleLineBlock),
                ToolTips = nameof(SingleLineBlock),
                Metadata = nameof(SingleLineBlock),
                Items = new List<ModuleBlock>
                {
                    new DegreeBlock
                    {
                        Name       = nameof(DegreeBlock),
                        Fallback   = 12,
                        Maximum    = 30,
                        Minimum    = 0,
                        DivideLine = 10,
                        Negative   = nameof(DegreeBlock.Negative),
                        Positive   = nameof(DegreeBlock.Positive),
                        ToolTips   = nameof(DegreeBlock),
                        Metadata   = nameof(DegreeBlock),
                    },
                    new DegreeBlock
                    {
                        Name       = nameof(DegreeBlock),
                        Fallback   = 12,
                        Maximum    = 30,
                        Minimum    = 0,
                        DivideLine = 10,
                        Negative   = nameof(DegreeBlock.Negative),
                        Positive   = nameof(DegreeBlock.Positive),
                        ToolTips   = nameof(DegreeBlock),
                        Metadata   = nameof(DegreeBlock),
                    },
                    new DegreeBlock
                    {
                        Name       = nameof(DegreeBlock),
                        Fallback   = 12,
                        Maximum    = 30,
                        Minimum    = 0,
                        DivideLine = 10,
                        Negative   = nameof(DegreeBlock.Negative),
                        Positive   = nameof(DegreeBlock.Positive),
                        ToolTips   = nameof(DegreeBlock),
                        Metadata   = nameof(DegreeBlock),
                    },
                }
            };
        }

        public static SequenceBlock CreateSequenceBlock()
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

        public static RadioBlock CreateRadioBlock()
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

        public static IEnumerable<ModuleBlock> CreateBlocks()
        {
            return new ModuleBlock[]
            {
                new SingleLineBlock
                {
                    Name     = nameof(SingleLineBlock),
                    Suffix   = nameof(SingleLineBlock),
                    Fallback = nameof(SingleLineBlock),
                    ToolTips = nameof(SingleLineBlock),
                    Metadata = nameof(SingleLineBlock),
                },
                new MultiLineBlock
                {
                    Name     = nameof(MultiLineBlock),
                    Fallback = nameof(MultiLineBlock),
                    ToolTips = nameof(MultiLineBlock),
                    Metadata = nameof(MultiLineBlock),
                },
                new ColorBlock
                {
                    Name     = nameof(ColorBlock),
                    Fallback = "#007ACC",
                    ToolTips = nameof(ColorBlock),
                    Metadata = nameof(ColorBlock),
                },
                new NumberBlock
                {
                    Name     = nameof(NumberBlock),
                    Fallback = 12,
                    Maximum  = 30,
                    Minimum  = 0,
                    Suffix   = nameof(NumberBlock),
                    ToolTips = nameof(NumberBlock),
                    Metadata = nameof(NumberBlock),
                },
                new SliderBlock
                {
                    Name     = nameof(SliderBlock),
                    Fallback = 12,
                    Maximum  = 30,
                    Minimum  = 0,
                    Suffix   = nameof(SliderBlock),
                    ToolTips = nameof(SliderBlock),
                    Metadata = nameof(SliderBlock),
                },

                new DegreeBlock
                {
                    Name       = nameof(DegreeBlock),
                    Fallback   = 12,
                    Maximum    = 30,
                    Minimum    = 0,
                    DivideLine = 10,
                    Negative   = nameof(DegreeBlock.Negative),
                    Positive   = nameof(DegreeBlock.Positive),
                    ToolTips   = nameof(DegreeBlock),
                    Metadata   = nameof(DegreeBlock),
                },
                new SwitchBlock
                {
                    Name     = nameof(SwitchBlock),
                    Fallback = false,
                    ToolTips = nameof(SwitchBlock),
                    Metadata = nameof(SwitchBlock),
                },
                new BinaryBlock
                {
                    Name     = nameof(BinaryBlock),
                    Fallback = false,
                    ToolTips = nameof(BinaryBlock),
                    Metadata = nameof(BinaryBlock),
                    Negative = nameof(DegreeBlock.Negative),
                    Positive = nameof(DegreeBlock.Positive),
                },
                new HeartBlock
                {
                    Name     = nameof(HeartBlock),
                    Fallback = false,
                    ToolTips = nameof(HeartBlock),
                    Metadata = nameof(HeartBlock),
                },
                new StarBlock
                {
                    Name     = nameof(StarBlock),
                    Fallback = false,
                    ToolTips = nameof(StarBlock),
                    Metadata = nameof(StarBlock),
                },


                CreateRadioBlock(),
                CreateSequenceBlock(),
                CreateGroupBlock(),


                new LikabilityBlock
                {
                    Name     = nameof(LikabilityBlock),
                    Fallback = 6,
                    Maximum  = 10,
                    Minimum  = 0,
                    ToolTips = nameof(LikabilityBlock),
                    Metadata = nameof(LikabilityBlock),
                },
                new RateBlock
                {
                    Name     = nameof(RateBlock),
                    Fallback = 6,
                    Maximum  = 10,
                    Minimum  = 0,
                    ToolTips = nameof(RateBlock),
                    Metadata = nameof(RateBlock),
                },


                new RadarBlock
                {
                    Name     = nameof(RadarBlock),
                    Maximum  = 10,
                    Minimum  = 0,
                    ToolTips = nameof(RadarBlock),
                    Metadata = nameof(RadarBlock),
                    Axis     = new []{"Axis1","Axis2","Axis3","Axis4"},
                    Fallback = new []{3,4,5,6},
                    Color    = "#007ACC"
                },
                new HistogramBlock{
                    Name     = nameof(HistogramBlock),
                    Maximum  = 10,
                    Minimum  = 0,
                    ToolTips = nameof(HistogramBlock),
                    Metadata = nameof(HistogramBlock),
                    Axis     = new []{"Axis1","Axis2","Axis3","Axis4"},
                    Fallback = new []{3,4,5,6},
                    Color    = "#007ACC"
                },


                new AudioBlock
                {
                    Name            = nameof(AudioBlock),
                    TargetName      = nameof(AudioBlock),
                    TargetSource    = nameof(AudioBlock),
                    TargetThumbnail = nameof(AudioBlock),
                    ToolTips        = nameof(AudioBlock),
                    Metadata        = nameof(AudioBlock),
                },
                new FileBlock
                {
                    Name            = nameof(FileBlock),
                    TargetName      = nameof(FileBlock),
                    TargetSource    = nameof(FileBlock),
                    TargetThumbnail = nameof(FileBlock),
                    ToolTips        = nameof(FileBlock),
                    Metadata        = nameof(FileBlock),
                },
                new VideoBlock
                {
                    Name            = nameof(VideoBlock),
                    TargetName      = nameof(VideoBlock),
                    TargetSource    = nameof(VideoBlock),
                    TargetThumbnail = nameof(VideoBlock),
                    ToolTips        = nameof(VideoBlock),
                    Metadata        = nameof(VideoBlock),
                },
                new MusicBlock
                {
                    Name            = nameof(MusicBlock),
                    TargetName      = nameof(MusicBlock),
                    TargetSource    = nameof(MusicBlock),
                    TargetThumbnail = nameof(MusicBlock),
                    ToolTips        = nameof(MusicBlock),
                    Metadata        = nameof(MusicBlock),
                },
                new ImageBlock
                {
                    Name            = nameof(ImageBlock),
                    TargetName      = nameof(ImageBlock),
                    TargetSource    = nameof(ImageBlock),
                    TargetThumbnail = nameof(ImageBlock),
                    ToolTips        = nameof(ImageBlock),
                    Metadata        = nameof(ImageBlock),
                },
                new ReferenceBlock
                {
                    Name            = nameof(ReferenceBlock),
                    DataSource      = ReferenceSource.Compose,
                    TargetName      = nameof(ReferenceBlock),
                    TargetSource    = nameof(ReferenceBlock),
                    TargetThumbnail = nameof(ReferenceBlock),
                    ToolTips        = nameof(ReferenceBlock),
                    Metadata        = nameof(ReferenceBlock),
                },
            };
        }

        public static ModuleBlockDataUI GetDataUI(ModuleBlock block)
        {
            return block switch
            {
                SingleLineBlock SingleLineBlock => new SingleLineBlockDataUI(SingleLineBlock),
                MultiLineBlock MultiLineBlock => new MultiLineBlockDataUI(MultiLineBlock),
                ColorBlock ColorBlock => new ColorBlockDataUI(ColorBlock),
                NumberBlock NumberBlock => new NumberBlockDataUI(NumberBlock),
                SliderBlock SliderBlock => new SliderBlockDataUI(SliderBlock),

                DegreeBlock DegreeBlock => new DegreeBlockDataUI(DegreeBlock),
                SwitchBlock SwitchBlock => new SwitchBlockDataUI(SwitchBlock),
                BinaryBlock BinaryBlock => new BinaryBlockDataUI(BinaryBlock),
                HeartBlock HeartBlock => new HeartBlockDataUI(HeartBlock),
                StarBlock StarBlock => new StarBlockDataUI(StarBlock),


                RadioBlock RadioBlock => new RadioBlockDataUI(RadioBlock),
                SequenceBlock SequenceBlock => new SequenceBlockDataUI(SequenceBlock),
                GroupBlock GroupBlock => new GroupBlockDataUI(GroupBlock),

                LikabilityBlock LikabilityBlock => new LikabilityBlockDataUI(LikabilityBlock),
                RateBlock RateBlock => new RateBlockDataUI(RateBlock),

                RadarBlock RadarBlock => new RadarBlockDataUI(RadarBlock),
                HistogramBlock HistogramBlock => new HistogramBlockDataUI(HistogramBlock),

                AudioBlock AudioBlock => new AudioBlockDataUI(AudioBlock),
                FileBlock FileBlock => new FileBlockDataUI(FileBlock),
                VideoBlock VideoBlock => new VideoBlockDataUI(VideoBlock),
                MusicBlock MusicBlock => new MusicBlockDataUI(MusicBlock),
                ImageBlock ImageBlock => new ImageBlockDataUI(ImageBlock),
                ReferenceBlock ReferenceBlock => new ReferenceBlockDataUI(ReferenceBlock),
                _ => throw new ArgumentOutOfRangeException(nameof(block), block, null)
            };
        }

        public static ModuleBlockEditUI GetEditUI(ModuleBlock block)
        {
            return block switch
            {
                SingleLineBlock SingleLineBlock => new SingleLineBlockEditUI(SingleLineBlock),
                MultiLineBlock MultiLineBlock => new MultiLineBlockEditUI(MultiLineBlock),
                ColorBlock ColorBlock => new ColorBlockEditUI(ColorBlock),
                NumberBlock NumberBlock => new NumberBlockEditUI(NumberBlock),
                SliderBlock SliderBlock => new SliderBlockEditUI(SliderBlock),

                DegreeBlock DegreeBlock => new DegreeBlockEditUI(DegreeBlock),
                SwitchBlock SwitchBlock => new SwitchBlockEditUI(SwitchBlock),
                BinaryBlock BinaryBlock => new BinaryBlockEditUI(BinaryBlock),
                HeartBlock HeartBlock => new HeartBlockEditUI(HeartBlock),
                StarBlock StarBlock => new StarBlockEditUI(StarBlock),


                RadioBlock RadioBlock => new RadioBlockEditUI(RadioBlock),
                SequenceBlock SequenceBlock => new SequenceBlockEditUI(SequenceBlock),
                GroupBlock GroupBlock => new GroupBlockEditUI(GroupBlock),

                LikabilityBlock LikabilityBlock => new LikabilityBlockEditUI(LikabilityBlock),
                RateBlock RateBlock => new RateBlockEditUI(RateBlock),

                RadarBlock RadarBlock => new RadarBlockEditUI(RadarBlock),
                HistogramBlock HistogramBlock => new HistogramBlockEditUI(HistogramBlock),

                AudioBlock AudioBlock => new AudioBlockEditUI(AudioBlock),
                FileBlock FileBlock => new FileBlockEditUI(FileBlock),
                VideoBlock VideoBlock => new VideoBlockEditUI(VideoBlock),
                MusicBlock MusicBlock => new MusicBlockEditUI(MusicBlock),
                ImageBlock ImageBlock => new ImageBlockEditUI(ImageBlock),
                ReferenceBlock ReferenceBlock => new ReferenceBlockEditUI(ReferenceBlock),
                _ => throw new ArgumentOutOfRangeException(nameof(block), block, null)
            };
        }
    }
}