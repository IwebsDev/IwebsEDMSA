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

namespace Galatee.Silverlight.Classes
{
    public class ContextMenuManagement
    {
        public static ChildWindow CreateChildWindow(string namespaces, string title, SessionObject.ExecMode pExecMode, string Tdem = "05")
        {
            ChildWindow form = null;
            try
            {
                Type cwType = Type.GetType(namespaces);
                object cw = new object();
                object UserConnected = SessionObject.objectSelected;
                DataGrid gridUser = SessionObject.gridUtilisateur;
                cw = Activator.CreateInstance(cwType, new object[] { UserConnected }, new SessionObject.ExecMode[] { pExecMode }, new DataGrid[] { gridUser }, Tdem);
                form = (ChildWindow)cw;
                form.Title = title;
                return form;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ChildWindow CreateChildWindow(string namespaces, SessionObject.ExecMode pExecMode)
        {
            ChildWindow form = null;
            try
            {
                Type cwType = Type.GetType(namespaces);
                object cw = new object();
                object UserConnected = SessionObject.objectSelected;
                DataGrid gridUser = SessionObject.gridUtilisateur;
                cw = Activator.CreateInstance(cwType, new object[] { UserConnected }, new SessionObject.ExecMode[] { pExecMode }, new DataGrid[] { gridUser });
                form = (ChildWindow)cw;
                return form;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static UserControl CreateUserControl(Galatee.Silverlight.Library.MenuItem pMenuItem, int pNumEtapeDevis)
        {
            UserControl form = null;
            try
            {
                Type cwType = Type.GetType(pMenuItem.Name);
                object cw = new object();
                object UserConnected = SessionObject.objectSelected;
                DataGrid gridUser = SessionObject.gridUtilisateur;
                cw = Activator.CreateInstance(cwType, new int[] { pNumEtapeDevis });
                form = (UserControl)cw;
                return form;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
