using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cqplayart.adv.initial
{
	public interface imageInitiate
	{
		initiateImageBehavior Init(string name, imageInitiateCallback callback);
		IEnumerator initiateImage(string path, string ojName);
		IEnumerator initiateImage(string path, string ojName, GameObject oj);
		IEnumerator initiateImage(string path, string ojName, GameObject parent, Vector3 bundlePosition, Vector3 bundleRotation, Vector3 bundleScale);
		IEnumerator initiateImage(string path, string ojName, GameObject oj, GameObject parent, Vector3 bundlePosition, Vector3 bundleRotation, Vector3 bundleScale);
	}
	public interface imageInitiateCallback
	{
		void initialStatus(string status);
		void getInitiatePorgress(float progress);
		void getInitialName(string name);

		GameObject setObjectName { set; }
		GameObject setInitiateProgress { set; }
	}
}