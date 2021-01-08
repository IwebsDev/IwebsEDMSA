using Galatee.Silverlight.Resources.Parametrage;
using Galatee.Silverlight.ServiceParametrage;
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
using System.Linq;
using Galatee.Silverlight.ServiceWorkflow;

namespace Galatee.Silverlight.Workflow
{
    public class WorkflowDmdManager
    {

        public delegate void InsertionComplete(string result);
        public event InsertionComplete InsertionDemandeWorkflowComplete;


        /// <summary>
        /// Créer une demande à inserer dans un  workflow déjà préconfiguré, 
        /// </summary>
        /// <param name="PKIDLaLigne">ID de la ligne de la table concernée par la demande</param>
        /// <param name="UcNameInitiation">Nom du formulaire de création de la demande</param>
        /// <returns></returns>
        public void InsererMaDemande(string PKIDLaLigne, string UcNameInitiation, int FKIDCentreDemande,
            string codeDemande_TableTravail)
        {
            string codeDemande = string.Empty;

            ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
            int back = LoadingManager.BeginLoading("Création de la demande en cours ...");
            //On recherche les informations sur l'opération concernée par le formulaire            
            client.SelectAllViewOperationFormulaireCompleted += (sender, args) =>
            {
                if (args.Cancelled || args.Error != null)
                {
                    LoadingManager.EndLoading(back);
                    string error = args.Error.Message;
                    Message.Show(error, Languages.ListeCodePoste);
                    return;
                }
                if (args.Result == null)
                {
                    LoadingManager.EndLoading(back);
                    Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                    return;
                }
                var LOperation = args.Result.Where(f => f.FULLNAMECONTROLE == UcNameInitiation)
                    .FirstOrDefault();
                if (null != LOperation)
                {
                    //Maintenant qu'on a l'opération, on connait le centre grâce à l'utilisateur connecté
                    //si c'est tout les centres, on récupère donc le 1er centre dans lequel l'opération es défini
                    client.SelectAllConfigurationWorkflowCentreCompleted += (c_sender, c_args) =>
                    {
                        if (c_args.Cancelled || c_args.Error != null)
                        {
                            LoadingManager.EndLoading(back);
                            string error = args.Error.Message;
                            Message.Show(error, Languages.ListeCodePoste);
                            return;
                        }
                        if (c_args.Result == null)
                        {
                            LoadingManager.EndLoading(back);
                            Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                            return;
                        }

                        Guid WorkflowId = Guid.Empty;
                        CsVwConfigurationWorkflowCentre wkfInfo = null;
                        if (UserConnecte.Centre == "000")
                        {
                            WorkflowId = c_args.Result.First().PK_ID;
                            wkfInfo = c_args.Result.First();
                        }
                        else if (0 != FKIDCentreDemande)
                        {
                            int centreId = UserConnecte.FK_IDCENTRE;
                            wkfInfo = c_args.Result.Where(w => w.CENTREID == FKIDCentreDemande && w.OPERATIONID == LOperation.PK_ID)
                                .FirstOrDefault();
                            WorkflowId = (null != wkfInfo) ? wkfInfo.PK_ID : Guid.Empty;
                        }

                        //Insertion de la demande
                        WorkflowClient wkfClient = new WorkflowClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Workflow"));
                        wkfClient.Endpoint.Binding.OpenTimeout = new TimeSpan(0, 1, 0);
                        wkfClient.Endpoint.Binding.CloseTimeout = new TimeSpan(5, 0, 0);
                        wkfClient.Endpoint.Binding.SendTimeout = new TimeSpan(5, 0, 0);

                        wkfClient.InsererMaDemandeCompleted += (wkfsender, _args) =>
                        {
                            LoadingManager.EndLoading(back);
                            if (_args.Cancelled || _args.Error != null)
                            {
                                string error = _args.Error.Message;
                                Message.Show(error, Languages.ListeCodePoste);
                                return;
                            }
                            if (_args.Result == null)
                            {
                                if (null != InsertionDemandeWorkflowComplete) InsertionDemandeWorkflowComplete("ERR: Une erreur est survenue");
                                return;
                            }
                            if (null != InsertionDemandeWorkflowComplete) InsertionDemandeWorkflowComplete(_args.Result);
                        };
                        wkfClient.InsererMaDemandeAsync(wkfInfo.CENTREID, wkfInfo.PK_ID, LOperation.PK_ID, PKIDLaLigne, UserConnecte.matricule, codeDemande_TableTravail);
                    };
                    client.SelectAllConfigurationWorkflowCentreAsync();
                }
            };
            client.SelectAllViewOperationFormulaireAsync();
        }

