using System;
using System.Collections.Generic;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Module;

namespace Acorisoft.FutureGL.MigaStudio.Modules.ViewModels
{
    public class ModuleBlockFactory
    {
        public static IEnumerable<ModuleBlock> CreateBlocks()
        {
            return new ModuleBlock[]
            {
                new SingleLineBlock(),
                new MultiLineBlock(),
                new ColorBlock(),
                new NumberBlock(),
                new SliderBlock(),
                
                new DegreeBlock(),
                new SwitchBlock(),
                new BinaryBlock(),
                new HeartBlock(),
                new StarBlock(),
                
                
                new RadioBlock(),
                new SequenceBlock(),
                new GroupBlock(),
                
                new LikabilityBlock(),
                new RateBlock(),
                
                new RadarBlock(),
                new HistogramBlock(),
                
                new AudioBlock(),
                new FileBlock(),
                new VideoBlock(),
                new MusicBlock(),
                new ImageBlock(),
                new ReferenceBlock(),
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