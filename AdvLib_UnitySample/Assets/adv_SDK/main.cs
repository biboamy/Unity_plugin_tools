using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using Mono.Data.Sqlite;
using cqplayart.adv.advjson;
using cqplayart.adv.initial;
using cqplayart.adv.download;
using cqplayart.adv.sql;

public class main : MonoBehaviour 
{
	//initiate json script
	advjson jsonScript;
	mysjsoncallback jsonCallback = new AdvJsoncallback();

	//initiate download script
	advWebRequest downloadScript;
	WebRequestCallback downloadCallback = new downloadcallback();

	//initiate dataset script
	datasetInitiate datasetScript;
	datasetInitiateCallback datasetCallback = new initiateDatasetCallback();

	//initiate movie script
	AVProInitial avproScript;
	AVProInitialCallback avproCallback = new initiateAVProCallback();

	//initiate sql script
	mySQL sqlScript;
	mySQLCallback sqlCallback = new sqlcallback();

	//list storage
	List<string> xmlList;
	List<List<List<string>>> targetList;
	List<List<List<string>>> parameterList;

	void Awake()
	{
		//initiate json script
		jsonScript = new AdvJsonBehavior(jsonCallback);
		//initiate download script
		downloadScript = new downloadBehavior(downloadCallback);
		//initiate dataset script
		datasetScript = GameObject.Find(name).AddComponent<initiateDatasetBehavior>();
		datasetScript.init(datasetCallback);
		//initiate avpro script
		avproScript = new initiateAVProBehavior(avproCallback);
		//initiate sql callback
		sqlScript = new sqlBehavior(sqlCallback);
	}

	public void connectToDb()
	{
		//create database and database table
		sqlScript = sqlScript.databaseConnection("/test.db");
		if (!sqlScript.TableExists("test"))
			sqlScript.CreateTable("test", new string[] { "title", "hash" }, new string[] { "text", "text" });
	}

	//取得xml json表資料
	public void getXmlJson()
	{
		StartCoroutine(_getXmlJson());
	}
	IEnumerator _getXmlJson()
	{
		string[] title = new string[] { "xmlPath", "datPath", "xmlHash", "trackImgPath", "trackImgHash", "count" };

		//parse url value
		IEnumerator e_gethtml = jsonScript.gethtml("https://app.cqplayart.com/wedding/ar_json.php?account=170329018");
		while (e_gethtml.MoveNext()) yield return e_gethtml.Current;
		string htmlData = jsonCallback.getHtmlString; 
		//turn string to json
		JsonData jsonData = jsonScript.getJsonData(htmlData);
		//get json value by title
		xmlList = jsonScript.setJsonList(jsonData, title);
	}

	//取得ar素材json黨資料
	public void getARJson()
	{
		StartCoroutine(_getARJson());
	}
	IEnumerator _getARJson()
	{
		string[] target = new string[] { "name", "path_android", "path_ios", "size", "transformSet", "transparent", "time", "hash" };
		string[] parameter = new string[] { "p_x", "p_y", "p_z", "s_x", "s_y", "s_z", "r_x", "r_y", "r_z" };

		//parse url value
		IEnumerator e_gethtml = jsonScript.gethtml("https://app.cqplayart.com/wedding/ar_asset_json.php?account=170329018");
		while (e_gethtml.MoveNext()) yield return e_gethtml.Current;
		string htmlData = jsonCallback.getHtmlString;
		//turn string to json
		JsonData jsonData = jsonScript.getJsonData(htmlData);
		//get json value by title (with title target*)
		string[] targetArray = new string[jsonData.Count/2];
		for (int i = 0; i < targetArray.Length; i++)
		{
			int j = i + 1;
			targetArray[i] = "target" + j;
		}
		targetList = jsonScript.setJsonThreeList(jsonData, targetArray, target);
		//get json value by title (with title transform*)
		string[] parameterArray = new string[jsonData.Count / 2];
		for (int i = 0; i < parameterArray.Length; i++)
		{
			int j = i + 1;
			parameterArray[i] = "transform" + j;
		}
		parameterList = jsonScript.setJsonThreeList(jsonData, parameterArray, parameter);
	}

