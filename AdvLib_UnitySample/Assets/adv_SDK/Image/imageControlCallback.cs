using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cqplayart.adv.image;

public class imageControlCallback : MonoBehaviour, controlImageCallback 
{
	public void imageStatus(string status)
	{
		Debug.Log(status);
	}

	public void albumImagePath(string path)
	{
		Debug.Log("image path: " + path);
	}

	public void returnImagePath(string path)
	{
		Debug.Log("image path:" + path);
	}

	public void screenShotPath(string path)
	{
		Debug.Log("screen shot path: " + path);
	}

	public void galleryPath(string path)
	{
		Debug.Log("gallery path: " + path);
	}
}
