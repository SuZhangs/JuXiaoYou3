using System.Text;
using Acorisoft.FutureGL.MigaDB.Contracts;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Modules;
using Acorisoft.FutureGL.MigaDB.Utils;

namespace Acorisoft.FutureGL.MigaDB.Data.DataParts
{
    public class PartOfModule : DataPart, IConvertibleDataSource
    {
        /// <summary>
        /// 清除所有属性的值。
        /// </summary>
        public void Clear()
        {
            Properties?.ForEach(x => x.ClearValue());
        }

        public static PartOfModule CreateSampleData()
        {
            return new PartOfModule
            {
                Properties = new List<ModuleProperty>
                {
                    CreateColor(),
                    CreateDegree(),
                    CreateText(),
                    CreatePage(),
                    CreateNumber(),
                    CreateSlider(),
                    
                    CreateHistogram(),
                    CreateRadar(),
                    
                    CreateFavorite(),
                    CreateTalent(),
                    CreateBinary(),
                    CreateSwitch(),
                    CreateRadio(),
                    
                    CreateSequence(),
                    CreateRate(),
                    CreateLikability(),
                    CreateGroup(),
                    
                    CreateMusic(),
                    CreateAudio(),
                    CreateFile(),
                    CreateVideo(),
                    CreateMusic(),
                    CreateReference()
                }
            };
        }

        /// <summary>
        /// 所有模组属性
        /// </summary>
        public List<ModuleProperty> Properties { get; init; }

        #region Properties

        public static GroupProperty CreateGroup()
        {
            return new GroupProperty
            {
                Id       = ID.Get(),
                Name     = nameof(GroupProperty),
                Metadata = nameof(GroupProperty),
                Items = new ModuleProperty[]
                {
                    CreateColor(),
                    CreateRate()
                }
            };
        }

        public static TextProperty CreateText()
        {
            return new TextProperty
            {
                Id       = ID.Get(),
                Name     = nameof(TextProperty),
                Metadata = nameof(TextProperty),
                Value    = nameof(TextProperty)
            };
        }

        public static PageProperty CreatePage()
        {
            return new PageProperty
            {
                Id       = ID.Get(),
                Name     = nameof(PageProperty),
                Metadata = nameof(PageProperty),
                Value    = nameof(PageProperty)
            };
        }

        public static ColorProperty CreateColor()
        {
            return new ColorProperty
            {
                Id       = ID.Get(),
                Name     = nameof(ColorProperty),
                Metadata = nameof(ColorProperty),
                Value    = "#007ACC"
            };
        }

        public static SliderProperty CreateSlider()
        {
            return new SliderProperty
            {
                Id       = ID.Get(),
                Name     = nameof(SliderProperty),
                Metadata = nameof(SliderProperty),
                Maximum  = 10,
                Minimum  = 1,
                Value    = "8"
            };
        }
        
        public static NumberProperty CreateNumber()
        {
            return new NumberProperty
            {
                Id       = ID.Get(),
                Name     = nameof(NumberProperty),
                Metadata = nameof(NumberProperty),
                Maximum  = 10,
                Minimum  = 1,
                Value    = "8"
            };
        }
        
        public static DegreeProperty CreateDegree()
        {
            return new DegreeProperty
            {
                Id         = ID.Get(),
                Name       = nameof(DegreeProperty),
                Metadata   = nameof(DegreeProperty),
                Negative   = nameof(DegreeProperty.Negative),
                Positive   = nameof(DegreeProperty.Positive),
                DivideLine = 6,
                Value      = "8"
            };
        }

        public static HistogramProperty CreateHistogram()
        {
            return new HistogramProperty
            {
                Id       = ID.Get(),
                Name     = nameof(HistogramProperty),
                Metadata = nameof(HistogramProperty),
                Axis = new []{ "Axis1", "Axis2", "Axis3", "Axis4"},
                Maximum = 20,
                FallbackValues = new []{3,4,5,6},
                ChartValues = new []{4,5,6,7},
            };
        }
        
        public static RadarProperty CreateRadar()
        {
            return new RadarProperty
            {
                Id             = ID.Get(),
                Name           = nameof(RadarProperty),
                Metadata       = nameof(RadarProperty),
                Axis           = new []{ "Axis1", "Axis2", "Axis3", "Axis4"},
                Maximum        = 20,
                FallbackValues = new []{3,4,5,6},
                ChartValues    = new []{4,5,6,7},
            };
        }

        public static RateProperty CreateRate()
        {
            return new RateProperty
            {
                Id       = ID.Get(),
                Name     = nameof(RateProperty),
                Metadata = nameof(RateProperty),
                Maximum = 10,
                Minimum = 1,
                Value = "3",
                Fallback = "4",
            };
        }
        
        public static LikabilityProperty CreateLikability()
        {
            return new LikabilityProperty
            {
                Id       = ID.Get(),
                Name     = nameof(LikabilityProperty),
                Metadata = nameof(LikabilityProperty),
                Maximum  = 10,
                Minimum  = 1,
                Value    = "3",
                Fallback = "4",
            };
        }
        
        public static SwitchProperty CreateSwitch()
        {
            return new SwitchProperty
            {
                Id       = ID.Get(),
                Name     = nameof(SwitchProperty),
                Metadata = nameof(SwitchProperty),
                Value    = "false",
                Fallback = "false",
            };
        }
        
