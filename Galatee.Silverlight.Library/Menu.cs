using System;
using System.Net;
using System.Windows;
//using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace Galatee.Silverlight.Library
{
    public class Menu : System.Windows.Controls.Grid
    {
        #region attributes
        public readonly DependencyProperty MenuItemProperty;
        public readonly DependencyProperty ImagesPathProperty;
        public readonly DependencyProperty CommandProperty;
        public readonly DependencyProperty BorderBrushProperty;
        public readonly DependencyProperty TopPanelBrushProperty;
        public readonly DependencyProperty ImageBackgroundBrushProperty;
        public readonly DependencyProperty FocusBrushProperty;
        public readonly DependencyProperty FocusBorderBrushProperty;
        public readonly DependencyProperty ForegroundProperty;
        public readonly DependencyProperty BackgroundProperty;
        System.Windows.Controls.StackPanel stackPanel = new System.Windows.Controls.StackPanel();
        static Dictionary<string, MenuItem> menuDictionary = new Dictionary<string, MenuItem>();

        //Brush borderBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x9E, 0xA9, 0xBD));
        Brush borderBrush = new SolidColorBrush(Colors.DarkGray);
        Brush topPanelBrush;
        Brush imageBackgroundBrush = new SolidColorBrush(Colors.Green);
        //Brush imageBackgroundBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xD8, 0xE1, 0xF0));
        Brush focusBrush = new SolidColorBrush(Colors.DarkGray);
        //Brush focusBrush = new SolidColorBrush(Colors.White);
        Brush focusBorderBrush = new SolidColorBrush(Colors.DarkGray);
        //Brush focusBorderBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xF5, 0xC3, 0x65));
        Brush foreground = new SolidColorBrush(Colors.White);
        Brush background = new SolidColorBrush(Colors.DarkGray);


        //static double verticalMenuWidth = double.NaN;
        static double verticalMenuWidth = 250;
        static int selectedMenuLevel = 0;
        string imagesPath = "Images/";
        ICommand command;
        public event EventHandler MenuItemClicked;
        bool isOverMenu = false;
        #endregion attributes

        #region constructor

        /// <summary>
        /// Permet de réinitialiser la barre de menu 
        /// Date : 22/11/2012
        /// Auteur : Hermann GBOGUE
        /// Statut Methode : Approuvée
        /// </summary>
        public void RefleshMenu()
        {
            try
            {
                stackPanel.Children.Clear();
                menuDictionary.Clear();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            

        }
        public Menu()
        {
            this.SetValue(System.Windows.Controls.Canvas.ZIndexProperty, 1000);
            MenuItemProperty =
            DependencyProperty.Register("MenuItem", typeof(MenuItem), typeof(Menu), 
            new PropertyMetadata(null,
            new PropertyChangedCallback(OnMenuItemChanged)));

            ImagesPathProperty =
            DependencyProperty.Register("ImagesPath", typeof(string), typeof(Menu),
            new PropertyMetadata(null,
            new PropertyChangedCallback(OnImagesPathChanged)));

            CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(Menu),
            new PropertyMetadata(null,
            new PropertyChangedCallback(OnCommandChanged)));

            BorderBrushProperty =
            DependencyProperty.Register("BorderBrush", typeof(Brush), typeof(Menu),
            new PropertyMetadata(null,
            new PropertyChangedCallback(OnBorderBrushChanged)));

            TopPanelBrushProperty =
            DependencyProperty.Register("TopPanelBrush", typeof(Brush), typeof(Menu),
            new PropertyMetadata(null,
            new PropertyChangedCallback(OnTopPanelBrushChanged)));

            ImageBackgroundBrushProperty =
            DependencyProperty.Register("ImageBackgroundBrush", typeof(Brush), typeof(Menu),
            new PropertyMetadata(null,
            new PropertyChangedCallback(OnImageBackgroundBrushChanged)));

            FocusBrushProperty =
            DependencyProperty.Register("FocusBrush", typeof(Brush), typeof(Menu),
            new PropertyMetadata(null,
            new PropertyChangedCallback(OnFocusBrushChanged)));

            FocusBorderBrushProperty =
            DependencyProperty.Register("FocusBorderBrush", typeof(Brush), typeof(Menu),
            new PropertyMetadata(null,
            new PropertyChangedCallback(OnFocusBorderBrushChanged)));

            ForegroundProperty =
            DependencyProperty.Register("Foreground", typeof(Brush), typeof(Menu),
            new PropertyMetadata(null,
            new PropertyChangedCallback(OnForegroundChanged)));

            BackgroundProperty =
            DependencyProperty.Register("Background", typeof(Brush), typeof(Menu),
            new PropertyMetadata(null,
            new PropertyChangedCallback(OnBackgroundChanged)));

            var lgb = new LinearGradientBrush() { StartPoint = new Point(0, 0), EndPoint = new Point(0, 1) };

            // lgb.GradientStops.Add(new GradientStop() { Offset = 0.0, Color = Colors.White });
            //lgb.GradientStops.Add(new GradientStop() { Offset = 1.0, Color = Colors.White});

            lgb.GradientStops.Add(new GradientStop() { Offset = 0.0, Color = Colors.DarkGray }); // initial value
            lgb.GradientStops.Add(new GradientStop() { Offset = 1.0, Color = Colors.DarkGray });

            topPanelBrush = lgb;

            var lgb2 = new LinearGradientBrush() { StartPoint = new Point(0, 0), EndPoint = new Point(0, 1) };

            lgb2.GradientStops.Add(new GradientStop() { Offset = 0.0, Color = Color.FromArgb(250, 54, 120, 155) });
            // lgb2.GradientStops.Add(new GradientStop() { Offset = 0.0, Color = Colors.Blue });
            //lgb2.GradientStops.Add(new GradientStop() { Offset = 1.0, Color = Colors.LightGray });
            //lgb2.GradientStops.Add(new GradientStop() { Offset = 1.0, Color = Color.FromArgb(0xFF, 0xFF, 0xDC, 0xA5) });

            focusBrush = lgb2;
            stackPanel.Orientation = System.Windows.Controls.Orientation.Horizontal;
            stackPanel.Background = topPanelBrush;
            stackPanel.Height = 35;
            stackPanel.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            stackPanel.SetValue(System.Windows.Controls.Canvas.ZIndexProperty, -5000);
            this.Children.Add(stackPanel);

            this.MouseEnter += (s1, e1) =>
            {
                isOverMenu = true;
            };

            this.MouseLeave += (s2, e2) =>
            {
                isOverMenu = false;
            };

            this.Loaded += (s3, e3) =>
                {
                    var parent = (FrameworkElement)this.Parent;
                    
                    parent.MouseLeftButtonUp += (s4, e4) =>
                        {
                            if (!isOverMenu)
                            {
                                foreach (var md in menuDictionary)
                                {
                                    if (md.Value.MenuGrid != null)
                                        md.Value.MenuGrid.Visibility = Visibility.Collapsed;
                                }
                                selectedMenuLevel = 0;
                            }
                        };
                };
        }

        #endregion constructor

        #region Methods
        
        public void ClearReferences(MenuItem menuItem)
        {
            foreach (var childMenuItem in menuItem.MenuItems)
            {
                ClearReferences(childMenuItem);
            }
            if (menuItem.MenuGrid != null)
            {
                menuItem.MenuGrid.Tag = null;
                menuItem.MenuGrid = null;
                menuItem.MenuGrid.Children.Clear();
            }
        }

        //public void RefreshMenuBar()
        //{
        //    if (menuItem.MenuGrid != null)
        //    {
        //        menuItem.MenuGrid.Tag = null;
        //        menuItem.MenuGrid = null;
        //    }
        //}

        public void AddMenuItem(MenuItem parent, MenuItem child)
        {
            // if menuDictionnary contains child ,we don't need to add yet


            if (!menuDictionary["mnuRecentFiles"].MenuItems.Contains(child))
            {
                menuDictionary["mnuRecentFiles"].MenuItems.Add(child);
                stackPanel.Children.Clear();
                ClearReferences(this.MenuItem);
                menuDictionary.Clear();
                DrawMenu(this, this.MenuItem);
            }
            
            //if(stackPanel.Children !=null)
            //{
            //    stackPanel.Children.Clear();
            //    menuDictionary.Clear();
            //    ClearReferences(this.MenuItem);
            //}

        }
        private void DrawMenu(Menu menu, MenuItem menuItem)
        {
            if (menuItem == null)
                return;

            double currentLeftMargin = 0;
            foreach (var mi2 in menuItem.MenuItems)
            {
                mi2.ParentName = menuItem.Name;
                mi2.Level = menuItem.Level + 1;

                double currentTopMargin = 0;
                var textBlockLevel1 = new System.Windows.Controls.TextBlock()
                {
                    Text = mi2.Text,
                    Margin = new Thickness(8, 4, 8, 4),
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = foreground
                    
                };

                //var fill = new LinearGradientBrush() { StartPoint = new Point(0, 0), EndPoint = new Point(0, 1) };
                //fill.GradientStops.Add(new GradientStop() { Offset = 0.0, Color = Color.FromArgb(0xFF, 0xFF, 0xFB, 0xF1) });
                //fill.GradientStops.Add(new GradientStop() { Offset = 1.0, Color = Color.FromArgb(0xFF, 0xFF, 0xDC, 0xA5) });

                var focusRectangle1 = new Rectangle()
                {
                    Fill = focusBrush,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                    VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                    Visibility = Visibility.Collapsed,
                    Margin = new Thickness(2, 2, 2, 2),
                    RadiusX = 2,
                    RadiusY = 2
                };

                var focusRectangle2 = new Rectangle()
                {
                    Stroke = focusBorderBrush,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                    VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                    Visibility = Visibility.Collapsed,
                    Margin = new Thickness(2, 2, 2, 2),
                    RadiusX = 2,
                    RadiusY = 2
                };

                var gridLevel1 = new System.Windows.Controls.Grid();

                gridLevel1.Children.Add(focusRectangle1);
                gridLevel1.Children.Add(textBlockLevel1);
                gridLevel1.Children.Add(focusRectangle2);
                gridLevel1.Tag = mi2.Name;

                gridLevel1.MouseEnter += new MouseEventHandler(grid_MouseEnter);
                gridLevel1.MouseLeave += new MouseEventHandler(grid_MouseLeave);
                gridLevel1.MouseLeftButtonUp += new MouseButtonEventHandler(gridLevel1_MouseLeftButtonUp);

                menu.stackPanel.Children.Add(gridLevel1);

                //LinearGradientBrush bkg = new LinearGradientBrush() { StartPoint = new Point(0, 0), EndPoint = new Point(0, 1) };
                //bkg.GradientStops.Add(new GradientStop() { Offset = 0.0, Color = Color.FromArgb(0xFF, 0xD8, 0xE1, 0xF0) });

                var txt = new System.Windows.Controls.TextBlock()
                {
                    Text = mi2.Text,
                    Margin = new Thickness(7, 1, 8, 4),
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = foreground
                };

                var brd = new System.Windows.Controls.Border()
                {
                    CornerRadius = new CornerRadius(2),
                    BorderBrush = borderBrush,
                    BorderThickness = new Thickness(1),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Margin = new Thickness(0, 2, 0, 0)
                };


                System.Windows.Controls.Grid grdVertical = null;

                //grdVertical = CreateVerticalGrid(menu, currentLeftMargin, currentTopMargin, mi2, focusBrush, txt, grdVertical);
                grdVertical = CreateVerticalGrid(menu, currentLeftMargin, currentTopMargin, mi2, txt, grdVertical);

                currentLeftMargin += textBlockLevel1.ActualWidth + 16;
            }
        }

        private System.Windows.Controls.Grid CreateVerticalGrid(
        System.Windows.Controls.Grid parentGrid,
        double currentLeftMargin,
        double currentTopMargin,
        MenuItem parentMenuItem,
        System.Windows.Controls.TextBlock txt,
        System.Windows.Controls.Grid grdVertical)
        {
            var verticalGrid = new System.Windows.Controls.Grid()
            {
                Width = verticalMenuWidth,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };

            grdVertical = new System.Windows.Controls.Grid()
            {
                Margin = new Thickness(currentLeftMargin, currentTopMargin, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            grdVertical.RowDefinitions.Add(new System.Windows.Controls.RowDefinition() { Height = new GridLength(35) }); // largeur menuitem
            grdVertical.RowDefinitions.Add(new System.Windows.Controls.RowDefinition() { Height = new GridLength(35) });

            parentGrid.Children.Add(grdVertical);

            parentMenuItem.MenuGrid = grdVertical;

            var brdAroundVerticalParent = new System.Windows.Controls.Border()
            {
                Margin = new Thickness(1, 1, 1, 1),
                CornerRadius = new CornerRadius(0),
                BorderBrush = borderBrush,
                BorderThickness = new Thickness(1),
                Background = background,
                Height = 35,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            brdAroundVerticalParent.RenderTransform = new TranslateTransform() { Y = 0 };
            brdAroundVerticalParent.SetValue(System.Windows.Controls.Grid.RowProperty, 0);
            brdAroundVerticalParent.SetValue(System.Windows.Controls.Canvas.ZIndexProperty, parentMenuItem.Level * 100);

            if (parentMenuItem.Level == 1)
                brdAroundVerticalParent.Child = txt;

            if (parentMenuItem.Level > 1)
                brdAroundVerticalParent.Visibility = Visibility.Collapsed;

            brdAroundVerticalParent.MouseLeftButtonUp += new MouseButtonEventHandler(brdAroundVerticalParent_MouseLeftButtonUp);

            grdVertical.Children.Add(brdAroundVerticalParent);

            var brdAroundVerticalChildren = new System.Windows.Controls.Border()
            {
                CornerRadius = new CornerRadius(2),
                BorderBrush = borderBrush,
                BorderThickness = new Thickness(1),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            brdAroundVerticalChildren.Child = verticalGrid;

            brdAroundVerticalChildren.RenderTransform = new TranslateTransform() { Y = 0 };
            brdAroundVerticalChildren.SetValue(System.Windows.Controls.Grid.RowProperty, 1);

            grdVertical.Children.Add(brdAroundVerticalChildren);
            Rectangle rectHideLine = new Rectangle()
            {
                Margin = new Thickness(0, 0, 0, 0),
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Stroke = background,
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                Height = 1
            };
            rectHideLine.SetValue(System.Windows.Controls.Grid.RowProperty, 1);
            Rectangle rectLineLeft = new Rectangle()
            {
                Margin = new Thickness(0, 0, 0, 0),
                Fill = borderBrush,
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                Width = 1,
                Height = 35
            };

            rectLineLeft.SetValue(System.Windows.Controls.Grid.RowProperty, 0);
            rectLineLeft.SetValue(System.Windows.Controls.Grid.RowSpanProperty, 2);

            if (parentMenuItem.Level < 2)
            {
                grdVertical.Children.Add(rectHideLine);
                grdVertical.Children.Add(rectLineLeft);
            }

            int gridRow = 0;
            foreach (var mi3 in parentMenuItem.MenuItems)
            {
                mi3.ParentName = parentMenuItem.Name;
                mi3.Level = parentMenuItem.Level + 1;

                var focusRectangle3 = new Rectangle()
                {
                    Fill = focusBrush,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                    VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                    Visibility = Visibility.Collapsed,
                    Margin = new Thickness(1, 0, 10, 0),
                    RadiusX = 2,
                    RadiusY = 2
                };

                var focusRectangle4 = new Rectangle()
                {
                    Stroke = focusBorderBrush,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                    VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                    Visibility = Visibility.Collapsed,
                    Margin = new Thickness(1, 0, 10, 0),
                    RadiusX = 2,
                    RadiusY = 2
                };

                var separatorRectangle = new Rectangle()
                {
                    Height = 1,
                    Fill = new SolidColorBrush(Colors.Black),
                    //Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xA0, 0xA0, 0xA0)),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(2, 0, 2, 0)
                };
                separatorRectangle.SetValue(System.Windows.Controls.Grid.ColumnProperty, 1);
                separatorRectangle.SetValue(System.Windows.Controls.Grid.ColumnSpanProperty, 2);

                System.Windows.Controls.Border brdImage = new System.Windows.Controls.Border()
                {
                    Width = 24,
                    Height = 35,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Background = new SolidColorBrush(Colors.DarkGray)//imageBackgroundBrush
                };
                System.Windows.Controls.Image imgLevel2 = new System.Windows.Controls.Image()
                {
                    Width = 16,
                    Height = 16,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(4, 0, 0, 0),
                    Opacity = mi3.IsEnabled ? 1.0 : 0.5
                };
                imgLevel2.SetValue(System.Windows.Controls.Grid.ColumnProperty, 0);

                System.Windows.Controls.Image imgArrow = new System.Windows.Controls.Image()
                {
                    Width = 16,
                    Height = 16,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(0, 0, 0, 0),
                    Opacity = mi3.IsEnabled ? 1.0 : 0.5
                };
                imgArrow.SetValue(System.Windows.Controls.Grid.ColumnProperty, 2);
                imgArrow.Source = new BitmapImage(new Uri(string.Format(@"{0}Arrow.png", imagesPath), UriKind.Relative));
                imgArrow.Visibility = mi3.MenuItems.Count > 0 ? Visibility.Visible : Visibility.Collapsed;

                if (mi3.IsCheckable)
                {
                    if (mi3.IsChecked)
                        mi3.ImagePath = string.Format("{0}Checked.png", imagesPath);
                    else
                        mi3.ImagePath = null;
                }
                else
                {
                    if (mi3.ImagePath == null)
                        mi3.ImagePath = string.Format("{0}{1}.png", imagesPath, mi3.Name);
                }

                if (mi3.ImagePath != null)
                {
                    imgLevel2.Source = new BitmapImage(new Uri(mi3.ImagePath, UriKind.Relative));
                }

                var textBlockLevel2 = new System.Windows.Controls.TextBlock()
                {
                    Text = mi3.Text,
                    Margin = new Thickness(8, 4, 8, 4),
                    VerticalAlignment = VerticalAlignment.Center,
                    // Foreground = new SolidColorBrush(Colors.Purple),//foreground,
                    Opacity = mi3.IsEnabled ? 1.0 : 0.5
                };

                var gridLevel2 = new System.Windows.Controls.Grid()
                {
                    Background = background
                };

                gridLevel2.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition() { Width = new GridLength(24) });
                gridLevel2.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition() { Width = new GridLength(verticalMenuWidth - 40) });
                gridLevel2.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition() { Width = new GridLength(24), MinWidth = 24 });

                focusRectangle3.SetValue(System.Windows.Controls.Grid.ColumnProperty, 0);
                focusRectangle3.SetValue(System.Windows.Controls.Grid.ColumnSpanProperty, 3);

                textBlockLevel2.SetValue(System.Windows.Controls.Grid.ColumnProperty, 1);
                focusRectangle4.SetValue(System.Windows.Controls.Grid.ColumnProperty, 0);
                focusRectangle4.SetValue(System.Windows.Controls.Grid.ColumnSpanProperty, 3);

                verticalGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition() { Height = new GridLength(22) });

                if (mi3.Text.Equals("-"))
                {
                    gridLevel2.Children.Add(brdImage);
                    gridLevel2.Children.Add(separatorRectangle);
                    verticalGrid.RowDefinitions[verticalGrid.RowDefinitions.Count - 1].Height = new GridLength(2);
                    separatorRectangle.SetValue(System.Windows.Controls.Grid.ColumnProperty, 1);
                    separatorRectangle.SetValue(System.Windows.Controls.Grid.ColumnSpanProperty, 2);
                }
                else
                {
                    gridLevel2.Children.Add(brdImage);
                    gridLevel2.Children.Add(focusRectangle3);
                    gridLevel2.Children.Add(imgLevel2);
                    gridLevel2.Children.Add(textBlockLevel2);
                    gridLevel2.Children.Add(imgArrow);
                    gridLevel2.Children.Add(focusRectangle4);

                    gridLevel2.MouseEnter += new MouseEventHandler(gridLevel2_MouseEnter);
                    gridLevel2.MouseLeave += new MouseEventHandler(gridLevel2_MouseLeave);
                    gridLevel2.MouseLeftButtonUp += new MouseButtonEventHandler(gridLevel2_MouseLeftButtonUp);
                }
                gridLevel2.Tag = mi3;
                gridLevel2.SetValue(System.Windows.Controls.Grid.RowProperty, gridRow);
                verticalGrid.Children.Add(gridLevel2);

                grdVertical.RowDefinitions[1].Height = new GridLength(grdVertical.RowDefinitions[1].Height.Value + 22);
                gridRow++;

                if (mi3.MenuItems.Count > 0)
                {
                    CreateVerticalGrid(parentGrid, currentLeftMargin + verticalMenuWidth, currentTopMargin, mi3, txt, grdVertical);
                }

                if (mi3.Text.Equals("-"))
                {
                    currentTopMargin += 2;
                }
                else
                {
                    currentTopMargin += 22;
                }

                if (!menuDictionary.ContainsKey(mi3.Name))
                    menuDictionary.Add(mi3.Name, mi3);
            }
            grdVertical.Visibility = Visibility.Collapsed;
            grdVertical.MouseLeave += new MouseEventHandler(grdVertical_MouseLeave);

            // modificatioN le 03/10/2012 GBOGUE
           // if (!menuDictionary.ContainsKey(parentMenuItem.Name))
            menuDictionary.Add(parentMenuItem.Name, parentMenuItem);

            grdVertical.SetValue(System.Windows.Controls.Canvas.ZIndexProperty, parentMenuItem.Level * 100);

            return grdVertical;
        }

        private void HideSiblings(MenuItem menuItem)
        {
            if (menuItem.ParentName != null)
            {
                MenuItem parentMenuItem = menuDictionary[menuItem.ParentName];
                foreach (var siblingMenu in parentMenuItem.MenuItems)
                {
                    if (siblingMenu.Name != menuItem.Name)
                    {
                        if (menuDictionary.ContainsKey(siblingMenu.Name))
                        {
                            if (menuDictionary[siblingMenu.Name].MenuGrid != null)
                                menuDictionary[siblingMenu.Name].MenuGrid.Visibility = Visibility.Collapsed;
                        }

                        foreach (var child in siblingMenu.MenuItems)
                        {
                            HideSiblings(child);
                        }
                    }
                }
            }
        }
        #endregion Methods

        #region Properties

        public MenuItem MenuItem
        {
            get { return (MenuItem)GetValue(MenuItemProperty); }
            set { SetValue(MenuItemProperty, value); }
        }

        public string ImagesPath
        {
            get { return (string)GetValue(ImagesPathProperty); }
            set { SetValue(ImagesPathProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public Brush BorderBrush
        {
            get { return (Brush)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }

        public Brush TopPanelBrush
        {
            get { return (Brush)GetValue(TopPanelBrushProperty); }
            set { SetValue(TopPanelBrushProperty, value); }
        }

        public Brush ImageBackgroundBrush
        {
            get { return (Brush)GetValue(ImageBackgroundBrushProperty); }
            set { SetValue(ImageBackgroundBrushProperty, value); }
        }

        public Brush FocusBrush
        {
            get { return (Brush)GetValue(FocusBrushProperty); }
            set { SetValue(FocusBrushProperty, value); }
        }

        public Brush FocusBorderBrush
        {
            get { return (Brush)GetValue(FocusBorderBrushProperty); }
            set { SetValue(FocusBorderBrushProperty, value); }
        }

        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        #endregion Properties

        #region Events

        public void OnMenuItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var menu = (Menu)d;
            var menuItem = (MenuItem)e.NewValue;

            DrawMenu(menu, menuItem);
        }

        public void OnImagesPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            imagesPath = e.NewValue.ToString();
        }

        public void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            command = (ICommand)e.NewValue;
        }

        public void OnBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            borderBrush = (Brush)e.NewValue;
        }

        public void OnTopPanelBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            topPanelBrush = (Brush)e.NewValue;
            stackPanel.Background = topPanelBrush;
        }

        public void OnImageBackgroundBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            imageBackgroundBrush = (Brush)e.NewValue;
        }

        public void OnFocusBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            focusBrush = (Brush)e.NewValue;
        }

        public void OnFocusBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            focusBorderBrush = (Brush)e.NewValue;
        }

        public void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            foreground = (Brush)e.NewValue;
        }

        public void OnBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            background = (Brush)e.NewValue;
        }

        void brdAroundVerticalParent_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var grid = (System.Windows.Controls.Grid)((System.Windows.Controls.Border)sender).Parent;
            grid.Visibility = Visibility.Collapsed;
            selectedMenuLevel = 0;
        }

        void gridLevel2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            selectedMenuLevel = 0;
            var grid = (System.Windows.Controls.Grid)sender;
            var menuItem = (MenuItem)grid.Tag;
            if (menuItem.IsEnabled)
            {
                foreach (var item in menuDictionary)
                {
                    if (item.Value.MenuGrid != null)
                        item.Value.MenuGrid.Visibility = Visibility.Collapsed;
                }
            }

            if (menuItem.IsCheckable)
            {
                menuItem.IsChecked = !menuItem.IsChecked;

                System.Windows.Controls.Image img = (System.Windows.Controls.Image)(grid.Children[2]);
                if (menuItem.IsChecked)
                    img.Source = new BitmapImage(new Uri(string.Format(@"{0}Checked.png", imagesPath), UriKind.Relative));
                else
                    img.Source = null;
            }

            if (MenuItemClicked != null)
                MenuItemClicked(menuItem, new EventArgs());

            if (command != null)
                command.Execute(menuItem);
        }

        void gridLevel2_MouseLeave(object sender, MouseEventArgs e)
        {
            var grid = (System.Windows.Controls.Grid)sender;
            grid.Children[1].Visibility = Visibility.Collapsed;
            grid.Children[5].Visibility = Visibility.Collapsed;
        }

        void gridLevel2_MouseEnter(object sender, MouseEventArgs e)
        {
            var grid = (System.Windows.Controls.Grid)sender;
            var menuItem = (MenuItem)grid.Tag;

            if (menuItem.IsEnabled)
            {
                grid.Children[1].Visibility = Visibility.Visible;
                grid.Children[5].Visibility = Visibility.Visible;
            }

            if (menuItem.MenuGrid != null)
            {
                menuItem.MenuGrid.Visibility = Visibility.Visible;
            }

            HideSiblings(menuItem);
        }
        /// <summary>
        /// Methode invoquée quant on effectue un click gauche sur les menus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gridLevel1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
            var grid = (System.Windows.Controls.Grid)sender;

            // cacher le Iframe du module 
            //menuDictionary[grid.Tag.ToString()].Iframe.SetAttribute("visibility", "hidden"); ;
            //menuDictionary[grid.Tag.ToString()].Iframe.SetStyleAttribute("width", "0%");
            //menuDictionary[grid.Tag.ToString()].Iframe.SetStyleAttribute("height", "0%");

            //menuDictionary[grid.Tag.ToString()].CloseButton.SetAttribute("visibility", "hidden");
            //menuDictionary[grid.Tag.ToString()].CloseButton.SetStyleAttribute("width", "0%");
            //menuDictionary[grid.Tag.ToString()].CloseButton.SetStyleAttribute("height", "0%");

            if (menuDictionary[grid.Tag.ToString()].MenuGrid.Visibility == Visibility.Collapsed)
            {
                menuDictionary[grid.Tag.ToString()].MenuGrid.Visibility = Visibility.Visible;
                selectedMenuLevel = 1;
            }
            else
            {
                menuDictionary[grid.Tag.ToString()].MenuGrid.Visibility = Visibility.Collapsed;
                selectedMenuLevel = 0;
            }
        }

        void grdVertical_MouseLeave(object sender, MouseEventArgs e)
        {
            var grid = (System.Windows.Controls.Grid)sender;
        }

        void grid_MouseLeave(object sender, MouseEventArgs e)
        {
            var grid = (System.Windows.Controls.Grid)sender;
            grid.Children[0].Visibility = Visibility.Collapsed;
            grid.Children[2].Visibility = Visibility.Collapsed;            
        }

        void grid_MouseEnter(object sender, MouseEventArgs e)
        {
            var grid = (System.Windows.Controls.Grid)sender;
            if (selectedMenuLevel == 0)
            {
                grid.Children[0].Visibility = Visibility.Visible;
                grid.Children[2].Visibility = Visibility.Visible;
            }
            else
            {
                foreach (var item in menuDictionary)
                {
                    if (item.Value.MenuGrid != null)
                        item.Value.MenuGrid.Visibility = Visibility.Collapsed;
                }
                menuDictionary[grid.Tag.ToString()].MenuGrid.Visibility = Visibility.Visible;
            }
        }

        #endregion Events
    }
}