        /// <summary>
        /// Créer une demande à inserer dans un  workflow déjà préconfiguré, 
        /// </summary>
        /// <param name="PKIDLaLigne">ID de la ligne de la table concernée par la demande</param>
        /// <param name="UcNameInitiation">Nom du formulaire de création de la demande</param>
        /// <returns></returns>
        public void InsererMaDemande(string PKIDLaLigne, int CodeTDem, int FKIDCentreDemande,
            string codeDemande_TableTravail)
        {
            string codeDemande = string.Empty;

            ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
            int back = LoadingManager.BeginLoading("Création de la demande en cours ...");
            //On recherche les informations sur l'opération concernée par le formulaire            
            client.SelectAllOperationCompleted += (sender, args) =>
            {
                if (args.Cancelled || args.Error != null)
                {
                    LoadingManager.EndLoading(back);
                    string error = args.Error.Message;
                    Message.Show(error, Languages.ListeCodePoste);
                    return;
                }
                if (args.Result == null)
                {
                    LoadingManager.EndLoading(back);
                    Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                    return;
                }
                var LOperation = args.Result.Where(f => f.CODE_TDEM == CodeTDem.ToString())
                    .FirstOrDefault();
                if (null != LOperation)
                {
                    //Maintenant qu'on a l'opération, on connait le centre grâce à l'utilisateur connecté
                    //si c'est tout les centres, on récupère donc le 1er centre dans lequel l'opération es défini
                    client.SelectAllConfigurationWorkflowCentreCompleted += (c_sender, c_args) =>
                    {
                        if (c_args.Cancelled || c_args.Error != null)
                        {
                            LoadingManager.EndLoading(back);
                            string error = args.Error.Message;
                            Message.Show(error, Languages.ListeCodePoste);
                            return;
                        }
                        if (c_args.Result == null)
                        {
                            LoadingManager.EndLoading(back);
                            Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                            return;
                        }

                        Guid WorkflowId = Guid.Empty;
                        CsVwConfigurationWorkflowCentre wkfInfo = null;
                        if (UserConnecte.Centre == "000")
                        {
                            //BSY:19/06/2017 (je prend le centre du poste au lieu du 1ere configuré sur le workflow
                            //WorkflowId = c_args.Result.First().PK_ID;
                            wkfInfo = c_args.Result.FirstOrDefault(w=>w.CENTREID==FKIDCentreDemande);
                            WorkflowId = wkfInfo.PK_ID;

                        }
                        else if (0 != FKIDCentreDemande)
                        {
                            int centreId = UserConnecte.FK_IDCENTRE;
                            wkfInfo = c_args.Result.Where(w => w.CENTREID == FKIDCentreDemande && w.OPERATIONID == LOperation.PK_ID)
                                .FirstOrDefault();
                            WorkflowId = (null != wkfInfo) ? wkfInfo.PK_ID : Guid.Empty;
                        }

                        //Insertion de la demande
                        WorkflowClient wkfClient = new WorkflowClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Workflow"));
                        wkfClient.Endpoint.Binding.OpenTimeout = new TimeSpan(0, 1, 0);
                        wkfClient.Endpoint.Binding.CloseTimeout = new TimeSpan(5, 0, 0);
                        wkfClient.Endpoint.Binding.SendTimeout = new TimeSpan(5, 0, 0);

                        wkfClient.InsererMaDemandeCompleted += (wkfsender, _args) =>
                        {
                            LoadingManager.EndLoading(back);
                            if (_args.Cancelled || _args.Error != null)
                            {
                                string error = _args.Error.Message;
                                Message.Show(error, Languages.ListeCodePoste);
                                return;
                            }
                            if (_args.Result == null)
                            {
                                if (null != InsertionDemandeWorkflowComplete) InsertionDemandeWorkflowComplete("ERR: Une erreur est survenue");
                                return;
                            }
                            if (null != InsertionDemandeWorkflowComplete) InsertionDemandeWorkflowComplete(_args.Result);
                        };
                        wkfClient.InsererMaDemandeAsync(wkfInfo.CENTREID, wkfInfo.PK_ID, LOperation.PK_ID, PKIDLaLigne, UserConnecte.matricule, codeDemande_TableTravail);
                    };
                    client.SelectAllConfigurationWorkflowCentreAsync();
                }
            };
            client.SelectAllOperationAsync();
        }


