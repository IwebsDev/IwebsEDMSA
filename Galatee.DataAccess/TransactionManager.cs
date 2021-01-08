
using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.SqlClient;

namespace Galatee.DataAccess
{
    /// <summary>
    /// Gestionnaire des transactionTransactionManager.
    /// </summary>
    public class TransactionManager
    {
        //private Database database;

        //private SqlDatabase database;

        private DbConnection connection;
        private DbTransaction transaction;

        private string _ConnectionStrings_Name = "";
        private bool _transactionOpen = false;

        public SqlCommand cmd { get; set; }
  

        /// <summary>
        ///	Nom de chaîne de Chaine de connexion.
        /// </summary>
        public string ConnectionStrings_Name
        {
            get { return _ConnectionStrings_Name; }
        } 

        /// <summary>
        ///		Constructeur .
        /// </summary>
        //public  TransactionManager() : this(""){}


        /// <summary>
        ///		Constructeur avec paramètres Nom de chaîne de Chaine de connexion.
        /// </summary>
        /// <param name="connectionStrings_Name">om de chaîne de Chaine de connexion..</param>
        public TransactionManager(string connectionStrings_Name)
        {

            this._ConnectionStrings_Name = Session.GetSqlConnexionString();
            //this.database = new DBRoot(connectionStrings_Name).GetDatabase();
            //this.database = Session.GetDatabase();
            //this.connection = this.database.CreateConnection();
        }


       // /// <summary>
       // /// Instance de <see cref="Database"/>.
       // /// </summary>
       // /// <value></value>
       //// public Database Database
       // public SqlDatabase Database
       // {
       //     get { return this.database; }
       // }


        /// <summary>
        ///	Récupération de l'objet <see cref="DbTransaction"/>.
        /// </summary>
        public DbTransaction TransactionObject
        {
            get { return this.transaction; }
        }

        /// <summary>
        ///	Flags déterminant l'état de la transaction (ouverte ou non ouverte)
        /// </summary>
        public bool IsOpen
        {
            get { return _transactionOpen; }
        }

        /// <summary>
        ///		Démarrage de la transaction.
        /// </summary>
        public void BeginTransaction()
        {
            BeginTransaction(IsolationLevel.ReadCommitted);
        }


        /// <summary>
        ///		Démarrage de la transaction.
        /// </summary>
        /// <param name="isolationLevel">Avec "isolation level" préciser</param>
        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            if (IsOpen)
                throw new InvalidOperationException("Transaction déja ouverte.");

            try
            {
                this.connection.Open();
                this.transaction = this.connection.BeginTransaction(isolationLevel);
                this._transactionOpen = true;
                this.cmd = new SqlCommand(_ConnectionStrings_Name );
            }
            catch (Exception)
            {
                if (this.connection != null) this.connection.Close();
                if (this.transaction != null) this.transaction.Dispose();
                throw;
            }
        }

        /// <summary>
        ///		Validation de la transaction.
        /// </summary>
        public void Commit()
        {
            if (!this.IsOpen)
            {
                throw new InvalidOperationException("Il faut d'abord démarrer la transaction");
            }

            this.transaction.Commit();
            this.connection.Close();
            this.transaction.Dispose();
            this._transactionOpen = false;
        }

        /// <summary>
        ///	Annulation de la transaction.
        /// </summary>
        public void Rollback()
        {
            if (!this.IsOpen)
            {
                throw new InvalidOperationException("Il faut d'abord démarrer la transaction.");
            }

            this.transaction.Rollback();
            this.connection.Close();
            this.transaction.Dispose();
            this._transactionOpen = false;
        }

    }
}
