using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using amy.initial;

public class initiateAssetBundleControl : MonoBehaviour 
{
	assetbundleInitial myscript;
	assetbundleInitialCallback mycallback = new initiateAssetBundleCallback();

	public GameObject progress;
	public GameObject initiateName;

	IEnumerator Start()
	{
		mycallback.setObjectName = initiateName;
		mycallback.setInitiateProgress = progress;

		myscript = new initiateAssetBundleBehavior(mycallback);

		IEnumerator e_initiateImage;

		//auto initiate asset object
		e_initiateImage = myscript.initiateAssetbundle("https://app.cqplayart.com/wedding/assets/android/170329018/20170329152340839/yjtx.assetbundle", "yjtx");
		while (e_initiateImage.MoveNext()) yield return e_initiateImage.Current;

		//auto initiate asset object, customized parent and customized parameters
		//GameObject parent = GameObject.Find("Main Camera");
		//e_initiateImage = myscript.initiateAssetbundle("http://advmedia.cqplayart.cn/ngsl.assetbundle", "ngsl", parent, Vector3.zero, Vector3.zero, Vector3.zero);
		//while (e_initiateImage.MoveNext()) yield return e_initiateImage.Current;
	}
}
