using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using Microsoft.Windows;

namespace Galatee.Silverlight.Rpnt.Helper
{
    public static class DragEventArgsExtensions
    {
        public static IEnumerable<T> GetData<T>(this DragEventArgs args)
{
    IEnumerable<T> results = null;

    // Get the dropped data from the Data property and cast it to the first format. 
    ItemDragEventArgs dragEventArgs = args.Data.GetData(args.Data.GetFormats()[0]) as ItemDragEventArgs;

    if (dragEventArgs == null)
        return results;

    // Get the collection of items
    SelectionCollection selectionCollection = dragEventArgs.Data as SelectionCollection;
    if (selectionCollection != null)
    {
        // cast each item to what is expected
        results = selectionCollection.Select(selection => selection.Item).OfType<T>();
    }

    return results;
}
    }
}