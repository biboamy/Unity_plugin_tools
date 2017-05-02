using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using amy.initial;

public class initiateAudioControl : MonoBehaviour 
{

	audioInitiate myscript;
	audioInitiateCallback mycallback = new initiateAudioCallback();

	public GameObject progress;
	public GameObject initiateName;

	IEnumerator Start()
	{
		mycallback.setInitiateProgress = progress;
		mycallback.setObjectName = initiateName;

		myscript = new initiateAudioBehavior(mycallback);

		IEnumerator e_initiateAudio;

		//auto initiate audio object
		e_initiateAudio = myscript.initiateAudio("file://" + Application.persistentDataPath + "/guide1.mp3", "guide1", AudioType.MPEG, true, true, 1.0f);
		while (e_initiateAudio.MoveNext()) yield return e_initiateAudio.Current;

		//customized audio object
		GameObject yourObject = new GameObject();
		e_initiateAudio = myscript.initiateAudio("file://" + Application.persistentDataPath + "/guide1.mp3", "guide2", AudioType.MPEG, yourObject, true, false, 0.5f);
		while (e_initiateAudio.MoveNext()) yield return e_initiateAudio.Current;

		Debug.Log(yourObject.name);
	}
}
