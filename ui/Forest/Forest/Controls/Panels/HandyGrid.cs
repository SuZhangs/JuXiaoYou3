using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text;
using System.Windows.Markup;

namespace Acorisoft.FutureGL.Forest.Controls.Panels
{
    public static class StringExtension
    {
        public static T Value<T>(this string input)
        {
            try
            {
                return (T) TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(input);
            }
            catch
            {
                return default;
            }
        }

        public static object Value(this string input, Type type)
        {
            try
            {
                return TypeDescriptor.GetConverter(type).ConvertFromString(input);
            }
            catch
            {
                return null;
            }
        }
    }

    public enum ColLayoutStatus
    {
        Xs,
        Sm,
        Md,
        Lg,
        Xl,
        Xxl,
        Auto
    }

    internal class TokenizerHelper
    {
        private char _quoteChar;

        private char _argSeparator;

        private string _str;

        private int _strLen;

        private int _charIndex;

        private int _currentTokenIndex;

        private int _currentTokenLength;

        public bool FoundSeparator { get; private set; }

        public TokenizerHelper(string str, IFormatProvider formatProvider)
        {
            var numberSeparator = GetNumericListSeparator(formatProvider);
            Initialize(str, '\'', numberSeparator);
        }

        private void Initialize(string str, char quoteChar, char separator)
        {
            _str               = str;
            _strLen            = str?.Length ?? 0;
            _currentTokenIndex = -1;
            _quoteChar         = quoteChar;
            _argSeparator      = separator;

            // immediately forward past any whitespace so
            // NextToken() logic always starts on the first
            // character of the next token.
            while (_charIndex < _strLen)
            {
                if (!char.IsWhiteSpace(_str, _charIndex))
                {
                    break;
                }

                ++_charIndex;
            }
        }

        public string GetCurrentToken() =>
            _currentTokenIndex < 0 ? null : _str.Substring(_currentTokenIndex, _currentTokenLength);

        internal bool NextToken() => NextToken(false);

        public bool NextToken(bool allowQuotedToken) => NextToken(allowQuotedToken, _argSeparator);

        public bool NextToken(bool allowQuotedToken, char separator)
        {
            _currentTokenIndex = -1;    // reset the currentTokenIndex
            FoundSeparator     = false; // reset

            // If we're at end of the string, just return false.
            if (_charIndex >= _strLen)
            {
                return false;
            }

            var currentChar = _str[_charIndex];

            // setup the quoteCount
            var quoteCount = 0;

            // If we are allowing a quoted token and this token begins with a quote,
            // set up the quote count and skip the initial quote
            if (allowQuotedToken &&
                currentChar == _quoteChar)
            {
                quoteCount++; // increment quote count
                ++_charIndex; // move to next character
            }

            var newTokenIndex  = _charIndex;
            var newTokenLength = 0;

            // loop until hit end of string or hit a , or whitespace
            // if at end of string ust return false.
            while (_charIndex < _strLen)
            {
                currentChar = _str[_charIndex];

                // if have a QuoteCount and this is a quote
                // decrement the quoteCount
                if (quoteCount > 0)
                {
                    // if anything but a quoteChar we move on
                    if (currentChar == _quoteChar)
                    {
                        --quoteCount;

                        // if at zero which it always should for now
                        // break out of the loop
                        if (0 == quoteCount)
                        {
                            ++_charIndex; // move past the quote
                            break;
                        }
                    }
                }
                else if (char.IsWhiteSpace(currentChar) || currentChar == separator)
                {
                    if (currentChar == separator)
                    {
                        FoundSeparator = true;
                    }

                    break;
                }

                ++_charIndex;
                ++newTokenLength;
            }

            // if quoteCount isn't zero we hit the end of the string
            // before the ending quote
            if (quoteCount > 0)
            {
                throw new InvalidOperationException("TokenizerHelperMissingEndQuote");
            }

            ScanToNextToken(separator); // move so at the start of the nextToken for next call

            // finally made it, update the _currentToken values
            _currentTokenIndex  = newTokenIndex;
            _currentTokenLength = newTokenLength;

            if (_currentTokenLength < 1)
            {
                throw new InvalidOperationException("TokenizerHelperEmptyToken");
            }

            return true;
        }

