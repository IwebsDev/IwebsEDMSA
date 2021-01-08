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

namespace Galatee.Silverlight.Security
{
    public class Securities
    {
        /// <summary>
        /// Methode invoquée quand l'admin réinitialise le mot de passe 
        /// </summary>
        /// <param name="newPassword"></param>
        /// <param name="confirmPassword"></param>
        public static void CheckConfirmPassword(string newPassword, string confirmPassword)
        {
            if (newPassword != confirmPassword)
                throw new Exception(Langue.MsgErrConfPwd);
        }
        public string pPassword = string.Empty;
        public static Status VerifierDonneesConnexion(string pLogin, string pPassword ,CsStrategieSecurite pSecurite, CsUtilisateur pUser)
        {
            try
            {
                if (pUser == null)
                    return new Status(false,new Exception(Langue.MsgErrLogin));

                if (pUser.ESTSUPPRIMER.Value)
                    return new Status(false, new Exception("Votre compte a été supprimé, vous n'avez plus le droit d'utiliser le système."));
                //verifier si le matricule correspond au groupe admin
                if (pUser.EsADMIN)
                    if (VerifierCorrespondancePasword(pUser, pPassword))
                        return new Status(false, null);
                    else
                        return new Status(false, new Exception(Langue.MsgErrPwd));

               // si le user courant n'est pas admin donc est soumi à un filtrage 
                
                //vérifier si le compte est valide pour la période de validité
                if (pUser.MATRICULE != "99999" && (pUser.DATEDEBUTVALIDITE != null && ((DateTime)pUser.DATEDEBUTVALIDITE).Date > DateTime.Now.Date))
                    return new Status(false, new Exception(string.Format(Langue.MsgUtilPasValid, pUser.LOGINNAME, pUser.MATRICULE)));

                if (pUser.MATRICULE != "99999" && (pUser.DATEFINVALIDITE != null && ((DateTime)pUser.DATEFINVALIDITE).Date < DateTime.Now.Date))
                    return new Status(false, new Exception(string.Format(Langue.MsgUtilPasValid2, pUser.LOGINNAME, pUser.MATRICULE)));


                //Durée Maximale Password
                if (pUser.MATRICULE != "99999" && (pSecurite.DUREEMAXIMALEPASSWORD != 0 && pUser.DATEDERNIEREMODIFICATIONPASSWORD != null))
                {
                    if (((DateTime)pUser.DATEDERNIEREMODIFICATIONPASSWORD).AddDays(Convert.ToDouble(pSecurite.DUREEMAXIMALEPASSWORD)) < DateTime.Now)
                        return new Status(false, new Exception(Langue.MsgPwdExpire));
                }

                //Verifier si le compte est actif
                if (pUser.FK_IDSTATUS != (int)Global.StatusCompte.Actif)
                {
                    //Le compte n'est pas actif
                    if (pUser.FK_IDSTATUS == (int)Global.StatusCompte.Inactif)
                        return new Status(false, new Exception(string.Format(Langue.MsgCptDesactiv, pUser.LOGINNAME, pUser.MATRICULE)));

                    if (pUser.FK_IDSTATUS == (int)Global.StatusCompte.Verrouille)
                        return new Status(false, new Exception(string.Format(Langue.MsgCptVerrouil, pUser.LOGINNAME, pUser.MATRICULE)));

                    if (pUser.FK_IDSTATUS == (int)Global.StatusCompte.VerrouilleOuvertureSession)
                        return new Status(false, new Exception(string.Format(Langue.MsgCptVerrouil, pUser.LOGINNAME, pUser.MATRICULE)));

                    if (pSecurite.DUREEVERROUILLAGECOMPTE == 0)
                        return new Status(false, new Exception(string.Format(Langue.MsgCptVerrouil, pUser.LOGINNAME, pUser.MATRICULE)));

                    //Verifier la durée de verrouillage s'est écoulée
                    //Si c'est non
                    if (pUser.DATEDERNIERVERROUILLAGE != null && ((DateTime)pUser.DATEDERNIERVERROUILLAGE).AddMinutes(Convert.ToDouble(pSecurite.DUREEVERROUILLAGECOMPTE)) > DateTime.Now)
                        return new Status(false, new Exception(string.Format(Langue.MsgCptVerrouil2, pUser.LOGINNAME, pUser.MATRICULE, pSecurite.DUREEVERROUILLAGECOMPTE)));

                    //si la durée de verrouillage s'est écoulée
                    pUser.DATEDERNIERVERROUILLAGE = null;
                    pUser.NOMBREECHECSOUVERTURESESSION = 0;

                    //Verifier Correspondance du pasword saisi par le pUser
                    if (!VerifierCorrespondancePasword(pUser, pPassword,true))
                        return new Status(true, new Exception(Langue.MsgErrPwd));
                    else
                        return new Status(true, null);
                    
                    //dbAdmUsers.Update(user);
                    //  return ;

                }
                else
                {
                    //NombreMaximalEchecsOuvertureSession <>0
                    if (pSecurite.NOMBREMAXIMALECHECSOUVERTURESESSION == 0)
                    {
                        pUser.DATEDERNIERECONNEXION = DateTime.Now;
                        if (!VerifierCorrespondancePasword(pUser, pPassword,false))
                        {
                             pUser.DERNIERECONNEXIONREUSSIE = false;
                             return new Status(true, new Exception(Langue.MsgErrPwd));
                        }
                        pUser.DERNIERECONNEXIONREUSSIE = true;
                        return new Status(true,null);
                        ///dbAdmUsers.Update(pUser);
                    }
                    else
                    {
                        //Si Derniere Connexion Reussie
                        if (pUser.DERNIERECONNEXIONREUSSIE == null || (bool)pUser.DERNIERECONNEXIONREUSSIE)
                        {
                            //if(pUser.InitUserPassword) // ajouter par HGB 22/10/2013 

                            if (!VerifierCorrespondancePasword(pUser, pPassword, true))
                            {
                                pUser.DERNIERECONNEXIONREUSSIE = false;

                                if (pUser.NOMBREECHECSOUVERTURESESSION == null)
                                    pUser.NOMBREECHECSOUVERTURESESSION = 1;
                                else
                                    pUser.NOMBREECHECSOUVERTURESESSION = pUser.NOMBREECHECSOUVERTURESESSION + 1;


                                if (pUser.MATRICULE != "99999" && (pUser.NOMBREECHECSOUVERTURESESSION >= pSecurite.NOMBREMAXIMALECHECSOUVERTURESESSION))
                                {
                                    pUser.DATEDERNIERVERROUILLAGE = pUser.DATEDERNIERECONNEXION;
                                    pUser.FK_IDSTATUS = (byte)Global.StatusCompte.VerrouilleOuvertureSession;
                                    return new Status(true, new Exception(string.Format(Langue.MsgErrMaxEchecConct, pUser.LOGINNAME, pUser.MATRICULE)));
                                }
                                else
                                    return new Status(true, new Exception(Langue.MsgErrPwd));
                            }
                            else
                            {
                                pUser.NOMBREECHECSOUVERTURESESSION = 0;
                                pUser.DERNIERECONNEXIONREUSSIE = null;

                                return new Status(true, null);
                            }
                        }
                        else
                        {
                            //Verifier si DateDerniereConnexion + delai Reinitialisation < Now
                            if (pUser.DATEDERNIERECONNEXION == null || ((DateTime)pUser.DATEDERNIERECONNEXION).AddMinutes(Convert.ToDouble(pSecurite.REINITIALISERCOMPTEURVERROUILLAGECOMPTEAPRES)) < DateTime.Now)
                            {
                                //Si oui
                                if (!VerifierCorrespondancePasword(pUser, pPassword, true))
                                {
                                    pUser.DERNIERECONNEXIONREUSSIE = false;
                                    if (pUser.NOMBREECHECSOUVERTURESESSION == null)
                                        pUser.NOMBREECHECSOUVERTURESESSION = 1;
                                    else
                                        pUser.NOMBREECHECSOUVERTURESESSION = pUser.NOMBREECHECSOUVERTURESESSION + 1;


                                    if (pUser.MATRICULE != "99999" && (pUser.NOMBREECHECSOUVERTURESESSION >= pSecurite.NOMBREMAXIMALECHECSOUVERTURESESSION))
                                    {
                                        pUser.DATEDERNIERVERROUILLAGE = pUser.DATEDERNIERECONNEXION;
                                        pUser.FK_IDSTATUS = (byte)Global.StatusCompte.VerrouilleOuvertureSession;
                                        return new Status(true, new Exception(string.Format(Langue.MsgErrMaxEchecConct, pUser.LOGINNAME, pUser.MATRICULE)));
                                    }
                                    else
                                        return new Status(true, new Exception(Langue.MsgErrPwd));
                                }
                                else
                                {
                                    pUser.NOMBREECHECSOUVERTURESESSION = 0;
                                    pUser.DERNIERECONNEXIONREUSSIE = null;

                                    return new Status(true, null);
                                }
                            }
                            else
                            {
                                // Sinon
                                //Incrémenter le nombre d'échecs
                                if (pUser.NOMBREECHECSOUVERTURESESSION == null)
                                    pUser.NOMBREECHECSOUVERTURESESSION = 1;
                                else
                                    pUser.NOMBREECHECSOUVERTURESESSION = pUser.NOMBREECHECSOUVERTURESESSION + 1;

                                //Si le nombre d'échecs a atteint le nombre maxi d'échecs autorisé
                                if (pUser.MATRICULE != "99999" && (pUser.NOMBREECHECSOUVERTURESESSION >= pSecurite.NOMBREMAXIMALECHECSOUVERTURESESSION))
                                {
                                    pUser.DATEDERNIERVERROUILLAGE = pUser.DATEDERNIERECONNEXION;
                                    pUser.FK_IDSTATUS = (byte)Global.StatusCompte.VerrouilleOuvertureSession;
                                    //dbAdmUsers.Update(pUser);
                                    return new Status(true, new Exception(string.Format(Langue.MsgErrMaxEchecConct, pUser.LOGINNAME, pUser.MATRICULE)));
                                }
                                //Si le nombre maxi d'échecs autorisé n'est pas atteint
                                if(!VerifierCorrespondancePasword(pUser, pPassword,true))
                                    return new Status(true, new Exception(Langue.MsgErrPwd));
                                else
                                    return new Status(true, null);
                            }
                        }

                    }
                }

                //return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        static bool VerifierCorrespondancePasword(CsUtilisateur pUser,string pPassword)
        {
         
            string _password;
            _password =  Cryptage.Encrypt(pPassword);

            return (_password == pUser.PASSE);
             
        }

        static bool VerifierCorrespondancePasword(CsUtilisateur pUser, string pPassword,bool tobeUpdate)
        {

            string _password;
            _password = Cryptage.Encrypt(pPassword);
            bool _match = (_password == pUser.PASSE);

            if (tobeUpdate)
            {
                pUser.DATEDERNIERECONNEXION = DateTime.Now;
                pUser.DERNIERECONNEXIONREUSSIE = _match;
                return (bool)pUser.DERNIERECONNEXIONREUSSIE;
            }
            else
                return _match;
            
        }

        public static int GetNombreJourAvantExpiration(CsUtilisateur pUser,CsStrategieSecurite pSecurity, bool pPasswordExpireJamais)
        {
            pPasswordExpireJamais = false;

            //AdmStrategieSecurite StrategieSecuriteActif = new DBAdmStrategieSecurite().GetActif();
            pPasswordExpireJamais = pSecurity.DUREEMAXIMALEPASSWORD == 0;
            if (pPasswordExpireJamais) return 0;

            Nullable<System.DateTime> DateExpiration = new Nullable<System.DateTime>();
            if (pUser.DATEDERNIEREMODIFICATIONPASSWORD != null)
            {
                DateExpiration = ((Nullable<System.DateTime>)pUser.DATEDERNIEREMODIFICATIONPASSWORD).Value.AddDays(Convert.ToDouble(pSecurity.DUREEMAXIMALEPASSWORD));
            }
            else
                if (pUser.DATECREATION != null)
                   DateExpiration = ((DateTime?)pUser.DATECREATION).Value.AddDays(Convert.ToDouble(pSecurity.DUREEMAXIMALEPASSWORD));
            
            if (DateExpiration.Value <= DateTime.Now)
                return -1;

            int nbJour = 0;
            while (DateExpiration >= DateTime.Now)
            {
                nbJour++;
                DateExpiration = DateExpiration.Value.AddDays(-1);
            }

            return nbJour;
        }

        /// <summary>
        /// procedure de changement de password par l'utilisateur lui-même
        /// </summary>
        /// <param name="admUser"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <param name="confirmPassword"></param>
        public static bool ChangePassword(CsUtilisateur pUser, CsStrategieSecurite pSecurity, string oldPassword, string newPassword, string confirmPassword, bool TenirCompteAncienPwd)
        {

            //Verifier si le changement de mot de passe peut se faire par rapport à la stratégie            
            //Durée Minimale Password
            if (pUser.INITUSERPASSWORD != null && !(bool)pUser.INITUSERPASSWORD && pSecurity.DUREEMINIMALEPASSWORD != 0)
            {
                if (pUser.DATEDERNIEREMODIFICATIONPASSWORD != null && ((DateTime)pUser.DATEDERNIEREMODIFICATIONPASSWORD).AddDays(Convert.ToDouble(pSecurity.DUREEMINIMALEPASSWORD)) > DateTime.Now)
                    throw new Exception(Langue.MsgModPwd);
            }

            if(TenirCompteAncienPwd)
               if (pUser.PASSE != Cryptage.Encrypt(oldPassword))  //EncryptWithOutKey(oldPassword)
                  throw new Exception(Langue.MsgErrAncPwd);

            if (newPassword != confirmPassword)
                throw new Exception(Langue.MsgErrConfPwd);



            if (pSecurity.NEPASCONTENIRNOMCOMPTE)
            {
                if (newPassword.ToLower().Contains(pUser.LOGINNAME.ToLower()))
                    throw new Exception(Langue.MsgPwdCpt);
            }

            pUser.PASSE = Cryptage.GetPasswordToBeSaved(pSecurity, pUser.LIBELLE, newPassword);
            pUser.INITUSERPASSWORD = false;
            pUser.DATEDERNIEREMODIFICATIONPASSWORD = DateTime.Now;
            //new DBAdmUsers().Update(admUser);
            return true;

        }

    }

    
    public class Status
    {
     public bool Isupdated {set;get;}
     public Exception ex {set;get;}
     public Status(bool pVerif,Exception pEx)
     {
         ex = pEx; Isupdated = pVerif;
     }
    }
}
