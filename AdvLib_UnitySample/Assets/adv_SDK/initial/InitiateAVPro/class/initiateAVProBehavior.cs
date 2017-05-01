using System.Collections;
using System.Collections.Generic;
using RenderHeads.Media.AVProVideo;
using UnityEngine.Networking;
using UnityEngine;

namespace cqplayart.adv.initial
{
	public class initiateAVProBehavior : MonoBehaviour, AVProInitial
	{
		/// <summary>
		/// Initiate callback
		/// </summary>
		AVProInitialCallback mycallback;

		/// <summary>
		/// Init the script.
		/// </summary>
		/// <returns>The "initialBehavior" script.</returns>
		/// <param name="callback">Callback object.</param>
		public initiateAVProBehavior (AVProInitialCallback callback)
		{
			mycallback = callback;
		}

		/// <summary>
		/// Initiates the movie (Video/Audio path, object's name and transparent property).
		/// </summary>
		/// <param name="path">Video/Audio path.</param>
		/// <param name="ojName">Plane object's name.</param>
		/// <param name="transparent">The video/audio has transparent property <c>true</c> Otherwise.</param>
		/// <param name="loop">Set the loop property - is loop <c>true</c> Otherwise.</param>
		/// <param name="autoOpen">Set the autoStart property - is auto start <c>true</c> Otherwise.</param>
		public void initiateMovie(string path, string ojName, bool transparent, bool loop, bool autoOpen)
		{
			GameObject oj = GameObject.CreatePrimitive(PrimitiveType.Plane);
			GameObject player = new GameObject();
			_initiateMovie(path, ojName, oj, player, transparent, loop, autoOpen);
		}

		/// <summary>
		/// Initiates the movie (Video/Audio path, object's name, plane object and transparent property).
		/// </summary>
		/// <param name="path">Video/Audio path.</param>
		/// <param name="ojName">Plane object's name.</param>
		/// /// <param name="oj">GameObject which you want to store the plane object in.</param>
		/// <param name="transparent">The video/audio has transparent property <c>true</c> Otherwise.</param>
		/// <param name="loop">Set the loop property - is loop <c>true</c> Otherwise.</param>
		/// <param name="autoOpen">Set the autoStart property - is auto start <c>true</c> Otherwise.</param>
		public void initiateMovie(string path, string ojName, GameObject oj, bool transparent, bool loop, bool autoOpen)
		{
			GameObject player = new GameObject();
			_initiateMovie(path, ojName, oj, player, transparent, loop, autoOpen);
		}

		/// <summary>
		/// Initiates the movie (Video/Audio path, object's name, plane object, mediaPlayer object and transparent property).
		/// </summary>
		/// <param name="path">Video/Audio path.</param>
		/// <param name="ojName">Plane object's name.</param>
		/// <param name="oj">GameObject which you want to store the plane object in.</param>
		/// <param name="mediaPlayer">GameObject which you want to store the MediaPlayer in.</param>
		/// <param name="transparent">The video/audio has transparent property <c>true</c> Otherwise.</param>
		/// <param name="loop">Set the loop property - is loop <c>true</c> Otherwise.</param>
		/// <param name="autoOpen">Set the autoStart property - is auto start <c>true</c> Otherwise.</param>
		public void initiateMovie(string path, string ojName, GameObject oj, GameObject mediaPlayer, bool transparent, bool loop, bool autoOpen)
		{
			_initiateMovie(path, ojName, oj, mediaPlayer, transparent, loop, autoOpen);
		}

		/// <summary>
		/// Initiates the movie (Video/Audio path, object's name, plane object, mediaPlayer object, parameters and transparent property).
		/// </summary>
		/// <param name="path">Video/Audio path.</param>
		/// <param name="ojName">Plane object's name.</param>
		/// <param name="parent">Parent.</param>
		/// <param name="bundlePosition">Object's position.</param>
		/// <param name="bundleRotation">Object's rotation.</param>
		/// <param name="bundleScale">Object's scale.</param>
		/// <param name="transparent">The video/audio has transparent property <c>true</c> Otherwise.</param>
		/// <param name="loop">Set the loop property - is loop <c>true</c> Otherwise.</param>
		/// <param name="autoOpen">Set the autoStart property - is auto start <c>true</c> Otherwise.</param>
		public void initiateMovie(string path, string ojName, GameObject parent, Vector3 bundlePosition, Vector3 bundleRotation, Vector3 bundleScale, bool transparent, bool loop, bool autoOpen)
		{
			GameObject oj = GameObject.CreatePrimitive(PrimitiveType.Plane);
			GameObject mediaPlayer = new GameObject();
			List<GameObject> myOj = _initiateMovie(path, ojName, oj, mediaPlayer, transparent, loop, autoOpen);
			setParameter(myOj[0], parent, bundlePosition, bundleRotation, bundleScale);
			setParameter(myOj[1], parent, bundlePosition, bundleRotation, bundleScale);
		}

