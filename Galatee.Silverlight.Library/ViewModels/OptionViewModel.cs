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
using Galatee.Silverlight.Library.Models;
using System.Linq;
using System.Collections.Generic;

namespace Galatee.Silverlight.Library.ViewModels
{
    public class OptionViewModel
    {
        private Option _option;

        public Option Option
        {
            get { return _option; }
            set { _option = value; }
        }

        public OptionViewModel(Option option)
        {
            _option = option;
        }

        public string Name
        {
            get { return _option.Name; }
        }

        public bool? IsChecked
        {
            get
            {
                bool anySelected = _option.Leaves.Any(leaf => leaf.Selected);
                bool anyNotSelected = _option.Leaves.Any(leaf => !leaf.Selected);
                return
                    (anySelected && !anyNotSelected) ? true :
                    (!anySelected && anyNotSelected) ? false :
                    (bool?)null;
            }
            set
            {
                foreach (OptionLeaf leaf in _option.Leaves)
                    leaf.Selected = value ?? false;
            }
        }

        public IEnumerable<OptionViewModel> Children
        {
            get
            {
                return _option.Children
                    .Select(child => new OptionViewModel(child));
            }
        }
    }
}
