using System.Collections.Generic;
using UnityEngine;
using amy.download;
using System;
public class downloadControl : MonoBehaviour
{
	public static advWebRequest myscript;
	public WebRequestCallback mycallback;
	int id = 0;
	public string[] path,dirname, filename;

	List<string> waitList = new List<string>();

	void Start()
	{
		//initial the callback 
		mycallback = new downloadcallback();
		//initial the script
		myscript = new downloadBehavior(mycallback);
		//connect to database and refresh the script

		if (myscript == null)
			Debug.Log("null");
	}

	public void addList(string path)
	{
		myscript.addDownloadList(path);
	}

	public void downloadList()
	{
		StartCoroutine(myscript.LoadList("/myDir"));
	}

	public void startdownload(int m) 
	{
	  StartCoroutine(myscript.LoadURL(path[m],dirname[m],filename[m]));
	}

}
