using GISWaterSupplyAndSewageServer.CommonTools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace GISWaterSupplyAndSewageServer.Database
{
    public class ConnectionFactory
    {
        private static IConfiguration _configuration;
        public enum DBConnNames
        {
            GISWaterSupplyAndSewageServer,
            PipeInspection_Smart_Water,
            PipeInspectionBase_Gis_DB,
            PipeInspectionBase_Gis_OutSide,
            PipeInspectionSmartNet,
            GISDB,
            ORCL,
            SDE,
            KM,
            CSSDE
        }
        public static IDbConnection GetDBConn(DBConnNames dbConnName)
        {
            return CreateConnection(Appsettings.app(new string[] { dbConnName.ToString() }));
        }

        private static IDbConnection CreateConnection(string connString)
        {
            IDbConnection conn = null;
            string DBName = Appsettings.app(new string[] { "DBName" });
            switch (DBName)
            {
                case "SQLServer":
                    conn = new SqlConnection(connString);
                    break;
                case "PostgreSQL":
                    conn = new NpgsqlConnection(connString);
                    break;
                case "OracleDAL":
                    conn = new OracleConnection(connString);
                    break;
                default:
                    conn = new SqlConnection(connString);
                    break;
            }

            conn.Open();
            return conn;
        }

        public static IDisposable GetDBConn(object dBConnName)
        {
            throw new NotImplementedException();
        }
    }
}
