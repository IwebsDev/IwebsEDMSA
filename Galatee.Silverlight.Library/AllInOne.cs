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
using System.Xml.Serialization;
using System.Text;
using System.Xml;
using System.Collections.Generic;


namespace Galatee.Silverlight.Library
{
    public static class AllInOne
    {

        /// <summary>
        /// Permet de trouver un controle du visualRoot par sson nom 
        /// </summary>
        /// <typeparam name="T">Type générique correspondant au type du controle à rechercher</typeparam>
        /// <param name="parent">Nom du conteneur dans lequel se trouve le controle à rechercher</param>
        /// <param name="targetType">Type générique correspondant au type du controle à rechercher</param>
        /// <param name="ControlName">Nom du controle qui fait l'objet de la recherche</param>
        /// <returns></returns>
        public static T FindControl<T>(UIElement parent, Type targetType, string ControlName) where T : FrameworkElement
        {

            if (parent == null) return null;

            if (parent.GetType() == targetType && ((T)parent).Name == ControlName)
            {
                return (T)parent;
            }
            T result = null;
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                UIElement child = (UIElement)VisualTreeHelper.GetChild(parent, i);

                if (FindControl<T>(child, targetType, ControlName) != null)
                {
                    result = FindControl<T>(child, targetType, ControlName);
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Retourne tous les controles du visualRoot( xaml) 
        /// </summary>
        /// <author>HGB</author>
        /// <DATE>01/03/2013</DATE>
        /// <returns></returns>
        public static IEnumerable<DependencyObject>ReturnControlsFromXaml(UIElement root) 
        {
                int count = VisualTreeHelper.GetChildrenCount(root);
                for (int i = 0; i < count; i++)
                {
                    UIElement child = (UIElement)VisualTreeHelper.GetChild(root, i);
                    yield return child;
                    foreach (var descendants in ReturnControlsFromXaml(child))
                        yield return descendants;
                }
        }

        /// <summary>
        /// Retourne tous les controles du visualRoot( xaml) sauf ceux du type de type Type en parametre
        /// </summary>
        /// <author>HGB</author>
        /// <DATE>01/03/2013</DATE>
        /// <param name="root">Element racine à partir duquel on scrute les elements descendants</param>
        /// <param name="typeException">Type de control non pris en compte dans le recherche</param>
        /// <returns></returns>
        public static IEnumerable<DependencyObject> ReturnControlsFromXaml(UIElement root,Type typeException)
        {
            int count = VisualTreeHelper.GetChildrenCount(root);
            for (int i = 0; i < count; i++)
            {
                UIElement child = (UIElement)VisualTreeHelper.GetChild(root, i);
                if (child.GetType() != typeException)
                {
                    yield return child;
                    foreach (var descendants in ReturnControlsFromXaml(child,child.GetType()))
                        yield return descendants;
                }
            }
        }

        /// <summary>
        /// Desactiver/Activer tous les controles du visualRoot à partir de l'element 
        /// root passé en parametre ( xaml) 
        /// </summary>
        /// <author>HGB</author>
        /// <DATE>01/03/2013</DATE>
        /// <returns></returns>
        public static void ActivateControlsFromXaml(UIElement layoutRoot, bool state)
        {
           
                foreach (var control in ReturnControlsFromXaml(layoutRoot))
                { 
                  try
                    {
                        Control current = (Control)control;
                        current.IsEnabled = state;
                    }
                  catch (Exception ex)
                  {
                      throw ex;
                  }
              }
           
        }

        /// Desactiver/Activer tous les controles du visualRoot à partir de l'element 
        /// root passé en parametre à l'exception des controles de type en parametre( xaml) 
        /// </summary>
        /// <author>HGB</author>
        /// <DATE>01/03/2013</DATE>
        /// <param name="layoutRoot">Element racine à partir duquel se fait la recherche d'éléments enfants</param>
        /// <param name="state">Etat (actif,ou Inactif) des controles</param>
        /// <param name="typeException">Type de controle non pris en compte dans la recherche</param>
        public static void ActivateControlsFromXaml(UIElement layoutRoot, bool state,Type typeException)
        {
            IEnumerable<DependencyObject> _treeObject = ReturnControlsFromXaml(layoutRoot);


            foreach (var control in _treeObject)
            {
                try
                {
                    Control current = null;
                    if (control.GetType() == typeof(Grid) || control.GetType() == typeof(Border) || control.GetType() == typeof(Rectangle)
                        || control.GetType() == typeof(ContentPresenter) || control.GetType() == typeof(TextBlock) || control.GetType() == typeof(ScrollContentPresenter))
                        continue;
                        current = (Control)control;
                    if(current.GetType() != typeException)
                       current.IsEnabled = state;
                }
                catch (Exception ex)
                {
                    //throw ex;
                }
            }

        }

    }
}