        public void InsererMaDemandeToGroupeDeValidation(string PKIDLaLigne, int CodeTDem, int FKIDCentreDemande,Guid leGroupeValidation,
            string codeDemande_TableTravail)
        {
            string codeDemande = string.Empty;

            ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
            int back = LoadingManager.BeginLoading("Création de la demande en cours ...");
            //On recherche les informations sur l'opération concernée par le formulaire            
            client.SelectAllOperationCompleted += (sender, args) =>
            {
                if (args.Cancelled || args.Error != null)
                {
                    LoadingManager.EndLoading(back);
                    string error = args.Error.Message;
                    Message.Show(error, Languages.ListeCodePoste);
                    return;
                }
                if (args.Result == null)
                {
                    LoadingManager.EndLoading(back);
                    Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                    return;
                }
                var LOperation = args.Result.Where(f => f.CODE_TDEM == CodeTDem.ToString())
                    .FirstOrDefault();
                if (null != LOperation)
                {
                    //Maintenant qu'on a l'opération, on connait le centre grâce à l'utilisateur connecté
                    //si c'est tout les centres, on récupère donc le 1er centre dans lequel l'opération es défini
                    client.SelectAllConfigurationWorkflowCentreCompleted += (c_sender, c_args) =>
                    {
                        if (c_args.Cancelled || c_args.Error != null)
                        {
                            LoadingManager.EndLoading(back);
                            string error = args.Error.Message;
                            Message.Show(error, Languages.ListeCodePoste);
                            return;
                        }
                        if (c_args.Result == null)
                        {
                            LoadingManager.EndLoading(back);
                            Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                            return;
                        }

                        Guid WorkflowId = Guid.Empty;
                        CsVwConfigurationWorkflowCentre wkfInfo = null;
                        if (UserConnecte.Centre == "000")
                        {
                            WorkflowId = c_args.Result.First().PK_ID;
                            wkfInfo = c_args.Result.First();
                        }
                        else if (0 != FKIDCentreDemande)
                        {
                            int centreId = UserConnecte.FK_IDCENTRE;
                            wkfInfo = c_args.Result.Where(w => w.CENTREID == FKIDCentreDemande && w.OPERATIONID == LOperation.PK_ID)
                                .FirstOrDefault();
                            WorkflowId = (null != wkfInfo) ? wkfInfo.PK_ID : Guid.Empty;
                        }

                        //Insertion de la demande
                        WorkflowClient wkfClient = new WorkflowClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Workflow"));
                        wkfClient.Endpoint.Binding.OpenTimeout = new TimeSpan(0, 1, 0);
                        wkfClient.Endpoint.Binding.CloseTimeout = new TimeSpan(5, 0, 0);
                        wkfClient.Endpoint.Binding.SendTimeout = new TimeSpan(5, 0, 0);

                        wkfClient.InsererMaDemandeToGroupeValidationCompleted += (wkfsender, _args) =>
                        {
                            LoadingManager.EndLoading(back);
                            if (_args.Cancelled || _args.Error != null)
                            {
                                string error = _args.Error.Message;
                                Message.Show(error, Languages.ListeCodePoste);
                                return;
                            }
                            if (_args.Result == null)
                            {
                                if (null != InsertionDemandeWorkflowComplete) InsertionDemandeWorkflowComplete("ERR: Une erreur est survenue");
                                return;
                            }
                            if (null != InsertionDemandeWorkflowComplete) InsertionDemandeWorkflowComplete(_args.Result);
                        };
                        wkfClient.InsererMaDemandeToGroupeValidationAsync(wkfInfo.CENTREID, wkfInfo.PK_ID, LOperation.PK_ID,leGroupeValidation, PKIDLaLigne, UserConnecte.matricule, codeDemande_TableTravail);
                    };
                    client.SelectAllConfigurationWorkflowCentreAsync();
                }
            };
            client.SelectAllOperationAsync();
        }

    }
}
