using Galatee.Entity.Model;
using Galatee.Structure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Threading;
using System.Data.Entity;

namespace Galatee.DataAccess
{
    public class ConditionChecker
    {

        #region Privée

        static Type GetListType(object list)
        {
            Type type = list.GetType();
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                return type.GetGenericArguments()[0];
            else
                throw new ArgumentException("list is not a List<T>", "list");
        }

        private static object GetObjectValueFromTable(string TABLEREFERENCE, galadbEntities Context, string IDLine,
            ref Type objType)
        {
            object theLine = default(object);
            //return (IList)instance;

            string NameSpaceClass = "Galatee.Entity.Model." + TABLEREFERENCE;
            Type classType = Type.GetType(NameSpaceClass);
            
            var dbSets = typeof(galadbEntities).GetProperties(BindingFlags.Public |
                                                           BindingFlags.Instance);
            var monDbSet = dbSets.Where(pi => pi.PropertyType.IsGenericTypeDefinition &&
                               pi.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) &&
                               pi.Name == TABLEREFERENCE)
                               .FirstOrDefault();             
            
            dynamic TABLE = Context.GetType().InvokeMember(TABLEREFERENCE.Trim(),
                BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance,
                null, Context, null);

            object theList = System.Linq.Enumerable.ToList((dynamic)TABLE);
            objType = GetListType(theList);

            var abon = Context.ABON;
            
            var listType = typeof(List<>);
            var constructedListType = listType.MakeGenericType(objType);
            var theCorrectList = Activator.CreateInstance(constructedListType);
            
            //On a le type, on va recuperer l'élement même de la table    
            Parallel.ForEach((IEnumerable<dynamic>)theList, (item, state) =>
                {
                    if (item.PK_ID.ToString() == IDLine)
                    {
                        theLine = item;

                        //On a trouve l'élément, on arrête le parallel foreach
                        state.Break();
                    }
                });

            //Disposition des variables
            GC.SuppressFinalize(listType);
            GC.SuppressFinalize(constructedListType);
            GC.SuppressFinalize(theCorrectList);
            GC.SuppressFinalize(objType);

            return theLine;
        }

        #endregion

