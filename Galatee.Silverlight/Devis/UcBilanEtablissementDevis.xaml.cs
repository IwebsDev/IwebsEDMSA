using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Galatee.Silverlight.ServiceAccueil ;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using Galatee.Silverlight.Resources.Devis;
using Galatee.Silverlight.Library;
using System.ComponentModel;
using System.Globalization;

namespace Galatee.Silverlight.Devis
{
    public partial class UcBilanEtablissementDevis : ChildWindow
    {
        List<ObjELEMENTDEVIS> lElements = new List<ObjELEMENTDEVIS>();
        ObjELEMENTDEVIS selectedElement = new ObjELEMENTDEVIS();
        ObjELEMENTDEVIS eltAdditional = new ObjELEMENTDEVIS();
        ObjTYPEDEVIS typeDevis = new ObjTYPEDEVIS();
        public List<ObjELEMENTDEVIS> MyElements { get; set; }
        List<ObjELEMENTDEVIS> SourceListeElementDevis = new List<ObjELEMENTDEVIS>();
        public decimal Frais { get; set; }
        public List<ObjFOURNITURE> MyFournitures { get; set; }
        public CsCtax Taxe { get; set; }
        private ObjDOCUMENTSCANNE doc = new ObjDOCUMENTSCANNE();
        private CsDemande myDemande = new CsDemande();
        private decimal montantHT = (decimal)0;
        private decimal montantTTC = (decimal)0;
        public SessionObject.ExecMode ExecMode { get; set; }
        public string NearestRoute { get; set; }
        public decimal Distance { get; set; }
        public ObjDOCUMENTSCANNE Schema { get; set; }
        public ObjMATRICULE Agent { get; set; }
        public SessionObject.ExecMode ModeExecution { get; set; }

        List<ObjELEMENTDEVIS> myElements = new List<ObjELEMENTDEVIS>();
        public UcBilanEtablissementDevis(CsDemande Demandedevis, List<ObjELEMENTDEVIS> _lElements, decimal montant,bool _IsValidationEtude)
        {
            InitializeComponent();
            this.myDemande = Demandedevis;
            this.myElements = _lElements;
            this.montantHT = montant;
            this.Txt_TypeDevis.Text = Demandedevis.LaDemande.LIBELLETYPEDEMANDE;
            this.Txt_NumDevis.Text = Demandedevis.LaDemande.NUMDEM;
        }

        public UcBilanEtablissementDevis()
        {
            InitializeComponent();
        }



