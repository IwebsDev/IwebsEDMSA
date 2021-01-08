using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Galatee.DataAccess.Common
{
    public class Messages
    {
        #region Définitions des variables privées globales à la classe
        private static Dictionary<int, string> _lstMessages = null;
        private static object _token = new object();
        #endregion

        #region Propriétés de classe, Accesseurs

        #endregion

        #region Traitements (logique métier)
        public static string getMessages(EnumAppMessage leTypeMessage)
        {
            getListeMessages();
            int intNumeroMessage = (int)leTypeMessage;
            if (_lstMessages != null)
            {
                if (_lstMessages.ContainsKey(intNumeroMessage))
                    return _lstMessages[intNumeroMessage];
            }
            return _lstMessages[(int)EnumAppMessage.AucunMessageDisponible];

        }
        private static Dictionary<int, string> getListeMessages()
        {
            if (_lstMessages == null)
            {
                lock (_token)
                {
                    _lstMessages = new Dictionary<int, string>();
                    _lstMessages.Add((int)EnumAppMessage.AucunMessageDisponible, "Aucun Message disposible");
                    _lstMessages.Add((int)EnumAppMessage.ConnexionImpossible, "Connexion impossible: Veuillez verifier les paramètres de connexion");
                    _lstMessages.Add((int)EnumAppMessage.ConnexionUtilisateur, "Connexion utilisateur");
                    _lstMessages.Add((int)EnumAppMessage.AppLibelleLong, "Pertes Non Techniques");
                    _lstMessages.Add((int)EnumAppMessage.AppLibelleCourt, "PNT");
                    _lstMessages.Add((int)EnumAppMessage.UniteOrganisationnelle, "Unité Organisationnelle");
                    _lstMessages.Add((int)EnumAppMessage.QuitterAppli, "Voulez-vous quitter PNT?");
                }
            }
            return _lstMessages;
        }
        #endregion

        #region Appels composant accès aux données
        #endregion

    }
}
