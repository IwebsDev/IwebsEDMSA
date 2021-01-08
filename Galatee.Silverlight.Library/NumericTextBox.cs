using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Galatee.Silverlight.Library
{
    public class NumericTextBox : TextBox
    {
        private bool caplsLocked = false;
        public NumericTextBox()
        {
            this.DefaultStyleKey = typeof(TextBox);
            this.KeyDown += NumTextBox_KeyDown;  
            this.GotFocus += NumTextBox_GotFocus;
        }

        void NumTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.SelectAll();
        }

        void NumTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            //Déterminer si la touche shift est pressée
            ModifierKeys keys = Keyboard.Modifiers;
            bool shiftKey = (keys & ModifierKeys.Shift) != 0;

            //Valider le contenu du textbox en tant que entier (Integer)
            if (this.IsInteger)
            {
                if ((e.Key < Key.D0 || e.Key > Key.D9))
                {
                    if (e.Key < Key.NumPad0 || e.Key > Key.NumPad9)
                    {
                        if (e.Key != Key.Back)
                        {
                            e.Handled = true;                            
                        }
                    }
                }
                else
                {
                    if (!shiftKey)
                    {
                        e.Handled = true;
                    }
                }
            }
            else
            {
                //Nombre à virgule flottante
                if (e.Key < Key.D0 || e.Key > Key.D9)
                {
                    char separator = ',';
                    if (this.Locale == locale.FR)
                        separator = ',';
                    else
                        separator = '.';

                    if (e.Key < Key.NumPad0 || e.Key > Key.NumPad9)
                    {
                        if (e.Key != Key.Back && e.Key != Key.Delete) //&& e.Key != Key.Decimal
                        {
                            e.Handled = true;
                        }
                        else if (!this.Text.Contains(separator))
                        {
                            e.Handled = true;
                        }
                    }
                }
                else
                {
                    if (!shiftKey)
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        /// <summary>
        /// PlaceHolder dependency property
        /// </summary>
        public static readonly DependencyProperty PlaceHolderProperty =
            DependencyProperty.Register(
                "PlaceHolder",
                typeof(string),
                typeof(NumericTextBox),
                new PropertyMetadata("Enter value ..."));

        /// <summary>
        /// PlaceHolder foreground property
        /// </summary>
        public static readonly DependencyProperty PlaceHolderForegroundColorProperty =
            DependencyProperty.Register(
                "PlaceHolderForegroundColor",
                typeof(Color),
                typeof(NumericTextBox),
                new PropertyMetadata(Colors.Gray));

        /// <summary>
        /// PlaceHolder isInteger property
        /// </summary>
        public static readonly DependencyProperty PlaceHolderIsIntegerProperty =
            DependencyProperty.Register(
                "PlaceHolderForegroundColor",
                typeof(bool),
                typeof(NumericTextBox),
                new PropertyMetadata(true));

        /// <summary>
        /// PlaceHolder isInteger property
        /// </summary>
        public static readonly DependencyProperty PlaceHolderLocaleProperty =
            DependencyProperty.Register(
                "PlaceHolderLocale",
                typeof(locale),
                typeof(NumericTextBox),
                new PropertyMetadata(locale.FR));

        /// <summary>
        /// Get or set the PlaceHolder value
        /// </summary>
        [Description("Gets or sets the placeholder value")]
        [Category("PlaceHolder")]
        public string PlaceHolder
        {
            get { return (string)GetValue(PlaceHolderProperty); }
            set { SetValue(PlaceHolderProperty, value); }
        }

        /// <summary>
        /// Get or set the PlaceHolder foreground 
        /// </summary>
        [Description("Gets or sets the placeholder foreground")]
        [Category("PlaceHolder")]
        public Color ForegroundColor
        {
            get { return (Color)GetValue(PlaceHolderForegroundColorProperty); }
            set { SetValue(PlaceHolderForegroundColorProperty, value); }
        }

        /// <summary>
        /// Get or set the type of input
        /// </summary>
        [Description("Indicate if the typed value is Integer or Decimal")]
        [Category("PlaceHolder")]
        public bool IsInteger
        {
            get { return (bool)GetValue(PlaceHolderIsIntegerProperty); }
            set { SetValue(PlaceHolderIsIntegerProperty, value); }
        }

        /// <summary>
        /// Get or set the local settings for the input
        /// </summary>
        [Description("Indicate the local settings to be applied")]
        [Category("PlaceHolder")]
        public locale Locale
        {
            get { return (locale)GetValue(PlaceHolderLocaleProperty); }
            set { SetValue(PlaceHolderLocaleProperty, value); }
        }


        public  enum locale
        {
            FR,
            US
        }
    }
}
