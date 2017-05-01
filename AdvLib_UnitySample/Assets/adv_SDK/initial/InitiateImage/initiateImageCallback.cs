using cqplayart.adv.initial;
using UnityEngine;
using UnityEngine.UI;

public class initiateImageCallback : MonoBehaviour, imageInitiateCallback 
{
	static GameObject objectName = null;
	static GameObject initiateProgress = null;

	public GameObject setObjectName { set { objectName = value; } }
	public GameObject setInitiateProgress { set { initiateProgress = value; } }

	public void getInitiatePorgress(float progress, string name)
	{
		Debug.Log("當前加載物件: " + name + " initiate progress: " + progress.ToString());
		if (initiateProgress != null)
			initiateProgress.GetComponent<Text>().text = progress.ToString();
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
