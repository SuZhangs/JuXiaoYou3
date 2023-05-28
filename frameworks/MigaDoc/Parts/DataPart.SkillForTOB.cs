using System.Collections.ObjectModel;

namespace Acorisoft.Miga.Doc.Parts
{
    public class SkillForTOB : CustomDataPart
    {
        public static SkillForTOB Create()
        {
            return new SkillForTOB
            {
                Id      = Guid.Parse("B005AE13-E04C-4FB5-981F-BCEA98F550D3"),
                Name    = "基础信息",
                Version = 1,
                Properties = new List<InputProperty>
                {
                    new TextProperty
                    {
                        Name = "名字",
                        Metadata = KnownMetadataNames.Name,
                        Fallback = "未命名",
                    },
                    new SequenceProperty
                    {
                        Name     = "分类",
                        Fallback = "战斗系",
                        Metadata = KnownMetadataNames.Skill_Identifier,
                        Values = new ObservableCollection<Value>
                        {
                            new Value{ Text = "战斗系"},
                            new Value{ Text = "天赋系"},
                            new Value{ Text = "生产系"},
                            new Value{ Text = "辅助系"},
                            new Value{ Text = "特殊系"},
                        }
                    },
                    new SequenceProperty
                    {
                        Name = "类型",
                        Fallback = "能力",
                        Metadata = KnownMetadataNames.Skill_Type,
                        Values = new ObservableCollection<Value>
                        {
                            new Value{ Text = "能力"},
                            new Value{ Text = "代价"},
                            new Value{ Text = "被动"},
                            new Value{ Text = "光环"},
                            new Value{ Text = "祝福"},
                            new Value{ Text = "诅咒"},
                        }
                    },
                    new SequenceProperty
                    {
                        Name     = "弹道",
                        Fallback = "方向",
                        Metadata = KnownMetadataNames.Skill_Attack,
                        Values = new ObservableCollection<Value>
                        {
                            new Value{ Text = "方向"},
                            new Value{ Text = "范围"},
                            new Value{ Text = "位置"},
                            new Value{ Text = "投掷"},
                            new Value{ Text = "自身"},
                            new Value{ Text = "特定"},
                            new Value{ Text = "附加"},
                        }
                    },
                    new SequenceProperty
                    {
                        Name     = "施放",
                        Fallback = "无",
                        Metadata = KnownMetadataNames.Skill_Cast,
                        Values = new ObservableCollection<Value>
                        {
                            new Value{ Text = "瞬发"},
                            new Value{ Text = "咏唱"},
                            new Value{ Text = "结印"},
                            new Value{ Text = "画符"},
                            new Value{ Text = "附加"},
                            new Value{ Text = "祝福"},
                            new Value{ Text = "诅咒"},
                            new Value{ Text = "无"},
                        }
                    },
                    new SequenceProperty
                    {
                        Name     = "属性",
                        Fallback = "无",
                        Metadata = KnownMetadataNames.Elemental,
                        Values = new ObservableCollection<Value>
                        {
                            new Value{ Text = "光"},
                            new Value{ Text = "风"},
                            new Value{ Text = "火"},
                            new Value{ Text = "水"},
                            new Value{ Text = "岩"},
                            new Value{ Text = "雷"},
                            new Value{ Text = "暗"},
                            new Value{ Text = "无"},
                        }
                    },
                    new SequenceProperty
                    {
                        Name     = "稀有度",
                        Fallback = "无",
                        Metadata = KnownMetadataNames.Rarity,
                        Values = new ObservableCollection<Value>
                        {
                            new Value{ Text = "普通"},
                            new Value{ Text = "精英"},
                            new Value{ Text = "稀有"},
                            new Value{ Text = "传奇"},
                            new Value{ Text = "史诗"},
                        }
                    },
                    new SequenceProperty
                    {
                        Name     = "槽位",
                        Fallback = "1",
                        Metadata = KnownMetadataNames.Skill_Slot,
                        Values = new ObservableCollection<Value>
                        {
                            new Value{ Text = "1"},
                            new Value{ Text = "2"},
                            new Value{ Text = "3"},
                            new Value{ Text = "4"},
                        }
                    },
                    new GroupProperty
                    {
                        Name     = "特殊选项",
                        Values = new ObservableCollection<OptionProperty>
                        {
                            new OptionProperty
                            {
                                Name = "权能",
                            },
                            new OptionProperty
                            {
                                Name = "领域"
                            },
                        }
                    },
                    new PageProperty
                    {
                        Name = "内容",
                        Metadata = KnownMetadataNames.Skill_Content,
                    }
                }
            };
        }
    }
}