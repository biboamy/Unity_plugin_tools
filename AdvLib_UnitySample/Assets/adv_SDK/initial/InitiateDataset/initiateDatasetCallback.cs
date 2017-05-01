using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cqplayart.adv.initial;

public class initiateDatasetCallback : MonoBehaviour, datasetInitiateCallback 
{
	public void onLoadStart(string msg)
	{
		Debug.Log(msg);
	}

	public void onLoadFinish(string msg)
	{
		Debug.Log(msg);
	}

	public void onLoadFailed(string msg)
	{
		Debug.Log(msg);
	}
}
