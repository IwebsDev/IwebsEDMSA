using System;
using System.Collections.Generic;
using System.Linq;
using UpdateControls.Fields;

namespace Galatee.Silverlight.Library.Models
{
    public class OptionLeaf : Option
    {
        private Independent<bool> _selected = new Independent<bool>();

        public OptionLeaf(string name, int? moduleId, int? pProgramId, int? MainMenuId, int? menuId, int? pkId, string pUSERCREATION, DateTime? pDATECREATION)
            : base(name, moduleId, pProgramId, MainMenuId, menuId, pkId,pUSERCREATION,pDATECREATION)
        {
        }

        public bool Selected
        {
            get { return _selected; }
            set { _selected.Value = value; }
        }

        public override IEnumerable<Option> Children
        {
            get { return Enumerable.Empty<Option>(); }
        }

        public override IEnumerable<OptionLeaf> Leaves
        {
            get { return Enumerable.Repeat(this, 1); }
        }
    }
}
