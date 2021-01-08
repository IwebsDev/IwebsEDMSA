using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Globalization;

namespace NumEditCtrlSL
{
    public enum DecimalSeparatorType : byte
    {
        System_Defined,
        Point,
        Comma
    }
    public enum NegativeSignType : byte
    {
        System_Defined,
        Minus
    }
    public enum NegativeSignSide : byte
    {
        System_Defined,
        Prefix,
        Suffix
    }
    public enum NegativePatternType : byte
    {
        System_Defined,
        Symbol_NoSpace,
        Symbol_Space,
        Parentheses
    }
    public enum GroupSeparatorType : byte
    {
        System_Defined,
        Comma,
        Point,
        Space,
        HalfSpace
    }
    public enum ScientificDisplayType : byte
    {
        CapitalE,
        SmallE,
        Pow10,
        ExpandOnExit
    }
    public class DisplayNumEdit : Control
    {
        static class SystemNumberInfo
        {
            static private NumberFormatInfo nfi;

            static SystemNumberInfo()
            {
                CultureInfo ci = CultureInfo.CurrentCulture;
                nfi = ci.NumberFormat;
            }
            public static string DecimalSeparator
            {
                get { return nfi.NumberDecimalSeparator; }
            }
            public static string GroupSeparator
            {
                get { return nfi.NumberGroupSeparator; }
            }
            public static string NegativeSign
            {
                get { return nfi.NegativeSign; }
            }
            public static bool IsNegativePrefix
            {
                // for values, see: http://msdn.microsoft.com/en-us/library/system.globalization.numberformatinfo.numbernegativepattern.aspx
                // Assume if negative number format is (xxx), number is prefixed.
                get
                {
                    return nfi.NumberNegativePattern < 3;
                }
            }
            public static bool IsNegativeParentheses
            {
                get
                {
                    return nfi.NumberNegativePattern == 0;
                }
            }
            public static bool IsNegativeSpaceSeparated
            {
                get
                {
                    return nfi.NumberNegativePattern == 2 || nfi.NumberNegativePattern == 4;
                }
            }
        }

        private static readonly DependencyProperty TextProperty =
                DependencyProperty.Register("Text", typeof(string), typeof(DisplayNumEdit), new PropertyMetadata(OnTextContentChanged));

        private static readonly DependencyProperty TextAlignmentProperty =
                DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(DisplayNumEdit), new PropertyMetadata(OnTextContentChanged));

        private static readonly DependencyProperty DecimalSeparatorTypeProperty =
                DependencyProperty.Register("DecimalSeparatorType", typeof(DecimalSeparatorType), typeof(DisplayNumEdit), null);

        private static readonly DependencyProperty NegativeSignTypeProperty =
                DependencyProperty.Register("NegativeSignType", typeof(NegativeSignType), typeof(DisplayNumEdit), null);

        private static readonly DependencyProperty NegativeSignSideProperty =
                DependencyProperty.Register("NegativeSignSide", typeof(NegativeSignSide), typeof(DisplayNumEdit), null);

        private static readonly DependencyProperty NegativePatternTypeProperty =
                DependencyProperty.Register("NegativePatternType", typeof(NegativePatternType), typeof(DisplayNumEdit), null);

        private static readonly DependencyProperty GroupSeparatorTypeProperty =
                DependencyProperty.Register("GroupSeparatorType", typeof(GroupSeparatorType), typeof(DisplayNumEdit), null);

        private static readonly DependencyProperty GroupSizeProperty =
                DependencyProperty.Register("GroupSize", typeof(int), typeof(DisplayNumEdit), null);

        private static readonly DependencyProperty NegativeTextBrushProperty =
                DependencyProperty.Register("NegativeTextBrush", typeof(Brush), typeof(DisplayNumEdit), null);

        private static readonly DependencyProperty ScientificDisplayTypeProperty =
                DependencyProperty.Register("ScientificDisplayType", typeof(ScientificDisplayType), typeof(DisplayNumEdit), new PropertyMetadata(ScientificDisplayType.Pow10));

        private static void OnTextContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DisplayNumEdit dne = d as DisplayNumEdit;

