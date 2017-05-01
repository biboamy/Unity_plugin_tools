using System.Data;
using System;
using UnityEngine;
using Mono.Data.Sqlite;

namespace cqplayart.adv.sql
{
	/// <summary>
	/// Sql behavior.
	/// </summary>
	public class sqlBehavior : MonoBehaviour, mySQL
	{
		private SqliteConnection dbConnection;
		private SqliteCommand dbCommand;
		private SqliteDataReader reader;

		private static mySQLCallback mycallback; 

		/// <summary>
		/// 創建 database
		/// </summary>
		/// <returns>database.</returns>
		public sqlBehavior databaseConnection(string path)
		{
			sqlBehavior db = null;

			//initial database connection path
			if(Application.platform == RuntimePlatform.OSXEditor)
				db = new sqlBehavior("data source=" + Application.persistentDataPath + path);
			else if(Application.platform == RuntimePlatform.Android)
	 			db = new sqlBehavior("URI=file://" + Application.persistentDataPath + path);
			else if(Application.platform == RuntimePlatform.IPhonePlayer)
				db = new sqlBehavior("URI=file:" + Application.persistentDataPath + path);
			else
				db = new sqlBehavior(@"Data Source=" + Application.persistentDataPath + path);

			return db;
		}

		/// <summary>
		/// sql behavior initial
		/// </summary>
		public sqlBehavior(mySQLCallback callback)
		{
			mycallback = callback;
		}

		private sqlBehavior(string connectionString)
		{
			OpenDB(connectionString);
		}

		private SqliteConnection GetdbConnection()
		{
			return dbConnection;
		}

		private void OpenDB(string connectionString)
		{
			try
			{
				dbConnection = new SqliteConnection(connectionString);
				dbConnection.Open();

				mycallback.connectionStatus("Connected to db");
			}
			catch (Exception e)
			{
				mycallback.connectionStatus(e.ToString());
			}
		}

		/// <summary>
		/// Disconnect to database
		/// </summary>
		public void CloseSqlConnection()
		{
			if (dbCommand != null)
			{
				dbCommand.Dispose();
			}

			dbCommand = null;

			if (reader != null)
			{
				reader.Dispose();
			}

			reader = null;

			if (dbConnection != null)
			{
				dbConnection.Close();
			}

			dbConnection = null;

			mycallback.connectionStatus("Disconnected to db");
		}

		private SqliteDataReader ExecuteQuery(string sqlQuery)
		{
			Debug.Log(sqlQuery);
			dbCommand = dbConnection.CreateCommand();
			dbCommand.CommandText = sqlQuery;

			reader = dbCommand.ExecuteReader();

			return reader;
		}

		/// <summary>
		/// Check whether the table exists or not
		/// </summary>
		/// <returns><c>true</c>, if the table exist, <c>false</c> otherwise.</returns>
		/// <param name="tableName">Table name.</param>
		public bool TableExists(string tableName)
		{
			using (SqliteCommand cmd = new SqliteCommand())
			{
				cmd.CommandType = CommandType.Text;
				cmd.Connection = dbConnection;
				cmd.CommandText = "SELECT * FROM sqlite_master WHERE type = 'table' AND name = @name";
				cmd.Parameters.AddWithValue("@name", tableName);

				using (SqliteDataReader sqlDataReader = cmd.ExecuteReader())
				{
					if (sqlDataReader.Read())
						return true;
					else
						return false;
				}
			}
		}

		/// <summary>
		/// Create the table with name
		/// </summary>
		/// <returns>The table.</returns>
		/// <param name="name">Table name.</param>
		/// <param name="col">Array for column name.</param>
		/// <param name="colType">Array for column type.</param>
		public SqliteDataReader CreateTable(string name, string[] col, string[] colType)
		{
			if (col.Length != colType.Length)
			{
				throw new SqliteException("columns.Length != colType.Length");
			}

			string query = "CREATE TABLE " + name + " (" + col[0] + " " + colType[0];

			for (int i = 1; i < col.Length; ++i)
			{
				query += ", " + col[i] + " " + colType[i];
			}

			query += ")";

			return ExecuteQuery(query);
		}

		/// <summary>
		/// Read full table
		/// </summary>
		/// <returns>Table reader.</returns>
		/// <param name="tableName">Table name.</param>
		public SqliteDataReader ReadFullTable(string tableName)
		{
			string query = "SELECT * FROM " + tableName;

			return ExecuteQuery(query);
		}

		/// <summary>
		/// Select from database
		/// </summary>
		/// <returns>Table reader.</returns>
		/// <param name="tableName">Table name.</param>
		/// <param name="col">Columns you want to select.</param>
		/// <param name="values">Values you want to select.</param>
		public SqliteDataReader SelectWhere(string tableName, string[] col, string[] values)
		{
			if (col.Length != values.Length)
			{
				throw new SqliteException("col.Length != values.Length");
			}

			string query = "SELECT * FROM ";

			query += tableName + " WHERE " + col[0] + "=" + addMark(values[0]);

			for (int i = 1; i < col.Length; ++i)
			{
				query += " AND " + col[i] + "=" + addMark(values[0]);
			}

			return ExecuteQuery(query);
		}

