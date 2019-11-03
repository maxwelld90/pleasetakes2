using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PleaseTakes.Core.Helpers.Database {

	internal class ParameterBuilder {
		private readonly List<SqlParameter> _parameters = new List<SqlParameter>();

		public List<SqlParameter> Parameters {
			get {
				return this._parameters;
			}
		}

		public void AddParameter(SqlDbType sqlDbType, string parameterName, object parameterValue) {
			SqlParameter parameter = new SqlParameter(parameterName, sqlDbType);
			parameter.Value = parameterValue ?? DBNull.Value;
			this._parameters.Add(parameter);
		}

		public SqlParameter AddOutputParameter(SqlDbType sqlDbType, string parameterName) {
			SqlParameter parameter = new SqlParameter(parameterName, sqlDbType);
			parameter.Direction = ParameterDirection.Output;
			this._parameters.Add(parameter);

			return parameter;
		}
	}

}
