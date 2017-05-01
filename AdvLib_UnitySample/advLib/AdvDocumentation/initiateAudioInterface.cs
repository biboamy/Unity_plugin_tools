using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cqplayart.adv.initial
{
	public interface audioInitiate
	{
		initiateAudioBehavior Init(string name, audioInitiateCallback callback);
		IEnumerator initiateAudio(string path, string ojName, AudioType type, bool autoPlay, bool loop, float volumn);
		IEnumerator initiateAudio(string path, string ojName, AudioType type, GameObject oj, bool autoPlay, bool loop, float volumn);
	}
	public interface audioInitiateCallback
	{
		void initialStatus(string status);
		void getInitiatePorgress(float progress);
		void getInitialName(string name);

		GameObject setObjectName { set; }
		GameObject setInitiateProgress { set; }
	}
}