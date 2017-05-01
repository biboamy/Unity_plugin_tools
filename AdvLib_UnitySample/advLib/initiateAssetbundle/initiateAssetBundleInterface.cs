using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cqplayart.adv.initial
{
	public interface assetbundleInitial
	{
		IEnumerator initiateAssetbundle(string path, string ojName);
		IEnumerator initiateAssetbundle(string path, string ojName, GameObject parent, Vector3 bundlePosition, Vector3 bundleRotation, Vector3 bundleScale);
	}
	public interface assetbundleInitialCallback
	{
		void getInitiatePorgress(float progress, string name);
		void onLoadStart(string msg);
		void onLoadFinish(string msg);
		void onLoadFailed(string msg);

		GameObject setObjectName { set; }
		GameObject setInitiateProgress { set; }
	}
}
