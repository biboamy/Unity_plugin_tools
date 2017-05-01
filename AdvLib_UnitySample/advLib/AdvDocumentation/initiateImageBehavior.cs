using System.Collections;
using UnityEngine.Networking;
using UnityEngine;

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
		public initiateImageBehavior Init(string name, imageInitiateCallback callback)
		{
			GameObject go = GameObject.Find(name);
			initiateImageBehavior instnace = go.AddComponent<initiateImageBehavior>();
			instnace.mycallback = callback;

			return instnace;
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
			mycallback.initialStatus("Start initiating");

			request = UnityWebRequest.GetTexture(path);
			yield return request.Send();

			if (request.isError)
			{
				mycallback.initialStatus("Initiate Error");
			}
			else
			{
				_initiateImage(ojName, oj, request);
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
			mycallback.initialStatus("Start initiating");

			request = UnityWebRequest.GetTexture(path);
			yield return request.Send();

			if (request.isError)
			{
				mycallback.initialStatus("Initiate Error");
			}
			else
			{
				GameObject myOj = _initiateImage(ojName, oj, request);
				setParameter(myOj, parent, bundlePosition, bundleRotation, bundleScale);
			}
		}

		/// <summary>
		/// Initiates the image (for implementation).
		/// </summary>
		/// <returns>The image gameobject.</returns>
		/// <param name="ojName">Object's name.</param>
		/// <param name="augmentation">GameObject which you want to store the image object in.</param>
		private GameObject _initiateImage(string ojName, GameObject augmentation, UnityWebRequest request)
		{
			//create plane and set texture
			augmentation.name = ojName;
			augmentation.GetComponent<Renderer>().material.mainTexture = (request.downloadHandler as DownloadHandlerTexture).texture;

			mycallback.getInitialName(ojName);
			mycallback.initialStatus("Initiate complete");

			request = null;

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
