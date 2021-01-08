using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Galatee.Structure;
using Galatee.DataAccess;

namespace Galatee.WorkflowManager
{

    public sealed class CodeActivityCancelCreationDemande : CodeActivity
    {

        #region In Parametres

        public InArgument<string> CodeDemande { get; set; }
        public InArgument<Guid> PKIDDemande { get; set; }

        #endregion

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            Guid pkID = context.GetValue<Guid>(PKIDDemande);
            string codedmd = context.GetValue<string>(CodeDemande);

            //Suppression de la demande dans la table demande
            List<CsDemandeWorkflow> lsDemandes = new DB_WORKFLOW().SelectAllDemande()
                .Where(dmd => dmd.PK_ID == pkID && dmd.CODE == codedmd)
                .ToList();

            //Une fois on les a trouvé on supprime Hi Hi Hi Hi Hi !!!!
            bool delete = new DB_WORKFLOW().DeleteDemande(lsDemandes);

            //et tout est ok, on dispose tout pour libérer la mémoire
            GC.SuppressFinalize(lsDemandes);            
        }
    }
}
