using cqplayart.adv.initial;
using UnityEngine;
using System.Collections;

public class initiateImageControl : MonoBehaviour 
{
	imageInitiate myscript;
	imageInitiateCallback mycallback = new initiateImageCallback();

	public GameObject progress;
	public GameObject initiateName;

	// Use this for initialization
	IEnumerator Start () 
	{
		mycallback.setObjectName = initiateName;
		mycallback.setInitiateProgress = progress;

		myscript = new initiateImageBehavior(mycallback);

		IEnumerator e_initiateImage;

		e_initiateImage = myscript.initiateImage("http://www.advmedia.com.tw/ciqikou/back.png", "track1");
		while (e_initiateImage.MoveNext()) yield return e_initiateImage.Current;

		GameObject yourPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
		e_initiateImage = myscript.initiateImage("http://www.advmedia.com.tw/ciqikou/back.png", "track2", yourPlane);
		while (e_initiateImage.MoveNext()) yield return e_initiateImage.Current;

		GameObject parent = GameObject.Find("Main Camera");
		e_initiateImage = myscript.initiateImage("http://www.advmedia.com.tw/ciqikou/back.png", "track3", parent, Vector3.zero, Vector3.zero, Vector3.zero);
		while (e_initiateImage.MoveNext()) yield return e_initiateImage.Current;

		GameObject yourPlane2 = GameObject.CreatePrimitive(PrimitiveType.Plane);
		e_initiateImage = myscript.initiateImage("http://www.advmedia.com.tw/ciqikou/back.png", "track4", yourPlane2, parent, Vector3.zero, Vector3.zero, Vector3.zero);
		while (e_initiateImage.MoveNext()) yield return e_initiateImage.Current;

		Debug.Log(yourPlane.name);
		Debug.Log(yourPlane2.name);
	}
}
