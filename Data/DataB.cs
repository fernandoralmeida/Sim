﻿using System;
using System.Data;
using System.Data.OleDb;

namespace Sim.Data
{
    class DataB : IData, IDisposable
    {
        /*
 * 
var dtUser = new DataTable();
dt.Columns.Add("ID",typeof(int));
dt.Columns.Add("Name",typeof(string));
List<User> userList = new List<User>(); //This will be filled with values
userList.ForEach(u => dtUser.Rows.Add(u.ID,u.Name));
 * 
 */

        private OleDbParameterCollection ParameterCollection = new OleDbCommand().Parameters;

        private OleDbConnection Conectar()
        {
            return new OleDbConnection(Connections.NetworkModules());
        }

        public void ClearParameters()
        {
            ParameterCollection.Clear();
        }

        public void AddParameters(string parameterName, object parameterValue)
        {
            ParameterCollection.Add(new OleDbParameter(parameterName, parameterValue));
        }

        public DataTable Read(string commandOrStoredProcedure)
        {

            OleDbConnection dbConnection = Conectar();

            try
            {

                dbConnection.Open();

                OleDbCommand dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandType = CommandType.Text;
                dbCommand.CommandText = commandOrStoredProcedure;

                foreach (OleDbParameter p in ParameterCollection)
                {
                    dbCommand.Parameters.Add(new OleDbParameter(p.ParameterName, p.Value));
                }

                DataTable dt = new DataTable();

                new OleDbDataAdapter(dbCommand).Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public bool Write(string commandOrStoredProcedure)
        {

            OleDbConnection dbConnection = Conectar();

            dbConnection.Open();

            //OleDbTransaction dbTransaction = dbConnection.BeginTransaction();

            try
            {

                OleDbCommand dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = commandOrStoredProcedure;
                dbCommand.CommandType = CommandType.Text;

                //dbCommand.Transaction = dbTransaction;

                foreach (OleDbParameter p in ParameterCollection)
                {
                    dbCommand.Parameters.Add(new OleDbParameter(p.ParameterName, p.Value));
                }

                dbCommand.ExecuteNonQuery();
                //dbTransaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                //dbTransaction.Rollback();
                return false;
                throw new Exception(ex.Message);
            }
            finally
            {
                //dbTransaction.Dispose();
                //dbTransaction = null;
                dbConnection.Close();
                dbConnection.Dispose();
                //dbConnection = null;
                this.Dispose();
            }
        }

        public void WriteR(string commandOrStoredProcedure)
        {

            OleDbConnection dbConnection = Conectar();

            dbConnection.Open();

            OleDbTransaction dbTransaction = dbConnection.BeginTransaction();

            //try
            //{

            OleDbCommand dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = commandOrStoredProcedure;
            dbCommand.CommandType = CommandType.Text;

            dbCommand.Transaction = dbTransaction;

            foreach (OleDbParameter p in ParameterCollection)
            {
                dbCommand.Parameters.Add(new OleDbParameter(p.ParameterName, p.Value));
            }

            dbCommand.ExecuteNonQuery();

            dbTransaction.Commit();
            //return true;
            //}
            //catch (Exception ex)
            //{
            //dbTransaction.Rollback();
            //return false;
            //throw new Exception(ex.Message);
            //}
            //finally
            //{
            dbTransaction.Dispose();
            dbTransaction = null;
            dbConnection.Close();
            dbConnection.Dispose();
            dbConnection = null;
            this.Dispose();
            //}
        }

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
