using System;
using System.Collections.Generic;
using System.Linq;

namespace Galatee.Silverlight.Library.Models
{
    public class OptionGroup : Option
    {
        private List<Option> _children = new List<Option>();

        public OptionGroup(string name, int? moduleId, int? pProgramId, int? MainMenuId, int? menuId,int? pkId,string USERCREATION,DateTime? DateCreation)
            : base(name, moduleId, pProgramId, MainMenuId, menuId, pkId, USERCREATION, DateCreation)
        {
        }

        public OptionGroup Add(Option child)
        {
            _children.Add(child);
            return this;
        }

        public override IEnumerable<Option> Children
        {
            get { return _children; }
        }

        public override IEnumerable<OptionLeaf> Leaves
        {
            get { return _children.SelectMany(child => child.Leaves); }
        }
    }
}
