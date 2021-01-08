using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Galatee.DataAccess.Common
{
	public class AlimentationResultat
    {
        #region Période de synchronisation

        [System.ComponentModel.Description("Date début de PRE Chargement"),
        System.ComponentModel.DefaultValue(""),
        System.ComponentModel.DisplayName("Date début de PRE Chargement:"),
        System.ComponentModel.Category("Periode de Synchronisation"), System.ComponentModel.ReadOnly(true)]
        public string Date_Debut_PRESynchronisation
        { get; set; }

        [System.ComponentModel.Description("Date Fin de PRE Chargement"),
        System.ComponentModel.DefaultValue(""),
        System.ComponentModel.DisplayName("Date Fin de PRE Chargement:"),
        System.ComponentModel.Category("Periode de Synchronisation"), System.ComponentModel.ReadOnly(true)]
        public string Date_Fin_PRESynchronisation
        { get; set; }

        [System.ComponentModel.Description("Date de début de la Synchronisation"),
		System.ComponentModel.DefaultValue(""),
		System.ComponentModel.DisplayName("Date de début de synchronisation:"),
		System.ComponentModel.Category("Periode de Synchronisation"),System.ComponentModel.ReadOnly(true)]
		public string Date_Debut_Synchronisation
		{ get; set; }

		[System.ComponentModel.Description("Date de Fin de la Synchronisation"),
		System.ComponentModel.DefaultValue(""),
		System.ComponentModel.DisplayName("Date de Fin de synchronisation:"),
		System.ComponentModel.Category("Periode de Synchronisation"),System.ComponentModel.ReadOnly(true)]
		public string Date_Fin_Synchronisation
		{ get; set; }

        #endregion

        #region Données BTA

        [System.ComponentModel.Description("Nombre de compteurs actuellement en base"),
        System.ComponentModel.DefaultValue("0 /0 "),
        System.ComponentModel.DisplayName("Nombre de Compteurs BTA traités:"),
        System.ComponentModel.Category("Données BTA"), System.ComponentModel.ReadOnly(true)]
        public string NombreCompteurEnBase
        { get; set; }

        [System.ComponentModel.Description("Nombre de nouveaux compteurs ajoutés"),
        System.ComponentModel.DefaultValue("0 /0 "),
        System.ComponentModel.DisplayName("Nombre de Compteurs BTA Ajoutés:"),
        System.ComponentModel.Category("Données BTA"), System.ComponentModel.ReadOnly(true)]
        public string NombreCompteurAjoutes
        { get; set; }

        [System.ComponentModel.Description("Nombre de compteurs mis à jour"),
        System.ComponentModel.DefaultValue("0/0"),
        System.ComponentModel.DisplayName("Nombre de Compteurs BTA mis à jour:"),
        System.ComponentModel.Category("Données BTA"), System.ComponentModel.ReadOnly(true)]
        public string NombreCompteurMAJ
        { get; set; }

        [System.ComponentModel.Description("Nombre de compteurs non correctement synchronisés"),
        System.ComponentModel.DefaultValue("0/0"),
        System.ComponentModel.DisplayName("Nombre de Compteurs BTA en erreur:"),
        System.ComponentModel.Category("Données BTA"), System.ComponentModel.ReadOnly(true)]
        public string NombreCompteursEnErreur
        { get; set; }

        [System.ComponentModel.Description("Nombre de branchements traités durant la synchro"),
        System.ComponentModel.DefaultValue("0 /0 "),
        System.ComponentModel.DisplayName("Nombre de Branchements BTA traités:"),
        System.ComponentModel.Category("Données BTA"), System.ComponentModel.ReadOnly(true)]
        public string NombreBranchementTraite
        { get; set; }

        [System.ComponentModel.Description("Nombre de nouveaux branchements ajoutés dans la base"),
		System.ComponentModel.DefaultValue("0/0"),
		System.ComponentModel.DisplayName("Nombre de Branchements BTA Ajouté(s):"),
		System.ComponentModel.Category("Données BTA"),System.ComponentModel.ReadOnly(true)]
		public string   NombreBranchementAjoutes
		{ get; set; }

		[System.ComponentModel.Description("Nombre de branchements mis à jour dans la base"),
		System.ComponentModel.DefaultValue("0 / 0"),
		System.ComponentModel.DisplayName("Nombre de Branchements BTA Mis à Jour:"),
		System.ComponentModel.Category("Données BTA"),System.ComponentModel.ReadOnly(true)]
		public string NombreBranchementMAJ
		{ get; set; }

        [System.ComponentModel.Description("Nombre de branchements non correctement synchronisés"),
        System.ComponentModel.DefaultValue("0 / 0"),
        System.ComponentModel.DisplayName("Nombre de Branchements BTA en erreur:"),
        System.ComponentModel.Category("Données BTA"), System.ComponentModel.ReadOnly(true)]
        public string NombreBranchementEnErreur
        { get; set; }

        [System.ComponentModel.Description("Nombre d'abonnés BTA traités durant la synchro"),
        System.ComponentModel.DefaultValue("0 /0 "),
        System.ComponentModel.DisplayName("Nombre d'abonnés BTA traités:"),
        System.ComponentModel.Category("Données BTA"), System.ComponentModel.ReadOnly(true)]
        public string NombreAbonnesBTATraites
        { get; set; }

        [System.ComponentModel.Description("Nombre de nouveaux Abonnés ajoutés"),
        System.ComponentModel.DefaultValue("0 /0 "),
        System.ComponentModel.DisplayName("Nombre d'abonnés BTA Ajoutés:"),
        System.ComponentModel.Category("Données BTA"), System.ComponentModel.ReadOnly(true)]
        public string NombreAbonnesBTAAjoutes
        { get; set; }

        [System.ComponentModel.Description("Nombre d'Abonnés dont les infos ont été mises à jour"),
        System.ComponentModel.DefaultValue("0/0"),
        System.ComponentModel.DisplayName("Nombre d'abonnés BTA mis à jour:"),
        System.ComponentModel.Category("Données BTA"), System.ComponentModel.ReadOnly(true)]
        public string NombreAbonnesBTAMAJ
        { get; set; }

        [System.ComponentModel.Description("Nombre d'Abonnés BTA non correctement synchronisés"),
        System.ComponentModel.DefaultValue("0/0"),
        System.ComponentModel.DisplayName("Nombre d'Abonnés BTA en erreur:"),
        System.ComponentModel.Category("Données BTA"), System.ComponentModel.ReadOnly(true)]
        public string NombreAbonnesBTAEnErreur
        { get; set; }

        [System.ComponentModel.Description("Nombre de consommations BTA traitées durant la synchro"),
        System.ComponentModel.DefaultValue("0 /0 "),
        System.ComponentModel.DisplayName("Nombre de consommations BTA traitées:"),
        System.ComponentModel.Category("Données BTA"), System.ComponentModel.ReadOnly(true)]
        public string NombreConsosBTATraites
        { get; set; }

        [System.ComponentModel.Description("Nombre de consommations BTA ajoutées"),
        System.ComponentModel.DefaultValue("0 /0 "),
        System.ComponentModel.DisplayName("Nombre de consommations BTA Ajoutés:"),
        System.ComponentModel.Category("Données BTA"), System.ComponentModel.ReadOnly(true)]
        public string NombreConsosBTAAjoutees
        { get; set; }

        [System.ComponentModel.Description("Nombre de consommations BTA dont les infos ont été mises à jour"),
        System.ComponentModel.DefaultValue("0/0"),
        System.ComponentModel.DisplayName("Nombre de consommations BTA mises à jour:"),
        System.ComponentModel.Category("Données BTA"), System.ComponentModel.ReadOnly(true)]
        public string NombreConsosBTAMAJ
        { get; set; }

        [System.ComponentModel.Description("Nombre de consommations BTA non correctement synchronisés"),
        System.ComponentModel.DefaultValue("0/0"),
        System.ComponentModel.DisplayName("Nombre de consommations BTA en erreur:"),
        System.ComponentModel.Category("Données BTA"), System.ComponentModel.ReadOnly(true)]
        public string NombreConsosBTAEnErreur
        { get; set; }

        [System.ComponentModel.Description("Nombre de BETs traités durant la synchro"),
        System.ComponentModel.DefaultValue("0 /0 "),
        System.ComponentModel.DisplayName("Nombre de BETs traités:"),
        System.ComponentModel.Category("Données BTA"), System.ComponentModel.ReadOnly(true)]
        public string NombreBETsTraites
        { get; set; }

        [System.ComponentModel.Description("Nombre de nouveaux BETs ajoutés"),
        System.ComponentModel.DefaultValue("0 /0 "),
        System.ComponentModel.DisplayName("Nombre de BETs Ajoutés:"),
        System.ComponentModel.Category("Données BTA"), System.ComponentModel.ReadOnly(true)]
        public string NombreBETsAjoutes
        { get; set; }

        [System.ComponentModel.Description("Nombre de BETs dont les infos ont été mises à jour"),
        System.ComponentModel.DefaultValue("0/0"),
        System.ComponentModel.DisplayName("Nombre de BETs mis à jour:"),
        System.ComponentModel.Category("Données BTA"), System.ComponentModel.ReadOnly(true)]
        public string NombreBETsMAJ
        { get; set; }

        [System.ComponentModel.Description("Nombre de BETs non correctement synchronisés"),
        System.ComponentModel.DefaultValue("0/0"),
        System.ComponentModel.DisplayName("Nombre de BETs en erreur:"),
        System.ComponentModel.Category("Données BTA"), System.ComponentModel.ReadOnly(true)]
        public string NombreBETsEnErreur
        { get; set; }

        #endregion

        #region Données HTA

        //[System.ComponentModel.Description("Nombre de comptages actuellement en base"),
        //System.ComponentModel.DefaultValue("0/0 "),
        //System.ComponentModel.DisplayName("Nombre de Comptages HTA traités:"),
        //System.ComponentModel.Category("Données HTA"), System.ComponentModel.ReadOnly(true)]
        //public string NombreComptageEnBase
        //{ get; set; }

        //[System.ComponentModel.Description("Nombre de nouveaux comptages ajoutés"),
        //System.ComponentModel.DefaultValue("0/0 "),
        //System.ComponentModel.DisplayName("Nombre de Comptages HTA Ajoutés:"),
        //System.ComponentModel.Category("Données HTA"), System.ComponentModel.ReadOnly(true)]
        //public string NombreComptageAjoutes
        //{ get; set; }

        //[System.ComponentModel.Description("Nombre de comptages mis à jour"),
        //System.ComponentModel.DefaultValue("0/0"),
        //System.ComponentModel.DisplayName("Nombre de Compatges HTA mis à jour:"),
        //System.ComponentModel.Category("Données HTA"), System.ComponentModel.ReadOnly(true)]
        //public string NombreComptageMAJ
        //{ get; set; }

        //[System.ComponentModel.Description("Nombre de comptages non correctement synchronisés"),
        //System.ComponentModel.DefaultValue("0/0"),
        //System.ComponentModel.DisplayName("Nombre de Comptages HTA en erreur:"),
        //System.ComponentModel.Category("Données HTA"), System.ComponentModel.ReadOnly(true)]
        //public string NombreComptageEnErreur
        //{ get; set; }

        [System.ComponentModel.Description("Nombre de raccordements traités durant la synchro"),
        System.ComponentModel.DefaultValue("0 /0 "),
        System.ComponentModel.DisplayName("Nombre de Raccordements HTA traités:"),
        System.ComponentModel.Category("Données HTA"), System.ComponentModel.ReadOnly(true)]
        public string NombreRaccordementTraites
        { get; set; }

        [System.ComponentModel.Description("Nombre de nouveaux raccordements ajoutés dans la base"),
        System.ComponentModel.DefaultValue("0/0"),
        System.ComponentModel.DisplayName("Nombre de Raccordements HTA Ajoutés:"),
        System.ComponentModel.Category("Données HTA"), System.ComponentModel.ReadOnly(true)]
        public string NombreRaccordementAjoutes
        { get; set; }

        [System.ComponentModel.Description("Nombre de raccordements mis à jour dans la base"),
        System.ComponentModel.DefaultValue("0/0"),
        System.ComponentModel.DisplayName("Nombre de Raccordements HTA mis à jour:"),
        System.ComponentModel.Category("Données HTA"), System.ComponentModel.ReadOnly(true)]
        public string NombreRaccordementMAJ
        { get; set; }

        [System.ComponentModel.Description("Nombre de raccordements non correctement synchronisés"),
        System.ComponentModel.DefaultValue("0/0"),
        System.ComponentModel.DisplayName("Nombre de Raccordements HTA en erreur:"),
        System.ComponentModel.Category("Données HTA"), System.ComponentModel.ReadOnly(true)]
        public string NombreRaccordementsEnErreur
        { get; set; }

        [System.ComponentModel.Description("Nombre d'abonnés HTA traités durant la synchro"),
        System.ComponentModel.DefaultValue("0 /0 "),
        System.ComponentModel.DisplayName("Nombre d'abonnés HTA traités:"),
        System.ComponentModel.Category("Données HTA"), System.ComponentModel.ReadOnly(true)]
        public string NombreAbonnesHTATraites
        { get; set; }

        [System.ComponentModel.Description("Nombre de nouveaux Abonnés ajoutés"),
        System.ComponentModel.DefaultValue("0 /0 "),
        System.ComponentModel.DisplayName("Nombre d'abonnés HTA Ajoutés:"),
        System.ComponentModel.Category("Données HTA"), System.ComponentModel.ReadOnly(true)]
        public string NombreAbonnesHTAAjoutes
        { get; set; }

        [System.ComponentModel.Description("Nombre d'Abonnés dont les infos ont été mises à jour"),
        System.ComponentModel.DefaultValue("0/0"),
        System.ComponentModel.DisplayName("Nombre d'abonnés HTA mis à jour:"),
        System.ComponentModel.Category("Données HTA"), System.ComponentModel.ReadOnly(true)]
        public string NombreAbonnesHTAMAJ
        { get; set; }

        [System.ComponentModel.Description("Nombre d'Abonnés HTA non correctement synchronisés"),
        System.ComponentModel.DefaultValue("0/0"),
        System.ComponentModel.DisplayName("Nombre d'Abonnés HTA en erreur:"),
        System.ComponentModel.Category("Données HTA"), System.ComponentModel.ReadOnly(true)]
        public string NombreAbonnesHTAEnErreur
        { get; set; }

        [System.ComponentModel.Description("Nombre de consommations HTA traitées durant la synchro"),
        System.ComponentModel.DefaultValue("0 /0 "),
        System.ComponentModel.DisplayName("Nombre de consommations HTA traitées:"),
        System.ComponentModel.Category("Données HTA"), System.ComponentModel.ReadOnly(true)]
        public string NombreConsosHTATraites
        { get; set; }

        [System.ComponentModel.Description("Nombre de consommations HTA ajoutées"),
        System.ComponentModel.DefaultValue("0 /0 "),
        System.ComponentModel.DisplayName("Nombre de consommations HTA Ajoutés:"),
        System.ComponentModel.Category("Données HTA"), System.ComponentModel.ReadOnly(true)]
        public string NombreConsosHTAAjoutees
        { get; set; }

        [System.ComponentModel.Description("Nombre de consommations HTA dont les infos ont été mises à jour"),
        System.ComponentModel.DefaultValue("0/0"),
        System.ComponentModel.DisplayName("Nombre de consommations HTA mises à jour:"),
        System.ComponentModel.Category("Données HTA"), System.ComponentModel.ReadOnly(true)]
        public string NombreConsosHTAMAJ
        { get; set; }

        [System.ComponentModel.Description("Nombre de consommations HTA non correctement synchronisés"),
        System.ComponentModel.DefaultValue("0/0"),
        System.ComponentModel.DisplayName("Nombre de consommations HTA en erreur:"),
        System.ComponentModel.Category("Données HTA"), System.ComponentModel.ReadOnly(true)]
        public string NombreConsosHTAEnErreur
        { get; set; }

        #endregion

        #region Données de référence

        [System.ComponentModel.Description("Nombre d'agents traités durant la synchro"),
        System.ComponentModel.DefaultValue("0 /0 "),
        System.ComponentModel.DisplayName("Nombre d'agents traités:"),
        System.ComponentModel.Category("Données de Référence"), System.ComponentModel.ReadOnly(true)]
        public string NombreAgentsTraites
        { get; set; }

        [System.ComponentModel.Description("Nombre de nouveaux agents ajoutés dans la base"),
        System.ComponentModel.DefaultValue("0/0"),
        System.ComponentModel.DisplayName("Nombre d'agents Ajouté(s):"),
        System.ComponentModel.Category("Données de Référence"), System.ComponentModel.ReadOnly(true)]
        public string NombreAgentsAjoutes
        { get; set; }

        [System.ComponentModel.Description("Nombre d'agents mis à jour dans la base"),
        System.ComponentModel.DefaultValue("0 / 0"),
        System.ComponentModel.DisplayName("Nombre d'agents Mis à Jour:"),
        System.ComponentModel.Category("Données de Référence"), System.ComponentModel.ReadOnly(true)]
        public string NombreAgentsMAJ
        { get; set; }

        [System.ComponentModel.Description("Nombre d'agents non correctement synchronisés"),
        System.ComponentModel.DefaultValue("0 / 0"),
        System.ComponentModel.DisplayName("Nombre d'agents en erreur:"),
        System.ComponentModel.Category("Données de Référence"), System.ComponentModel.ReadOnly(true)]
        public string NombreAgentsEnErreur
        { get; set; }

        #endregion

        #region Autres

        [System.ComponentModel.Description("Libellé de l'exploitation pour laquelle à eu lieu la synchronisation"),
        System.ComponentModel.DefaultValue(""),
        System.ComponentModel.DisplayName("Exploitation:"),
        System.ComponentModel.Category("Autres"), System.ComponentModel.ReadOnly(true)]
        public string Exploitation
        { get; set; }

        [System.ComponentModel.Description("Type de synchronisation (Différentielle ou totale)"),
		System.ComponentModel.DefaultValue(""),
		System.ComponentModel.DisplayName("Type de synchronisation:"),
		System.ComponentModel.Category("Autres"), System.ComponentModel.ReadOnly(true)]
		public string TypeSynchronisation
		{ get; set; }

		[System.ComponentModel.Description("Méthode de synchronisation (manuelle ou automatique)"),
		System.ComponentModel.DefaultValue("Manuelle"),
		System.ComponentModel.DisplayName("Méthode de synchronisation:"),
		System.ComponentModel.Category("Autres"), System.ComponentModel.ReadOnly(true)]
		public string MethodeDeSynchronisation
		{ get; set; }

        [System.ComponentModel.Description("Résultat de la synchronisation"),
        System.ComponentModel.DefaultValue(""),
        System.ComponentModel.DisplayName("Résultat traitement:"),
        System.ComponentModel.Category("Autres"), System.ComponentModel.ReadOnly(true)]
        public string ResultatCodeFinDeSynchro
        { get; set; }

        #endregion

        public AlimentationResultat()
		{
       
		}
	}
}
