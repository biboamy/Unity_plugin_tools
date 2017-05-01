using UnityEngine;
using cqplayart.adv.initial;
using UnityEngine.UI;

public class initiateAVProCallback : AVProInitialCallback 
{
	static GameObject objectName = null;
	static GameObject initiateProgress = null;

	public GameObject setObjectName { set { objectName = value; } }
	public GameObject setInitiateProgress { set { initiateProgress = value; } }

	public void getInitiatePorgress(float progress)
	{
		Debug.Log("initiate progress: " + progress.ToString());
		if (initiateProgress != null)
			initiateProgress.GetComponent<Text>().text = progress.ToString();
	}

	public void getInitialName(string name)
	{
		Debug.Log("當前加載物件: " + name);
		if (objectName != null)
			objectName.GetComponent<Text>().text = name;
	}

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
