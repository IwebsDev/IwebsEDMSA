using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Windows.Markup;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Browser;

namespace Galatee.Silverlight.Library
{
    [ContentProperty("MenuItems")]
    public class MenuItem : ObservableCollection<MenuItem>
    {
        #region attributes
        bool isEnabled = true;
        bool isCheckable = false;
        bool isChecked = false;
        int level = 0;
        #endregion attributes

        #region Events
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            string s = "";
        }
        #endregion Events

        #region Properties

        public IList<MenuItem> MenuItems
        {
            get { return Items; }
            set
            {
                foreach (MenuItem item in value)
                {
                    Items.Add(item);
                }
            }
        }

        public bool IsControl { get; set; }

        public HtmlElement Iframe = null;

        public HtmlElement CloseButton = null;

        public string Name { get; set; }

        public string FormName { get; set; }

        public string Module { get; set; }

        public string Title { get; set; }

        public int? Tdem { get; set; }

        public int MenuID { get; set; }

        public string ParentName { get; set; }

        public string Text { get; set; }

        public string ImagePath { get; set; }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        public bool IsCheckable
        {
            get { return isCheckable; }
            set { isCheckable = value; }
        }

        public bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }

        public Grid MenuGrid { get; set; }

        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        public byte? TypeReedition { get; set; }

        #endregion Properties

        #region Methods
        public override string ToString()
        {
            return this.Name;
        }
        #endregion Methods
    } 

}