        public static TalentProperty CreateTalent()
        {
            return new TalentProperty
            {
                Id       = ID.Get(),
                Name     = nameof(TalentProperty),
                Metadata = nameof(TalentProperty),
                Value    = "false",
                Fallback = "false",
            };
        }
        
        public static FavoriteProperty CreateFavorite()
        {
            return new FavoriteProperty
            {
                Id       = ID.Get(),
                Name     = nameof(FavoriteProperty),
                Metadata = nameof(FavoriteProperty),
                Value    = "false",
                Fallback = "false",
            };
        }
        
        public static BinaryProperty CreateBinary()
        {
            return new BinaryProperty
            {
                Id       = ID.Get(),
                Name     = nameof(BinaryProperty),
                Metadata = nameof(BinaryProperty),
                Negative = nameof(BinaryProperty.Negative),
                Positive = nameof(BinaryProperty.Positive),
                Value    = "false",
                Fallback = "false",
            };
        }
        
        public static RadioProperty CreateRadio()
        {
            return new RadioProperty
            {
                Id       = ID.Get(),
                Name     = nameof(RadioProperty),
                Metadata = nameof(RadioProperty),
                Items = new []
                {
                    new RadioProperty.Item{ Name = "Item1", Value = "Item1"},
                    new RadioProperty.Item{ Name = "Item2", Value = "Item2"},
                    new RadioProperty.Item{ Name = "Item3", Value = "Item3"},
                },
                Value    = "Item3",
                Fallback = "Item1",
            };
        }
        
        public static SequenceProperty CreateSequence()
        {
            return new SequenceProperty
            {
                Id       = ID.Get(),
                Name     = nameof(SequenceProperty),
                Metadata = nameof(SequenceProperty),
                Items = new []
                {
                    new SequenceProperty.Item{ Name = "Item1"},
                    new SequenceProperty.Item{ Name = "Item2"},
                    new SequenceProperty.Item{ Name = "Item3"},
                },
                Value    = "Item3",
                Fallback = "Item1",
            };
        }
        
        public static ReferenceProperty CreatReference()
        {
            return new ReferenceProperty
            {
                Id            = ID.Get(),
                Name          = nameof(ReferenceProperty),
                Metadata      = nameof(ReferenceProperty),
                DisplayName   = nameof(ReferenceProperty),
                DisplaySource = nameof(ReferenceProperty),
                Thumbnail     = nameof(ReferenceProperty),
                Value         = "Item3",
                Fallback      = "Item1",
            };
        }
        
        public static ImageProperty CreatImage()
        {
            return new ImageProperty
            {
                Id            = ID.Get(),
                Name          = nameof(ImageProperty),
                Metadata      = nameof(ImageProperty),
                DisplayName   = nameof(ImageProperty),
                DisplaySource = nameof(ImageProperty),
                Thumbnail     = nameof(ImageProperty),
                Value         = "Item3",
                Fallback      = "Item1",
            };
        }
        
        public static ReferenceProperty CreateReference()
        {
            return new ReferenceProperty
            {
                Id            = ID.Get(),
                Name          = nameof(ReferenceProperty),
                Metadata      = nameof(ReferenceProperty),
                DisplayName   = nameof(ReferenceProperty),
                DisplaySource = nameof(ReferenceProperty),
                Thumbnail     = nameof(ReferenceProperty),
                Value         = "Item3",
                Fallback      = "Item1",
            };
        }
        public static AudioProperty CreateAudio()
        {
            return new AudioProperty
            {
                Id            = ID.Get(),
                Name          = nameof(AudioProperty),
                Metadata      = nameof(AudioProperty),
                DisplayName   = nameof(AudioProperty),
                DisplaySource = nameof(AudioProperty),
                Thumbnail     = nameof(AudioProperty),
                Value         = "Item3",
                Fallback      = "Item1",
            };
        }
        
        public static MusicProperty CreateMusic()
        {
            return new MusicProperty
            {
                Id            = ID.Get(),
                Name          = nameof(MusicProperty),
                Metadata      = nameof(MusicProperty),
                DisplayName   = nameof(MusicProperty),
                DisplaySource = nameof(MusicProperty),
                Thumbnail     = nameof(MusicProperty),
                Value         = "Item3",
                Fallback      = "Item1",
            };
        }
        
        public static FileProperty CreateFile()
        {
            return new FileProperty
            {
                Id            = ID.Get(),
                Name          = nameof(FileProperty),
                Metadata      = nameof(FileProperty),
                DisplayName   = nameof(FileProperty),
                DisplaySource = nameof(FileProperty),
                Thumbnail     = nameof(FileProperty),
                Value         = "Item3",
                Fallback      = "Item1",
            };
        }
        
        
        public static VideoProperty CreateVideo()
        {
            return new VideoProperty
            {
                Id            = ID.Get(),
                Name          = nameof(VideoProperty),
                Metadata      = nameof(VideoProperty),
                DisplayName   = nameof(VideoProperty),
                DisplaySource = nameof(VideoProperty),
                Thumbnail     = nameof(VideoProperty),
                Value         = "Item3",
                Fallback      = "Item1",
            };
        }
        #endregion

        public string ToPlainText()
        {
            var sb = new StringBuilder();
            foreach (var property in Properties)
            {
                sb.Append(property.ToPlainText());
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        public string ToMarkdown()
        {
            var sb = new StringBuilder();
            foreach (var property in Properties)
            {
                sb.Append(property.ToMarkdown());
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}