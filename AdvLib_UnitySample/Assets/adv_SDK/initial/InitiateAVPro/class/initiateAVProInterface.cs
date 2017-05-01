using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cqplayart.adv.initial
{
	public interface AVProInitial
	{
		void initiateMovie(string path, string ojName, bool transparent, bool loop, bool autoOpen);
		void initiateMovie(string path, string ojName, GameObject oj, bool transparent, bool loop, bool autoOpen);
		void initiateMovie(string path, string ojName, GameObject oj, GameObject player, bool transparent, bool loop, bool autoOpen);
		void initiateMovie(string path, string ojName, GameObject oj, Vector3 bundlePosition, Vector3 bundleRotation, Vector3 bundleScale, bool transparent, bool loop, bool autoOpen);
		void initiateMovie(string path, string ojName, GameObject oj, GameObject player, GameObject parent, Vector3 bundlePosition, Vector3 bundleRotation, Vector3 bundleScale, bool transparent, bool loop, bool autoOpen);
	}

	public interface AVProInitialCallback
	{
		void getInitialName(string name);
		void onLoadStart(string msg);
		void onLoadFinish(string msg);
		void onLoadFailed(string msg);

		GameObject setObjectName { set; }
	}
}