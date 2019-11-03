using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PleaseTakes.Core.Helpers.Database {

	internal sealed class Provider {

		private static SqlConnection CreateConnection() {
			try {
				SqlConnection connection = new SqlConnection(WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Application.Database.ConnectionString);
				connection.Open();

				return connection;
			}
			catch (SqlException e) {
				throw new DatabaseConnectionException(e.Message);
			}
		}

		public static void TestConnection() {
			try {
				SqlConnection connection = new SqlConnection(WebServer.PleaseTakes.Session.CurrentInstance.School.Settings.Application.Database.ConnectionString);
				connection.Open();
			}
			catch (SqlException e) {
				throw new DatabaseConnectionTestException(e.Message);
			}
		}

		private static string GetSqlCommand(string path) {
			return (new Constructor(path).ToString());
		}

		public static SqlDataReader ExecuteReader(string sqlCommandPath, List<SqlParameter> parameters) {
			SqlConnection connection = Provider.CreateConnection();
			SqlCommand command = new SqlCommand(Provider.GetSqlCommand(sqlCommandPath), connection);

			if (parameters != null)
				command.Parameters.AddRange(parameters.ToArray());

			return command.ExecuteReader(CommandBehavior.CloseConnection);
		}

		public static DataTable GetDataTable(SqlDataReader reader) {
			DataTable dataTable = new DataTable();
			dataTable.Load(reader);

			return dataTable;
		}

		public static object ExecuteScalar(string sqlCommandPath, List<SqlParameter> parameters) {
			object scalar;

			using (SqlConnection connection = Provider.CreateConnection()) {
				SqlCommand command = new SqlCommand(Provider.GetSqlCommand(sqlCommandPath), connection);

				if (parameters != null)
					command.Parameters.AddRange(parameters.ToArray());

				scalar = command.ExecuteScalar();
			}

			return scalar;
		}

		public static int ExecuteNonQuery(string sqlCommandPath, List<SqlParameter> parameters) {
			int rowsAffected;

			using (SqlConnection connection = Provider.CreateConnection()) {
				SqlCommand command = new SqlCommand(Provider.GetSqlCommand(sqlCommandPath), connection);

				if (parameters != null)
					command.Parameters.AddRange(parameters.ToArray());

				rowsAffected = command.ExecuteNonQuery();
			}

			return rowsAffected;
		}

	}

	public class DatabaseConnectionException : Exception {
		public DatabaseConnectionException(string message) : base(message) { }
	}

	public class DatabaseConnectionTestException : Exception {
		public DatabaseConnectionTestException(string message)
			: base(message) {
			WebServer.PleaseTakes.Session.ScrubInstance();
		}
	}

}