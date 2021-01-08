using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Galatee.Structure;
using System.Data;
using Galatee.DataAccess;
using System.Threading.Tasks;

namespace WcfService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Parametrage" à la fois dans le code, le fichier svc et le fichier de configuration.
    public class Parametrage : IParametrage
    {
        # region PARAMETRAGE

        public CsLibelle returnLibelle()
        {
            return new CsLibelle();
        }
     

        public EnumProcedureStockee returnEnumProcedureStockee()
        {
            return new EnumProcedureStockee();
        }

        public List<CsLibelle> SelectCoperLibelle100()
        {
            try
            {
                return new DB_DEMCOUT().SelectCoperLibelle100();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }


        public List<CsTdemCout> SelectAllTypeDemCout()
        {
            DataRow row = null;
            try
            {
                DataSet ds = new DB_DEMCOUT().SelectAll_DEMCOUT();

                List<CsTdemCout> listeaTA0 = new List<CsTdemCout>();
                List<CsProduit> produits = new List<CsProduit>();
                List<CsCtax> taxes = new List<CsCtax>();
                List<CsLibelle> copers = new List<CsLibelle>();

                produits.AddRange(SelectAllProducts());
                taxes.AddRange(SelectAll_CTAX());
                copers.AddRange(SelectCoperLibelle100());
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    row = ds.Tables[0].Rows[i];


                    CsTdemCout ta = new CsTdemCout()
                    {
                        ROWID = row["ROWID"] as byte[],
                        CENTRE = row["CENTRE"].ToString(),
                        TDEM = row["TDEM"].ToString(),
                        TRANS = string.IsNullOrEmpty(row["TRANS"].ToString()) ? string.Empty : row["TRANS"].ToString(),
                        PRODUIT = string.IsNullOrEmpty(row["PRODUIT"].ToString()) ? string.Empty : row["PRODUIT"].ToString(),
                        DMAJ = Convert.ToDateTime(row["DMAJ"].ToString()),
                        COPER1 = string.IsNullOrEmpty(row["COPER1"].ToString()) ? string.Empty : row["COPER1"].ToString(),
                        COPER2 = string.IsNullOrEmpty(row["COPER2"].ToString()) ? string.Empty : row["COPER2"].ToString(),
                        COPER3 = string.IsNullOrEmpty(row["COPER3"].ToString()) ? string.Empty : row["COPER3"].ToString(),
                        COPER4 = string.IsNullOrEmpty(row["COPER4"].ToString()) ? string.Empty : row["COPER4"].ToString(),
                        COPER5 = string.IsNullOrEmpty(row["COPER5"].ToString()) ? string.Empty : row["COPER5"].ToString(),
                        TAXE1 = string.IsNullOrEmpty(row["TAXE1"].ToString()) ? string.Empty : row["TAXE1"].ToString(),
                        TAXE2 = string.IsNullOrEmpty(row["TAXE2"].ToString()) ? string.Empty : row["TAXE2"].ToString(),
                        TAXE3 = string.IsNullOrEmpty(row["TAXE3"].ToString()) ? string.Empty : row["TAXE3"].ToString(),
                        TAXE4 = string.IsNullOrEmpty(row["TAXE4"].ToString()) ? string.Empty : row["TAXE4"].ToString(),
                        TAXE5 = string.IsNullOrEmpty(row["TAXE5"].ToString()) ? string.Empty : row["TAXE5"].ToString(),
                        AUTO1 = string.IsNullOrEmpty(row["AUTO1"].ToString()) ? "0" : row["AUTO1"].ToString(),
                        AUTO2 = string.IsNullOrEmpty(row["AUTO2"].ToString()) ? "0" : row["AUTO2"].ToString(),
                        AUTO3 = string.IsNullOrEmpty(row["AUTO3"].ToString()) ? "0" : row["AUTO3"].ToString(),
                        AUTO4 = string.IsNullOrEmpty(row["AUTO4"].ToString()) ? "0" : row["AUTO4"].ToString(),
                        AUTO5 = string.IsNullOrEmpty(row["AUTO5"].ToString()) ? "0" : row["AUTO5"].ToString(),
                        OBLI1 = string.IsNullOrEmpty(row["OBLI1"].ToString()) ? "0" : row["OBLI1"].ToString(),
                        OBLI2 = string.IsNullOrEmpty(row["OBLI2"].ToString()) ? "0" : row["OBLI2"].ToString(),
                        OBLI3 = string.IsNullOrEmpty(row["OBLI3"].ToString()) ? "0" : row["OBLI3"].ToString(),
                        OBLI5 = string.IsNullOrEmpty(row["OBLI5"].ToString()) ? "0" : row["OBLI5"].ToString(),
                        OBLI4 = string.IsNullOrEmpty(row["OBLI4"].ToString()) ? "0" : row["OBLI4"].ToString(),
                        MONTANT1 = string.IsNullOrEmpty(row["MONTANT1"].ToString()) ? 0 : Convert.ToDecimal(row["MONTANT1"].ToString()),
                        MONTANT2 = string.IsNullOrEmpty(row["MONTANT2"].ToString()) ? 0 : Convert.ToDecimal(row["MONTANT2"].ToString()),
                        MONTANT3 = string.IsNullOrEmpty(row["MONTANT3"].ToString()) ? 0 : Convert.ToDecimal(row["MONTANT3"].ToString()),
                        MONTANT4 = string.IsNullOrEmpty(row["MONTANT4"].ToString()) ? 0 : Convert.ToDecimal(row["MONTANT4"].ToString()),
                        MONTANT5 = string.IsNullOrEmpty(row["MONTANT5"].ToString()) ? 0 : Convert.ToDecimal(row["MONTANT5"].ToString())
                    };

                    //string Codeproduit = string.IsNullOrEmpty(row["PRODUIT"].ToString()) ? string.Empty : row["PRODUIT"].ToString();
                    //string CodeCoper1 = string.IsNullOrEmpty(row["COPER1"].ToString()) ? string.Empty : row["COPER1"].ToString();
                    //string CodeCoper2 = string.IsNullOrEmpty(row["COPER2"].ToString()) ? string.Empty : row["COPER2"].ToString();
                    //string CodeCoper3 = string.IsNullOrEmpty(row["COPER3"].ToString()) ? string.Empty : row["COPER3"].ToString();
                    //string CodeCoper4 = string.IsNullOrEmpty(row["COPER4"].ToString()) ? string.Empty : row["COPER4"].ToString();
                    //string CodeCoper5 = string.IsNullOrEmpty(row["COPER5"].ToString()) ? string.Empty : row["COPER5"].ToString();
                    //string CodeTaxe1 = string.IsNullOrEmpty(row["TAXE1"].ToString()) ? string.Empty : row["TAXE1"].ToString();
                    //string CodeTaxe2 = string.IsNullOrEmpty(row["TAXE2"].ToString()) ? string.Empty : row["TAXE2"].ToString();
                    //string CodeTaxe3 = string.IsNullOrEmpty(row["TAXE3"].ToString()) ? string.Empty : row["TAXE3"].ToString();
                    //string CodeTaxe4=  string.IsNullOrEmpty(row["TAXE4"].ToString()) ? string.Empty : row["TAXE4"].ToString();
                    //string CodeTaxe5 = string.IsNullOrEmpty(row["TAXE5"].ToString()) ? string.Empty : row["TAXE5"].ToString();

                    //CsTdemCout ta = new CsTdemCout()
                    //{
                    //    ROWID = row["ROWID"] as byte[],
                    //    CENTRE = row["CENTRE"].ToString(),
                    //    TRANS = string.IsNullOrEmpty(row["TRANS"].ToString()) ? string.Empty : row["TRANS"].ToString(),
                    //    PRODUIT = new CsLibelle() { CODE = Codeproduit, LIBELLE=produits.FirstOrDefault(p => p.CODE == Codeproduit).LIBELLE }, 
                    //    DMAJ = Convert.ToDateTime(row["DMAJ"].ToString()),
                    //    COPER1 = new CsLibelle() { CODE = CodeCoper1, LIBELLE = copers.FirstOrDefault(p => p.CODE == CodeCoper1).LIBELLE },
                    //    COPER2 = new CsLibelle() { CODE = CodeCoper2, LIBELLE = copers.FirstOrDefault(p => p.CODE == CodeCoper2).LIBELLE },
                    //    COPER3 = new CsLibelle() { CODE = CodeCoper3, LIBELLE = copers.FirstOrDefault(p => p.CODE == CodeCoper3).LIBELLE },
                    //    COPER4 = new CsLibelle() { CODE = CodeCoper4, LIBELLE = copers.FirstOrDefault(p => p.CODE == CodeCoper4).LIBELLE },
                    //    COPER5 = new CsLibelle() { CODE = CodeCoper5, LIBELLE = copers.FirstOrDefault(p => p.CODE == CodeCoper5).LIBELLE },
                    //    TAXE1 = new CsLibelle() { CODE = CodeTaxe1, LIBELLE = copers.FirstOrDefault(p => p.CODE == CodeTaxe1).LIBELLE },
                    //    TAXE2 = new CsLibelle() { CODE = CodeTaxe2, LIBELLE = copers.FirstOrDefault(p => p.CODE == CodeTaxe2).LIBELLE },
                    //    TAXE3 = new CsLibelle() { CODE = CodeTaxe3, LIBELLE = copers.FirstOrDefault(p => p.CODE == CodeTaxe3).LIBELLE },
                    //    TAXE4 = new CsLibelle() { CODE = CodeTaxe4, LIBELLE = copers.FirstOrDefault(p => p.CODE == CodeTaxe4).LIBELLE },
                    //    TAXE5 = new CsLibelle() { CODE = CodeTaxe5, LIBELLE = copers.FirstOrDefault(p => p.CODE == CodeTaxe5).LIBELLE },
                    //    AUTO1 = string.IsNullOrEmpty(row["AUTO1"].ToString()) ? "0": row["AUTO1"].ToString(),
                    //    AUTO2 = string.IsNullOrEmpty(row["AUTO2"].ToString()) ? "0" : row["AUTO2"].ToString(),
                    //    AUTO3 = string.IsNullOrEmpty(row["AUTO3"].ToString()) ? "0" : row["AUTO3"].ToString(),
                    //    AUTO4 = string.IsNullOrEmpty(row["AUTO4"].ToString()) ? "0" : row["AUTO4"].ToString(),
                    //    AUTO5 = string.IsNullOrEmpty(row["AUTO5"].ToString()) ? "0" : row["AUTO5"].ToString(),
                    //    OBLI1 = string.IsNullOrEmpty(row["OBLI1"].ToString()) ? "0" : row["OBLI1"].ToString(),
                    //    OBLI2 = string.IsNullOrEmpty(row["OBLI2"].ToString()) ? "0" : row["OBLI2"].ToString(),
                    //    OBLI3 = string.IsNullOrEmpty(row["OBLI3"].ToString()) ? "0" : row["OBLI3"].ToString(),
                    //    OBLI5 = string.IsNullOrEmpty(row["OBLI5"].ToString()) ? "0" : row["OBLI5"].ToString(),
                    //    OBLI4 = string.IsNullOrEmpty(row["OBLI4"].ToString()) ? "0" : row["OBLI4"].ToString(),
                    //    MONTANT1 = string.IsNullOrEmpty(row["MONTANT1"].ToString()) ? 0 : Convert.ToDecimal(row["MONTANT1"].ToString()),
                    //    MONTANT2 = string.IsNullOrEmpty(row["MONTANT2"].ToString()) ? 0 : Convert.ToDecimal(row["MONTANT2"].ToString()),
                    //    MONTANT3 = string.IsNullOrEmpty(row["MONTANT3"].ToString()) ? 0 : Convert.ToDecimal(row["MONTANT3"].ToString()),
                    //    MONTANT4 = string.IsNullOrEmpty(row["MONTANT4"].ToString()) ? 0 : Convert.ToDecimal(row["MONTANT4"].ToString()),
                    //    MONTANT5 = string.IsNullOrEmpty(row["MONTANT5"].ToString()) ? 0 : Convert.ToDecimal(row["MONTANT5"].ToString())
                    //};


                    listeaTA0.Add(ta);

                }

                return listeaTA0;

            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsCtax> SelectAll_CTAX()
        {
            try
            {
                return new DBCTAX().GetAll();
            }
            catch (Exception ec)
            {
                string error = ec.Message;
                return null;
            }
        }

        public List<CsRegrou> SelectAll_REGROU()
        {
            DataRow row = null;
            CsRegrou ta;
            try
            {
                DataSet ds = new DB_REGROU().SelectAll_REGROU();
                List<CsRegrou> listeRegrou = new List<CsRegrou>();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    row = ds.Tables[0].Rows[i];
                    ta = new CsRegrou()
                    {
                        CENTRE = row["CENTRE"].ToString(),
                        CUBDIV = int.Parse(row["CUBDIV"].ToString()),
                        CUBGEN = int.Parse(row["CUBGEN"].ToString()),
                        DMAJ = Convert.ToDateTime(row["DMAJ"]),
                        NOM = row["NOM"].ToString(),
                        PRODUIT = row["PRODUIT"].ToString(),
                        REGROU = row["REGROU"].ToString(),
                        TRANS = row["TRANS"].ToString(),
                        ROWID = row["ROWID"] as byte[]
                    };

                    listeRegrou.Add(ta);
                }
                return listeRegrou;
            }
            catch (Exception ec)
            {
                string error = ec.Message;
                return null;
            }
        }

        public List<CsProduit> SelectAllProducts()
        {
            DataRow row = null;
            CsProduit ta;
            try
            {
                DataSet ds = new DB_INIT().SELECT_Produits();
                List<CsProduit> listeaTA = new List<CsProduit>();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    row = ds.Tables[0].Rows[i];
                    ta = new CsProduit()
                    {
                        CODE = row["code"].ToString(),
                        LIBELLE = row["libelle"].ToString()
                    };

                    listeaTA.Add(ta);
                }
                return listeaTA;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsPuissance> SelectAll_PUISSANCE()
        {
            DataRow row = null;
            CsPuissance ta;
            try
            {
                //DataSet ds = new DB_PUISSANCE().SelectAll_PUISSANCE();
                List<CsPuissance> listeaPuiss = new List<CsPuissance>();

                //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //{
                //    row = ds.Tables[0].Rows[i];
                //    ta = new CsPuissance()
                //    {
                //        //CENTRE = row["CENTRE"].ToString(),
                //        //CODETARIF = row["CODETARIF"].ToString(),
                //        //COMMUNE = row["COMMUNE"].ToString(),
                //        CONSOMMATIONMAXI = int.Parse(string.IsNullOrEmpty(row["CONSOMMATIONMAXI"].ToString()) ? "0" : row["CONSOMMATIONMAXI"].ToString()),
                //        DEBUTAPPLICATION = Convert.ToDateTime(string.IsNullOrEmpty(row["DEBUTAPPLICATION"].ToString()) ? null : row["DEBUTAPPLICATION"].ToString()),
                //        FINAPPLICATION = Convert.ToDateTime(string.IsNullOrEmpty(row["FINAPPLICATION"].ToString()) ? null : row["FINAPPLICATION"].ToString()),
                //        FRAISDERETARD = Convert.ToDecimal(string.IsNullOrEmpty(row["FRAISDERETARD"].ToString()) ? "0" : row["FRAISDERETARD"].ToString()),
                //        //PUISSANCE = Convert.ToDecimal(string.IsNullOrEmpty(row["PUISSANCE"].ToString()) ? "0" : row["PUISSANCE"].ToString()),
                //        //PRODUIT = row["PRODUIT"].ToString(),
                //        DMAJ = Convert.ToDateTime(row["DMAJ"].ToString()),
                //        TRANS = row["TRANS"].ToString(),
                //        ROWID = row["ROWID"] as byte[],
                //    };

                //    listeaPuiss.Add(ta);
                //}
                return listeaPuiss;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public bool insertOrUpdateREGROU(List<CsRegrou> insert, List<CsRegrou> update)
        {
            try
            {
                List<CsRegrou> toBeInserted = new List<CsRegrou>();
                // testons l'unicite des occurences dans la BD
                foreach (CsRegrou t in insert)
                {
                    if (!string.IsNullOrEmpty(t.CENTRE) && !string.IsNullOrEmpty(t.REGROU) && !string.IsNullOrEmpty(t.PRODUIT))
                        if (!new DB_REGROU().Testunicite_REGROU(t.CENTRE, t.REGROU, t.PRODUIT))
                            toBeInserted.Add(t);
                }
                if (toBeInserted.Count > 0)
                    new DB_REGROU().Insertion_REGROU(toBeInserted);

                if (update.Count > 0)
                    new DB_REGROU().MiseAJour_REGROU(update);

                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        public bool insertOrUpdateInit(List<CsInit> insert1, List<CsInit> update1)
        {
            try
            {
                List<CsInit> toBeInserted = new List<CsInit>();
                List<CsInit> toBeUpdated = new List<CsInit>();

                // testons l'unicite des occurences dans la BD
                foreach (CsInit t in insert1)
                {
                    if (!new DB_INIT().Testunicite(t.CENTRE, t.PRODUIT, t.NTABLE, t.ZONE))
                        toBeInserted.Add(t);
                }
                if (toBeInserted.Count > 0)
                    new DB_INIT().Insert(toBeInserted);

                new DB_INIT().Update(update1);

                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        
        public bool insertOrUpdatePUISSANC(List<CsPuissance> rowInsert, List<CsPuissance> rowUpdate)
        {
            try
            {
                List<CsPuissance> toBeInserted = new List<CsPuissance>();
                List<CsPuissance> toBeUpdated = new List<CsPuissance>();

                // testons l'unicite des occurences dans la BD
                foreach (CsPuissance t in rowInsert)
                {
                    //if (!new DB_PUISSANCE().Testunicite_PUISSANCE(t.CENTRE, t.PRODUIT, t.COMMUNE, t.CODETARIF, t.PUISSANCE, t.DEBUTAPPLICATION))
                    //    toBeInserted.Add(t);
                }
                if (toBeInserted.Count > 0)
                    new DB_PUISSANCE().Insertion_PUISSANCE(toBeInserted);

                new DB_PUISSANCE().MiseAJour_PUISSANCE(rowUpdate);

                return true;
            }

            catch (Exception ex)
            {
                string eror = ex.Message;
                return false;
            }

        }
        
        public List<CsTdemCout> insertOrUpdateTypeDemandeCout(List<CsTdemCout> data, List<CsTdemCout> maj)
        {
            try
            {
                List<CsTdemCout> toBeInserted = new List<CsTdemCout>();
                List<CsTdemCout> toBeUpdated = new List<CsTdemCout>();

                // testons l'unicite des occurences dans la BD
                foreach (CsTdemCout demande in data)
                {
                    if (!string.IsNullOrEmpty(demande.CENTRE) && !string.IsNullOrEmpty(demande.TDEM) && !string.IsNullOrEmpty(demande.PRODUIT))
                        if (!new DB_DEMCOUT().Testunicite_DEMCOUT(demande.CENTRE, demande.PRODUIT, demande.TDEM))
                            toBeInserted.Add(demande);
                }

                foreach (CsTdemCout dem in maj)
                {
                    if (dem.ROWID != null)
                        toBeUpdated.Add(dem);
                }

                if (toBeInserted.Count > 0)
                    new DB_DEMCOUT().Insertion_DEMCOUT(toBeInserted);
                if (toBeUpdated.Count > 0)
                    new DB_DEMCOUT().MiseAJour_DEMCOUT(toBeUpdated);

                return this.SelectAllTypeDemCout();
            }

            catch (Exception ex)
            {
                string eror = ex.Message;
                return null;
            }
        }

        public bool Delete_PUISSANC(string Centre, string Produit, string Commune, string CodeTarif, Decimal Puissance, DateTime DebutApplication)
        {
            try
            {
                new DB_PUISSANCE().Delete_PUISSANCE(Centre, Produit, Commune, CodeTarif, Puissance, DebutApplication);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        
        public bool Delete_REGROU(string Centre, string CodeRegroupement, string CodeProduit)
        {
            try
            {
                if (new DB_REGROU().Testunicite_REGROU(Centre, CodeRegroupement, CodeProduit))
                    new DB_REGROU().Delete_REGROU(Centre, CodeRegroupement, CodeProduit);
                return true;
            }
            catch (Exception ex)
            {
                string eror = ex.Message;
                return false;
            }
        }

        
        public bool DeleteINIT(string Centre, string Produit, string Ntable, string Zone)
        {
            try
            {
                if (new DB_INIT().Testunicite(Centre, Produit, Ntable, Zone))
                    new DB_INIT().DeleteINIT(Centre, Produit, Ntable, Zone);
                return true;

            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        
        public bool DeleteDemCouT(string Centre, string produit, string tdem)
        {
            bool status;

            try
            {
                if (new DB_DEMCOUT().Testunicite_DEMCOUT(Centre, produit, tdem))
                {
                    status = new DB_DEMCOUT().Delete_DEMCOUT(Centre, produit, tdem);
                    return status;
                }
                return false;

            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        #region Table DENOMINATION

        public List<CsDenomination> SelectAllDenomination()
        {
            try
            {
                return new DB_Denomination().SelectAllDenomination();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
                //throw ex;
            }
        }

        public bool DeleteDenomination(CsDenomination pDenomination)
        {
            try
            {
                return new DB_Denomination().Delete(pDenomination);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateDenomination(List<CsDenomination> pDenominationCollection)
        {
            try
            {
                return new DB_Denomination().Update(pDenominationCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertDenomination(List<CsDenomination> pDenominationCollection)
        {
            try
            {
                return new DB_Denomination().Insert(pDenominationCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeDenomination(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsDenomination = SelectAllDenomination();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsDenomination);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        #endregion

        #region Table PRODUIT

        public List<CsProduit> SelectAllProduit()
        {
            try
            {
                return new DB_Produit().SelectAllProduit();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteProduit(CsProduit pProduit)
        {
            try
            {
                return new DB_Produit().Delete(pProduit);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateProduit(List<CsProduit> pProduitCollection)
        {
            try
            {
                return new DB_Produit().Update(pProduitCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertProduit(List<CsProduit> pProduitCollection)
        {
            try
            {
                return new DB_Produit().Insert(pProduitCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeProduit(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                List<CsProduit> lsObject = SelectAllProduit();
                List<CsPrint> listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                Printings.PrintingsService printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        #endregion

        public List<CsTypeBranchement> SelectAllCategorieBranchement()
        {
            try
            {
                return new DB_CategorieBranchement().SelectAllTypeBranchement();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteCategorieBranchement(CsTypeBranchement pCategorieBranchement)
        {
            try
            {
                return new DB_CategorieBranchement().Delete(pCategorieBranchement);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateCategorieBranchement(List<CsTypeBranchement> pCategorieBranchementCollection)
        {
            try
            {
                return new DB_CategorieBranchement().Update(pCategorieBranchementCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertCategorieBranchement(List<CsTypeBranchement> pCategorieBranchementCollection)
        {
            try
            {
                return new DB_CategorieBranchement().Insert(pCategorieBranchementCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeCategorieBranchement(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                List<CsTypeBranchement> lsObject = SelectAllCategorieBranchement();
                List<CsPrint> listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                Printings.PrintingsService printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CsCategorieClient> SelectAllCategorieClient()
        {
            try
            {
                return new DB_CategorieClient().SelectAllCategorieClient();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteCategorieClient(CsCategorieClient pCategorieClient)
        {
            try
            {
                return new DB_CategorieClient().Delete(pCategorieClient);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateCategorieClient(List<CsCategorieClient> pCategorieClientCollection)
        {
            try
            {

                return new DB_CategorieClient().Update(pCategorieClientCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertCategorieClient(List<CsCategorieClient> pCategorieClientCollection)
        {
            try
            {
                return new DB_CategorieClient().Insert(pCategorieClientCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeCategorieClient(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                List<CsCategorieClient> lsObject = SelectAllCategorieClient();
                List<CsPrint> listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                Printings.PrintingsService printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CsConsommateur> SelectAllConsommateur()
        {
            try
            {
                return new DB_Consommateur().SelectAllConsommateur();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteConsommateur(CsConsommateur pConsommateur)
        {
            try
            {
                return new DB_Consommateur().Delete(pConsommateur);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateConsommateur(List<CsConsommateur> pConsommateurCollection)
        {
            try
            {
                return new DB_Consommateur().Update(pConsommateurCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertConsommateur(List<CsConsommateur> pConsommateurCollection)
        {
            try
            {
                return new DB_Consommateur().Insert(pConsommateurCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeConsommateur(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = SelectAllConsommateur();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CsParametresGeneraux> SelectAllParametresGeneraux()
        {
            try
            {
                return new DB_ParametresGeneraux().SelectAllParametresGeneraux();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteParametresGeneraux(CsParametresGeneraux pParametresGeneraux)
        {
            try
            {
                return new DB_ParametresGeneraux().Delete(pParametresGeneraux);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateParametresGeneraux(List<CsParametresGeneraux> pParametresGenerauxCollection)
        {
            try
            {
                return new DB_ParametresGeneraux().Update(pParametresGenerauxCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertParametresGeneraux(List<CsParametresGeneraux> pParametresGenerauxCollection)
        {
            try
            {
                return new DB_ParametresGeneraux().Insert(pParametresGenerauxCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeParametresGeneraux(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = SelectAllParametresGeneraux();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CsParametreEvenement> SelectAllParametreEvenement()
        {
            try
            {
                return new DB_ParametreEvenement().SelectAllParametresEvenement();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteParametreEvenement(CsParametreEvenement pParametreEvenement)
        {
            try
            {
                return new DB_ParametreEvenement().Delete(pParametreEvenement);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateParametreEvenement(List<CsParametreEvenement> pParametreEvenementCollection)
        {
            try
            {
                return new DB_ParametreEvenement().Update(pParametreEvenementCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertParametreEvenement(List<CsParametreEvenement> pParametreEvenementCollection)
        {
            try
            {
                return new DB_ParametreEvenement().Insert(pParametreEvenementCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeParametreEvenement(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = SelectAllParametreEvenement();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CsProprietaire> SelectAllProprietaire()
        {
            try
            {
                return new DB_Proprietaire().SelectAllProprietaire();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteProprietaire(CsProprietaire pProprietaire)
        {
            try
            {
                return new DB_Proprietaire().Delete(pProprietaire);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateProprietaire(List<CsProprietaire> pProprietaireCollection)
        {
            try
            {
                return new DB_Proprietaire().Update(pProprietaireCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertProprietaire(List<CsProprietaire> pProprietaireCollection)
        {
            try
            {
                return new DB_Proprietaire().Insert(pProprietaireCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeProprietaire(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = SelectAllProprietaire();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CsNationalite> SelectAllNationalite()
        {
            try
            {
                return new DB_Nationalite().SelectAllNationalite();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteNationalite(CsNationalite pNationalite)
        {
            try
            {
                return new DB_Nationalite().Delete(pNationalite);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateNationalite(List<CsNationalite> pNationaliteCollection)
        {
            try
            {
                return new DB_Nationalite().Update(pNationaliteCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertNationalite(List<CsNationalite> pNationaliteCollection)
        {
            try
            {
                return new DB_Nationalite().Insert(pNationaliteCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeNationalite(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = SelectAllNationalite();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CsCelluleDuSiege> SelectAllCelluleDuSiege()
        {
            try
            {
                return new DB_CelluleDuSiege().SelectAllCelluleDuSiege();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteCelluleDuSiege(CsCelluleDuSiege pCelluleDuSiege)
        {
            try
            {
                return new DB_CelluleDuSiege().Delete(pCelluleDuSiege);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateCelluleDuSiege(List<CsCelluleDuSiege> pCelluleDuSiegeCollection)
        {
            try
            {
                return new DB_CelluleDuSiege().Update(pCelluleDuSiegeCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertCelluleDuSiege(List<CsCelluleDuSiege> pCelluleDuSiegeCollection)
        {
            try
            {
                return new DB_CelluleDuSiege().Insert(pCelluleDuSiegeCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeCelluleDuSiege(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = SelectAllCelluleDuSiege();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }


        #region DEPART HTA


        public List<CsCodeDepart> SelectAllCodeDepart()
        {
            try
            {
                return new DB_CodeDepart().SelectAllCodeDepart();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteCodeDepart(CsCodeDepart pCodeDepart)
        {
            try
            {
                return new DB_CodeDepart().Delete(pCodeDepart);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateCodeDepart(List<CsCodeDepart> pCodeDepartCollection)
        {
            try
            {
                return new DB_CodeDepart().Update(pCodeDepartCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertCodeDepart(List<CsCodeDepart> pCodeDepartCollection)
        {
            try
            {
                return new DB_CodeDepart().Insert(pCodeDepartCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeCodeDepart(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = SelectAllCodeDepart();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        #endregion   


        #region POSTE DE TRANSFORMATION

        public List<CsPosteTransformation> SelectAllPosteTransformation()
        {
            try
            {
                return new DB_PosteTransformation().SelectAllPosteTransformation();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeletePosteTransformation(CsPosteTransformation pCodePoste)
        {
            try
            {
                return new DB_PosteTransformation().Delete(pCodePoste);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdatePosteTransformation(List<CsPosteTransformation> pCodePosteCollection)
        {
            try
            {
                return new DB_PosteTransformation().Update(pCodePosteCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertPosteTransformation(List<CsPosteTransformation> pCodePosteCollection)
        {
            try
            {
                return new DB_PosteTransformation().Insert(pCodePosteCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListePosteTransformation(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = SelectAllPosteTransformation();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        #endregion   
    



        #region POSTE SOURCE

        public List<CsPosteSource> SelectAllPosteSource()
        {
            try
            {
                return new DB_PosteSource().SelectAllPosteSource();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeletePosteSource(CsPosteSource pPosteSource)
        {
            try
            {
                return new DB_PosteSource().Delete(pPosteSource);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdatePosteSource(List<CsPosteSource> pPosteSourceCollection)
        {
            try
            {
                return new DB_PosteSource().Update(pPosteSourceCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertPosteSource(List<CsPosteSource> pPosteSourceCollection)
        {
            try
            {
                return new DB_PosteSource().Insert(pPosteSourceCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListePosteSource(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = SelectAllPosteSource();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        #endregion   
 


        
        public List<CsLibelleTop> SelectAllLibelleTop()
        {
            try
            {
                return new DB_LibelleTop().SelectAllLibelleTop();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteLibelleTop(CsLibelleTop pLibelleTop)
        {
            try
            {
                return new DB_LibelleTop().Delete(pLibelleTop);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateLibelleTop(List<CsLibelleTop> pLibelleTopCollection)
        {
            try
            {
                return new DB_LibelleTop().Update(pLibelleTopCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertLibelleTop(List<CsLibelleTop> pLibelleTopCollection)
        {
            try
            {
                return new DB_LibelleTop().Insert(pLibelleTopCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeLibelleTop(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = SelectAllLibelleTop();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CsNumeroInstallation> SelectAllNumeroInstallation()
        {
            try
            {
                return new DB_NumeroInstallation().SelectAllNumeroInstallation();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteNumeroInstallation(CsNumeroInstallation pNumeroInstallation)
        {
            try
            {
                return new DB_NumeroInstallation().Delete(pNumeroInstallation);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateNumeroInstallation(List<CsNumeroInstallation> pNumeroInstallationCollection)
        {
            try
            {
                return new DB_NumeroInstallation().Update(pNumeroInstallationCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertNumeroInstallation(List<CsNumeroInstallation> pNumeroInstallationCollection)
        {
            try
            {
                return new DB_NumeroInstallation().Insert(pNumeroInstallationCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeNumeroInstallation(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = SelectAllNumeroInstallation();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<aBanque> SelectAllBanque()
        {
            try
            {
                return new DB_BANQUES().SelectAllBanque();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteBanque(aBanque pBanque)
        {
            try
            {
                return new DB_BANQUES().Delete(pBanque);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateBanque(List<aBanque> pBanqueCollection)
        {
            try
            {
                return new DB_BANQUES().Update(pBanqueCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertBanque(List<aBanque> pBanqueCollection)
        {
            try
            {
                return new DB_BANQUES().Insert(pBanqueCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeBanque(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = SelectAllBanque();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CsDomBanc> SelectAllDomBanc()
        {
            try
            {
                return new DB_DOMBANC().SelectAllDomBanc();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteDomBanc(CsDomBanc pDomBanc)
        {
            try
            {
                return new DB_DOMBANC().Delete(pDomBanc);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateDomBanc(List<CsDomBanc> pDomBancCollection)
        {
            try
            {
                return new DB_DOMBANC().Update(pDomBancCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertDomBanc(List<CsDomBanc> pDomBancCollection)
        {
            try
            {
                return new DB_DOMBANC().Insert(pDomBancCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeDomBanc(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = SelectAllDomBanc();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CsParamAbonUtilise> SelectAllParamAbonUtilise()
        {
            try
            {
                return new DB_PARAMABONUTILISE().SelectAllParamAbonUtilise();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteParamAbonUtlise(CsParamAbonUtilise pParamAbonUtilise)
        {
            try
            {
                return new DB_PARAMABONUTILISE().Delete(pParamAbonUtilise);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateParamAbonUtlise(List<CsParamAbonUtilise> pParamAbonUtiliseCollection)
        {
            try
            {
                return new DB_PARAMABONUTILISE().Update(pParamAbonUtiliseCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertParamAbonUtlise(List<CsParamAbonUtilise> pParamAbonUtiliseCollection)
        {
            try
            {
                return new DB_PARAMABONUTILISE().Insert(pParamAbonUtiliseCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeParamAbonUtlise(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = SelectAllParamAbonUtilise();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CsRegCli> SelectAllRegCli()
        {
            try
            {
                return new DB_REGCLI().SelectAllRegCli();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteRegCli(CsRegCli pRegCli)
        {
            try
            {
                return new DB_REGCLI().Delete(pRegCli);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateRegCli(List<CsRegCli> pRegCliCollection)
        {
            try
            {
                return new DB_REGCLI().Update(pRegCliCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertRegCli(List<CsRegCli> pRegCliCollection)
        {
            try
            {
                return new DB_REGCLI().Insert(pRegCliCollection);
            }
            //catch (FaultException<Errors> e)
            //{
            //    throw e;
            //}
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeRegCli(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = SelectAllRegCli();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CsRegExo> SelectAllRegExo()
        {
            try
            {
                return new DB_REGEXO().SelectAllRegExo();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteRegExo(CsRegExo pRegExo)
        {
            try
            {
                return new DB_REGEXO().Delete(pRegExo);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateRegExo(List<CsRegExo> pRegExoCollection)
        {
            try
            {
                return new DB_REGEXO().Update(pRegExoCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertRegExo(List<CsRegExo> pRegExoCollection)
        {
            try
            {
                return new DB_REGEXO().Insert(pRegExoCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeRegExo(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = SelectAllRegExo();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CsCentre> SelectAllCentre()
        {
            try
            {
                return new DB_Centre().SelectAllCentre();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsCentre> SelectCentreByCodeSite(int pCodeSite)
        {
            try
            {
                return new DB_Centre().SelectCentreBySiteId(pCodeSite);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteCentre(CsCentre pCentre)
        {
            try
            {
                return new DB_Centre().Delete(pCentre);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateCentre(List<CsCentre> pCentreCollection)
        {
            try
            {
                return new DB_Centre().Update(pCentreCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertCentre(List<CsCentre> pCentreCollection)
        {
            try
            {
                return new DB_Centre().Insert(pCentreCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeCentre(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = SelectAllCentre();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CsTypeCentre> SelectAllTypeCentre()
        {
            try
            {
                return new DB_TypeCentre().SelectAllTypeCentre();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteTypeCentre(CsTypeCentre pTypeCentre)
        {
            try
            {
                return new DB_TypeCentre().Delete(pTypeCentre);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateTypeCentre(List<CsTypeCentre> pTypeCentreCollection)
        {
            try
            {
                return new DB_TypeCentre().Update(pTypeCentreCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertTypeCentre(List<CsTypeCentre> pTypeCentreCollection)
        {
            try
            {
                return new DB_TypeCentre().Insert(pTypeCentreCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeTypeCentre(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = SelectAllTypeCentre();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CsSite> SelectAllSites()
        {
            try
            {
                return new DB_Site().SelectAllSite();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteSite(CsSite pSite)
        {
            try
            {
                return new DB_Site().Delete(pSite);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateSite(List<CsSite> pSiteCollection)
        {
            try
            {
                return new DB_Site().Update(pSiteCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertSite(List<CsSite> pSiteCollection)
        {
            try
            {
                return new DB_Site().Insert(pSiteCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeSite(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = SelectAllSites();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public ObjTYPEDEVIS GetTypeDevisById(int id)
        {
            try
            {
                return DBTYPEDEVIS.GetById(id);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<ObjTYPEDEVIS> GetTypeDevisByIdProduit(int pIdProduit)
        {
            try
            {
                return DBTYPEDEVIS.GetByProduitId(pIdProduit);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<ObjTYPEDEVIS> GetAllTypeDevis()
        {
            try
            {
                return DBTYPEDEVIS.GetAll();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }


        public bool EditerTypeDevis(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = GetAllTypeDevis();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public ObjTACHEDEVIS GetTacheDevisById(int id)
        {
            try
            {
                return DBTACHEDEVIS.GetById(id);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<ObjTACHEDEVIS> GetAllTacheDevis()
        {
            try
            {
                return DBTACHEDEVIS.GetAll();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }


        public bool EditerTacheDevis(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = GetAllTacheDevis();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public ObjPIECEIDENTITE GetPieceIdentiteById(byte id)
        {
            try
            {
                return DBPIECEIDENTITE.GetById(id);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<ObjPIECEIDENTITE> GetAllPieceIdentite()
        {
            try
            {
                return DBPIECEIDENTITE.GetAll();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool Delete(List<ObjPIECEIDENTITE> pPieceIdentitesCollection)
        {
            try
            {
                return new DBPIECEIDENTITE().Delete(pPieceIdentitesCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool Update(List<ObjPIECEIDENTITE> pPieceIdentitesCollection)
        {
            try
            {
                return new DBPIECEIDENTITE().Update(pPieceIdentitesCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool Insert(List<ObjPIECEIDENTITE> pPieceIdentiteCollection)
        {
            try
            {
                return new DBPIECEIDENTITE().Insert(pPieceIdentiteCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerPieceIdentite(string key, Dictionary<string, string> pDictionary)
        {
            throw new NotImplementedException();
        }

        public ObjETAPEDEVIS GetEtapeDevisById(int id)
        {
            try
            {
                return DBETAPEDEVIS.GetById(id);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<ObjETAPEDEVIS> GetEtapeDevisForDisplay()
        {
            try
            {
                return DBETAPEDEVIS.GetEtapeDevisForDisplay();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<ObjETAPEDEVIS> GetEtapeDevisByCodeProduit(int codeProduit)
        {
            try
            {
                return DBETAPEDEVIS.GetByCodeProduit(codeProduit);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        //public List<ObjETAPEDEVIS> GetEtapeDevisByCodeFonction(string codeFonction)
        //{
        //    try
        //    {
        //        return DBETAPEDEVIS.GetByCodeFonction(codeFonction);
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogException(this, ex);
        //        return null;
        //    }
        //}

        public List<ObjETAPEDEVIS> GetEtapeDevisByIdTypeDevis(int idTypeDevis)
        {
            try
            {
                return DBETAPEDEVIS.GetByIdTypeDevis(idTypeDevis);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<ObjETAPEDEVIS> GetEtapeDevisByIdTacheDevis(int idTacheDevis)
        {
            try
            {
                return DBETAPEDEVIS.GetByIdTacheDevis(idTacheDevis);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<ObjETAPEDEVIS> GetEtapeDevisByIdTacheSuivante(int idTacheSuivante)
        {
            try
            {
                return DBETAPEDEVIS.GetByIdTacheSuivante(idTacheSuivante);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<ObjETAPEDEVIS> GetEtapeDevisByIdTacheRejet(int idTacheRejet)
        {
            try
            {
                return DBETAPEDEVIS.GetByIdTacheRejet(idTacheRejet);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<ObjETAPEDEVIS> GetEtapeDevisByIdTacheSaut(int idTacheSaut)
        {
            try
            {
                return DBETAPEDEVIS.GetByIdTacheSaut(idTacheSaut);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<ObjETAPEDEVIS> GetEtapeDevisByIdTacheIntermediaire(int idTacheIntermediaire)
        {
            try
            {
                return DBETAPEDEVIS.GetByIdTacheIntermediaire(idTacheIntermediaire);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public ObjETAPEDEVIS GetEtapeDevisByIdTypeDevisCodeProduitIdTache(int idTypeDevis, int IdProduit, int idTacheCourante)
        {
            try
            {
                return DBETAPEDEVIS.GetByIdTypeDevisIdProduitIdTache(idTypeDevis, IdProduit, idTacheCourante);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public ObjETAPEDEVIS GetEtapeDevisByIdTypeDevisNumEtape(int idTypeDevis, int numEtape)
        {
            try
            {
                return DBETAPEDEVIS.GetByIdTypeDevisNumEtape(idTypeDevis, numEtape);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        //public bool DeleteEtapeDevis(List<ObjETAPEDEVIS> pEtapeDevisCollection)
        //{
        //    try
        //    {
        //        return new DBETAPEDEVIS().Delete(pEtapeDevisCollection);
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogException(this, ex);
        //        return false;
        //    }
        //}





        public bool EditerListeEtapeDevis(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = GetEtapeDevisForDisplay();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CsFonction> SelectAllFonction()
        {
            try
            {
                return new DBFonction().SelectAllFonction();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteFonction(CsFonction pFonction)
        {
            try
            {
                return new DBFonction().Delete(pFonction);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateFonction(List<CsFonction> pFonctionCollection)
        {
            try
            {
                return new DBFonction().Update(pFonctionCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertFonction(List<CsFonction> pFonctionCollection)
        {
            try
            {
                return new DBFonction().Insert(pFonctionCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeFonction(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = SelectAllFonction();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CsCommune> SelectAllCommune()
        {
            try
            {
                return new DB_Commune().SelectAllCommune();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteCommune(CsCommune pCommune)
        {
            try
            {
                return new DB_Commune().Delete(pCommune);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateCommune(List<CsCommune> pCommuneCollection)
        {
            try
            {
                return new DB_Commune().Update(pCommuneCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertCommune(List<CsCommune> pCommuneCollection)
        {
            try
            {
                return new DB_Commune().Insert(pCommuneCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeCommune(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = SelectAllCommune();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CsQuartier> SelectAllQuartier()
        {
            try
            {
                return new DB_QUARTIER().SelectAllQuartier();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteQuartier(CsQuartier pQuartier)
        {
            try
            {
                return new DB_QUARTIER().Delete(pQuartier);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateQuartier(List<CsQuartier> pQuartierCollection)
        {
            try
            {
                return new DB_QUARTIER().Update(pQuartierCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertQuartier(List<CsQuartier> pQuartierCollection)
        {
            try
            {
                return new DB_QUARTIER().Insert(pQuartierCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeQuartier(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = SelectAllQuartier();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CsRues> SelectAllRues()
        {
            try
            {
                return new DB_RUES().SelectAllRues();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteRues(CsRues pRues)
        {
            try
            {
                return new DB_RUES().Delete(pRues);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateRues(List<CsRues> pRuesCollection)
        {
            try
            {
                return new DB_RUES().Update(pRuesCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertRues(List<CsRues> pRuesCollection)
        {
            try
            {
                return new DB_RUES().Insert(pRuesCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeRues(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = SelectAllRues();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        #region REGLAGECOMPTEUR

        public List<CsReglageCompteur > SelectAllReglageCompteur()
        {
            try
            {
                return new DB_REGLAGECOMPTEUR().SelectAllReglageCompteur  ();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteDiacomp(CsReglageCompteur  pDiacomp)
        {
            try
            {
                return new DB_REGLAGECOMPTEUR().Delete(pDiacomp);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateDiacomp(List<CsReglageCompteur> pDiacompCollection)
        {
            try
            {
                return new DB_REGLAGECOMPTEUR().Update(pDiacompCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertDiacomp(List<CsReglageCompteur> pDiacompCollection)
        {
            try
            {
                return new DB_REGLAGECOMPTEUR().Insert(pDiacompCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

    

        public List<CsReglageCompteur> SelectAllReglageCompteurByCentre(int pCentre)
        {
            try
            {
                return new DB_REGLAGECOMPTEUR().SelectAllReglageCompteurByCentre (pCentre);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsReglageCompteur> SelectAllReglageCompteurByCentreIdProduitId(int pCentreId, int pProduitId)
        {
            try
            {
                return new DB_REGLAGECOMPTEUR().SelectAllReglageCompteurByCentreIdProduitId (pCentreId, pProduitId);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsReglageCompteur> SelectAllReglageCompteurByProduit(int pProduit)
        {
            try
            {
                return new DB_REGLAGECOMPTEUR().SelectAllReglageCompteurByProduit (pProduit);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        #endregion

        #region CTAX

        public bool DeleteCtax(List<CsCtax> entityCollection)
        {
            try
            {
                return new DBCTAX().Delete(entityCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CsCtax> GetAllCtax()
        {
            try
            {
                return new DBCTAX().GetAll();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public CsCtax GetCtaxByCENTRECTAXDEBUTAPPLICATION(CsCtax entity)
        {
            try
            {
                return new DBCTAX().GetByCENTRECTAXDEBUTAPPLICATION(entity);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool InsertCtax(List<CsCtax> entityCollection)
        {
            try
            {
                return new DBCTAX().Insert(entityCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateCtax(List<CsCtax> entityCollection)
        {
            try
            {
                return new DBCTAX().Update(entityCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        #endregion
        #region TYPETAXE
        public bool DeleteTypetax(List<CsTypeTaxe> entityCollection)
        {
            try
            {
                return new DBTYPEDETAXE().Delete(entityCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        public List<CsTypeTaxe> GetAllTypeTaxe()
        {
            try
            {
                return new DBTYPEDETAXE().GetAll();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public bool InsertListeTypeTaxe(List<CsTypeTaxe> entityCollection)
        {
            try
            {
                return new DBTYPEDETAXE().Insert(entityCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        public bool UpdateLitsTypeTaxe(List<CsTypeTaxe> entityCollection)
        {
            try
            {
                return new DBTYPEDETAXE().Update(entityCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertTypeTaxe(CsTypeTaxe entityCollection)
        {
            try
            {
                return new DBTYPEDETAXE().Insert(entityCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        public bool UpdateTypeTaxe(CsTypeTaxe entityCollection)
        {
            try
            {
                return new DBTYPEDETAXE().Update(entityCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        #endregion
        #region TYPECOMPTAGE
        public bool DeleteTypeComptage(List<CsTypeComptage> entityCollection)
        {
            try
            {
                return new DBTYPECOMPTAGE().Delete(entityCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        public List<CsTypeComptage> GetAllTypeComptage()
        {
            try
            {
                return new DBTYPECOMPTAGE().GetAll();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public bool InsertListeTypeComptage(List<CsTypeComptage> entityCollection)
        {
            try
            {
                return new DBTYPECOMPTAGE().Insert(entityCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        public bool UpdateLitsTypeComptage(List<CsTypeComptage> entityCollection)
        {
            try
            {
                return new DBTYPECOMPTAGE().Update(entityCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertTypeComptage(CsTypeComptage entityCollection)
        {
            try
            {
                return new DBTYPECOMPTAGE().Insert(entityCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        public bool UpdateTypeComptage(CsTypeComptage entityCollection)
        {
            try
            {
                return new DBTYPECOMPTAGE().Update(entityCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }


        public bool EditerListeComptage(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = GetAllTypeComptage();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        #endregion
        #endregion

        #region MARQUECOMPTEUR

        public List<CsMarqueCompteur> SelectAllMarqueCompteur()
        {
            try
            {
                return new DBMarqueCompteur().SelectAllMarqueCompteur();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteMarqueCompteur(CsMarqueCompteur pMarqueCompteur)
        {
            try
            {
                return new DBMarqueCompteur().Delete(pMarqueCompteur);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateMarqueCompteur(List<CsMarqueCompteur> pMarqueCompteurCollection)
        {
            try
            {
                return new DBMarqueCompteur().Update(pMarqueCompteurCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertMarqueCompteur(List<CsMarqueCompteur> pMarqueCompteurCollection)
        {
            try
            {
                return new DBMarqueCompteur().Insert(pMarqueCompteurCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeMarqueCompteur(string key, Dictionary<string, string> pDictionary)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region GRC_GROUPE

        public List<CsGroupe> SelectAllGRCGroupe()
        {
            try
            {
                return new DBAccueil().SelectAllGRCGroupe();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteGRCGroupe(CsGroupe pGroupe)
        {
            try
            {
                return new DBAccueil().DeleteGRCGroupe(pGroupe);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateGRCGroupe(List<CsGroupe> pGroupeCollection)
        {
            try
            {
                return new DBAccueil().UpdateGRCGroupe(pGroupeCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertGRCGroupe(List<CsGroupe> pGroupeCollection)
        {
            try
            {
                return new DBAccueil().InsertGRCGroupe(pGroupeCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeGroupe(string key, Dictionary<string, string> pDictionary)
        {
            throw new NotImplementedException();
        }

        #endregion



        #region TCOMPT

        public List<CsTcompteur> SelectAllTcompt()
        {
            try
            {
                return new DB_TCOMPT().SelectAllTcompt();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteTcompt(CsTcompteur pTcompt)
        {
            try
            {
                return new DB_TCOMPT().Delete(pTcompt);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateTcompt(List<CsTcompteur> pTcomptCollection)
        {
            try
            {
                return new DB_TCOMPT().Update(pTcomptCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertTcompt(List<CsTcompteur> pTcomptCollection)
        {
            try
            {
                return new DB_TCOMPT().Insert(pTcomptCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeTcompt(string key, Dictionary<string, string> pDictionary)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region AdmMenus

        public List<CSMenuGalatee> GetAdmMenus()
        {
            try
            {
                return DBETAPEDEVIS.GetAdmMenus();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        #endregion

        #region CASIND

        public bool DeleteCasind(List<CsCasind> entityCollection)
        {
            try
            {
                return new DB_CASIND().Delete(entityCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CsCasind> GetAllCasind()
        {
            try
            {
                return new DB_CASIND().SelectAllCasDeReleve();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsCasind> GetCasindByCentreCas(CsCasind entity)
        {
            try
            {
                return new DB_CASIND().SelectAllCasDeReleveByCentreCas(entity);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsCasind> GetCasindEcrasableByCas()
        {
            try
            {
                return new DB_CASIND().SelectAllCasDeReleveEcrasable();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool InsertCasind(List<CsCasind> entityCollection)
        {
            try
            {
                return new DB_CASIND().Insert(entityCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateCasind(List<CsCasind> entityCollection)
        {
            try
            {
                return new DB_CASIND().Update(entityCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeCasind(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = GetAllCasind();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        #endregion

        #region ENTREPRISE

        public bool DeleteEntreprise(CsEntreprise entity)
        {
            try
            {
                return new DBENTREPRISE().Delete(entity);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public CsEntreprise GetAllEntreprise()
        {
            try
            {
                return new DBENTREPRISE().GetAll();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public CsEntreprise GetEntrepriseById(CsEntreprise entity)
        {
            try
            {
                return new DBENTREPRISE().GetById(entity);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool InsertEntreprise(CsEntreprise entity)
        {
            try
            {
                return new DBENTREPRISE().Insert(entity);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateEntreprise(CsEntreprise entity)
        {
            try
            {
                return new DBENTREPRISE().Update(entity);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        #endregion

        #region PERIODICITEFACTURATION

        public List<CsPeriodiciteFacturation> SelectAllPeriodiciteFacturation()
        {
            try
            {
                return new DBPERIODICITEFACTURATION().SelectAllPeriodeFacturation();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeletePeriodiciteFacturation(CsPeriodiciteFacturation pPeriodiciteFacturation)
        {
            try
            {
                return new DBPERIODICITEFACTURATION().Delete(pPeriodiciteFacturation);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdatePeriodiciteFacturation(List<CsPeriodiciteFacturation> pPeriodiciteFacturationCollection)
        {
            try
            {
                return new DBPERIODICITEFACTURATION().Update(pPeriodiciteFacturationCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertPeriodiciteFacturation(List<CsPeriodiciteFacturation> pPeriodiciteFacturationCollection)
        {
            try
            {
                return new DBPERIODICITEFACTURATION().Insert(pPeriodiciteFacturationCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListePeriodiciteFacturation(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = SelectAllPeriodiciteFacturation();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        #endregion

        #region RECHERCHETARIF

        public List<CsRechercheTarif> SelectAllRechercheTarif()
        {
            try
            {
                return new DBRECHERCHETARIF().SelectAllRechercheTarif();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteRechercheTarif(CsRechercheTarif pPeriodeFacturation)
        {
            try
            {
                return new DBRECHERCHETARIF().Delete(pPeriodeFacturation);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateRechercheTarif(List<CsRechercheTarif> pPeriodeFacturationCollection)
        {
            try
            {
                return new DBRECHERCHETARIF().Update(pPeriodeFacturationCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertRechercheTarif(List<CsRechercheTarif> pPeriodeFacturationCollection)
        {
            try
            {
                return new DBRECHERCHETARIF().Insert(pPeriodeFacturationCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeRechercheTarif(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = SelectAllRechercheTarif();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        #endregion

        #region MODECALCUL

        public List<CsModeCalcul> SelectAllModeCalcul()
        {
            try
            {
                return new DBMODECALCUL().SelectAllModeCalcul();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteModeCalcul(CsModeCalcul pModeCalcul)
        {
            try
            {
                return new DBMODECALCUL().Delete(pModeCalcul);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateModeCalcul(List<CsModeCalcul> pModeCalculCollection)
        {
            try
            {
                return new DBMODECALCUL().Update(pModeCalculCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertModeCalcul(List<CsModeCalcul> pModeCalculCollection)
        {
            try
            {
                return new DBMODECALCUL().Insert(pModeCalculCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeModeCalcul(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = SelectAllModeCalcul();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        #endregion

        #region NIVEAUTARIF

        public List<CsNiveauTarif> SelectAllNiveauTarif()
        {
            try
            {
                return new DBNIVEAUTARIF().SelectAllNiveauTarif();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteNiveauTarif(CsNiveauTarif pNiveauTarif)
        {
            try
            {
                return new DBNIVEAUTARIF().Delete(pNiveauTarif);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateNiveauTarif(List<CsNiveauTarif> pNiveauTarifCollection)
        {
            try
            {
                return new DBNIVEAUTARIF().Update(pNiveauTarifCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertNiveauTarif(List<CsNiveauTarif> pNiveauTarifCollection)
        {
            try
            {
                return new DBNIVEAUTARIF().Insert(pNiveauTarifCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeNiveauTarif(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = SelectAllNiveauTarif();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        #endregion

        #region UNITECOMPTAGE

        public List<CsUniteComptage> SelectAllUniteComptage()
        {
            try
            {
                return new DBUNITECOMPTAGE().SelectAllUniteComptage();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteUniteComptage(CsUniteComptage pUniteComptage)
        {
            try
            {
                return new DBUNITECOMPTAGE().Delete(pUniteComptage);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateUniteComptage(List<CsUniteComptage> pUniteComptageCollection)
        {
            try
            {
                return new DBUNITECOMPTAGE().Update(pUniteComptageCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertUniteComptage(List<CsUniteComptage> pUniteComptageCollection)
        {
            try
            {
                return new DBUNITECOMPTAGE().Insert(pUniteComptageCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeUniteComptage(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = SelectAllUniteComptage();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        #endregion

        #region MOIS

        public List<CsMois> SelectAllMois()
        {
            try
            {
                return new DBMOIS().SelectAllMois();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteMois(CsMois pMois)
        {
            try
            {
                return new DBMOIS().Delete(pMois);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateMois(List<CsMois> pMoisCollection)
        {
            try
            {
                return new DBMOIS().Update(pMoisCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertMois(List<CsMois> pMoisCollection)
        {
            try
            {
                return new DBMOIS().Insert(pMoisCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeMois(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = SelectAllMois();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        #endregion

        #region ETATCOMPTEUR

        public List<CsEtatCompteur> SelectAllEtatCompteur()
        {
            try
            {
                return new DBETATCOMPTEUR().SelectAllEtatComptage();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteEtatCompteur(CsEtatCompteur pEtatCompteur)
        {
            try
            {
                return new DBETATCOMPTEUR().Delete(pEtatCompteur);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateEtatCompteur(List<CsEtatCompteur> pEtatCompteurCollection)
        {
            try
            {
                return new DBETATCOMPTEUR().Update(pEtatCompteurCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertEtatCompteur(List<CsEtatCompteur> pEtatCompteurCollection)
        {
            try
            {
                return new DBETATCOMPTEUR().Insert(pEtatCompteurCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeEtatCompteur(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = SelectAllEtatCompteur();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        #endregion

        #region APPLICATIONTAXE

        public List<CsApplicationTaxe> SelectAllApplicationTaxe()
        {
            try
            {
                return new DBAPPLICATIONTAXE().SelectAllApplicationTaxe();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteApplicationTaxe(CsApplicationTaxe pApplicationTaxe)
        {
            try
            {
                return new DBAPPLICATIONTAXE().Delete(pApplicationTaxe);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateApplicationTaxe(List<CsApplicationTaxe> pApplicationTaxeCollection)
        {
            try
            {
                return new DBAPPLICATIONTAXE().Update(pApplicationTaxeCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertApplicationTaxe(List<CsApplicationTaxe> pApplicationTaxeCollection)
        {
            try
            {
                return new DBAPPLICATIONTAXE().Insert(pApplicationTaxeCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeApplicationTaxe(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = SelectAllApplicationTaxe();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        #endregion

        #region TYPEMESSAGE

        public List<CsTypeMessage> SelectAllTypeMessage()
        {
            try
            {
                return new DBTYPEMESSAGE().SelectAllTypeMessage();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteTypeMessage(CsTypeMessage pTypeMessage)
        {
            try
            {
                return new DBTYPEMESSAGE().Delete(pTypeMessage);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateTypeMessage(List<CsTypeMessage> pTypeMessageCollection)
        {
            try
            {
                return new DBTYPEMESSAGE().Update(pTypeMessageCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertTypeMessage(List<CsTypeMessage> pTypeMessageCollection)
        {
            try
            {
                return new DBTYPEMESSAGE().Insert(pTypeMessageCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeTypeMessage(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = SelectAllTypeMessage();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        #endregion

        #region TARIF

        public List<CsTarif> SelectAllTarif()
        {
            try
            {
                return new DB_TARIF().SelectAllTarif();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteTarif(CsTarif pTarif)
        {
            try
            {
                return new DB_TARIF().Delete(pTarif);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateTarif(List<CsTarif> pTarifCollection)
        {
            try
            {
                return new DB_TARIF().Update(pTarifCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertTarif(List<CsTarif> pTarifCollection)
        {
            try
            {
                return new DB_TARIF().Insert(pTarifCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeTarif(string key, Dictionary<string, string> pDictionary, List<CsTarif> pTarifCollection)
        {
            try
            {
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(pTarifCollection);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        #endregion

        #region FORFAIT

        public bool DeleteForfait(CsForfait pForfait)
        {
            try
            {
                return new DB_FORFAIT().Delete(pForfait);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateForfait(List<CsForfait> pForfaitCollection)
        {
            try
            {
                return new DB_FORFAIT().Update(pForfaitCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertForfait(List<CsForfait> pForfaitCollection)
        {
            try
            {
                return new DB_FORFAIT().Insert(pForfaitCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeForfait(string key, Dictionary<string, string> pDictionary, List<CsForfait> pForfaitCollection)
        {
            try
            {
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(pForfaitCollection);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CsForfait> SelectAllForfait()
        {
            try
            {
                return new DB_FORFAIT().SelectAllForfait();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        #endregion

        #region AJUFIN

        public List<CsAjufin> SelectAllAjufin()
        {
            try
            {
                return new DBAJUFIN().GetAll();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteAjufin(CsAjufin pAjufin)
        {
            try
            {
                return new DBAJUFIN().Delete(pAjufin);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateAjufin(List<CsAjufin> pAjufinCollection)
        {
            try
            {
                return new DBAJUFIN().Update(pAjufinCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertAjufin(List<CsAjufin> pAjufinCollection)
        {
            try
            {
                return new DBAJUFIN().Insert(pAjufinCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeAjufin(string key, Dictionary<string, string> pDictionary, List<CsAjufin> pAjufinCollection)
        {
            try
            {
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(pAjufinCollection);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        #endregion

        #region MONNAIE

        public List<CsMonnaie> SelectAllMonnaie()
        {
            try
            {
                return new DBMONNAIE().GetAll();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteMonnaie(CsMonnaie pMonnaie)
        {
            try
            {
                return new DBMONNAIE().Delete(pMonnaie);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateMonnaie(List<CsMonnaie> pMonnaieCollection)
        {
            try
            {
                return new DBMONNAIE().Update(pMonnaieCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertMonnaie(List<CsMonnaie> pMonnaieCollection)
        {
            try
            {
                return new DBMONNAIE().Insert(pMonnaieCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeMonnaie(string key, Dictionary<string, string> pDictionary, List<CsMonnaie> pMonnaieCollection)
        {
            try
            {
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(pMonnaieCollection);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        #endregion

        #region REDEVANCE

        public List<CsRedevance> SelectAllRedevance()
        {
            try
            {
                return new DBREDEVANCE().GetAll();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteRedevance(CsRedevance pRedevance)
        {
            try
            {
                return new DBREDEVANCE().Delete(pRedevance);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateRedevance(List<CsRedevance> pRedevanceCollection)
        {
            try
            {
                return new DBREDEVANCE().Update(pRedevanceCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertRedevance(List<CsRedevance> pRedevanceCollection)
        {
            try
            {
                return new DBREDEVANCE().Insert(pRedevanceCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeRedevance(string key, Dictionary<string, string> pDictionary, List<CsRedevance> pRedevanceCollection)
        {
            try
            {
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(pRedevanceCollection);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        #endregion

        

        #region secteur
        public List<CsSecteur> SelectAllSecteur()
        {
            try
            {
                return new DBSECTEUR().SelectAllSecteur();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteSecteur(CsSecteur pSecteur)
        {
            try
            {
                return new DBSECTEUR().Delete(pSecteur);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateSecteur(List<CsSecteur> pSecteurCollection)
        {
            try
            {
                return new DBSECTEUR().Update(pSecteurCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertSecteur(List<CsSecteur> pSecteurCollection)
        {
            try
            {
                return new DBSECTEUR().Insert(pSecteurCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeSecteur(string key, Dictionary<string, string> pDictionary, List<CsSecteur> pSecteurCollection)
        {
            try
            {
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(pSecteurCollection);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        #endregion

        #region Caisse
        public List<CsCaisse> SelectAllCaisse()
        {
            try
            {
                return new DB_CAISSE().SelectAllCaisse();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteCaisse(CsCaisse cCaisse)
        {
            try
            {
                return new DB_CAISSE().Delete(cCaisse);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateCaisse(List<CsCaisse> cCaisseCollection)
        {
            try
            {
                return new DB_CAISSE().Update(cCaisseCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertCaisse(List<CsCaisse> cCaisseCollection)
        {
            try
            {
                return new DB_CAISSE().Insert(cCaisseCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        #endregion

        #region Appareils
        public List<CsAppareils> SelectAllAPPAREILS()
        {
            try
            {
                return new DB_APPAREILS().SelectAllAppareils();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteAppareils(CsAppareils cAppareils)
        {
            try
            {
                return new DB_APPAREILS().Delete(cAppareils);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateAppareils(List<CsAppareils> CsAppareils)
        {
            try
            {
                return new DB_APPAREILS().Update(CsAppareils);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertAppareils(List<CsAppareils> cAppareilsCollection)
        {
            try
            {
                return new DB_APPAREILS().Insert(cAppareilsCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeAppareils(string key, Dictionary<string, string> pDictionary, List<CsAppareils> AppareilsCollection)
        {
            try
            {
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(AppareilsCollection);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }


        #endregion

        #region COUTCOPER
        public List<CsCoutCoper> SelectAllCoutcoper()
        {
            try
            {
                return new DB_CoutCoper().SelectAllCoutCoper();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteCoutcoper(CsCoutCoper CsCoutCoper)
        {
            try
            {
                return new DB_CoutCoper().Delete(CsCoutCoper);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateCoutcoper(List<CsCoutCoper> cCoutcoper)
        {
            try
            {
                return new DB_CoutCoper().Update(cCoutcoper);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertCoutcoper(List<CsCoutCoper> cCoutcoperCollection)
        {
            try
            {
                return new DB_CoutCoper().Insert(cCoutcoperCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeCoutcoper(string key, Dictionary<string, string> pDictionary, List<CsCoutCoper> CoutcoperCollection)
        {
            try
            {
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(CoutcoperCollection);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }


        #endregion

        #region COPER
        public List<CsCoper> SelectAllCoper()
        {
            try
            {
                return new DB_Coper().SelectAllCoper();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool Deletecoper(CsCoper CsCoper)
        {
            try
            {
                return new DB_Coper().Delete(CsCoper);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool Updatecoper(List<CsCoper> ccoper)
        {
            try
            {
                return new DB_Coper().Update(ccoper);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool Insertcoper(List<CsCoper> ccoperCollection)
        {
            try
            {
                return new DB_Coper().Insert(ccoperCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListecoper(string key, Dictionary<string, string> pDictionary, List<CsCoper> coperCollection)
        {
            try
            {
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(coperCollection);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }


        #endregion


        #region COPERDEMANDE
        public List<CsCoutDemande> SelectAllCoperDemande()
        {
            try
            {
                return new DB_CoperDemande().SelectAllCoperDemande();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteCoperDemande(CsCoutDemande cCoperDemande)
        {
            try
            {
                return new DB_CoperDemande().Delete(cCoperDemande);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateCoperDemande(List<CsCoutDemande> cCoperDemande)
        {
            try
            {
                return new DB_CoperDemande().Update(cCoperDemande);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertCoperDemande(List<CsCoutDemande> cCoperDemande)
        {
            try
            {
                return new DB_CoperDemande().Insert(cCoperDemande);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeCoperDemande(string key, Dictionary<string, string> pDictionary, List<CsCoutDemande> coperDeCollection)
        {
            try
            {
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(coperDeCollection);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }


        #endregion

        #region TDEM
        public List<CsTdem> SelectAllDTEM()
        {
            try
            {
                return new DB_TDEM1().SelectAllTdem();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }



        #endregion

        #region Fourniture
        public List<ObjFOURNITURE> SelectAllFourniture()
        {
            try
            {
                return DBFOURNITURE.GetAllFourniture();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteFourniture(ObjFOURNITURE cFourniture)
        {
            try
            {
                return DBFOURNITURE.DeleteFourniture(cFourniture);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateFourniture(List<ObjFOURNITURE> cFourniture)
        {
            try
            {
                return  DBFOURNITURE.UpdateFourniture (cFourniture);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertFourniture(List<ObjFOURNITURE> cFourniture)
        {
            try
            {
                return DBFOURNITURE.InsertFourniture(cFourniture);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool EditerListeFourniture(string key, Dictionary<string, string> pDictionary, List<ObjFOURNITURE> FournitureCollection)
        {
            try
            {
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(FournitureCollection);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }


        #endregion

        #region Materiel
        public List<CsMaterielDemande > SelectAllMateriel()
        {
            try
            {
                return DBMATERIEL.GetAll();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteMateriel(CsMaterielDemande cMateriel)
        {
            try
            {
                return DBMATERIEL.Delete (cMateriel);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateMateriel(List<CsMaterielDemande> cMateriel)
        {
            try
            {
                return DBMATERIEL.Update(cMateriel);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertMateriel(List<CsMaterielDemande> cMateriel)
        {
            try
            {
                return DBMATERIEL.Insert (cMateriel);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }


        public bool EditerListeMateriel(string key, Dictionary<string, string> pDictionary)
        {
            try
            {
                var lsObject = SelectAllMateriel();
                var listePrint = new List<CsPrint>();
                listePrint.AddRange(lsObject);
                var printService = new Printings.PrintingsService();
                printService.setFromWebPart(listePrint, key, pDictionary);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        #endregion

        #region MarqueModele
        public List<CsMarque_Modele > SelectAllMarqueModele()
        {
            try
            {
                return DB_MARQUEMODEL.SelectAllMarqueModel();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteMarqueModele(CsMarque_Modele cMarqueModele)
        {
            try
            {
                return DB_MARQUEMODEL.Delete(cMarqueModele);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateMarqueModele(List<CsMarque_Modele> cMarqueModele)
        {
            try
            {
                return DB_MARQUEMODEL.Update(cMarqueModele);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertMarqueModele(List<CsMarque_Modele> cMarqueModele)
        {
            try
            {
                return DB_MARQUEMODEL.Insert(cMarqueModele);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        #endregion

        #region Modele
        public List<CsMarque_Modele> SelectAllModel()
        {
            try
            {
                return DB_MODEL.SelectAllModel();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteModele(CsMarque_Modele cModele)
        {
            try
            {
                return DB_MODEL.Delete(cModele);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateModele(List<CsMarque_Modele> cModele)
        {
            try
            {
                return DB_MODEL.Update(cModele);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertModele(List<CsMarque_Modele> cModele)
        {
            try
            {
                return DB_MODEL.Insert(cModele);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        #endregion

        #region Couleur
        public List<CsCouleurScelle> SelectAllCouleur()
        {
            try
            {
                return DB_COULEUR.SelectAllCouleur();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteCouleur(CsCouleurScelle cCouleur)
        {
            try
            {
                return DB_COULEUR.Delete(cCouleur);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateCouleur(List<CsCouleurScelle> cCouleur)
        {
            try
            {
                return DB_COULEUR.Update(cCouleur);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertCouleur(List<CsCouleurScelle> cCouleur)
        {
            try
            {
                return DB_COULEUR.Insert(cCouleur);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        #endregion

        #region Activiter
        public List<CsActivite> SelectAllActivite()
        {
            try
            {
                return DB_ACTIVITE.SelectAllActivite();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteActivite(CsActivite cActivite)
        {
            try
            {
                return DB_ACTIVITE.Delete(cActivite);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateActivite(List<CsActivite> cActivite)
        {
            try
            {
                return DB_ACTIVITE.Update(cActivite);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertActivite(List<CsActivite> cActivite)
        {
            try
            {
                return DB_ACTIVITE.Insert(cActivite);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        #endregion

        #region ActiviterCouleur
        public List<CsCouleurActivite> SelectAllActiviteCouleur()
        {
            try
            {
                return DB_ACTIVITECOULEUR.SelectAllActiviteCouleur();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool DeleteActiviteCouleur(CsCouleurActivite cActivite)
        {
            try
            {
                return DB_ACTIVITECOULEUR.Delete(cActivite);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateActiviteCouleur(List<CsCouleurActivite> cActivite)
        {
            try
            {
                return DB_ACTIVITECOULEUR.Update(cActivite);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertActiviteCouleur(List<CsCouleurActivite> cActivite)
        {
            try
            {
                return DB_ACTIVITECOULEUR.Insert(cActivite);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        #endregion

        #region PARAMETRECOUPURESCGC
        public List<CsParametreCoupureSGC> SelectAllParamatreSCGC()
        {
            try
            {
                return new  DBPARAMETRECOUPURESGC().SelectAllParametre();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        #endregion



        #region WORKFLOW

        public List<CsOperation> SelectAllOperationParent()
        {
            try
            {
                return new DB_WORKFLOW().SelectAllOperation();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsOperation> SelectAllOperation()
        {
            try
            {
                return new DB_WORKFLOW().SelectAllOperation2();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool InsertOperation(List<CsOperation> pOperationCollection)
        {
            try
            {
                return new DB_WORKFLOW().Insert(pOperationCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateOperation(List<CsOperation> pOperationCollection)
        {
            try
            {
                return new DB_WORKFLOW().UpdateOperation(pOperationCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool DeleteOperation(List<CsOperation> pOperationCollection)
        {
            try
            {
                return new DB_WORKFLOW().DeleteOperation(pOperationCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CsOperationFormulaire> SelectAllViewOperationFormulaire()
        {
            try
            {
                return new DB_WORKFLOW().SelectAllViewOperationFormulaire();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsVwConfigurationWorkflowCentre> SelectAllConfigurationWorkflowCentre()
        {
            try
            {
                return new DB_WORKFLOW().SelectAllConfigurationWorkflowCentre();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        #region Gestion des étapes

        public List<CsEtape> SelectAllEtapes()
        {
            try
            {
                return new DB_WORKFLOW().SelectAllEtapes();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsEtape> SelectAllEtapesByOperationId(Guid OperationId)
        {
            try
            {
                return new DB_WORKFLOW().SelectAllEtapes(OperationId);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool InsertEtape(List<CsEtape> pEtapeCollection)
        {
            try
            {
                return new DB_WORKFLOW().InsertEtape(pEtapeCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateEtape(List<CsEtape> pEtapeCollection)
        {
            try
            {
                return new DB_WORKFLOW().UpdateEtape(pEtapeCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }



        #endregion

        #region Formulaires

        public List<CsFormulaire> SelectAllFormulaire()
        {
            try
            {
                return new DB_WORKFLOW().SelectAllFormulaire();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        #endregion

        public Dictionary<CsGroupeValidation, List<CsRHabilitationGrouveValidation>> SelectAllGroupeValidation()
        {
            try
            {
                return new DB_WORKFLOW().SelectAllGroupeValidation();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool InsertGroupeValidation(Dictionary<CsGroupeValidation, List<CsRHabilitationGrouveValidation>> lsGroupeEtHabilitation)
        {
            try
            {
                return new DB_WORKFLOW().InsertGroupeValidation(lsGroupeEtHabilitation);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateGroupeValidation(Dictionary<CsGroupeValidation, List<CsRHabilitationGrouveValidation>> pGrpCollection)
        {
            try
            {
                return new DB_WORKFLOW().UpdateGroupValidation(pGrpCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }


        public bool UpdateGroupValidationCopieCircuit(string idDemande, string numdem, Guid idGroup)
        {
            try
            {
                return new DB_WORKFLOW().UpdateCopieCircuit(idDemande, numdem, idGroup);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }



        public bool DeleteGroupeValidation(List<CsGroupeValidation> pGrpCollection)
        {
            try
            {
                return new DB_WORKFLOW().DeleteGroupeValidation(pGrpCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CsGroupeValidation> SelectGroupeValidationByUserId(int UserId)
        {
            try
            {
                List<CsGroupeValidation> lsGroupes = new List<CsGroupeValidation>();
                var huumm = new DB_WORKFLOW().SelectAllGroupeValidationUser(UserId);
                foreach (CsGroupeValidation grp in huumm.Keys.ToList())
                {
                    var lsHab = huumm[grp];
                    if (null != lsHab.Where(h => h.FK_IDADMUTILISATEUR == UserId).FirstOrDefault())
                    {
                        grp.ESTCONSULTATION =lsHab.Where(h => h.FK_IDADMUTILISATEUR == UserId).FirstOrDefault().ESTCONSULTATION == null ? false :
                            lsHab.Where(h => h.FK_IDADMUTILISATEUR == UserId).FirstOrDefault().ESTCONSULTATION.Value ; 
                        lsGroupes.Add(grp);
                    }
                }

                return lsGroupes;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsWorkflow> SelectAllWorkflow()
        {
            try
            {
                return new DB_WORKFLOW().SelectAllWorkflows();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsTableDeTravail> SelectAllTableTravail()
        {
            try
            {
                return new DB_WORKFLOW().SelectAllTableDeTravail();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool InsertWorkflow(List<CsWorkflow> pWorkflowCollection)
        {
            try
            {
                return new DB_WORKFLOW().InsertWorkflow(pWorkflowCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateWorkflow(List<CsWorkflow> pWorkflowCollection)
        {
            try
            {
                return new DB_WORKFLOW().UpdateWorkflow(pWorkflowCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool DeleteWorkflow(List<CsWorkflow> pWorkflowCollection)
        {
            try
            {
                return new DB_WORKFLOW().DeleteWorkflow(pWorkflowCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement> SelectAllAffectationEtapeWorkflow(Guid pRWKF)
        {
            try
            {
                return new DB_WORKFLOW().SelectAllAffectationEtapeWorkflow(pRWKF);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement> SelectAllAffectationEtapeCircuitDetourne(Guid pOrigineAffectationEtape)
        {
            try
            {
                return new DB_WORKFLOW().SelectAllAffectationEtapeCircuitDetourne(pOrigineAffectationEtape);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool InsertAffectationEtapeWorkflow(Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement> lsEtapesEtConditions)
        {
            try
            {
                return new DB_WORKFLOW().InsertAffectationEtape(lsEtapesEtConditions);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool InsertAffectationEtapeWorkflowParCentre(List<Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement>> lsEtapesEtConditions)
        {
            try
            {
                return new DB_WORKFLOW().InsertAffectationEtape(lsEtapesEtConditions);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateAffectationEtapeWorkflow(Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement> lsEtapesEtConditions)
        {
            try
            {
                return new DB_WORKFLOW().UpdateAffectationEtape(lsEtapesEtConditions);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool DeleteAffectationEtapeWorkflow(List<CsRAffectationEtapeWorkflow> lsEtapes)
        {
            try
            {
                return new DB_WORKFLOW().DeleteWorkflowAffectationEtape(lsEtapes);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CsRWorkflow> SelectAllRWorkflowCentre(Guid pKIDWKF, int CpKID, Guid OpPKID)
        {
            try
            {
                return new DB_WORKFLOW().SelectAllRWorkflowCentre(pKIDWKF, CpKID, OpPKID);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        //public List<CsRWorkflow> SelectAllRWorkflowTousLesCentres(Guid pKIDWKF, int SpKID, Guid OpPKID)
        //{
        //    try
        //    {
        //        return new DB_WORKFLOW().SelectAllRWorkflowCentre(pKIDWKF, CpKID, OpPKID);
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogException(this, ex);
        //        return null;
        //    }
        //}


        public bool InsertRWorkflow(List<CsRWorkflow> lsRWorkflow)
        {
            try
            {
                return new DB_WORKFLOW().InsertRWorkflowCentre(lsRWorkflow);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool UpdateRWorkflow(List<CsRWorkflow> lsRWorkflow)
        {
            try
            {
                return new DB_WORKFLOW().UpdateRWorkflowCentre(lsRWorkflow);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool DeleteRWorkflow(List<CsRWorkflow> lsRWorkflow)
        {
            try
            {
                return new DB_WORKFLOW().DeleteRWorkflowCentre(lsRWorkflow);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool ConfigurerPlusieurWorkflowEtCentre(List<CsRWorkflow> rwkfCentres, Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement> configurationsEtapes, List<CsRenvoiRejet> _renvois)
        {
            try
            {
                return new DB_WORKFLOW().ConfigurerPlusieurWorkflowEtCentre(rwkfCentres, configurationsEtapes, _renvois);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        //public bool SupprimerUneConfiguration(Guid RWKFCentre)
        //{
        //}

        public string[] GetColumnsOfWorkingTable(string TableName)
        {
            try
            {
                return new DB_WORKFLOW().LesColonnesDeLaTable(TableName);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        #region Gestion des demandes

        public List<CsVwDashboardDemande> SelectVwDashBoardDemande(List<int> lstCentre, int IdAgentConnet)
        {
            try
            {
                return new DB_WORKFLOW().RetourneTypeDemandeDashboard(lstCentre, IdAgentConnet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsVwDashboardDemande> SelectVwDashBoardDemandeByHabilitation(int centreId, List<Guid> GrpsValidation)
        {
            try
            {
                var toutDabord = new DB_WORKFLOW().SelectVwDashBoardDemande().Where(d => d.FK_IDCENTRE == centreId)
                    .ToList();
                List<CsVwDashboardDemande> dashboard = new List<CsVwDashboardDemande>();
                foreach (var grp in GrpsValidation)
                {
                    dashboard.AddRange(toutDabord.Where(g => g.FK_IDGROUPEVALIDATIOIN == grp));
                }
                return dashboard;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsVwDashboardDemande> SelectVwDashBoardDemandeByHabilitationListCentre(List<int> centresId, List<Guid> GrpsValidation)
        {
            try
            {
                var toutDabord = new DB_WORKFLOW().SelectVwDashBoardDemande().Where(t => centresId.Contains(t.FK_IDCENTRE))
                    .ToList();
                List<CsVwDashboardDemande> dashboard = new List<CsVwDashboardDemande>();
                foreach (var grp in GrpsValidation)
                {
                    dashboard.AddRange(toutDabord.Where(g => g.FK_IDGROUPEVALIDATIOIN == grp));
                }
                return dashboard;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //A Suivre pour gerer les transferts
        //public List<CsVwDashboardDemande> SelectVwDashBoardDemandeByHabilitationListCentre(List<int> centresId, List<Guid> GrpsValidation)
        //{
        //    try
        //    {
        //        List<CsVwDashboardDemande> AllDabord = new DB_WORKFLOW().SelectVwDashBoardDemande();

        //        //if (AllDabord.Where(p => p.FK_IDOPERATION.ToString() == "CB96F09C-919D-412C-9153-BFD6327C77FE") != null)
        //        //{
        //        //    List<CsCentre> lstCentre = new DBAccueil().ChargerLesDonneesDesSite(false);
        //        //}
        //        List<CsVwDashboardDemande> toutDabord = AllDabord.Where(t => centresId.Contains(t.FK_IDCENTRE)).ToList();
        //        //List<CsVwDashboardDemande> Transfert = AllDabord.Where(t => centresId.Contains(t.FK_IDCENTRE)).ToList();
        //        List<CsVwDashboardDemande> dashboard = new List<CsVwDashboardDemande>();
        //        foreach (var grp in GrpsValidation)
        //        {
        //            dashboard.AddRange(toutDabord.Where(g => g.FK_IDGROUPEVALIDATIOIN == grp));
        //        }
        //        return dashboard;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public List<CsVwJournalDemande> SelectVwJournalDemande()
        {
            try
            {
                return new DB_WORKFLOW().SelectVwJournalDemande();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsVwJournalDemande> SelectAllDemandeUser(List<int> lstCentreId, List<Guid> lstIdDemande,int Fk_idEtape, string Matricule)
        {
            try
            {
                return new DB_WORKFLOW().SelectAllDemandeUser(lstCentreId,lstIdDemande, Fk_idEtape, Matricule);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsVwJournalDemande> SelectAllDemandeUserEtape(List<int> lstCentreId, int Fk_idEtape, string Matricule)
        {
            try
            {
                return new DB_WORKFLOW().SelectAllDemandeUserEtape(lstCentreId, Fk_idEtape, Matricule);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsJournalDemandeWorkflow> SelectJournalDeLaDemande(string codeDemande)
        {
            try
            {
                return new DB_WORKFLOW().SelectJournalDeLaDemande(codeDemande);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CsVwJournalDemande SelectInformationDemande(string codeDemande)
        {
            try
            {
                var lsDemande = new DB_WORKFLOW().SelectVwJournalDemande();
                return lsDemande.Where(d => d.CODE == codeDemande)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public KeyValuePair<CsCopieDmdCircuit, KeyValuePair<CsGroupeValidation,
            List<CsRHabilitationGrouveValidation>>> RecupererInfoEtapeSuivante(string codeDemande)
        {
            try
            {
                DB_WORKFLOW dbWorkflow = new DB_WORKFLOW();
                var lEtapeActuelle = dbWorkflow.RecupererEtapeCourante(codeDemande);
                if (null != lEtapeActuelle.Key)
                {
                    string messageErreur = string.Empty;
                    bool DerniereEtape = false;
                    //On a l'étape courante, on récupère l'étape précédente
                    //Sans considérer pour le moement la gestion des conditions
                    var Next = dbWorkflow.ProchaineOuPrecedenteEtape(codeDemande, lEtapeActuelle.Key.FK_IDETAPE,
                        lEtapeActuelle.Key.PK_ID, 1, false);
                    if (null != Next.Key)
                    {
                        //ON récupère le groupe de validation de cette étape
                        Guid grpId = Next.Key.FK_IDGROUPEVALIDATIOIN;
                        KeyValuePair<CsGroupeValidation, List<CsRHabilitationGrouveValidation>> leGroupe = dbWorkflow.SelectAllGroupeValidation()
                            .Where(g => g.Key.PK_ID == grpId)
                            .FirstOrDefault();
                        if (null != leGroupe.Key && null != leGroupe.Value)
                            return new KeyValuePair<CsCopieDmdCircuit, KeyValuePair<CsGroupeValidation,
                                List<CsRHabilitationGrouveValidation>>>(Next.Key, leGroupe);
                        else return new KeyValuePair<CsCopieDmdCircuit, KeyValuePair<CsGroupeValidation,
                            List<CsRHabilitationGrouveValidation>>>(null,
                                new KeyValuePair<CsGroupeValidation, List<CsRHabilitationGrouveValidation>>(null, null));
                    }
                    else return new KeyValuePair<CsCopieDmdCircuit, KeyValuePair<CsGroupeValidation,
                            List<CsRHabilitationGrouveValidation>>>(null,
                                new KeyValuePair<CsGroupeValidation, List<CsRHabilitationGrouveValidation>>(null, null));
                }
                else return new KeyValuePair<CsCopieDmdCircuit, KeyValuePair<CsGroupeValidation,
                            List<CsRHabilitationGrouveValidation>>>(null,
                                new KeyValuePair<CsGroupeValidation, List<CsRHabilitationGrouveValidation>>(null, null));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public KeyValuePair<CsCopieDmdCircuit, KeyValuePair<CsGroupeValidation,List<CsRHabilitationGrouveValidation>>> RecupererInfoEtapeSuivanteByCodeWorkflow(string codeDemande)
        {
            try
            {
                DB_WORKFLOW dbWorkflow = new DB_WORKFLOW();
                var lEtapeActuelle = dbWorkflow.RecupererEtapeCourantebyCodeWorkflow(codeDemande);
                if (null != lEtapeActuelle.Key)
                {
                    string messageErreur = string.Empty;
                    bool DerniereEtape = false;
                    //On a l'étape courante, on récupère l'étape précédente
                    //Sans considérer pour le moement la gestion des conditions
                    var Next = dbWorkflow.ProchaineOuPrecedenteEtape(codeDemande, lEtapeActuelle.Key.FK_IDETAPE,
                        lEtapeActuelle.Key.PK_ID, 1, false);
                    if (null != Next.Key)
                    {
                        //ON récupère le groupe de validation de cette étape
                        Guid grpId = Next.Key.FK_IDGROUPEVALIDATIOIN;
                        KeyValuePair<CsGroupeValidation, List<CsRHabilitationGrouveValidation>> leGroupe = dbWorkflow.SelectAllGroupeValidation()
                            .Where(g => g.Key.PK_ID == grpId)
                            .FirstOrDefault();
                        if (null != leGroupe.Key && null != leGroupe.Value)
                            return new KeyValuePair<CsCopieDmdCircuit, KeyValuePair<CsGroupeValidation,
                                List<CsRHabilitationGrouveValidation>>>(Next.Key, leGroupe);
                        else return new KeyValuePair<CsCopieDmdCircuit, KeyValuePair<CsGroupeValidation,
                            List<CsRHabilitationGrouveValidation>>>(null,
                                new KeyValuePair<CsGroupeValidation, List<CsRHabilitationGrouveValidation>>(null, null));
                    }
                    else return new KeyValuePair<CsCopieDmdCircuit, KeyValuePair<CsGroupeValidation,
                            List<CsRHabilitationGrouveValidation>>>(null,
                                new KeyValuePair<CsGroupeValidation, List<CsRHabilitationGrouveValidation>>(null, null));
                }
                else return new KeyValuePair<CsCopieDmdCircuit, KeyValuePair<CsGroupeValidation,
                            List<CsRHabilitationGrouveValidation>>>(null,
                                new KeyValuePair<CsGroupeValidation, List<CsRHabilitationGrouveValidation>>(null, null));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsAffectationDemandeUser> SelectLesAffectationsTraitementDemande(string matriculeUser)
        {
            try
            {
                return new DB_WORKFLOW().SelectLesAffectationsTraitementDemande(matriculeUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Dictionary<CsAffectationDemandeUser, CsVwJournalDemande> GetLesDemandesAffecteFromMatriculeUser(string matriculeUser, int idEtape,
            Guid idOperation)
        {
            try
            {
                Dictionary<CsAffectationDemandeUser, CsVwJournalDemande> LesDemandesAffectees = new Dictionary<CsAffectationDemandeUser, CsVwJournalDemande>();
                var lsAffectation = new DB_WORKFLOW().SelectLesAffectationsTraitementDemande()
                    .Where(a => a.MATRICULEUSERCREATION == matriculeUser && a.OPERATIONID == idOperation && a.FK_IDETAPEFROM == idEtape)
                        .ToList();
                
                Parallel.ForEach(lsAffectation, (aff, state) =>
                {
                    //on recherche l'info de la vue journale demande
                    CsVwJournalDemande vueJrnl = RetourneDemandeWkfAll().Where(c => c.CODE == aff.CODEDEMANDE)
                        .FirstOrDefault();
                    if (null != vueJrnl)
                    {
                        LesDemandesAffectees.Add(aff, vueJrnl);
                    }
                });
                return LesDemandesAffectees;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Dictionary<CsAffectationDemandeUser, CsVwJournalDemande> GetLesDemandesAffecteForMatriculeUser(string matriculeUser, int idEtape,
            Guid idOperation)
        {
            try
            {
                Dictionary<CsAffectationDemandeUser, CsVwJournalDemande> LesDemandesAffectees = new Dictionary<CsAffectationDemandeUser, CsVwJournalDemande>();
                var lsAffectation = new DB_WORKFLOW().SelectLesAffectationsTraitementDemande()
                    .Where(a => a.MATRICULEUSER == matriculeUser && a.OPERATIONID == idOperation && a.FK_IDETAPE == idEtape)
                        .ToList();
                Parallel.ForEach(lsAffectation, (aff, state) =>
                {
                    //on recherche l'info de la vue journale demande
                    CsVwJournalDemande vueJrnl = SelectVwJournalDemande().Where(c => c.CODE == aff.CODEDEMANDE)
                        .FirstOrDefault();
                    if (null != vueJrnl)
                    {
                        LesDemandesAffectees.Add(aff, vueJrnl);
                    }
                });
                return LesDemandesAffectees;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool AffecterDemande(List<CsAffectationDemandeUser> lesAffectations)
        {
            try
            {
                return new DB_WORKFLOW().AffecterDemande(lesAffectations);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CheckForAffectationFromEtape(int idEtape, Guid IdOperation, string MatriculeUserCreation)
        {
            try
            {
                return new DB_WORKFLOW().SelectLesAffectationsTraitementDemande()
                    .Exists(a => a.FK_IDETAPEFROM == idEtape && a.OPERATIONID == IdOperation && a.MATRICULEUSERCREATION == MatriculeUserCreation);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CheckForAffectationForEtape(int idEtape, Guid IdOperation, string MatriculeUserAffected)
        {
            try
            {
                return new DB_WORKFLOW().SelectLesAffectationsTraitementDemande()
                    .Exists(a => a.FK_IDETAPE == idEtape && a.OPERATIONID == IdOperation && a.MATRICULEUSER == MatriculeUserAffected);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsCommentaireRejet> SelectCommentaireRejet(Guid pKIDDemande)
        {
            try
            {
                return new DB_WORKFLOW().SelectCommentaireRejet(pKIDDemande);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool VerifierAppartenanceGroupeValidation(int UserId)
        {
            try
            {
                return new DB_WORKFLOW().VerifierAppartenanceGroupeValidation(UserId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*
        [OperationContract]
        [FaultContract(typeof(Errors))] */
        public bool InsertCommentaireRejet(List<CsCommentaireRejet> lsCommentaires)
        {
            try
            {
                return new DB_WORKFLOW().InsertCommentaireRejet(lsCommentaires);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /*[OperationContract]
        [FaultContract(typeof(Errors))]
        bool UpdateCommentaireRejet(List<CsCommentaireRejet> lsCommentaires);
        */

        public List<CsCopieDmdCircuit> SelectEtapesFromDemande(string CodeDemande)
        {
            try
            {
                Dictionary<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> lsEtapes = new DB_WORKFLOW().SelectAllCircuitEtapeDemandeWorkflow(CodeDemande);
                return lsEtapes.Keys.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsCopieDmdConditionBranchement> SelectConditionParEtapes(List<Guid> PKIDEtapeCopieCircuit)
        {
            try
            {
                List<CsCopieDmdConditionBranchement> conditions = new List<CsCopieDmdConditionBranchement>();
                foreach (Guid g in PKIDEtapeCopieCircuit)
                {
                    var C = new DB_WORKFLOW().SelectConditionCopieCirucit(g);
                    if (null != C && C.Count > 0) conditions.AddRange(C);
                }
                return conditions;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public CsDemandeWorkflow GetInfoDemandeWorkflowByCodeDemande(string numDmd)
        {
            try
            {
                return new DB_WORKFLOW().SelectLaDemande(numDmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsDemandeWorkflow> GetInfoDemandeWorkflowByListCodeDemande(List<string> PlisteDemande)
        {
            try
            {
                return new DB_WORKFLOW().SelectLaDemande(PlisteDemande);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<CsGroupeValidation> SelectAllGroupeValidationSpecifique(int TypeGroupe)
        {
            try
            {
                return new DB_WORKFLOW().SelectAllGroupeValidationSpecifique(TypeGroupe);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public List<CsRenvoiRejet> GetLesRenvoisRejet(int fkIdEtapeActuelle)
        {
            try
            {
                return new DB_WORKFLOW().GetLesRenvoisRejet(fkIdEtapeActuelle);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool AjouterRenvoisRejet(List<CsRenvoiRejet> __renvois)
        {
            try
            {
                return new DB_WORKFLOW().InsertRenvoiRejet(__renvois);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SupprimerLesRenvoisRejet(int fkIdEtapeActuelle)
        {
            try
            {
                return new DB_WORKFLOW().DeleteRenvoiRejet(fkIdEtapeActuelle);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public List<CsVwJournalDemande> RetourneDemandeWkfEtape(List<int> lstCentreId, int Fk_idetape, string matriculeUser)
        {
            try
            {
                List<CsVwJournalDemande> lstDemandeEtape = new DB_WORKFLOW().RetourneDemandeWkfEtape(lstCentreId,Fk_idetape,matriculeUser,true ).Where(t => lstCentreId.Contains(t.FK_IDCENTRE)).ToList();
                //CsEtape leEtape = new DB_WORKFLOW().SelectAllEtapesbyId(Fk_idetape);
                //if (leEtape != null && leEtape.CONTROLEETAPE == "Galatee.Silverlight.Devis.UcMetrer")
                //{
                //    List<CsAffectationDemandeUser> lsAffectation = new DB_WORKFLOW().SelectLesAffectationsTraitementDemandeUSer(matriculeUser, Fk_idetape);
                //    List<CsVwJournalDemande> lstDemandeAffecter = new List<CsVwJournalDemande>();
                //    lstDemandeAffecter = lstDemandeEtape.Where(i => lsAffectation.Select(t => t.CODEDEMANDE).Contains(i.CODE)).ToList();
                //    return lstDemandeAffecter;
                //}
                //else
                return lstDemandeEtape;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsVwJournalDemande> RetourneDemandeWkfClient(List<int> lstCentreId, int Fk_idetape, string matriculeUser, string NumeroDemande,bool IsToutAfficher)
        {
            try
            {
                List<CsVwJournalDemande> lstDemandeEtape = new DB_WORKFLOW().RetourneListeDemande(lstCentreId, Fk_idetape, matriculeUser, NumeroDemande, IsToutAfficher);
                return lstDemandeEtape;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsVwJournalDemande> RetourneDemandeWkf(List<int> lstIdCentre)
        {
            try
            {
                return new DB_WORKFLOW().RetourneDemandeWkf().Where(p => lstIdCentre.Contains(p.FK_IDCENTRE)).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsVwJournalDemande> RetourneDemandeWkfAll()
        {
            try
            {
                return new DB_WORKFLOW().RetourneDemandeWkf();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion




    }
}