		/// <summary>
		/// Search whether the row exists or not.
		/// </summary>
		/// <returns><c>true</c>, if row exists, <c>false</c> otherwise.</returns>
		/// <param name="tableName">Table name.</param>
		/// <param name="col">Columns you want to search.</param>
		/// <param name="values">Values you want to search.</param>
		public bool SearchRow(string tableName, string[] col, string[] values)
		{
			if (col.Length != values.Length)
			{
				throw new SqliteException("col.Length != values.Length");
			}

			string query = "SELECT * FROM ";

			query += tableName + " WHERE " + col[0] + "=" + addMark(values[0]);

			for (int i = 1; i < col.Length; ++i)
			{
				query += " AND " + col[i] + "=" + "'" + values[i] + "' ";
			}

			using (SqliteDataReader sqlDataReader = ExecuteQuery(query))
			{
				if (sqlDataReader.Read())
					return true;
				else
					return false;
			}
		}

		/// <summary>
		/// Sort the database according to time.
		/// </summary>
		/// <returns>Table reader.</returns>
		/// <param name="tableName">Table name.</param>
		/// <param name="column">Column you want to sort.</param>
		/// <param name="status">Increase(ASC) or decrease(DESC).</param>
		public SqliteDataReader SearchAndOrder(string tableName, string column, string status)
		{
			string query = "SELECT * FROM " + tableName;

			query += " ORDER BY " + column + " " + status;

			return ExecuteQuery(query);
		}

		/// <summary>
		/// Search from table.
		/// </summary>
		/// <returns>Table reader.</returns>
		/// <param name="tableName">Table name.</param>
		/// <param name="column">Columns you want to search.</param>
		/// <param name="Value">Values you want to search.</param>
		public SqliteDataReader searchBlurryData(string tableName, string[] column, string[] Value)
		{
			string query = "SELECT * FROM ";
			query += tableName + " WHERE " + column[0] + " LIKE " + "'%";

			for (int i = 0; i < Value.Length; i++)
			{
				query += Value[i] + "%";
			}
			query += "'";

			return ExecuteQuery(query);
		}

		/// <summary>
		/// Insert into the value.
		/// </summary>
		/// <returns>Table reader.</returns>
		/// <param name="tableName">Table name.</param>
		/// <param name="values">Array for values.</param>
		public SqliteDataReader InsertInto(string tableName, string[] values)
		{
			string query = "INSERT INTO " + tableName + " VALUES (" + addMark(values[0]);

			for (int i = 1; i < values.Length; ++i)
			{
				query += ", " + addMark(values[i]);
			}

			query += ")";

			return ExecuteQuery(query);
		}

		/// <summary>
		/// Updates the specified column.
		/// </summary>
		/// <returns>Table reader.</returns>
		/// <param name="tableName">Table name.</param>
		/// <param name="cols">Array for setting (column name).</param>
		/// <param name="colsvalues">Array for setting (column value).</param>
		/// <param name="selectkey">Array for key (column name).</param>
		/// <param name="selectvalue">Array for key (column value).</param>
		public SqliteDataReader UpdateInto(string tableName, string[] cols, string[] colsvalues, string[] selectkey, string[] selectvalue)
		{
			string query = "UPDATE " + tableName + " SET " + cols[0] + " = " + addMark(colsvalues[0]);

			for (int i = 1; i < colsvalues.Length; ++i)
			{
				query += ", " + cols[i] + " =" + addMark(colsvalues[i]);
			}

			query += " WHERE " + selectkey[0] + " = " + addMark(selectvalue[0]);

			for (int i = 1; i < selectkey.Length; ++i)
			{
				query += " AND " + selectkey[i] + " = " + addMark(selectvalue[i]);
			}

			return ExecuteQuery(query);
		}

		/// <summary>
		/// Deletes the whole database value.
		/// </summary>
		/// <returns>Table reader.</returns>
		/// <param name="tableName">Table name.</param>
		public SqliteDataReader DeleteContents(string tableName)
		{
			string query = "DELETE FROM " + tableName;

			return ExecuteQuery(query);
		}

		/// <summary>
		/// Delete the specified row by column name.
		/// </summary>
		/// <returns>Table reader.</returns>
		/// <param name="tableName">Table name.</param>
		/// <param name="cols">Column name.</param>
		/// <param name="colsvalues">Column value.</param>
		public SqliteDataReader Delete(string tableName, string[] cols, string[] colsvalues)
		{
			string query = "DELETE FROM " + tableName + " WHERE " + cols[0] + " = " + addMark(colsvalues[0]);

			for (int i = 1; i < colsvalues.Length; ++i)
			{
				query += " AND " + cols[i] + " = " + addMark(colsvalues[i]);
			}

			return ExecuteQuery(query);
		}

		private string addMark(string value)
		{
			return "'" + value + "'";
		}

		public bool checkVersion(string[] key, string[] value, string table, string checkValue, int col)
		{
			bool testChange = false;

			using (SqliteDataReader sqReader = SelectWhere(table, key, value))
			{
				if ((sqReader.Read()))
					testChange = sqReader.GetString(col) == checkValue ? false : true;
				else
					testChange = true;
			}

			return testChange;
		}
	}
}
