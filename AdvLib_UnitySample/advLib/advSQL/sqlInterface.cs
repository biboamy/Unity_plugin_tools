using UnityEngine;
using System;
using Mono.Data.Sqlite;

namespace cqplayart.adv.sql
{
	public interface mySQL
	{
		sqlBehavior databaseConnection(string path);
		void CloseSqlConnection();
		bool TableExists(string tableName);
		SqliteDataReader CreateTable(string name, string[] col, string[] colType);
		SqliteDataReader ReadFullTable(string tableName);
		SqliteDataReader SelectWhere(string tableName, string[] col, string[] values);
		bool SearchRow(string tableName, string[] col, string[] values);
		SqliteDataReader SearchAndOrder(string tableName, string column, string status);
		SqliteDataReader searchBlurryData(string tableName, string[] column, string[] Value);
		SqliteDataReader InsertInto(string tableName, string[] values);
		SqliteDataReader UpdateInto(string tableName, string[] cols, string[] colsvalues, string[] selectkey, string[] selectvalue);
		SqliteDataReader DeleteContents(string tableName);
		SqliteDataReader Delete(string tableName, string[] cols, string[] colsvalues);
		bool checkVersion(string[] key, string[] value, string table, string checkValue, int col);
	}

	public interface mySQLCallback
	{
		void connectionStatus(string status);
	}
}