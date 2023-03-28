using System;
using System.Globalization;
using System.Windows.Data;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{
    public enum ModuleTemplateConvertMode
    {
        Mystery,
        Name,
        Author,
        Contract,
        Intro,
        Block,
        Metadata
    }

    public class ModuleTemplateStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is not ModuleTemplateConvertMode mode)
            {
                mode = ModuleTemplateConvertMode.Name;
            }

            var language        = Language.Culture;
            var maybeNullString = value?.ToString();
            var isNull          = string.IsNullOrEmpty(maybeNullString);

            var pattern = mode switch
            {
                ModuleTemplateConvertMode.Mystery  => GetMysteryName(isNull, maybeNullString, language),
                ModuleTemplateConvertMode.Author   => GetAuthorList(isNull, maybeNullString, language),
                ModuleTemplateConvertMode.Contract => GetContractList(isNull, maybeNullString, language),
                ModuleTemplateConvertMode.Name     => GetName(isNull, maybeNullString, language),
                ModuleTemplateConvertMode.Intro    => GetIntro(isNull, maybeNullString, language),
                _                                  => GetCommonString(mode, language),
            };

            return string.IsNullOrEmpty(maybeNullString) ? pattern : maybeNullString;
        }

        private static string GetMysteryName(bool isNull, string value, CultureArea language)
        {
            if (isNull)
            {
                return language switch
                {
                    CultureArea.Chinese => "世界观名称",
                    _                   => "Mystery"
                };
            }

            var pattern = language switch
            {
                CultureArea.Chinese => "世界观名称：{0}",
                _                   => "MysteryName: {0}"
            };

            return string.Format(pattern, value);
        }

        private static string GetContractList(bool isNull, string value, CultureArea language)
        {
            if (isNull)
            {
                return language switch
                {
                    CultureArea.Chinese => "联系方式：未知",
                    _                   => "Contract: Unknown"
                };
            }

            var pattern = language switch
            {
                CultureArea.Chinese => "联系方式：{0}",
                _                   => "Contract: {0}"
            };

            return string.Format(pattern, value);
        }

        private static string GetAuthorList(bool isNull, string value, CultureArea language)
        {
            if (isNull)
            {
                return language switch
                {
                    CultureArea.Chinese => "作者：佚名",
                    _                   => "Author: Unknown"
                };
            }

            var pattern = language switch
            {
                CultureArea.Chinese => "作者：{0}",
                _                   => "Author: {0}"
            };

            return string.Format(pattern, value);
        }

        private static string GetCommonString(ModuleTemplateConvertMode mode, CultureArea language)
        {
            return language switch
            {
                CultureArea.Chinese => mode switch
                {
                    ModuleTemplateConvertMode.Block => "内容",
                    _                               => "喵喵咒语"
                },
                _ => mode switch
                {
                    ModuleTemplateConvertMode.Block => "Blocks",
                    _                               => "Kitty Spell"
                }
            };
        }


        private static string GetIntro(bool isNull, string value, CultureArea language)
        {
            if (isNull)
            {
                return language switch
                {
                    CultureArea.Chinese => "简介：暂无",
                    _                   => "Intro: nothing in here"
                };
            }

            return value;
        }

        private static string GetName(bool isNull, string value, CultureArea language)
        {
            if (isNull)
            {
                return language switch
                {
                    CultureArea.Chinese => "模板名字：暂无",
                    _                   => "Name: nothing in here"
                };
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}