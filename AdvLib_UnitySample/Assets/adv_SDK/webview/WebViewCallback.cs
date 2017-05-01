using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using cqplayart.adv.webview;

public class WebViewCallback : MonoBehaviour, webviewCallback
{
	Text uname;

	void Start()
	{
		uname = GameObject.Find("info").GetComponent<Text>();
	}

	public void onLoadFail(string url)
	{
		Debug.Log("call onLoadFail : " + url);
		uname.text = "onLoadFail" + url;
	}

	public void onLoadFinish(string url)
	{
		Debug.Log("call onLoadFinish : " + url);
		uname.text = "onLoadFinish" + url;
	}

	public void onLoadStart( string url )
	{
		Debug.Log( "call onLoadStart : " + url );
		uname.text = "onLoadStart"+ url;
	}
}