        public static bool CheckIfConditionIsRespected(string Condition, string TableName, ref string msgErr, string IDLine,
            ref bool isOk)
        {
            bool isRespected = true;
            try
            {
                Type T = null;
                galadbEntities context = new galadbEntities();
                object theLine = GetObjectValueFromTable(TableName, context, IDLine, ref T);
                if (null != theLine)
                {
                    //On a l'objet et le type de l'objet, on va tester donc la condition
                    string[] tokens = Condition.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    //normalement on a trois tokens
                    if (tokens.Count() == 3)
                    {
                        string columnName = tokens[0]; //Nom de la colonne;
                        OPERATEUR op = EnumerationString.GetOperateurEnum(tokens[1]); //l'Operateur
                        string value = tokens[2];   //La valeur

                        //Maintenant on compile notre If
                        var currentValue = theLine.GetType().GetProperty(columnName).GetValue(theLine);
                        Type propType = theLine.GetType().GetProperty(columnName).GetMethod.ReturnType;
                        if (propType.IsGenericType && propType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                        {
                            //Si le type est nullable alors, on récupère la destination
                            //Par exemple si c'est Nullable<int>, la destination est int
                            propType = Nullable.GetUnderlyingType(propType);
                        }
                        var correctValue = null != currentValue ? Convert.ChangeType(currentValue, propType) : null;
                        var conditionCorrectValue = "null" != value.ToLower() ? Convert.ChangeType(GetValueFromString(value), propType) : null;

                        //Bon sincèrement il n'ya pas de plusieurs type de données dans la base généralement
                        //ce qui est sûr on teste 
                        switch (op)
                        {
                            case OPERATEUR.Equals:
                                isRespected = (correctValue == conditionCorrectValue);
                                break;
                            case OPERATEUR.Different:
                                isRespected = (correctValue != conditionCorrectValue);
                                break;
                            case OPERATEUR.GreaterOrEquals:
                            case OPERATEUR.GreatherThan:
                            case OPERATEUR.LessOrEquals:
                            case OPERATEUR.LessThan:
                                {
                                    //peut importe, le type, on utilise double qui est plus grand que
                                    //int, decimal
                                    double dbValueLine = double.Parse(correctValue.ToString());
                                    double dbValueCondition = double.Parse(conditionCorrectValue.ToString());

                                    if (op == OPERATEUR.GreatherThan) isRespected = dbValueLine > dbValueCondition;
                                    else if (op == OPERATEUR.GreaterOrEquals) isRespected = dbValueLine >= dbValueCondition;
                                    else if (op == OPERATEUR.LessThan) isRespected = dbValueLine < dbValueCondition;
                                    else if (op == OPERATEUR.LessOrEquals) isRespected = dbValueLine <= dbValueCondition;
                                };
                                break;
                            default:
                                isRespected = true;
                                break;
                        }
                        
                        //Disposition des var
                        if (null != currentValue) GC.SuppressFinalize(currentValue);
                        if (null != correctValue) GC.SuppressFinalize(correctValue);
                        if (null != conditionCorrectValue) GC.SuppressFinalize(conditionCorrectValue);
                        if (null != propType) GC.SuppressFinalize(propType);

                        isOk = true;
                    }

                    else
                    {
                        msgErr = "La valeur n'existe pas dans la base de données";
                        isOk = false;
                        isRespected = false;
                    }

                    //disposition des variables
                    context.Dispose();

                    GC.SuppressFinalize(context);
                    GC.SuppressFinalize(theLine);
                    GC.SuppressFinalize(T);
                }                
            }
            catch (Exception ex)
            {
                msgErr = ex.Message;
                isOk = false;
                isRespected = false;
            }
            return isRespected;
        }

        public static bool CheckIfConditionIsRespected<T>(string Condition, T table, ref string msgErr, ref bool isOk)
            where T : class, new()
        {
            bool isRespected = true;
            //On a l'objet et le type de l'objet, on va tester donc la condition
            string[] tokens = Condition.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            //normalement on a trois tokens
            if (tokens.Count() == 3)
            {
                string columnName = tokens[0]; //Nom de la colonne;
                OPERATEUR op = EnumerationString.GetOperateurEnum(tokens[1]); //l'Operateur
                string value = tokens[2];   //La valeur

                //Maintenant on compile notre If
                Type propType = null;
                object currentValue = RetourneValueFromClasse<T>(table, columnName, ref propType);
                if (propType.IsGenericType && propType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    //Si le type est nullable alors, on récupère la destination
                    //Par exemple si c'est Nullable<int>, la destination est int
                    propType = Nullable.GetUnderlyingType(propType);
                }
                var correctValue = null != currentValue ? Convert.ChangeType(currentValue, propType) : null;
                var conditionCorrectValue = "null" != value.ToLower() ? Convert.ChangeType(GetValueFromString(value), propType) : null;

                //Bon sincèrement il n'ya pas de plusieurs type de données dans la base généralement
                //ce qui est sûr on teste 
                switch (op)
                {
                    case OPERATEUR.Equals:
                        isRespected = (correctValue.ToString() == conditionCorrectValue.ToString()) ;
                        break;
                    case OPERATEUR.Different:
                        {
                            isRespected = (correctValue.ToString() != conditionCorrectValue.ToString());
                            break;
                        }
                    case OPERATEUR.GreaterOrEquals:
                    case OPERATEUR.GreatherThan:
                    case OPERATEUR.LessOrEquals:
                    case OPERATEUR.LessThan:
                        {
                            //peut importe, le type, on utilise double qui est plus grand que
                            //int, decimal
                            double dbValueLine = double.Parse(correctValue.ToString());
                            double dbValueCondition = double.Parse(conditionCorrectValue.ToString());

                            if (op == OPERATEUR.GreatherThan) isRespected = dbValueLine > dbValueCondition ? true : false;
                            else if (op == OPERATEUR.GreaterOrEquals) isRespected = dbValueLine >= dbValueCondition ? true : false;
                            else if (op == OPERATEUR.LessThan) isRespected = dbValueLine < dbValueCondition ? true : false;
                            else if (op == OPERATEUR.LessOrEquals) isRespected = dbValueLine <= dbValueCondition ? true : false;
                        };
                        break;
                    default:
                        isRespected = true;
                        break;
                }

                //Disposition des var
                if (null != currentValue) GC.SuppressFinalize(currentValue);
                if (null != correctValue) GC.SuppressFinalize(correctValue);
                if (null != conditionCorrectValue) GC.SuppressFinalize(conditionCorrectValue);
                if (null != propType) GC.SuppressFinalize(propType);

                isOk = true;
            }

            else
            {
                msgErr = "La valeur n'existe pas dans la base de données";
                isOk = false;
                isRespected = false;
            }

            return isRespected;
        }

        static object GetValueFromString(string _value)
        {
            if ("true" == _value.ToLower() || "vrai" == _value.ToLower() || "vraie" == _value.ToLower()) return true;
            else if ("false" == _value.ToLower() || "faux" == _value.ToLower() || "fausse" == _value.ToLower()) return false;
            else return _value;
        }

        static object RetourneValueFromClasse<T>(T _Classe, string _Colonne, ref Type propType) where T : class, new()
        {
            try
            {
                object LavaleurColonne = string.Empty;
                PropertyInfo[] properties1 = _Classe.GetType().GetProperties();
                for (int attrNum = 0; attrNum < properties1.Length; attrNum++)
                {
                    if (properties1[attrNum].Name.Equals(_Colonne))
                    {
                        string t = string.Empty;
                        LavaleurColonne = properties1[attrNum].GetValue(_Classe, null);
                        propType = properties1[attrNum].GetMethod.ReturnType;
                        break;
                    }
                }

                return LavaleurColonne;
            }
            catch (Exception ex)
            {                
                throw;
            }
        }

    }
}

