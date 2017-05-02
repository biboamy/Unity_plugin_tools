using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace amy.initial
{
	public interface audioInitiate
	{
		IEnumerator initiateAudio(string path, string ojName, AudioType type, bool autoPlay, bool loop, float volumn);
		IEnumerator initiateAudio(string path, string ojName, AudioType type, GameObject oj, bool autoPlay, bool loop, float volumn);
	}
	public interface audioInitiateCallback
	{
		void getInitiatePorgress(float progress, string name);
		void onLoadStart(string msg);
		void onLoadFinish(string msg);
		void onLoadFailed(string msg);

		GameObject setObjectName { set; }
		GameObject setInitiateProgress { set; }
	}
}