		/// <summary>
		/// Initiates the movie (Video/Audio path, object's name, plane object, mediaPlayer object, parameters and transparent property).
		/// </summary>
		/// <param name="path">Video/Audio path.</param>
		/// <param name="ojName">Plane object's name.</param>
		/// <param name="oj">GameObject which you want to store the plane object in.</param>
		/// <param name="mediaPlayer">GameObject which you want to store the MediaPlayer in.</param>
		/// <param name="parent">Parent.</param>
		/// <param name="bundlePosition">Object's position.</param>
		/// <param name="bundleRotation">Object's rotation.</param>
		/// <param name="bundleScale">Object's scale.</param>
		/// <param name="transparent">The video/audio has transparent property <c>true</c> Otherwise.</param>
		/// <param name="loop">Set the loop property - is loop <c>true</c> Otherwise.</param>
		/// <param name="autoOpen">Set the autoStart property - is auto start <c>true</c> Otherwise.</param>
		public void initiateMovie(string path, string ojName, GameObject oj, GameObject mediaPlayer, GameObject parent, Vector3 bundlePosition, Vector3 bundleRotation, Vector3 bundleScale, bool transparent, bool loop, bool autoOpen)
		{
			List<GameObject> myOj = _initiateMovie(path, ojName, oj, mediaPlayer, transparent, loop, autoOpen);
			setParameter(myOj[0], parent, bundlePosition, bundleRotation, bundleScale);
			setParameter(myOj[1], parent, bundlePosition, bundleRotation, bundleScale);
		}

		/// <summary>
		/// Initiates the movie (for implementation).
		/// </summary>
		/// <returns>The movie plane object.</returns>
		/// <param name="path">Video/Audio path.</param>
		/// <param name="ojName">Plane object's name.</param>
		/// <param name="oj">GameObject which you want to store the plane object in.</param>
		/// <param name="mediaPlayer">GameObject which you want to store the MediaPlayer in.</param>
		/// <param name="transparent">The video/audio has transparent property <c>true</c> Otherwise.</param>
		/// <param name="loop">Set the loop property - is loop <c>true</c> Otherwise.</param>
		/// <param name="autoOpen">Set the autoStart property - is auto start <c>true</c> Otherwise.</param>
		private List<GameObject> _initiateMovie(string path, string ojName, GameObject augmentation, GameObject mediaPlayer, bool transparent, bool loop, bool autoOpen)
		{
			mycallback.onLoadStart("Start initiating: " + path);

			List<GameObject> myOj = new List<GameObject>();

			//create audio
			mediaPlayer.name = "media_" + ojName;
			mediaPlayer.AddComponent<MediaPlayer>();
			mediaPlayer.GetComponent<MediaPlayer>().m_VideoLocation = MediaPlayer.FileLocation.RelativeToPeristentDataFolder;
			mediaPlayer.GetComponent<MediaPlayer>().m_VideoPath = path;
			mediaPlayer.GetComponent<MediaPlayer>().m_Loop = loop;
			mediaPlayer.GetComponent<MediaPlayer>().m_AutoStart = autoOpen;

			//create plane and set texture
			augmentation.name = ojName;
			augmentation.AddComponent<ApplyToMesh>();
			augmentation.GetComponent<ApplyToMesh>().MeshRenderer = augmentation.GetComponent<MeshRenderer>();
			augmentation.GetComponent<ApplyToMesh>().Player = mediaPlayer.GetComponent<MediaPlayer>();

			if (transparent)
			{
				augmentation.GetComponent<Renderer>().sharedMaterial.shader = Shader.Find("AVProVideo/Unlit/Transparent (texture+color+fog+packed alpha)");
				mediaPlayer.GetComponent<MediaPlayer>().m_AlphaPacking = AlphaPacking.LeftRight;
			}
			else
				mediaPlayer.GetComponent<MediaPlayer>().m_AlphaPacking = AlphaPacking.None;

			mycallback.getInitialName(ojName);
			mycallback.onLoadFinish("Initiate complete: " + path);

			myOj.Add(augmentation);
			myOj.Add(mediaPlayer);

			return myOj;
		}

		/// <summary>
		/// Set gameobject's basic parameters.
		/// </summary>
		/// <param name="augmentation">GameObject.</param>
		/// <param name="parent">GameObject's parent object.</param>
		/// <param name="bundlePosition">Object's position.</param>
		/// <param name="bundleRotation">Object's rotation.</param>
		/// <param name="bundleScale">Object's scale.</param>
		private void setParameter(GameObject augmentation, GameObject parent, Vector3 bundlePosition, Vector3 bundleRotation, Vector3 bundleScale)
		{
			augmentation.transform.parent = parent.transform;
			augmentation.transform.localPosition = bundlePosition;

			augmentation.transform.localEulerAngles = bundleRotation;
			augmentation.transform.localScale = bundleScale;
		}
	}
}