        private void RemplirElements()
        {
            try
            {
                ObjELEMENTDEVIS ObjetElementDevis = null;
                List<ObjELEMENTDEVIS> ListeAfficher = new List<ObjELEMENTDEVIS>();
                if ((this.myElements == null) || (this.myElements.Count == 0))
                    throw new Exception(Languages.msgEmptyFournitures);

                myElements = myElements.Where(t => t.QUANTITE != 0 && t.QUANTITE != null).ToList();
                foreach (ObjELEMENTDEVIS elt in this.myElements.Where(t => t.QUANTITE != 0 && t.QUANTITE != null))
                {
                    elt.QUANTITECONSOMMEE = 0;
                    elt.QUANTITERECAP = elt.QUANTITE.ToString();
                    elt.MontantRecap = elt.MONTANTHT.Value .ToString(SessionObject.FormatMontant);
                    ListeAfficher.Add(elt);
                }
                // Ajout du trait de séparation entre ...
                ObjetElementDevis = new ObjELEMENTDEVIS();
                ObjetElementDevis.DESIGNATION = "_________________________________";
                ObjetElementDevis.QUANTITERECAP = "_______________";
                ObjetElementDevis.MontantRecap = "_____________________________";
                ListeAfficher.Add(ObjetElementDevis);
                // affichage du montant HT ...
                ObjetElementDevis = new ObjELEMENTDEVIS();
                ObjetElementDevis.DESIGNATION = Languages.lblMontantHT;
                ObjetElementDevis.QUANTITERECAP = "";
                ObjetElementDevis.MontantRecap = myElements.Sum(t => t.MONTANTHT).Value .ToString(SessionObject.FormatMontant);
                ListeAfficher.Add(ObjetElementDevis);
                // Affichage des frais de TVA
                ObjetElementDevis = new ObjELEMENTDEVIS();
                ObjetElementDevis.DESIGNATION = Languages.lblMontantTVA;
                decimal fraistva = myElements.Sum(t => t.MONTANTTAXE ).Value ;
                ObjetElementDevis.MontantRecap = fraistva.ToString(SessionObject.FormatMontant);
                ListeAfficher.Add(ObjetElementDevis);
                //Affichage du montant TTC
                ObjetElementDevis = new ObjELEMENTDEVIS();
                ObjetElementDevis.DESIGNATION = Languages.lblMontantTTC;
                ObjetElementDevis.QUANTITERECAP = Languages.lblTTC;
                this.montantTTC = myElements.Sum(t => t.MONTANTTTC).Value ;
                ObjetElementDevis.MontantRecap = montantTTC.ToString(SessionObject.FormatMontant);
                ListeAfficher.Add(ObjetElementDevis);

                dataGridElementDevis.ItemsSource = ListeAfficher;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private decimal Arrondir(decimal montant)
        //{
        //    string[] partie = montant.ToString().Split(new char[] { ',' });
        //    if (partie.Length == 1)
        //        return decimal.Parse(partie[0]);
        //    if (int.Parse(partie[1].Substring(0, 1)) >= 5)
        //        return decimal.Parse(partie[0]) + (decimal)1;
        //    else
        //        return decimal.Parse(partie[0]);
        //}

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EnregisterOuTransmetre(false);
            }
            catch (Exception ex)
            {
                LayoutRoot.Cursor = Cursors.Arrow;
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (ModeExecution == SessionObject.ExecMode.Consultation)
                this.DialogResult = true;
            this.DialogResult = false;
            this.Close();
        }
        void Translate()
        {
            this.lab_AmountOfDeposit.Content = Languages.lblNumeroDuDevis;
            this.lbl_TypeDevis.Content = Languages.lbl_TypeDevis;
            this.Btn_Transmettre.Content = Languages.Btn_Transmettre;
        }
        void EnregisterOuTransmetre(bool IsTransmetre)
        {

            LayoutRoot.Cursor = Cursors.Wait;
            if (myDemande.EltDevis != null && myDemande.EltDevis.Count == 0)
            {
                myDemande.EltDevis = new List<ObjELEMENTDEVIS>();
                myDemande.EltDevis = myElements;

            }
            this.myDemande.LaDemande.NUMDEM = Txt_NumDevis.Text;
            this.myDemande.EltDevis.ForEach(el => el.FK_IDTDEM = this.myDemande.LaDemande.PK_ID);
            this.myDemande.EltDevis.ForEach(el => el.NUMDEM = this.myDemande.LaDemande.NUMDEM);

            myDemande.LstCanalistion = null;
            myDemande.Abonne  = null;

            AcceuilServiceClient clientDevis = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            clientDevis.ValiderDemandeCompleted += (ss, b) =>
            {
                this.EditerButton.IsEnabled = true;
                if (b.Cancelled || b.Error != null)
                {
                    string error = b.Error.Message;
                    Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                    return;
                }
                if (IsTransmetre)
                {
                    List<string> codes = new List<string>();
                    codes.Add(myDemande.InfoDemande.CODE);
                    Galatee.Silverlight.Shared.ClasseMEthodeGenerique.TransmettreDemande(codes, true ,this );
                    //List<CsUtilisateur> leUser = new List<CsUtilisateur>();
                    //if (myDemande.InfoDemande != null && myDemande.InfoDemande.CODE != null)
                    //{
                    //    foreach (CsUtilisateur item in myDemande.InfoDemande.UtilisateurEtapeSuivante)
                    //        leUser.Add(item);
                    //    Shared.ClasseMEthodeGenerique.NotifierMailDemande(leUser, "0001", myDemande.LaDemande.NUMDEM, myDemande.LaDemande.LIBELLETYPEDEMANDE);
                    //}

                }
                LayoutRoot.Cursor = Cursors.Arrow;
            };
            clientDevis.ValiderDemandeAsync(myDemande);
        }
        private List<ObjELEMENTDEVIS> LireElements()
        {
            try
            {
                List<ObjELEMENTDEVIS> ListElementDevis = new List<ObjELEMENTDEVIS>();
                foreach (ObjELEMENTDEVIS elementDevis in dataGridElementDevis.ItemsSource)
                {
                    ListElementDevis.Add(elementDevis);
                }
                return ListElementDevis;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int DecToInt(decimal montant)
        {
            char[] separateur = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToCharArray();
            string[] partie = montant.ToString().Split(separateur);
            return int.Parse(partie[0]);
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    this.Btn_Transmettre.IsEnabled = false;
                    this.CancelButton.IsEnabled = true;
                }
                this.RemplirElements();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void EditerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.EditerButton.IsEnabled = false;
                EnregisterOuTransmetre(true);
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}