            if (dne != null)
                dne.Display();
        }
        
        private bool Initialized = false;

        public DisplayNumEdit()
        {
            this.DefaultStyleKey = typeof(DisplayNumEdit);

            this.IsEnabledChanged += new DependencyPropertyChangedEventHandler(DisplayNumEdit_IsEnabledChanged);
            this.LayoutUpdated += new EventHandler(DisplayNumEdit_LayoutUpdated);
        }

        void DisplayNumEdit_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Display();
        }

        void DisplayNumEdit_LayoutUpdated(object sender, EventArgs e)
        {
            if (!Initialized && Display())
                Initialized = true;
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        public DecimalSeparatorType DecimalSeparatorType
        {
            get { return (DecimalSeparatorType)GetValue(DecimalSeparatorTypeProperty); }
            set { SetValue(DecimalSeparatorTypeProperty, value); }
        }

        public NegativeSignType NegativeSignType
        {
            get { return (NegativeSignType)GetValue(NegativeSignTypeProperty); }
            set { SetValue(NegativeSignTypeProperty, value); }
        }

        public NegativeSignSide NegativeSignSide
        {
            get { return (NegativeSignSide)GetValue(NegativeSignSideProperty); }
            set { SetValue(NegativeSignSideProperty, value); }
        }

        public NegativePatternType NegativePatternType
        {
            get { return (NegativePatternType)GetValue(NegativePatternTypeProperty); }
            set { SetValue(NegativePatternTypeProperty, value); }
        }

        public GroupSeparatorType GroupSeparatorType
        {
            get { return (GroupSeparatorType)GetValue(GroupSeparatorTypeProperty); }
            set { SetValue(GroupSeparatorTypeProperty, value); }
        }

        public int GroupSize
        {
            get { return (int)GetValue(GroupSizeProperty); }
            set { SetValue(GroupSizeProperty, value); }
        }

        public Brush NegativeTextBrush
        {
            get { return (Brush)GetValue(NegativeTextBrushProperty); }
            set { SetValue(NegativeTextBrushProperty, value); }
        }

        public ScientificDisplayType ScientificDisplayType
        {
            get { return (ScientificDisplayType)GetValue(ScientificDisplayTypeProperty); }
            set { SetValue(ScientificDisplayTypeProperty, value); }
        }

        public string GetDecimalSeparator()
        {
            switch (DecimalSeparatorType)
            {
                case DecimalSeparatorType.Point:
                    return ".";
                case DecimalSeparatorType.Comma:
                    return ",";
                case DecimalSeparatorType.System_Defined:
                default:
                    return SystemNumberInfo.DecimalSeparator;
            }
        }

        public string GetNegativeSign()
        {
            switch (NegativeSignType)
            {
                case NegativeSignType.Minus:
                    return "-";
                case NegativeSignType.System_Defined:
                default:
                    return SystemNumberInfo.NegativeSign;
            }
        }

        public bool IsNegativePrefix()
        {
            switch (NegativeSignSide)
            {
                case NegativeSignSide.Prefix:
                    return true;
                case NegativeSignSide.Suffix:
                    return false;
                case NegativeSignSide.System_Defined:
                default:
                    return SystemNumberInfo.IsNegativePrefix;
            }
        }

        private string GetNegativePrefix()
        {
            if (NegativePatternType == NegativePatternType.Parentheses ||
                (NegativePatternType == NegativePatternType.System_Defined && SystemNumberInfo.IsNegativeParentheses))
                return "(";

            if (IsNegativePrefix())
            {
                if (NegativePatternType == NegativePatternType.Symbol_Space ||
                    (NegativePatternType == NegativePatternType.System_Defined && SystemNumberInfo.IsNegativeSpaceSeparated))
                    return GetNegativeSign() + " ";
                else
                    return GetNegativeSign();
            }
            return "";
        }

        private string GetNegativeSuffix()
        {
            if (NegativePatternType == NegativePatternType.Parentheses ||
                (NegativePatternType == NegativePatternType.System_Defined && SystemNumberInfo.IsNegativeParentheses))
                return ")";

            if (!IsNegativePrefix())
            {
                if (NegativePatternType == NegativePatternType.Symbol_Space ||
                    (NegativePatternType == NegativePatternType.System_Defined && SystemNumberInfo.IsNegativeSpaceSeparated))
                    return " " + GetNegativeSign();
                else
                    return GetNegativeSign();
            }
            return "";
        }

        private string GetGroupSeparator()
        {
            switch (GroupSeparatorType)
            {
                case GroupSeparatorType.System_Defined:
                    return SystemNumberInfo.GroupSeparator;

                case GroupSeparatorType.Comma:
                    return ",";

                case GroupSeparatorType.Point:
                    return ".";

                case GroupSeparatorType.Space:
                case GroupSeparatorType.HalfSpace:
                default:
                    return " ";
            }   
        }

        public void GetWholeNumeratorDenominator(string value, out string Whole, out string Scientific, out string Numerator, out string Denominator, out string Sign)
        {
            int i = 0, j = 0;
            string[] str = new string[3];
            bool HasFraction = false;
            Scientific = "";
            bool IsScientific = false, HasScientific = false;

            Sign = (value.Length > 0 && ((IsNegativePrefix() && value[0] == GetNegativeSign()[0]) ||
                 (!IsNegativePrefix() && value[value.Length - 1] == GetNegativeSign()[0]))) ? GetNegativeSign() : "";

            if (Sign == GetNegativeSign())
                value = value.Trim(GetNegativeSign()[0]);
                       
            while (i != value.Length && j < 3)
            {
                if (value[i] == 'e' || value[i] == 'E')
                {
                    IsScientific = true;
                    HasScientific = true;
                }
                else if (value[i] == ' ' || value[i] == '/')
                {
                    j++;
                    IsScientific = false;
                }
                else if (IsScientific)
                    Scientific += value[i];
                else
                    str[j] += value[i];

                if (value[i] == '/')
                    HasFraction = true;

                i++;
            }
            if (j == 1) 
            {
                if (HasFraction) // case where no space: no whole. Only a numerator and denominator
                {
                    str[2] = (string.IsNullOrEmpty(str[1])) ? "1" : str[1];
                    str[1] = (string.IsNullOrEmpty(str[0])) ? "1" : str[0];
                    str[0] = "";
                }
                // case when user enters something like " 3456"
                // in this case, assume user meant to enter "3456" and not "3456/1"
                else if (string.IsNullOrEmpty(str[0]) && !string.IsNullOrEmpty(str[1]))
                {
                    str[0] = str[1];
                    str[1] = "";
                }
                else if (!string.IsNullOrEmpty(str[1])) // No fraction explicitly entered. assume it is /1
                {
                    str[2] = "1";
                }
            }
            if (j == 2)
            {
                if (string.IsNullOrEmpty(str[1]))
                    str[1] = "1";
                if (string.IsNullOrEmpty(str[2]))
                    str[2] = "1";
            }
            if (HasScientific && Scientific == "" && string.IsNullOrEmpty(str[0]))
                str[0] = "1"; // Assume e <=> e0 <=> 1

            Whole = str[0];
            Numerator = str[1];
            Denominator = str[2];
        }

        private string FormatNumber(string strNumber)
        {
            if (strNumber != null)
            {
                int EndIndex = strNumber.IndexOf(GetDecimalSeparator()[0]);

                if (EndIndex == -1)
                    EndIndex = strNumber.Length;

                int Index = EndIndex - 1;
                int counter = -1;

                while (Index >= 0 && strNumber[Index] >= '0' && strNumber[Index] <= '9')
                {
                    counter++;

                    if (counter == GroupSize)
                    {
                        counter = 0;
                        strNumber = strNumber.Insert(Index + 1, GetGroupSeparator());
                    }
                    Index--;
                }
            }
            return strNumber;
        }

        private void GetScientificSymbolFontRatio(string Main, out string Symbol, out double FontRatio)
        {
            switch (ScientificDisplayType)
            {
                case ScientificDisplayType.CapitalE:
                    Symbol = "E";
                    FontRatio = 0.9;
                    break;

                case ScientificDisplayType.SmallE:
                    Symbol = "e";
                    FontRatio = 0.9;
                    break;

                case ScientificDisplayType.Pow10:
                default:
                    Symbol = !string.IsNullOrEmpty(Main) ? "x10" : "10";
                    FontRatio = 0.7;
                    break;
            }
        }  

        private void ProcessTextBlock(string Txt, ref TextBlock tbBlock, ref Point InitOffset, double FontRatio = -1.0, double VertOffset = 0.0)
        {
            if (!string.IsNullOrEmpty(Txt))
            {
                tbBlock.Text = Txt;
                tbBlock.Margin = new Thickness(InitOffset.X, InitOffset.Y + VertOffset, 0.0, 0.0);
                tbBlock.TextTrimming = TextTrimming.WordEllipsis;
                
                if (FontRatio != -1.0)
                    tbBlock.FontSize = FontRatio * FontSize;

                InitOffset.X += tbBlock.ActualWidth;
            }
            else if (tbBlock != null)
                tbBlock.Text = "";
        }

        private void ProcessMainTextBlock(string Main, ref TextBlock tbMain, ref Point InitOffset)
        {
            if (GroupSeparatorType == GroupSeparatorType.HalfSpace)
            {
                string digitset = "";

                for (int i = 0; i < Main.Length; i++)
                {
                    if (Main[i] == ' ')
                    {
                        Run tbr = new Run();
                        tbr.Text = digitset;
                        tbMain.Inlines.Add(tbr);

                        Run tbrspace = new Run();
                        tbrspace.Text = " ";
                        tbrspace.FontSize = FontSize / 2.0;
                        tbMain.Inlines.Add(tbrspace);
                        
                        digitset = "";
                    }
                    else
                        digitset += Main[i];               
                }
                Run tbrlast = new Run();
                tbrlast.Text = digitset;
                tbMain.Inlines.Add(tbrlast);
            }
            else
                tbMain.Text = Main;

            tbMain.Margin = new Thickness(InitOffset.X, InitOffset.Y, 0.0, 0.0);
            tbMain.TextTrimming = TextTrimming.WordEllipsis;
            InitOffset.X += tbMain.ActualWidth;
        }

        private double GetSumTextBlockWidth(params TextBlock[] TxtBlockArray)
        {
            double sumWidth = 0.0;

            foreach (TextBlock TxtBlock in TxtBlockArray)
                sumWidth += TxtBlock.ActualWidth;

            return sumWidth;
        }

        private void OffsetTextBlocks(double offset, params TextBlock[] TxtBlockArray)
        {
            foreach (TextBlock tbBlock in TxtBlockArray)
                tbBlock.Margin = new Thickness(tbBlock.Margin.Left + offset, tbBlock.Margin.Top, tbBlock.Margin.Right, tbBlock.Margin.Bottom);
        }

        private void SetFontColor(bool IsNegative, params TextBlock[] TxtBlockArray)
        {      
            foreach (TextBlock tbBlock in TxtBlockArray)
            {
                if (IsEnabled)
                {
                    if (NegativeTextBrush != null)
                        tbBlock.Foreground = ((IsNegative) ? NegativeTextBrush : Foreground);
                    else
                        tbBlock.Foreground = Foreground;
                }
                else
                {
                    if (NegativeTextBrush != null)
                        tbBlock.Foreground = ((IsNegative) ? NegativeTextBrush : Foreground);
                    else
                        tbBlock.Foreground = Foreground;

                    Color col = ((SolidColorBrush)tbBlock.Foreground).Color;
                    tbBlock.Foreground = new SolidColorBrush(Color.FromArgb(0xA0, col.R, col.G, col.B));
                }
            }
        }

        public bool Display()
        {
            TextBlock tbPrefixSign = (TextBlock)this.GetTemplateChild("PrefixSign");
            TextBlock tbMain = (TextBlock)this.GetTemplateChild("Main");
            TextBlock tbPreSci = (TextBlock)this.GetTemplateChild("PreSci");
            TextBlock tbScientific = (TextBlock)this.GetTemplateChild("Scientific");
            TextBlock tbNumerator = (TextBlock)this.GetTemplateChild("Numerator");
            TextBlock tbDenominator = (TextBlock)this.GetTemplateChild("Denominator");
            TextBlock tbSlash = (TextBlock)this.GetTemplateChild("Slash");
            TextBlock tbSuffixSign = (TextBlock)this.GetTemplateChild("SuffixSign");

            if (tbPrefixSign == null || tbMain == null || tbPreSci == null || tbScientific == null || tbNumerator == null
                                || tbDenominator == null || tbSlash == null || tbSuffixSign == null)
                return false;
            else if (string.IsNullOrEmpty(Text))
                return true;

            string Main, Scientific, Numerator, Denominator, Sign;
            string NegPrefix = null, NegSuffix = null;
            
            GetWholeNumeratorDenominator(Text, out Main, out Scientific, out Numerator, out Denominator, out Sign);
            Point InitOffset = new Point(3.0, 3.0);

            // VerticalContentAlignment does not work in Silverlight - does not work for a standard TextBox either.

            if (!string.IsNullOrEmpty(Sign))
            {
                SetFontColor(true, tbPrefixSign, tbMain, tbPreSci, tbScientific, tbNumerator, tbDenominator, tbSlash, tbSuffixSign);
                NegPrefix = GetNegativePrefix();
                NegSuffix = GetNegativeSuffix();
            }
            else
                SetFontColor(false, tbPrefixSign, tbMain, tbPreSci, tbScientific, tbNumerator, tbDenominator, tbSlash, tbSuffixSign);

            ProcessTextBlock(NegPrefix, ref tbPrefixSign, ref InitOffset);
                          
            tbMain.Text = "";

            if (!string.IsNullOrEmpty(Main))
            {
                Main = FormatNumber(Main);
                ProcessMainTextBlock(Main, ref tbMain, ref InitOffset);
            }

            string SciSymbol = null;
            double FontRatio = -1.0;

            if (!string.IsNullOrEmpty(Scientific))
            {
                GetScientificSymbolFontRatio(Main, out SciSymbol, out FontRatio);
            }

            ProcessTextBlock(SciSymbol, ref tbPreSci, ref InitOffset);
            ProcessTextBlock(Scientific, ref tbScientific, ref InitOffset, FontRatio);
            
            double TotalWidth, MainFracOffset = FontSize / 3.0, // MainFracOffset is distance separating main number from start of fraction
                       SlashExtraWidth = FontSize / 10.0;       // Extra width for slash for better look

            if (!string.IsNullOrEmpty(Numerator) && !string.IsNullOrEmpty(Denominator))
            {
                InitOffset.X += MainFracOffset;

                double NumVOffset = -(FontSize / 12.0), DenomVOffset = (FontSize / 3.0);

                ProcessTextBlock(Numerator, ref tbNumerator, ref InitOffset, 1.0 / 1.3, NumVOffset);

                string slash = "";
                slash += (char)164;

                tbSlash.Text = slash;
                tbSlash.Margin = new Thickness(InitOffset.X, InitOffset.Y, 0.0, 0.0);
                tbSlash.FontFamily = new FontFamily("Symbol");

                InitOffset.X += SlashExtraWidth;
                ProcessTextBlock(Denominator, ref tbDenominator, ref InitOffset, 1.0 / 1.3, DenomVOffset);
            }
            else
            {
                ProcessTextBlock(Numerator, ref tbNumerator, ref InitOffset, 0.0, 0.0);
                ProcessTextBlock(null, ref tbSlash, ref InitOffset, 0.0, 0.0);
                ProcessTextBlock(Denominator, ref tbDenominator, ref InitOffset, 0.0, 0.0);
            }

            ProcessTextBlock(NegSuffix, ref tbSuffixSign, ref InitOffset);

            TotalWidth = GetSumTextBlockWidth(tbPrefixSign, tbMain, tbPreSci, tbScientific, tbNumerator, tbDenominator, tbSlash, tbSuffixSign);

            if (!string.IsNullOrEmpty(Numerator) && !string.IsNullOrEmpty(Denominator))
                TotalWidth += MainFracOffset + SlashExtraWidth;

            double AvailableWidth = ActualWidth - 7.0;

            if (TotalWidth < AvailableWidth)
            {
                if (TextAlignment == TextAlignment.Right)
                    OffsetTextBlocks(AvailableWidth - TotalWidth, tbPrefixSign, tbMain, tbPreSci, tbScientific, tbNumerator, tbDenominator, tbSlash, tbSuffixSign);                 
                else if (TextAlignment == TextAlignment.Center)
                    OffsetTextBlocks((AvailableWidth - TotalWidth) / 2.0, tbPrefixSign, tbMain, tbPreSci, tbScientific, tbNumerator, tbDenominator, tbSlash, tbSuffixSign);                 
            }
            return true;
        }
    }
}


