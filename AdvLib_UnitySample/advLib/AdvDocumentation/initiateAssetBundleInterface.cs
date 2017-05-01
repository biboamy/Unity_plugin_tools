using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cqplayart.adv.initial
{
	public interface assetbundleInitial
	{
		initiateAssetBundleBehavior Init(string name, assetbundleInitialCallback callback);
		IEnumerator initiateAssetbundle(string path, string ojName);
		IEnumerator initiateAssetbundle(string path, string ojName, GameObject parent, Vector3 bundlePosition, Vector3 bundleRotation, Vector3 bundleScale);
	}
	public interface assetbundleInitialCallback
	{
		void initialStatus(string status);
		void getInitiatePorgress(float progress);
		void getInitialName(string name);

		GameObject setObjectName { set; }
		GameObject setInitiateProgress { set; }
	}
}
