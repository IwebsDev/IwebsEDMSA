using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Data;
using System.Collections;
using System.Windows.Forms;



namespace Inova.Tools.Utilities
{
    public static class Utilitaires
    {

        private static object CreateObject(Type classType, DataRow row)
        {
            object o = null;
            try
            {
                o = Activator.CreateInstance(classType);

                PropertyInfo[] props = classType.GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    object[] attributes = prop.GetCustomAttributes(typeof(MappingFieldAttribute), true);
                    if (attributes.Length > 0)
                    {
                        string fieldName = ((MappingFieldAttribute)attributes[0]).FieldName;
                        int index = row.Table.Columns.IndexOf(fieldName);
                        if (index != -1)
                        {
                            if (row[index] != System.DBNull.Value)
                                prop.SetValue(o, row[index], null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return o;
        }
        public static IList CreateCollection(Type classType, DataTable table)
        {
            Type leType = typeof(List<>);
            Type[] param = new Type[] { classType };
            Type objGeneric = leType.MakeGenericType(param);

            IList o = (IList)Activator.CreateInstance(objGeneric);

            foreach (DataRow row in table.Rows)
            {
                o.Add(CreateObject(classType, row));
            }
            return o;
        }

        public static DataTable CreateGenericDataset(Type classType, ArrayList stCollection)
        {
            DataTable dt = new DataTable();
            int index;
            for (index = 0; index < stCollection.Count; index++)
            {
                PropertyInfo[] props = classType.GetProperties();
                DataRow row = dt.NewRow();
                foreach (PropertyInfo prop in props)
                {
                    object[] attributes = prop.GetCustomAttributes(typeof(MappingFieldAttribute), true);
                    if (attributes.Length > 0)
                    {
                        string fieldName = ((MappingFieldAttribute)attributes[0]).FieldName;

                        if (!dt.Columns.Contains(fieldName))
                        {
                            DataColumn dc = new DataColumn(fieldName);
                            dc.DataType = prop.PropertyType;
                            dt.Columns.Add(dc);
                        }
                        row[fieldName] = prop.GetValue(stCollection[index], null);
                    }
                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        public static DataTable CreateGenericDatasetToDisplay(Type classType, ArrayList stCollection)
        {

            DataTable dt = new DataTable();
            int index;
            try
            {


                for (index = 0; index < stCollection.Count; index++)
                {
                    PropertyInfo[] props = classType.GetProperties();
                    DataRow row = null;
                    foreach (PropertyInfo prop in props)
                    {
                        object[] attributes = prop.GetCustomAttributes(typeof(MappingFieldAttribute), true);
                        object[] aEdit = prop.GetCustomAttributes(typeof(EditableAttribute), true);

                        object[] aLab = prop.GetCustomAttributes(typeof(LabelFieldAttribute), true);
                        object[] aDisp = prop.GetCustomAttributes(typeof(DisplayAttribute), true);

                        if (attributes.Length > 0)
                        {
                            row = dt.NewRow();
                            string fieldName = ((MappingFieldAttribute)attributes[0]).FieldName;
                            if (!dt.Columns.Contains(fieldName))
                            {
                                if (aDisp.Length > 0)
                                {
                                    if (((DisplayAttribute)aDisp[0]).Display)
                                    {
                                        DataColumn dc = new DataColumn(fieldName);
                                        dc.DataType = prop.PropertyType;
                                        //dc.ReadOnly = true;
                                        if (aEdit.Length > 0)
                                            dc.ReadOnly = !((EditableAttribute)aEdit[0]).Editable;
                                        dt.Columns.Add(dc);
                                    }
                                }
                            }
                            if (dt.Columns[fieldName] != null)
                                row[fieldName] = prop.GetValue(stCollection[index], null);

                        }

                    }
                    if (row != null)
                        dt.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return dt;
        }

        public static void ConstruireListview(System.Windows.Forms.ListView lv,
            Type classType, System.Collections.ArrayList stCollection, int ImageIndex)
        {
            try
            {
                lv.Clear();
                lv.Columns.Clear();
                PropertyInfo[] props = classType.GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    foreach (object o in prop.GetCustomAttributes(typeof(DisplayAttribute), false))
                    {
                        if (((DisplayAttribute)o).Display)
                        {
                            object[] attributes = prop.GetCustomAttributes(typeof(MappingFieldAttribute), true);
                            if (attributes.Length > 0)
                            {
                                string colname = ((LabelFieldAttribute)(prop.GetCustomAttributes(typeof(LabelFieldAttribute), true)[0])).FieldName;
                                ColumnHeader ch = new ColumnHeader();
                                ch.Text = colname;
                                ch.Tag = prop;
                                ch.Width = -2;
                                if (prop.PropertyType != typeof(String))
                                    ch.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
                                else
                                {
                                    ch.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
                                }
                                lv.Columns.Add(ch);
                            }
                        }
                    }
                }
                lv.View = System.Windows.Forms.View.Details;
                lv.FullRowSelect = true;
                lv.HideSelection = false;
                if (stCollection !=null )
                {
                    foreach (object o in stCollection)
                    {
                        System.Windows.Forms.ListViewItem li = null;
                        for (int i = 0; i < lv.Columns.Count; i++)
                        {
                            PropertyInfo pop = (PropertyInfo)lv.Columns[i].Tag;
                            string _value = string.Empty;
                            if (pop.GetValue(o, null) != null)
                                _value = ReturnObjectValue(pop, o);
                            if (li == null)
                                li = new ListViewItem(_value);
                            else
                                li.SubItems.Add(_value);
                        }
                        li.ImageIndex = ImageIndex;
                        li.Tag = o;
                        lv.Items.Add(li);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            //AutoResizeByContentAndHeader(lv);
            //lv.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);



        }
        public static void AutoResizeByContentAndHeader(ref ListView list)
        {
            foreach (ColumnHeader column in list.Columns)
            {
                column.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                int contentAutoWidth = column.Width;
                column.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                if (contentAutoWidth > column.Width)
                {
                    column.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                }
            }
        }
        public static void ConstruireListview(System.Windows.Forms.ListView lv,
           Type classType, System.Collections.ArrayList stCollection, int ImageIndex, ColumnHeaderAutoResizeStyle _ColumnContent)
        {
            ConstruireListview(lv, classType, stCollection, ImageIndex);
            //lv.AutoResizeColumns(_ColumnContent);
        }
        private static string ReturnObjectValue(PropertyInfo pop, object o)
        {
         
            Type T = pop.PropertyType.BaseType;
            if (pop.PropertyType == typeof(DateTime))
                return ((DateTime)pop.GetValue(o, null)).ToShortDateString();
            if ((pop.PropertyType == typeof(int)))
                return pop.GetValue(o, null).ToString();
            if ((pop.PropertyType == typeof(Boolean)))
            {
                if ((bool)pop.GetValue(o, null))
                    return "Oui";
                else
                    return "Non";
            }
            return pop.GetValue(o, null).ToString();
        }

    }
}
