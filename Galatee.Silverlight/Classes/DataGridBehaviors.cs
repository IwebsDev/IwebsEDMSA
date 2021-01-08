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
using System.Windows.Interactivity;
using System.Windows.Controls.Primitives;
using System.Collections.Generic;

namespace Galatee.Silverlight.Classes
{
    public class DataGridBehaviors : Behavior<DataGrid>
    {
        internal Popup Popup { get; set; }
        private List<ObjectLists> liste;

        public DataGridBehaviors() { }

        public List<ObjectLists> Liste
        {
            get { return liste; }
            set { liste = value; }
        }

        #region inherited function
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.MouseRightButtonDown += new MouseButtonEventHandler(AssociatedObject_MouseRightButtonDown);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.MouseRightButtonDown -= new MouseButtonEventHandler(AssociatedObject_MouseRightButtonDown);
        }

        #endregion

        #region Customizing behavior

        void AssociatedObject_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            /* A modifier */
            DataGrid d = this.AssociatedObject as DataGrid ;
            ListBox menu = new ListBox();

            if (d.SelectedItem == null)
            {

                liste = new List<ObjectLists>()
            {
                new ObjectLists(){ Code="creation",Label="Create"},
                new ObjectLists(){ Code="search",Label="Search"},
                new ObjectLists(){ Code="consult",Label="Consult"},
                new ObjectLists(){ Code="modify",Label="Modify"}
            };
            }
            else
            {
                liste = new List<ObjectLists>()
            {
                //new ObjectLists(){ code="creation",label="Create"},
                //new ObjectLists(){ code="search",label="Search"},
                new ObjectLists(){ Code="consult",Label="Consult"},
                new ObjectLists(){ Code="modify",Label="Modify"}
            };
            }
            menu.SelectedValuePath = "Code";
            menu.DisplayMemberPath = "Label";

            menu.ItemsSource = liste;
          
            menu.SelectionChanged += (s, args) =>
                {
                    MessageBox.Show((menu.SelectedItem as ObjectLists).Label);
                    Popup.IsOpen = false;
                  
                };
            PerformPlacement(menu, e.GetPosition(null));

            e.Handled = true;
        }

        #endregion

        #region Popup

        private void PerformPlacement(FrameworkElement content, Point point)
        {
            PerformPlacement(content, point.X, point.Y);
        }

        private void PerformPlacement(FrameworkElement content, double x, double y)
        {
            Canvas elementOutside = new Canvas();
            Canvas childCanvas = new Canvas();
            
            elementOutside.Background = new SolidColorBrush(Colors.Transparent);

            if (Popup != null)
            {
                Popup.IsOpen = false;
                if (Popup.Child is Canvas) ((Canvas)Popup.Child).Children.Clear();
            }
            Popup = new Popup();

            Popup.Child = childCanvas;

            elementOutside.MouseLeftButtonDown += new MouseButtonEventHandler((o, e) => Popup.IsOpen = false);
            elementOutside.Width = Application.Current.Host.Content.ActualWidth;
            elementOutside.Height = Application.Current.Host.Content.ActualHeight;

            childCanvas.Children.Add(elementOutside);
            childCanvas.Children.Add(content);

            Canvas.SetLeft(content, x);
            Canvas.SetTop(content, y);

            Popup.IsOpen = true;
        }

        #endregion

    }

    public class ObjectLists
    {
        string code;

        public string Code
        {
            get { return code; }
            set { code = value; }
        }
        string label;

        public string Label
        {
            get { return label; }
            set { label = value; }
        }
    }
}
