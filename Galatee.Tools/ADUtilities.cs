using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.DirectoryServices;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;

namespace Galatee.Tools
{
    /// <summary>
    /// ADUtilities intéragie avec un annuaire Active Directory LDAP.
    /// </summary>
    public class ADUtilities
    {

        #region Propriétés

        DirectoryEntry _LDAPConnexion;
        PrincipalContext _principalContext;
        string _userName;
        string _passwd;
        string _domaineName;
        bool _secureConnexion = false;

        public DirectoryEntry LDAPConnexion
        {
            get { return _LDAPConnexion; }
        }        

        public string Domaine { get { return _domaineName; } }

        public string UserName { get { return _userName; } }

        public string Password { get { return _passwd; } }

        #endregion

        #region Constructeurs               

        public ADUtilities(string userName, string psswd, string adresseDomaine)
        {
            _userName = userName;
            _passwd = psswd;
            _domaineName = adresseDomaine;

            _principalContext = new PrincipalContext(ContextType.Domain, _domaineName, _userName, _passwd);

            _LDAPConnexion = new DirectoryEntry();
            _LDAPConnexion.Path = _domaineName;
            _LDAPConnexion.Username = _userName;
            _LDAPConnexion.Password = _passwd;
            _LDAPConnexion.AuthenticationType = AuthenticationTypes.Secure;

            _secureConnexion = true;
        }

        /// <summary>
        /// Instancie la classe ADUtilities, en précisant le chemin du domaine actuel
        /// </summary>
        /// <param name="CheminDomaine">Chemin du domaine</param>
        public ADUtilities(params string[] DC)
        {
            _domaineName = "LDAP://";
            foreach (string __str in DC)
            {
                _domaineName += __str +",";
            }
            _domaineName  = _domaineName.Substring(0, _domaineName.Length - 1);
            //_domaineName = "LDAP://DC=" + CheminDomaine;
            CreerDirectoryEntry();
        }

        public ADUtilities(string userName, string psswd, params string[] DC)
        {
            _userName = userName;
            _passwd = psswd;
            _domaineName = "LDAP://";
            foreach (string __str in DC)
            {
                _domaineName += __str + ",";
            }
            _domaineName = _domaineName.Substring(0, _domaineName.Length - 1);
        }

        #endregion

        #region Méthodes

        internal void CreerDirectoryEntry()
        {
            if (null != _domaineName && string.Empty != _domaineName)
            {
                _LDAPConnexion = new DirectoryEntry();
                _LDAPConnexion.Path = _domaineName;
                _LDAPConnexion.AuthenticationType = AuthenticationTypes.Secure;
            }
        }

        internal List<ADUser> RecupererUtilisateur()
        {
            if (null != _LDAPConnexion)
            {
                List<ADUser> LesUtilisateurs = new List<ADUser>();

                DirectorySearcher searcher = new DirectorySearcher(_LDAPConnexion);
                searcher.Filter = "(&(objectClass=user)(objectCategory=person))";
                searcher.PropertiesToLoad.Add("samaccountname");
                searcher.PropertiesToLoad.Add("mail");
                searcher.PropertiesToLoad.Add("usergroup");
                searcher.PropertiesToLoad.Add("displayname");
                SearchResult result;
                SearchResultCollection collectionResult = searcher.FindAll();
                if (null != collectionResult)
                {
                    for (int i = 0; i < collectionResult.Count; i++)
                    {
                        result = collectionResult[i];
                        ADUser u = new ADUser();
                        try
                        {
                            u.UserName = (null != result.Properties["samaccountname"][0]) ? (string)result.Properties["samaccountname"][0]
                                : string.Empty;
                        }
                        catch { u.UserName = string.Empty; }

                        try
                        {
                            u.DisplayName = (null != result.Properties["displayname"][0]) ? (string)result.Properties["displayname"][0]
                                : string.Empty;
                        }
                        catch { u.DisplayName = string.Empty; }

                        try
                        {
                            u.Email = (null != result.Properties["mail"][0]) ? (string)result.Properties["mail"][0]
                                : string.Empty;
                        }
                        catch { u.Email = string.Empty; }

                        try
                        {
                            u.Groupe = (null != result.Properties["usergroup"][0]) ? (string)result.Properties["usergroup"][0]
                                : string.Empty;
                        }
                        catch { u.Groupe = string.Empty; }

                        LesUtilisateurs.Add(u);
                    }
                }
                return LesUtilisateurs;
            }
            return null;
        }

        internal List<ADUser> RecupererUtilisateurWithSecurity()
        {
            if (null != _principalContext)
            {
                List<ADUser> LesUtilisateurs = new List<ADUser>();
                using (var searcher = new PrincipalSearcher(new UserPrincipal(_principalContext)))
                {
                    foreach (var result in searcher.FindAll())
                    {
                        DirectoryEntry de = result.GetUnderlyingObject() as DirectoryEntry;
                        ADUser u = new ADUser();
                        try
                        {
                            u.UserName = (null != de.Properties["samaccountname"][0]) ? (string)de.Properties["samaccountname"][0]
                                : string.Empty;
                        }
                        catch { u.UserName = string.Empty; }

                        try
                        {
                            u.DisplayName = (null != de.Properties["displayname"][0]) ? (string)de.Properties["displayname"][0]
                                : string.Empty;
                        }
                        catch { u.DisplayName = string.Empty; }

                        try
                        {
                            u.Email = (null != de.Properties["mail"][0]) ? (string)de.Properties["mail"][0]
                                : string.Empty;
                        }
                        catch { u.Email = string.Empty; }

                        try
                        {
                            u.Groupe = (null != de.Properties["usergroup"][0]) ? (string)de.Properties["usergroup"][0]
                                : string.Empty;
                        }
                        catch { u.Groupe = string.Empty; }

                        LesUtilisateurs.Add(u);
                    }
                }
                return LesUtilisateurs;
            }
            else return null;
        }

        /// <summary>
        /// Retourne la liste des utilisateurs de façon asynchrone.
        /// </summary>
        /// <example>var users = await GetUserAsync()</example>
        /// <returns>Liste des utilisateurs Active Directory</returns>
        /// <exception cref="ADException"></exception>
        public async Task<List<ADUser>> GetUserAsync()
        {
            try
            {
                if (_secureConnexion) return this.RecupererUtilisateurWithSecurity();
                else return this.RecupererUtilisateur();
            }
            catch (Exception ex) { throw new ADException(ex.Message); }
        }

        /// <summary>
        /// Retourne la liste des utilisateurs de façon.
        /// </summary>        
        /// <returns>Liste des utilisateurs Active Directory</returns>
        /// <exception cref="ADException"></exception>
        public List<ADUser> GetUser()
        {
            try
            {
                if (_secureConnexion) return this.RecupererUtilisateurWithSecurity();
                else return this.RecupererUtilisateur();
            }
            catch (Exception ex) { throw new ADException(ex.Message); }
        }

        #endregion

    }
}
