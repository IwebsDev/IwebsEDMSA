using System;
using System.Collections.Generic;
using System.Text;

namespace Inova.Tools.Utilities
{
    public class MappingFieldAttribute : Attribute
    {
        public MappingFieldAttribute(string fieldName)
        {
            this.fieldName = fieldName;
        }
        private string fieldName;
        internal string FieldName
        {
            get
            {
                return fieldName;
            }
        }
    }
    public class LabelFieldAttribute : Attribute
    {
        public LabelFieldAttribute(string fieldName)
        {
            this.fieldName = fieldName;
        }
        private string fieldName;
        internal string FieldName
        {
            get
            {
                return fieldName;
            }
        }
    }
    public class MappingTableAttribute : Attribute
    {
        public MappingTableAttribute(string fieldName)
        {
            this.fieldName = fieldName;
        }
        private string fieldName;
        internal string FieldName
        {
            get
            {
                return fieldName;
            }
        }
    }
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class DisplayAttribute : Attribute
    {
        private bool display;
        public DisplayAttribute(bool display)
        {
            this.display = display;
        }
        public bool Display
        {
            get
            {
                return this.display;
            }
            set
            {
                this.display = value;
            }
        }
    }
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class EditableAttribute : Attribute
    {
        private bool editable;
        public EditableAttribute(bool editable)
        {
            this.editable = editable;
        }
        public bool Editable
        {
            get
            {
                return this.editable;
            }
            set
            {
                this.editable = value;
            }
        }
    }
}