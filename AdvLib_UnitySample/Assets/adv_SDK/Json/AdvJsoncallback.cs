using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cqplayart.adv.advjson;
using UnityEngine.UI;
using LitJson;

public class AdvJsoncallback : MonoBehaviour, mysjsoncallback
{
	public string htmlText = null;
	public JsonData jsonData = null;
	List<string> json_array = null;
	List<List<string>> json_array2 = null;

	public string getHtmlString { get { return htmlText; } }
	public JsonData getJsonData { get { return jsonData; } }
	public List<string> getJsonArray { get { return json_array; } }
	public List<List<string>> getJsonArray2 { get { return json_array2; } }

	public void onLoadHtmlText(string mtxt)
	{
        Debug.Log("htmlText:" + mtxt);
		htmlText = mtxt;
	}

	public void onjsonData(JsonData jsonvale, int num)
	{
		Debug.Log("jsonCount:" + num);
		jsonData = jsonvale;
	}

	public void onArrayList(List<string> array)
	{
		for (int i = 0; i < array.Count; i++)
			Debug.Log("i=" + i + "-" + array[i]);
	}

	public void onjsonList(List<string> json_target)
	{
		json_array = json_target;

		for (int i = 0; i < json_target.Count; i++)
			Debug.Log("i=" + i + "-" + json_target[i]);
	}

	public void onjsonTwoList(List<List<string>> json_target)
	{
		json_array2 = json_target;

		for (int i = 0; i < json_target.Count; i++)
			for (int j = 0; j < json_target[i].Count; j++)
				Debug.Log("i=" + i + ",j=" + j + "-" + json_target[i][j]);
	}

	public void onjsonThreeList(List<List<List<string>>> json_target)
	{
		for (int i = 0; i < json_target.Count; i++)
			for (int j = 0; j < json_target[i].Count; j++)
				for (int k = 0; k < json_target[i][j].Count; k++)
					Debug.Log("i=" + i + ",j=" + j + ",k=" + k + "-" + json_target[i][j][k]);
	}

	public void statusCallback(string status)
	{
		Debug.Log(status);
	}
}
