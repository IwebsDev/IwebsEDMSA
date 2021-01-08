
using Galatee.Silverlight.Library.ViewModels;
namespace Galatee.Silverlight.Library.Models
{
    public class DesignTimeData
    {
        private OptionViewModel optionView;

        public OptionViewModel Root
        {
            get { return optionView; }
            set { optionView = value; }
        }
        //public OptionViewModel Root
        //{
        //    get
        //    {
        //        return new OptionViewModel(
        //            new OptionGroup("All options")
        //                .Add(new OptionGroup("Basic options")
        //                    .Add(new OptionLeaf("Floor mats"))
        //                    .Add(new OptionLeaf("Passenger vanity mirror"))
        //                )
        //                .Add(new OptionGroup("Deluxe options")
        //                    .Add(new OptionLeaf("Chrome wheels"))
        //                    .Add(new OptionLeaf("Driver vanity mirror"))
        //                    .Add(new OptionGroup("Performance package")
        //                        .Add(new OptionLeaf("Fuel injection"))
        //                        .Add(new OptionLeaf("Anti-lock breaks"))
        //                    )
        //                )
        //                .Add(new OptionGroup("Luxury options")
        //                    .Add(new OptionLeaf("Leather seats"))
        //                    .Add(new OptionLeaf("Leather steering wheel cover"))
        //                    .Add(new OptionLeaf("Wood panel interior"))
        //                )
        //            );
        //    }
        //}

    }
}