        private void ScanToNextToken(char separator)
        {
            // if already at end of the string don't bother
            if (_charIndex >= _strLen) return;

            var currentChar = _str[_charIndex];

            // check that the currentChar is a space or the separator.  If not
            // we have an error. this can happen in the quote case
            // that the char after the quotes string isn't a char.
            if (currentChar != separator && !char.IsWhiteSpace(currentChar))
            {
                throw new InvalidOperationException("TokenizerHelperExtraDataEncountered");
            }

            // loop until hit a character that isn't
            // an argument separator or whitespace.
            // !!!Todo: if more than one argSet throw an exception
            var argSepCount = 0;
            while (_charIndex < _strLen)
            {
                currentChar = _str[_charIndex];

                if (currentChar == separator)
                {
                    FoundSeparator = true;
                    ++argSepCount;
                    _charIndex++;

                    if (argSepCount > 1)
                    {
                        throw new InvalidOperationException("TokenizerHelperEmptyToken");
                    }
                }
                else if (char.IsWhiteSpace(currentChar))
                {
                    ++_charIndex;
                }
                else
                {
                    break;
                }
            }

            // if there was a separatorChar then we shouldn't be
            // at the end of string or means there was a separator
            // but there isn't an arg

            if (argSepCount > 0 && _charIndex >= _strLen)
            {
                throw new InvalidOperationException("TokenizerHelperEmptyToken");
            }
        }

        internal static char GetNumericListSeparator(IFormatProvider provider)
        {
            var numericSeparator = ',';

            // Get the NumberFormatInfo out of the provider, if possible
            // If the IFormatProvider doesn't not contain a NumberFormatInfo, then
            // this method returns the current culture's NumberFormatInfo.
            var numberFormat = NumberFormatInfo.GetInstance(provider);

            // Is the decimal separator is the same as the list separator?
            // If so, we use the ";".
            if (numberFormat.NumberDecimalSeparator.Length > 0 && numericSeparator == numberFormat.NumberDecimalSeparator[0])
            {
                numericSeparator = ';';
            }

            return numericSeparator;
        }
    }