	//將需要下載的資源加入下載列
	public void addFileToDownload()
	{
		SqliteDataReader sqReader;

		//取得hash碼
		sqReader = sqlScript.SelectWhere("test", new string[] { "title" }, new string[] { "xmlHash" });
		sqReader.Read();
		//判斷是不是第一次寫入  以及hash碼一不一致
		if (!sqlScript.SearchRow("test", new string[] { "title" }, new string[] { "xmlHash" }) || sqReader.GetString(1) != xmlList[2])
		{
			//add xml file
			downloadScript.addDownloadList(xmlList[0]); 
			//add dat file
			downloadScript.addDownloadList(xmlList[1]);
			//不是第一次 更新table
			if (sqlScript.SearchRow("test", new string[] { "title" }, new string[] { "xmlHash" }))
				sqlScript.UpdateInto("test", new string[] { "title" }, new string[] { "xmlHash" }, new string[] { "hash" }, new string[] { xmlList[2] });
			//第一次 新增table
			else
				sqlScript.InsertInto("test", new string[] { "xmlHash", xmlList[2] });
		}

		sqReader = sqlScript.SelectWhere("test", new string[] { "title" }, new string[] { "trackHash" });
		sqReader.Read();
		if (!sqlScript.SearchRow("test", new string[] { "title" }, new string[] { "trackHash" }) || sqReader.GetString(1) != xmlList[4])
		{
			//add track image
			downloadScript.addDownloadList(xmlList[3]);

			if (sqlScript.SearchRow("test", new string[] { "title" }, new string[] { "trackHash" }))
				sqlScript.UpdateInto("test", new string[] { "title" }, new string[] { "trackHash" }, new string[] { "hash" }, new string[] { xmlList[4] });
			else
				sqlScript.InsertInto("test", new string[] { "trackHash", xmlList[4] });
		}

		//add target
		for (int i = 0; i < targetList.Count; i++)
		{
			sqReader = sqlScript.SelectWhere("test", new string[] { "title" }, new string[] { "target" + i.ToString() });
			sqReader.Read();
			if (!sqlScript.SearchRow("test", new string[] { "title" }, new string[] { "target" + i.ToString() }) || sqReader.GetString(1) != targetList[i][0][7])
			{
				if (Application.platform == RuntimePlatform.IPhonePlayer)
					downloadScript.addDownloadList(targetList[i][0][2]); //ios path
				else
					downloadScript.addDownloadList(targetList[i][0][1]); //android path

				if (sqlScript.SearchRow("test", new string[] { "title" }, new string[] { "target" + i.ToString() }))
					sqlScript.UpdateInto("test", new string[] { "title" }, new string[] { "target" + i.ToString() }, new string[] { "hash" }, new string[] { targetList[i][0][7] });
				else
					sqlScript.InsertInto("test", new string[] { "target" + i.ToString(), targetList[i][0][7] });
			}
		}
	}

	//開始下載
	public void startDownload()
	{
		StartCoroutine(_startDownload());
	}
	IEnumerator _startDownload()
	{
		IEnumerator e_getjson = downloadScript.LoadList("/myDir");
		while (e_getjson.MoveNext()) yield return e_getjson.Current;
	}

	//動態家載xml tracking包
	public void loadDataset()
	{
		string[] _xmlName = xmlList[0].Split('/');
		string xmlName = _xmlName[_xmlName.Length - 1];Debug.Log(xmlName);
		string[] targetGameobjectName = { "oj1" };
		//xml path
		datasetScript.loadDataset(Application.persistentDataPath + "/myDir/" + xmlName, targetGameobjectName);
	}

	//動態家載movie
	public void initiateMovie()
	{
		for (int i = 0; i < targetList.Count; i++)
		{
			string[] _assetName = targetList[i][0][1].Split('/');
			string assetName = _assetName[_assetName.Length - 1];
			GameObject parent = GameObject.Find("oj1");

			for (int j = 0; j < targetList[i].Count; j++)
			{
				Vector3 position = new Vector3(int.Parse(parameterList[i][j][0]), int.Parse(parameterList[i][j][1]), int.Parse(parameterList[i][j][2]));
				Vector3 scale = new Vector3(int.Parse(parameterList[i][j][3]), int.Parse(parameterList[i][j][4]), int.Parse(parameterList[i][j][5]));
				Vector3 rotation = new Vector3(int.Parse(parameterList[i][j][6]), int.Parse(parameterList[i][j][7]), int.Parse(parameterList[i][j][8]));
				avproScript.initiateMovie("myDir/" + assetName, assetName, parent, position, rotation, scale, bool.Parse(targetList[i][j][5]), true, true);
			}
		}
	}
}
