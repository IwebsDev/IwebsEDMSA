using System;
using System.Collections.ObjectModel;
using System.Linq;
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
using Galatee.Silverlight.ServiceAdministration;
//using Galatee.Silverlight.ServiceAdministration;

namespace Galatee.Silverlight.Classes
{
    public class SlectionChangedManager
    {
        #region Private members

        private event SelectionChangedEventHandler _selectionChanged;

        #endregion

        #region Constructor

        /// <summary>

        /// Initializes a new instance of the <see cref="MouseClickManager"/> class.

        /// </summary>

        /// <param name="control">The control.</param>

        public SlectionChangedManager()
        {

            //this.Clicked = false;

            //this.DoubleClickTimeout = doubleClickTimeout;

        }

        #endregion

        #region Events

        public event SelectionChangedEventHandler SelectionClick
        {

            add { _selectionChanged += value; }

            remove { _selectionChanged -= value; }

        }

        /// <summary>

        /// Called when [click].

        /// </summary>

        /// <param name="sender">The sender.</param>

        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>

        /// <summary>

        /// Called when [double click].

        /// </summary>

        /// <param name="sender">The sender.</param>

        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>

        private void OnSelectionClick(object sender, SelectionChangedEventArgs e)
        {

            if (_selectionChanged != null)
            {

                _selectionChanged(sender, e);

            }

        }

        /// <summary>

        /// Handles the click.

        /// </summary>

        /// <param name="sender">The sender.</param>

        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>

        public void HandleClick(object sender, SelectionChangedEventArgs e)
        {

            lock (this)
            {

                OnSelectionClick(sender, e);

            }

        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Private

        /// <summary>

        /// Resets the thread.

        /// </summary>

        /// <param name="state">The state.</param>

        #endregion

        #endregion
    }
    public class DataGridContexMenuBehavior : Behavior<DataGrid>
    {

        internal Popup Popup { get; set; }
        private List<ContextMenuItem> liste;
        private CsUtilisateur obj;
        private ContextMenu contextMenu = new ContextMenu();
       
        public DataGridContexMenuBehavior() {

            //sendCreation = SessionObject.UCCreationExecMode;
        }

        void _ListBoxSelectionManager_SelectionClick(object sender, SelectionChangedEventArgs e)
        {
        }

        void menu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show("yes selected");
        }
        // liste de titre du controle utilisateur
        public List<ContextMenuItem> Liste
        {
            get { return liste; }
            set { liste = value; }
        }

        public CsUtilisateur ojbectsSelected 
        {
            get { return obj; }
            set { obj = value; }
        }

        //// liste du sens de creation des formulaire
        //List<Title> sendCreation = null;

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
            DataGrid d = this.AssociatedObject ; 
            try
            {
                if (d != null)
                {
                    //Liste d'element constitutifs du menu contextuel
                    var contextMenuItem = SessionObject.MenuContextuelItem;
                    contextMenu.Items.Clear();
                    if (contextMenu != null) contextMenu.Visibility = Visibility.Visible;
                    var rowCount = d.ItemsSource != null ? d.ItemsSource.OfType<object>().Count() : 0;
                    foreach (var item in contextMenuItem)
                    {
                        var menuItem = new MenuItem {Header = item.Label, Tag = item};
                        if (d.SelectedItem == null || rowCount == 0)
                        {
                            if (((ContextMenuItem) menuItem.Tag).ModeExcecution != SessionObject.ExecMode.Creation)
                                menuItem.IsEnabled = false;
                        }
                        menuItem.Click += new RoutedEventHandler(menuItem_Click);
                        contextMenu.Items.Add(menuItem);
                    }
                    PerformPlacement(contextMenu, e.GetPosition(null));
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
               Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void menuItem_Click(Object sender, RoutedEventArgs e)
        {
            try
            {
                var contextMenuItem = ((MenuItem)sender).Tag as ContextMenuItem;
                SessionObject.MenuItemClicked = (MenuItem)sender;
                if (contextMenuItem != null && !string.IsNullOrEmpty(contextMenuItem.Code))
                    CreateUserView(contextMenuItem.Code, contextMenuItem.Title, contextMenuItem.ModeExcecution);
                if (contextMenu != null) contextMenu.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Popup

        private void PerformPlacement(FrameworkElement content, Point point)
        {
            try
            {
                PerformPlacement(content, point.X, point.Y);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PerformPlacement(FrameworkElement content, double x, double y)
        {
            try
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
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        #endregion

        public void CreateUserView(string namespaces, string title, SessionObject.ExecMode pExecMode)
        {
            try
            {
                Type cwType = Type.GetType(namespaces);
                object cw = new object();
                object UserConnected = SessionObject.objectSelected;
                DataGrid gridUser = SessionObject.gridUtilisateur;

                cw = Activator.CreateInstance(cwType, new object[] { UserConnected }, new SessionObject.ExecMode[] { pExecMode }, new DataGrid[] { gridUser });

                ChildWindow form = (ChildWindow)cw;
                form.Title = title;
                form.Show();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
        //void CreateUserView(string namespaces, string title, SessionObject.ExecMode pExecMode)
        //{
        //    try
        //    {
        //        Type cwType = Type.GetType(namespaces);
        //        object cw = new object();
        //        object UserConnected = SessionObject.objectSelected;
        //        DataGrid gridUser = SessionObject.gridUtilisateur;
        //        cw = Activator.CreateInstance(cwType, new object[] { UserConnected }, new SessionObject.ExecMode[] { pExecMode }, new DataGrid[] { gridUser });
        //        ChildWindow form = (ChildWindow)cw;
        //        form.Title = title;
        //        form.Show();
        //    }
        //    catch (Exception ex)
        //    {
        //        Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
        //    }
        //}
    }

    public class ContextMenuItem 
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

        public SessionObject.ExecMode ModeExcecution { get; set; }

        public string Title { get; set; }

    }

    }
