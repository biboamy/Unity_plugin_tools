using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using cqplayart.adv.advjson;

public class AdvJsonControl : MonoBehaviour 
{
	//initiate script and callback
	advjson  myscript ;
	mysjsoncallback mycallback;

	private string[] jsonarray;
	private string[] oneDimension = new string[] { "xmlPath", "datPath", "xmlHash", "trackImgPath", "trackImgHash", "asdf" };
	private string[] twoDimension = new string[] { "key", "title", "profile_img", "hash", "wed_date" };
	private string[] threeDimension = new string[] { "p_x", "p_y", "p_z", "s_x", "s_y", "s_z", "r_x", "r_y", "r_z" };

	string htmlString;
	JsonData jsonData;

	//public GameObject mtext;
	//設定靜態指針來調用
	public string[] url; 

    void Awake() 
	{
        //initial the callback 
        mycallback = new AdvJsoncallback();
        //initial the script
        myscript = new AdvJsonBehavior(mycallback);

		if (myscript == null)
			Debug.Log("null");
	}

	public void parseJson(int url_index)
	{
		StartCoroutine(_parseJson(url_index));
	}

	//url to get jsonText
	IEnumerator _parseJson(int url_index)
	{
		//parse through url
		IEnumerator e_gethtml = myscript.gethtml(url[url_index]);
		while (e_gethtml.MoveNext()) yield return e_gethtml.Current;

		//get parse result (html string)
		htmlString = mycallback.getHtmlString;

		//turn html string to json data
		jsonData = myscript.getJsonData(htmlString);

		//Parse array json
		if (url_index == 0)
		{
			myscript.setArrayList(jsonData);
		}
		//Parse one dimension json with customized title
		else if (url_index == 1)
		{
			myscript.setJsonList(jsonData, oneDimension);
		}
		//Parse two dimension json with customized title
		else if (url_index == 2)
		{
			jsonarray = new string[jsonData.Count];
			for (int i = 0; i < jsonData.Count; i++)
			{
				int j = i + 1;
				jsonarray[i] = "couple" + j;
			}
			myscript.setJsonTwoList(jsonData, jsonarray, twoDimension);
		}
		//Parse three dimension json with customized title
		else
		{
			jsonarray = new string[jsonData.Count];
			for (int i = 0; i < 10; i++)
			{
				int j = i + 1;
				jsonarray[i] = "transform" + j;
			}
			myscript.setJsonThreeList(jsonData, jsonarray, threeDimension);
		}
    }
}


