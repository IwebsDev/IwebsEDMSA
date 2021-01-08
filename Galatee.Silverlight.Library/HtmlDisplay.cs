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
using System.Windows.Browser;

namespace Galatee.Silverlight.Library
{
    public class HtmlDisplay : ContentControl
    {
        private HtmlElement div;
        private HtmlElement iFrame;
        UIElement _element = null;
        public HtmlDisplay()
        {
            //_element = element;
           // CreateFrame(element);
            this.Loaded += new RoutedEventHandler(HtmlDisplay_Loaded);
        }

        void HtmlDisplay_Loaded(object sender, RoutedEventArgs e)
        {
            HtmlDocument htmlDocument = HtmlPage.Document;
            div = htmlDocument.CreateElement("div");
            div.Id = "monDiv";
            div.SetStyleAttribute("position", "absolute");
            div.SetStyleAttribute("height", this.Height.ToString() + "px");
            div.SetStyleAttribute("width", this.Width.ToString() + "px");
            GeneralTransform gt = this.TransformToVisual(Application.Current.RootVisual);
            Point position = gt.Transform(new Point(0, 0));
            div.SetStyleAttribute("left", position.X + "px");
            div.SetStyleAttribute("top", position.Y + "px");


            iFrame = htmlDocument.CreateElement("iframe");
            iFrame.Id = "monIFrame";
            iFrame.SetProperty("frameborder", "no");
            iFrame.SetStyleAttribute("height", this.Height.ToString() + "px");
            iFrame.SetStyleAttribute("width", this.Width.ToString() + "px");
            iFrame.SetStyleAttribute("position", "relative");


            div.AppendChild(iFrame);
            htmlDocument.Body.AppendChild(div);
        }

        void CreateFrame(UIElement ui)
        {
            HtmlDocument htmlDocument = HtmlPage.Document;
            div = htmlDocument.CreateElement("div");
            div.Id = "monDiv";
            div.SetStyleAttribute("position", "absolute");
            div.SetStyleAttribute("height", this.Height.ToString() + "px");
            div.SetStyleAttribute("width", this.Width.ToString() + "px");
            GeneralTransform gt = this.TransformToVisual(ui);
            Point position = gt.Transform(new Point(0, 0));
            div.SetStyleAttribute("left", position.X + "px");
            div.SetStyleAttribute("top", position.Y + "px");


            iFrame = htmlDocument.CreateElement("iframe");
            iFrame.Id = "monIFrame";
            iFrame.SetProperty("frameborder", "no");
            iFrame.SetStyleAttribute("height", this.Height.ToString() + "px");
            iFrame.SetStyleAttribute("width", this.Width.ToString() + "px");
            iFrame.SetStyleAttribute("position", "relative");


            div.AppendChild(iFrame);
            htmlDocument.Body.AppendChild(div);
        }
        public void Naviguer(string url)
        {
            if (iFrame != null)
            {
                iFrame.SetAttribute("src", url);
            }
        }
    } 
}
