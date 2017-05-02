using System.Collections;
using UnityEngine.Networking;
using UnityEngine;
using System;

namespace amy.initial
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
		/// Init the script.
		/// </summary>
		/// <returns>The "initialBehavior" script.</returns>
		/// <param name="callback">Callback object.</param>
		public initiateAudioBehavior (audioInitiateCallback callback)
		{
			mycallback = callback;
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
			mycallback.onLoadStart("Start initiating: " + path);

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
			mycallback.onLoadStart("Start initiating: " + path);

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
				mycallback.onLoadFailed("Volumn out of range");
			else
				mySource.volume = volumn;

			try
			{
				request = UnityWebRequest.GetAudioClip(path, type);

			}
			catch (Exception e)
			{
				mycallback.onLoadFailed("Initiate Error: " + e);
			}

			AsyncOperation ao = request.Send();
			while (!ao.isDone)
			{
				mycallback.getInitiatePorgress(request.downloadProgress, ojName);
				yield return new WaitForEndOfFrame();
			}

			yield return request.isDone == true;


			try
			{
				AudioClip clip = (request.downloadHandler as DownloadHandlerAudioClip).audioClip;
				clip.name = ojName;
				mySource.clip = clip;

				if (autoPlay)
					mySource.Play();

				do
				{
					mycallback.getInitiatePorgress(request.downloadProgress, ojName);
				} while (request.downloadProgress > 0 && request.downloadProgress < 1);
				mycallback.getInitiatePorgress(1, ojName);
				mycallback.onLoadFinish("Initiate complete: " + path);
			}
			catch (Exception e)
			{
				mycallback.onLoadFailed("Initiate Error: " + e);
			}

			request = null;
		}
	}
}