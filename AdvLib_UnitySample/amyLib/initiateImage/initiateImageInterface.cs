using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace amy.initial
{
	public interface imageInitiate
	{
		IEnumerator initiateImage(string path, string ojName);
		IEnumerator initiateImage(string path, string ojName, GameObject oj);
		IEnumerator initiateImage(string path, string ojName, GameObject parent, Vector3 bundlePosition, Vector3 bundleRotation, Vector3 bundleScale);
		IEnumerator initiateImage(string path, string ojName, GameObject oj, GameObject parent, Vector3 bundlePosition, Vector3 bundleRotation, Vector3 bundleScale);
	}
	public interface imageInitiateCallback
	{
		void getInitiatePorgress(float progress, string name);
		void onLoadStart(string msg);
		void onLoadFinish(string msg);
		void onLoadFailed(string msg);

		GameObject setObjectName { set; }
		GameObject setInitiateProgress { set; }
	}
}