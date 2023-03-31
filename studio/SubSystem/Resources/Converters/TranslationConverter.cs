﻿using System;
using System.Globalization;
using System.Windows.Data;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Services;
using Acorisoft.FutureGL.Forest.Styles;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaDB.Interfaces;
using Acorisoft.FutureGL.MigaStudio.Models;


namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{
    public class TranslationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                MainTheme mt       => GetMainTheme(mt),
                CultureArea ca     => GetCultureArea(ca),
                DocumentType dt    => GetDocumentType(dt),
                FilteringOption fo => GetFilteringOption(fo),
                OrderingOption oo  => GetOrderingOption(oo),
                _                  => string.Empty
            };
        }

        private static string GetFilteringOption(FilteringOption value)
        {
            return value switch
            {
                FilteringOption.Name => Language.Culture switch
                {
                    CultureArea.Chinese => "名字",
                    _                   => "Name"
                },
                FilteringOption.TimeOfCreated => Language.Culture switch
                {
                    CultureArea.Chinese => "创建时间",
                    _                   => "Created Time"
                },
                _ => Language.Culture switch
                {
                    CultureArea.Chinese => "修改时间",
                    _                   => "Modified Time"
                },
            };
        }

        private static string GetOrderingOption(OrderingOption value)
        {
            return value switch
            {
                OrderingOption.Ascending => Language.Culture switch
                {
                    CultureArea.Chinese => "升序",
                    _                   => "Ascending"
                },
                _ => Language.Culture switch
                {
                    CultureArea.Chinese => "降序",
                    _                   => "Descending"
                },
            };
        }

        private static string GetMainTheme(MainTheme value)
        {
            return value switch
            {
                MainTheme.Light => Language.Culture switch
                {
                    CultureArea.Chinese => "亮色模式",
                    _                   => "Light Theme"
                },
                MainTheme.Custom => Language.Culture switch
                {
                    CultureArea.Chinese => "自定义主题",
                    _                   => "Custom Theme"
                },
                _ => Language.Culture switch
                {
                    CultureArea.Chinese => "夜间模式",
                    _                   => "Dark Theme"
                },
            };
        }


        private static string GetDocumentType(DocumentType type)
        {
            return type switch
            {
                DocumentType.CharacterDocument => Language.Culture switch
                {
                    CultureArea.Chinese => "人物",
                    _                   => "Character"
                },
                DocumentType.AbilityDocument => Language.Culture switch
                {
                    CultureArea.Chinese => "能力",
                    _                   => "Ability"
                },
                DocumentType.GeographyDocument => Language.Culture switch
                {
                    CultureArea.Chinese => "地图",
                    _                   => "Geography"
                },
                DocumentType.ItemDocument => Language.Culture switch
                {
                    CultureArea.Chinese => "物品",
                    _                   => "Item"
                },
                _ => Language.Culture switch
                {
                    CultureArea.Chinese => "其他",
                    _                   => "Other"
                },
            };
        }

        private static string GetCultureArea(CultureArea value)
        {
            return value switch
            {
                CultureArea.English  => "English",
                CultureArea.French   => "Français",
                CultureArea.Japanese => "日本語",
                CultureArea.Korean   => "한국어",
                CultureArea.Russian  => "Русский",
                _                    => "简体中文"
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}