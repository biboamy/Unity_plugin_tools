using System.Collections;
using UnityEngine.Networking;
using UnityEngine;
using System;

namespace cqplayart.adv.initial
{
	public class initiateImageBehavior : MonoBehaviour, imageInitiate
	{
		/// <summary>
		/// Get initiate assets
		/// </summary>
		UnityWebRequest request = null;

		/// <summary>
		/// Initiate callback
		/// </summary>
		imageInitiateCallback mycallback;

		/// <summary>
		/// Init the script.
		/// </summary>
		/// <returns>The "initialBehavior" script.</returns>
		/// <param name="callback">Callback object.</param>
		public initiateImageBehavior (imageInitiateCallback callback)
		{
			mycallback = callback;
		}

		/// <summary>
		/// Initiates the image.
		/// </summary>
		/// <returns>Wait for complete.</returns>
		/// <param name="path">Image path.</param>
		/// <param name="ojName">Object name.</param>
		public IEnumerator initiateImage(string path, string ojName)
		{
			GameObject oj = GameObject.CreatePrimitive(PrimitiveType.Plane);
			IEnumerator e_initiateImage = initiateImage(path, ojName, oj);
			while (e_initiateImage.MoveNext()) yield return e_initiateImage.Current;
		}

		/// <summary>
		/// Initiates the image.
		/// </summary>
		/// <returns>Wait for complete.</returns>
		/// <param name="path">Image path.</param>
		/// <param name="ojName">Object name.</param>
		/// <param name="oj">GameObject which you want to store the image object in.</param>
		public IEnumerator initiateImage(string path, string ojName, GameObject oj)
		{
			mycallback.onLoadStart("Start initiating: " + path);

			try
			{
				request = UnityWebRequest.GetTexture(path);
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


			if (request.isError)
			{
				mycallback.onLoadFailed("Initiate Error");
			}
			else
			{
				_initiateImage(ojName, oj);
			}
		}

		/// <summary>
		/// Initiates the image.
		/// </summary>
		/// <returns>Wait for complete.</returns>
		/// <param name="path">Image path.</param>
		/// <param name="ojName">Object's name.</param>
		/// <param name="parent">Parent.</param>
		/// <param name="bundlePosition">Object's position.</param>
		/// <param name="bundleRotation">Object's rotation.</param>
		/// <param name="bundleScale">Object's scale.</param>
		public IEnumerator initiateImage(string path, string ojName, GameObject parent, Vector3 bundlePosition, Vector3 bundleRotation, Vector3 bundleScale)
		{
			GameObject oj = GameObject.CreatePrimitive(PrimitiveType.Plane);
			IEnumerator e_initiateImage = initiateImage(path, ojName, oj, parent, bundlePosition, bundleRotation, bundleScale);
			while (e_initiateImage.MoveNext()) yield return e_initiateImage.Current;
		}

		/// <summary>
		/// Initiates the image.
		/// </summary>
		/// <returns>Wait for complete.</returns>
		/// <param name="path">Image path.</param>
		/// <param name="ojName">Object's name.</param>
		/// <param name="oj">GameObject which you want to store the image object in.</param>
		/// <param name="parent">Parent.</param>
		/// <param name="bundlePosition">Object's position.</param>
		/// <param name="bundleRotation">Object's rotation.</param>
		/// <param name="bundleScale">Object's scale.</param>
		public IEnumerator initiateImage(string path, string ojName, GameObject oj, GameObject parent, Vector3 bundlePosition, Vector3 bundleRotation, Vector3 bundleScale)
		{
			mycallback.onLoadStart("Start initiating: " + path);

			try
			{
				request = UnityWebRequest.GetTexture(path);
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

			if (request.isError)
			{
				mycallback.onLoadFailed("Initiate Error");
			}
			else
			{
				GameObject myOj = _initiateImage(ojName, oj);
				setParameter(myOj, parent, bundlePosition, bundleRotation, bundleScale);
			}
		}

		/// <summary>
		/// Initiates the image (for implementation).
		/// </summary>
		/// <returns>The image gameobject.</returns>
		/// <param name="ojName">Object's name.</param>
		/// <param name="augmentation">GameObject which you want to store the image object in.</param>
		private GameObject _initiateImage(string ojName, GameObject augmentation)
		{
			//create plane and set texture
			augmentation.name = ojName;
			try
			{
				augmentation.GetComponent<Renderer>().material.mainTexture = (request.downloadHandler as DownloadHandlerTexture).texture;
			}
			catch (Exception e)
			{
				mycallback.onLoadFailed("Initiate Error: " + e);
			}

			do
			{
				mycallback.getInitiatePorgress(request.downloadProgress, ojName);
			} while (request.downloadProgress > 0 && request.downloadProgress<1);
			mycallback.getInitiatePorgress(1, ojName);
			mycallback.onLoadFinish("Initiate complete: " + ojName);

			return augmentation;
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
			augmentation.gameObject.SetActive(false);
		}

	}
}
