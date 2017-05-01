using UnityEngine;
using cqplayart.adv.initial;
using System.IO;
using System.Collections;

public class initiateAVProControl : MonoBehaviour 
{
	AVProInitial myscript;
	AVProInitialCallback mycallback = new initiateAVProCallback();

	public GameObject progress;
	public GameObject initiateName;

	IEnumerator Start()
	{
		//for demo, download the demo asset first
		IEnumerator e_initiateImage = assetDownload();
		while (e_initiateImage.MoveNext()) yield return e_initiateImage.Current;

		mycallback.setObjectName = initiateName;

		//initiate script
		myscript = new initiateAVProBehavior(mycallback);

		//auto initiate player and movie plane
		myscript.initiateMovie("target0-0.mp4", "target0.mp4", false, true, true);

		//auto initiate player and customized movie plane
		GameObject yourPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
		myscript.initiateMovie("target0-0.mp4", "target1.mp4", yourPlane, false, true, true);

		//customized player and customized movie plane
		GameObject yourPlane2 = GameObject.CreatePrimitive(PrimitiveType.Plane);
		GameObject player = new GameObject();
		myscript.initiateMovie("target0-0.mp4", "target2.mp4", yourPlane2, player, false, true, true);

		//auto initiate player and movie plane, customized parent and customized parameters
		GameObject parent = GameObject.Find("Main Camera");
		myscript.initiateMovie("target0-0.mp4", "target3.mp4", parent, Vector3.zero, Vector3.zero, Vector3.zero, false, true, true);

		//customized player and customized movie plane, customized parent and customized parameters
		GameObject yourPlane3 = GameObject.CreatePrimitive(PrimitiveType.Plane);
		GameObject player2 = new GameObject();
		myscript.initiateMovie("target0-0.mp4", "target4.mp4", yourPlane3, player2, parent, Vector3.zero, Vector3.zero, Vector3.zero, false, true, true);

		Debug.Log(yourPlane.name);

		Debug.Log(yourPlane2.name);
		Debug.Log(player.name);

		Debug.Log(yourPlane3.name);
		Debug.Log(player2.name);
	}

	IEnumerator assetDownload()
	{
		string moviePath = "https://app.cqplayart.com/wedding/assets/android/170322073/20170328162603177/15072402104_09 (1).mp4";
		WWW www = new WWW(moviePath);
		yield return www;
		File.WriteAllBytes(Application.persistentDataPath + "/target0-0.mp4", www.bytes);
	}
}