    public class ColLayoutConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext typeDescriptorContext, Type sourceType)
        {
            switch (Type.GetTypeCode(sourceType))
            {
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.String:
                    return true;
                default:
                    return false;
            }
        }

        public override bool CanConvertTo(ITypeDescriptorContext typeDescriptorContext, Type destinationType) =>
            destinationType == typeof(InstanceDescriptor) || destinationType == typeof(string);

        public override object ConvertFrom(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object source)
        {
            if (source == null) throw GetConvertFromException(null);

            return source switch
            {
                string s => FromString(s, cultureInfo),
                double d => new ColLayout((int)d),
                _        => new ColLayout(Convert.ToInt32(source, cultureInfo))
            };
        }

        [SecurityCritical]
        public override object ConvertTo(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object value, Type destinationType)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            if (destinationType == null) throw new ArgumentNullException(nameof(destinationType));

            if (!(value is ColLayout th)) throw new ArgumentException("UnexpectedParameterType");

            if (destinationType == typeof(string)) return ToString(th, cultureInfo);
            if (destinationType == typeof(InstanceDescriptor))
            {
                var ci = typeof(ColLayout).GetConstructor(new[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int) });
                return new InstanceDescriptor(ci, new object[] { th.Xs, th.Sm, th.Md, th.Lg, th.Xl, th.Xxl });
            }

            throw new ArgumentException("CannotConvertType");
        }

        private static string ToString(ColLayout th, CultureInfo cultureInfo)
        {
            var listSeparator = TokenizerHelper.GetNumericListSeparator(cultureInfo);

            // Initial capacity [128] is an estimate based on a sum of:
            // 72 = 6x double (twelve digits is generous for the range of values likely)
            //  4 = 4x separator characters
            var sb = new StringBuilder(128);

            sb.Append(th.Xs.ToString(cultureInfo));
            sb.Append(listSeparator);
            sb.Append(th.Sm.ToString(cultureInfo));
            sb.Append(listSeparator);
            sb.Append(th.Md.ToString(cultureInfo));
            sb.Append(listSeparator);
            sb.Append(th.Lg.ToString(cultureInfo));
            sb.Append(listSeparator);
            sb.Append(th.Xl.ToString(cultureInfo));
            sb.Append(listSeparator);
            sb.Append(th.Xxl.ToString(cultureInfo));
            return th.ToString();
        }

        private static ColLayout FromString(string s, CultureInfo cultureInfo)
        {
            var th      = new TokenizerHelper(s, cultureInfo);
            var lengths = new int[6];
            var i       = 0;

            while (th.NextToken())
            {
                if (i >= 6)
                {
                    i = 7; // Set i to a bad value. 
                    break;
                }

                lengths[i] = th.GetCurrentToken().Value<int>();
                i++;
            }

            return i switch
            {
                1 => new ColLayout(lengths[0]),
                2 => new ColLayout { Xs = lengths[0], Sm = lengths[1] },
                3 => new ColLayout { Xs = lengths[0], Sm = lengths[1], Md = lengths[2] },
                4 => new ColLayout { Xs = lengths[0], Sm = lengths[1], Md = lengths[2], Lg = lengths[3] },
                5 => new ColLayout
                {
                    Xs = lengths[0],
                    Sm = lengths[1],
                    Md = lengths[2],
                    Lg = lengths[3],
                    Xl = lengths[4]
                },
                6 => new ColLayout
                {
                    Xs  = lengths[0],
                    Sm  = lengths[1],
                    Md  = lengths[2],
                    Lg  = lengths[3],
                    Xl  = lengths[4],
                    Xxl = lengths[5]
                },
                _ => throw new FormatException("InvalidStringColLayout")
            };
        }
    }

    [TypeConverter(typeof(ColLayoutConverter))]
    public class ColLayout : MarkupExtension
    {
        public static readonly int ColMaxCellCount = 24;

        public static readonly int HalfColMaxCellCount = 12;

        public static readonly int XsMaxWidth = 768;

        public static readonly int SmMaxWidth = 992;

        public static readonly int MdMaxWidth = 1200;

        public static readonly int LgMaxWidth = 1920;

        public static readonly int XlMaxWidth = 2560;

        public int Xs { get; set; } = 24;

        public int Sm { get; set; } = 12;

        public int Md { get; set; } = 8;

        public int Lg { get; set; } = 6;

        public int Xl { get; set; } = 4;

        public int Xxl { get; set; } = 2;

        public ColLayout()
        {
        }

        public ColLayout(int uniformWidth)
        {
            Xs  = uniformWidth;
            Sm  = uniformWidth;
            Md  = uniformWidth;
            Lg  = uniformWidth;
            Xl  = uniformWidth;
            Xxl = uniformWidth;
        }

        public ColLayout(int xs, int sm, int md, int lg, int xl, int xxl)
        {
            Xs  = xs;
            Sm  = sm;
            Md  = md;
            Lg  = lg;
            Xl  = xl;
            Xxl = xxl;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new ColLayout
            {
                Xs  = Xs,
                Sm  = Sm,
                Md  = Md,
                Lg  = Lg,
                Xl  = Xl,
                Xxl = Xxl
            };
        }

        public static ColLayoutStatus GetLayoutStatus(double width)
        {
            if (width < MdMaxWidth)
            {
                if (width < SmMaxWidth)
                {
                    if (width < XsMaxWidth)
                    {
                        return ColLayoutStatus.Xs;
                    }

                    return ColLayoutStatus.Sm;
                }

                return ColLayoutStatus.Md;
            }

            if (width < XlMaxWidth)
            {
                if (width < LgMaxWidth)
                {
                    return ColLayoutStatus.Lg;
                }

                return ColLayoutStatus.Xl;
            }

            return ColLayoutStatus.Xxl;
        }

        public override string ToString()
        {
            var cultureInfo   = CultureInfo.CurrentCulture;
            var listSeparator = TokenizerHelper.GetNumericListSeparator(cultureInfo);

            // Initial capacity [128] is an estimate based on a sum of:
            // 72 = 6x double (twelve digits is generous for the range of values likely)
            //  4 = 4x separator characters
            var sb = new StringBuilder(128);

            sb.Append(Xs.ToString(cultureInfo));
            sb.Append(listSeparator);
            sb.Append(Sm.ToString(cultureInfo));
            sb.Append(listSeparator);
            sb.Append(Md.ToString(cultureInfo));
            sb.Append(listSeparator);
            sb.Append(Lg.ToString(cultureInfo));
            sb.Append(listSeparator);
            sb.Append(Xl.ToString(cultureInfo));
            sb.Append(listSeparator);
            sb.Append(Xxl.ToString(cultureInfo));
            return sb.ToString();
        }
    }

    public class Row : Panel
    {
        /// <summary>
        ///     是否在正浮点数范围内（包括0）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInRangeOfPosDoubleIncludeZero(object value)
        {
            var v = (double) value;
            return !(double.IsNaN(v) || double.IsInfinity(v)) && v >= 0;
        }
        
        private ColLayoutStatus _layoutStatus;

        private double _maxChildDesiredHeight;

        private double _totalAutoWidth;

        public static readonly DependencyProperty GutterProperty = DependencyProperty.Register(
            nameof(Gutter), typeof(double), typeof(Row), new PropertyMetadata(Boxing.DoubleValues[0], null, OnGutterCoerce), IsInRangeOfPosDoubleIncludeZero);

        private static object OnGutterCoerce(DependencyObject d, object baseValue) => IsInRangeOfPosDoubleIncludeZero(baseValue) ? baseValue : .0;

        public double Gutter
        {
            get => (double)GetValue(GutterProperty);
            set => SetValue(GutterProperty, value);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var totalCellCount = 0;
            var totalRowCount  = 1;
            var gutterHalf     = Gutter / 2;
            _totalAutoWidth = 0;

            foreach (var child in InternalChildren.OfType<Column>())
            {
                child.Margin = new Thickness(gutterHalf);
                child.Measure(constraint);
                var childDesiredSize = child.DesiredSize;

                if (_maxChildDesiredHeight < childDesiredSize.Height)
                {
                    _maxChildDesiredHeight = childDesiredSize.Height;
                }

                var cellCount = child.GetLayoutCellCount(_layoutStatus);
                totalCellCount += cellCount;

                if (totalCellCount > ColLayout.ColMaxCellCount)
                {
                    totalCellCount = cellCount;
                    totalRowCount++;
                }

                if (cellCount == 0 || child.IsFixed)
                {
                    _totalAutoWidth += childDesiredSize.Width;
                }
            }

            return new Size(0, _maxChildDesiredHeight * totalRowCount - Gutter);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var totalCellCount = 0;
            var gutterHalf     = Gutter / 2;
            var itemWidth      = (finalSize.Width - _totalAutoWidth + Gutter) / ColLayout.ColMaxCellCount;
            itemWidth = Math.Max(0, itemWidth);

            var childBounds = new Rect(-gutterHalf, -gutterHalf, 0, _maxChildDesiredHeight);
            _layoutStatus = ColLayout.GetLayoutStatus(finalSize.Width);

            foreach (var child in InternalChildren.OfType<Column>())
            {
                if (!child.IsVisible)
                {
                    continue;
                }

                var cellCount = child.GetLayoutCellCount(_layoutStatus);
                totalCellCount += cellCount;

                var childWidth = cellCount > 0 ? cellCount * itemWidth : child.DesiredSize.Width;

                childBounds.Width =  childWidth;
                childBounds.X     += child.Offset * itemWidth;
                if (totalCellCount > ColLayout.ColMaxCellCount)
                {
                    childBounds.X  =  -gutterHalf;
                    childBounds.Y  += _maxChildDesiredHeight;
                    totalCellCount =  cellCount;
                }

                child.Arrange(childBounds);
                childBounds.X += childWidth;
            }

            return finalSize;
        }
    }

    public class Column : ForestContentControlBase
    {
        public static readonly DependencyProperty LayoutProperty = DependencyProperty.Register(
            nameof(Layout), typeof(ColLayout), typeof(Column), new PropertyMetadata(default(ColLayout)));

        public ColLayout Layout
        {
            get => (ColLayout)GetValue(LayoutProperty);
            set => SetValue(LayoutProperty, value);
        }

        public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register(
            nameof(Offset), typeof(int), typeof(Column), new PropertyMetadata(Boxing.IntValues[0]));

        public int Offset
        {
            get => (int)GetValue(OffsetProperty);
            set => SetValue(OffsetProperty, value);
        }

        public static readonly DependencyProperty SpanProperty = DependencyProperty.Register(
            nameof(Span), typeof(int), typeof(Column), new PropertyMetadata(24), OnSpanValidate);

        private static bool OnSpanValidate(object value)
        {
            var v = (int)value;
            return v is >= 1 and <= 24;
        }

        public int Span
        {
            get => (int)GetValue(SpanProperty);
            set => SetValue(SpanProperty, value);
        }

        public static readonly DependencyProperty IsFixedProperty = DependencyProperty.Register(
            nameof(IsFixed), typeof(bool), typeof(Column), new PropertyMetadata(Boxing.False));

        public bool IsFixed
        {
            get => (bool)GetValue(IsFixedProperty);
            set => SetValue(IsFixedProperty, Boxing.Box(value));
        }

        internal int GetLayoutCellCount(ColLayoutStatus status)
        {
            var result = 0;

            if (Layout != null)
            {
                if (!IsFixed)
                {
                    switch (status)
                    {
                        case ColLayoutStatus.Xs:
                            result = Layout.Xs;
                            break;
                        case ColLayoutStatus.Sm:
                            result = Layout.Sm;
                            break;
                        case ColLayoutStatus.Md:
                            result = Layout.Md;
                            break;
                        case ColLayoutStatus.Lg:
                            result = Layout.Lg;
                            break;
                        case ColLayoutStatus.Xl:
                            result = Layout.Xl;
                            break;
                        case ColLayoutStatus.Xxl:
                            result = Layout.Xxl;
                            break;
                        case ColLayoutStatus.Auto:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(status), status, null);
                    }
                }
            }
            else
            {
                result = Span;
            }

            return result;
        }
    }
}