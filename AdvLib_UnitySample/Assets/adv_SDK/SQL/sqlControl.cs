using UnityEngine.UI;
using UnityEngine;
using amy.sql;
using Mono.Data.Sqlite;
using System;

public class sqlControl : MonoBehaviour
{
	public static mySQL myscript;
	public mySQLCallback mycallback;
	int id = 0;


	void Start()
	{
	
		//initial the callback 
		mycallback = new sqlcallback();
		//initial the script
		myscript = new sqlBehavior(mycallback);
		//connect to database and refresh the script
		myscript = myscript.databaseConnection("/test.db");
		if (myscript == null)
			Debug.Log("null");
		//create table
		if (!myscript.TableExists("test"))
			myscript.CreateTable("test", new string[] { "id", "value", "time" }, new string[] { "text", "text", "text" });
	}

	void OnApplicationQuit()
	{
		//delete the content in the table
		myscript.DeleteContents("test");
		//disconnect to sql
		myscript.CloseSqlConnection();
	}

	public void enterText()
	{
		//get the input text and store the value in the database
		string inputText = GameObject.Find("inputText").GetComponent<Text>().text;
		id++;
		myscript.InsertInto("test", new string[] { id.ToString(), inputText, System.DateTime.Now.ToString() });

		//read the sql values and display them on ui
		Text outputText = GameObject.Find("outputText").GetComponent<Text>();
		outputText.text = "";
		SqliteDataReader sqReader = myscript.ReadFullTable("test");
		while (sqReader.Read())
		{
			outputText.text += sqReader.GetString(1) + " / " + sqReader.GetString(2) + '\n';
		}
	}

	public void delete()
	{
		//delete the newest value
		SqliteDataReader sqReader = myscript.Delete("test", new string[] { "id" }, new string[] { id.ToString() });
		id--;

		//read the sql values and display them on ui
		Text outputText = GameObject.Find("outputText").GetComponent<Text>();
		outputText.text = "";
		SqliteDataReader sqReader1 = myscript.ReadFullTable("test");
		while (sqReader1.Read())
		{
			outputText.text += sqReader1.GetString(1) + " / " + sqReader1.GetString(2) + '\n';
		}
	}

	public void search()
	{
		string inputText = GameObject.Find("inputText").GetComponent<Text>().text;

		bool exist = myscript.SearchRow("test", new string[] { "value" }, new string[] { inputText });
		Text existText = GameObject.Find("exist").GetComponent<Text>();
		existText.text = exist.ToString();
	}

	public void increaseOrder()
	{
		Text outputText = GameObject.Find("outputText").GetComponent<Text>();
		outputText.text = "";
		SqliteDataReader sqReader = myscript.SearchAndOrder("test", "time", "ASC");
		while (sqReader.Read())
		{
			outputText.text += sqReader.GetString(1) + " / " + sqReader.GetString(2) + '\n';
		}
	}

	public void decreaseOrder()
	{
		Text outputText = GameObject.Find("outputText").GetComponent<Text>();
		outputText.text = "";
		SqliteDataReader sqReader = myscript.SearchAndOrder("test", "time", "DESC");
		while (sqReader.Read())
		{
			outputText.text += sqReader.GetString(1) + " / " + sqReader.GetString(2) + '\n';
		}
	}
}
