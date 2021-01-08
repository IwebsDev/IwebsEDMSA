using System;

namespace Galatee.DataAccess
{

    #region Enumerations

    public enum STATUSDEMANDE
    {
        Initiee = 1,
        EnAttenteValidation = 2,
        Suspendue = 3,
        Annulee = 4,
        Rejetee = 5,
        Terminee = 6
    }

    public enum CODEACTION
    {
        TRANSMETTRE = 1,
        REJETER = 2,
        ANNULER = 3,
        SUSPENDRE = 4
    }

    public enum RESULTACTION
    {
        TRANSMISE = 1,
        REJETEE = 2,
        FINDECIRCUIT = 3,
        DEBUTDECIRCUIT = 4,
        ERREURINCONNUE = 5,
        ANNULEE = 6,
        SUSPENDUE = 7
    }

    public enum OPERATEUR
    {
        GreatherThan = 1,
        GreaterOrEquals = 2,
        LessThan = 3,
        LessOrEquals = 4,
        Different = 5,
        Equals = 6,
        None = -1
    }

    #endregion

    public static class EnumerationString
    {

        public static OPERATEUR GetOperateurEnum(string strOperateur)
        {
            if (string.Empty != strOperateur)
            {
                switch (strOperateur)
                {
                    case ">": return OPERATEUR.GreatherThan;                        
                    case ">=": return OPERATEUR.GreaterOrEquals;                        
                    case "<": return OPERATEUR.LessThan;                        
                    case "<=": return OPERATEUR.LessOrEquals;                        
                    case "<>": return OPERATEUR.Different;    
                    case "=" : return OPERATEUR.Equals;
                    default: return OPERATEUR.None;
                }
            }
            else return OPERATEUR.None;
        }

    }

}