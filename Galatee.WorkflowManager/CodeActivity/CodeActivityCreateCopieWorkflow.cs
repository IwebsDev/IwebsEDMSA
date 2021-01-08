using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Galatee.DataAccess;
using Galatee.Entity;
using Galatee.Structure;

namespace Galatee.WorkflowManager
{

    public sealed class CodeActivityCreateCopieWorkflow : CodeActivity<bool>
    {

        #region In Parametre

        public InArgument<Guid> PKIDDmd { get; set; }
        public InArgument<Guid> PkRWK { get; set; }
        public InArgument<string> CodeDemande { get; set; }

        #endregion

        #region Out Parametre

        public OutArgument<string> MessageErreur { get; set; }

        #endregion

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override bool Execute(CodeActivityContext context)
        {
            bool doCopy = true;
            try
            {
                string _codeDmd = context.GetValue<string>(this.CodeDemande);
                Guid _pkIdDmd = context.GetValue<Guid>(this.PKIDDmd);
                Guid _pkRWK = context.GetValue<Guid>(this.PkRWK);

                //On va essayé de faire une copie du circuit
                doCopy = new DB_WORKFLOW().CopieCicruitEtapeDemande(_pkRWK, _pkIdDmd, _codeDmd);
                if (doCopy) MessageErreur.Set(context, string.Empty);
                else MessageErreur.Set(context, "Une erreur s'est produite");
            }
            catch (Exception ex)
            {
                doCopy = false;
                MessageErreur.Set(context, ex.Message);
            }

            return doCopy;
        }
    }
}
