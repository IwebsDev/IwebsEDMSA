using Galatee.WorkflowManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Workflow.Interfaces
{
    /// <summary>
    /// Interface définissant le comportement d'une fenêtre d'initialisation 
    /// d'une demande de Workflow.
    /// </summary>
    public interface IWKFInitForm
    {        
        string DoYourActionAndGiveMeTheLineID(ref bool toutEstBon, ref string msg);
        string Title { get; set; }
        event ExecutionOkButtonEventHandler ExecutionOkButtonCompleted;
    }
}
