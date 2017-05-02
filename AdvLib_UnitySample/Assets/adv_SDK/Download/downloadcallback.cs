using UnityEngine;
using UnityEngine.UI;
using amy.download;

public class downloadcallback : MonoBehaviour, WebRequestCallback
{
	public void onLoadStart(string status)
	{
		Debug.Log(status);
		Text mText = GameObject.Find("status").GetComponent<Text>();
		mText.text = "status:"+status;
	}

	public void onLoadFinish(string status)
	{
		Debug.Log(status);
		Text mText = GameObject.Find("status").GetComponent<Text>();
		mText.text = "status:"+status;
	}

	public void onLoadFail(string status)
	{
		Debug.Log(status);
		Text mText = GameObject.Find("status").GetComponent<Text>();
		mText.text = "status:"+status;
	}

	public void onLoadStatus(string[] status)
	{
		Debug.Log("Asset name: " + status[3] + "current MB:" + status[0] + "MB  TotalMB:" + status[2] + "MB  current % :" + status[1] + "%");
		Text mText = GameObject.Find("status").GetComponent<Text>();
		mText.text="Asset name: " + status[3] + "current MB:" + status[0] + "MB  TotalMB:"+status[2]+"MB  current % :"+status[1]+"%";
	}
}