using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using amy.sql;
public class sqlcallback : MonoBehaviour, mySQLCallback
{
	public GameObject LogText;

	void awake() { 


	}
	public void connectionStatus(string status)
	{

		Debug.Log(status);
		Text mText = GameObject.Find("exist").GetComponent<Text>();
	
	 	mText.text = "status:"+status;


	}
}