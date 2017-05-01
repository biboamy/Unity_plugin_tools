using System.Collections;
using UnityEngine.Networking;
using UnityEngine;

namespace cqplayart.adv.initial
{
	public class initiateAudioBehavior : MonoBehaviour, audioInitiate
	{

		/// <summary>
		/// Get initiate assets
		/// </summary>
		UnityWebRequest request = null;

		/// <summary>
		/// Initiate callback
		/// </summary>
		audioInitiateCallback mycallback;

		/// <summary>
		/// Return the initiate progress value
		/// </summary>
		void Update()
		{
			if (request != null)
				mycallback.getInitiatePorgress(request.downloadProgress);
		}

		/// <summary>
		/// Init the script.
		/// </summary>
		/// <returns>The "initialBehavior" script.</returns>
		/// <param name="name">GameObject's name (which you want to put the script on).</param>
		/// <param name="callback">Callback object.</param>
		public initiateAudioBehavior Init(string name, audioInitiateCallback callback)
		{
			GameObject go = GameObject.Find(name);
			initiateAudioBehavior instnace = go.AddComponent<initiateAudioBehavior>();
			instnace.mycallback = callback;

			return instnace;
		}

		/// <summary>
		/// Initiates the audio object (for implementation).
		/// </summary>
		/// <returns>Wait for complete.</returns>
		/// <param name="path">Source path.</param>
		/// <param name="ojName">Object's name.</param>
		/// <param name="type">Audio's type.</param>
		/// <param name="autoPlay">Audio's auto playing setting.</param>
		/// <param name="loop">Audio's loop setting.</param>
		/// <param name="volumn">Audio's volumn.</param>
		public IEnumerator initiateAudio(string path, string ojName, AudioType type, bool autoPlay, bool loop, float volumn)
		{
			mycallback.initialStatus("Start initiating");

			GameObject oj = new GameObject();
			IEnumerator e_initiateAudio = _initiateAudio(path, ojName, type, oj, autoPlay, loop, volumn);
			while (e_initiateAudio.MoveNext()) yield return e_initiateAudio.Current;
		}

		/// <summary>
		/// Initiates the audio object (for implementation).
		/// </summary>
		/// <returns>Wait for complete.</returns>
		/// <param name="path">Source path.</param>
		/// <param name="ojName">Object's name.</param>
		/// <param name="type">Audio's type.</param>
		/// <param name="oj">The object you want to add the audio source on.</param>
		/// <param name="autoPlay">Audio's auto playing setting.</param>
		/// <param name="loop">Audio's loop setting.</param>
		/// <param name="volumn">Audio's volumn.</param>
		public IEnumerator initiateAudio(string path, string ojName, AudioType type, GameObject oj, bool autoPlay, bool loop, float volumn)
		{
			mycallback.initialStatus("Start initiating");

			IEnumerator e_initiateAudio = _initiateAudio(path, ojName, type, oj, autoPlay, loop, volumn);
			while (e_initiateAudio.MoveNext()) yield return e_initiateAudio.Current;
		}

		/// <summary>
		/// Initiates the audio object (for implementation).
		/// </summary>
		/// <returns>Wait for complete.</returns>
		/// <param name="path">Source path.</param>
		/// <param name="ojName">Object's name.</param>
		/// <param name="type">Audio's type.</param>
		/// <param name="augmentation">The object you want to add the audio source on.</param>
		/// <param name="autoPlay">Audio's auto playing setting.</param>
		/// <param name="loop">Audio's loop setting.</param>
		/// <param name="volumn">Audio's volumn.</param>
		private IEnumerator _initiateAudio(string path, string ojName, AudioType type, GameObject augmentation, bool autoPlay, bool loop, float volumn)
		{
			augmentation.name = ojName;

			AudioSource mySource = augmentation.AddComponent<AudioSource>();
			mySource.loop = loop;
			if(volumn>1 || volumn<0)
				mycallback.initialStatus("Volumn out of range");
			else
				mySource.volume = volumn;

			request = UnityWebRequest.GetAudioClip(path, type);
			yield return request.Send();

			if (request.isError)
			{
				mycallback.initialStatus("Initiate Error");
			}
			else
			{
				AudioClip clip = (request.downloadHandler as DownloadHandlerAudioClip).audioClip;
				clip.name = ojName;
				mySource.clip = clip;

				if (autoPlay)
					mySource.Play();

				mycallback.getInitialName(ojName);
				mycallback.initialStatus("Initiate complete");
			}

			request = null;
		}
	}
}