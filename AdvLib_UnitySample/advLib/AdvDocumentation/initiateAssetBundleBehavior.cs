using System.Collections;
using UnityEngine.Networking;
using UnityEngine;
using System.Collections.Generic;

namespace cqplayart.adv.initial
{
	public class initiateAssetBundleBehavior : MonoBehaviour, assetbundleInitial
	{

		/// <summary>
		/// Get initiate assets
		/// </summary>
		UnityWebRequest request = null;

		/// <summary>
		/// Initiate callback
		/// </summary>
		assetbundleInitialCallback mycallback;

		/// <summary>
		/// Init the script.
		/// </summary>
		/// <returns>The "initialBehavior" script.</returns>
		/// <param name="name">GameObject's name (which you want to put the script on).</param>
		/// <param name="callback">Callback object.</param>
		public initiateAssetBundleBehavior Init(string name, assetbundleInitialCallback callback)
		{
			GameObject go = GameObject.Find(name);
			initiateAssetBundleBehavior instnace = go.AddComponent<initiateAssetBundleBehavior>();
			instnace.mycallback = callback;

			return instnace;
		}

		/// <summary>
		/// Return the initiate progress value
		/// </summary>
		void Update()
		{
			if (request != null)
				mycallback.getInitiatePorgress(request.downloadProgress);
		}

		/// <summary>
		/// Initiates the assetbundle.
		/// </summary>
		/// <returns>Wait for complete.</returns>
		/// <param name="path">Bundle path.</param>
		/// <param name="ojName">Object name.</param>
		public IEnumerator initiateAssetbundle(string path, string ojName)
		{
			GameObject oj = null;
			IEnumerator e_initiateImage = initiateAssetbundle(path, ojName, oj);
			while (e_initiateImage.MoveNext()) yield return e_initiateImage.Current;
		}

		/// <summary>
		/// Initiates the assetbundle.
		/// </summary>
		/// <returns>Wait for complete.</returns>
		/// <param name="path">Bundle path.</param>
		/// <param name="ojName">Object name.</param>
		/// <param name="augmentation">GameObject which you want to store the assetbundle in.</param>
		private IEnumerator initiateAssetbundle(string path, string ojName, GameObject augmentation)
		{
			mycallback.initialStatus("Start initiating");

			request = UnityWebRequest.GetAssetBundle(path);
			yield return request.Send();

			if (request.isError)
			{
				mycallback.initialStatus("Initiate Error");
			}
			else
			{
				//create bundle
				AssetBundle curBundleObj = (request.downloadHandler as DownloadHandlerAssetBundle).assetBundle;
				AssetBundleRequest obj = curBundleObj.LoadAssetAsync(ojName, typeof(GameObject));    //异步加载GameObject类型
				yield return obj;
				augmentation = Instantiate(obj.asset) as GameObject;
				ResetShader(augmentation);
				yield return null;
				curBundleObj.Unload(false);     //卸载所有包含在bundle中的对象,已经加载的才会卸载

				mycallback.getInitialName(ojName);
				mycallback.initialStatus("Initiate complete");

				request = null;
			}
		}

		/// <summary>
		/// Initiates the assetbundle.
		/// </summary>
		/// <returns>Wait for complete.</returns>
		/// <param name="path">Bundle path.</param>
		/// <param name="ojName">Object name.</param>
		/// <param name="parent">Parent.</param>
		/// <param name="bundlePosition">Object's position.</param>
		/// <param name="bundleRotation">Object's rotation.</param>
		/// <param name="bundleScale">Object's scale.</param>
		public IEnumerator initiateAssetbundle(string path, string ojName, GameObject parent, Vector3 bundlePosition, Vector3 bundleRotation, Vector3 bundleScale)
		{
			GameObject oj = null;
			IEnumerator e_initiateImage = initiateAssetbundle(path, ojName, oj, parent, bundlePosition, bundleRotation, bundleScale);
			while (e_initiateImage.MoveNext()) yield return e_initiateImage.Current;
		}

		/// <summary>
		/// Initiates the assetbundle.
		/// </summary>
		/// <returns>Wait for complete.</returns>
		/// <param name="path">Bundle path.</param>
		/// <param name="ojName">Object name.</param>
		/// <param name="augmentation">GameObject which you want to store the assetbundle in.</param>
		/// <param name="parent">Parent.</param>
		/// <param name="bundlePosition">Object's position.</param>
		/// <param name="bundleRotation">Object's rotation.</param>
		/// <param name="bundleScale">Object's scale.</param>
		private IEnumerator initiateAssetbundle(string path, string ojName, GameObject augmentation, GameObject parent, Vector3 bundlePosition, Vector3 bundleRotation, Vector3 bundleScale)
		{
			mycallback.initialStatus("Start initiating");

			request = UnityWebRequest.GetAssetBundle(path);
			yield return request.Send();

			if (request.isError)
			{
				mycallback.initialStatus("Initiate Error");
			}
			else
			{
				//create bundle
				AssetBundle curBundleObj = (request.downloadHandler as DownloadHandlerAssetBundle).assetBundle;
				AssetBundleRequest obj = curBundleObj.LoadAssetAsync(ojName, typeof(GameObject));
				yield return obj;
				augmentation = Instantiate(obj.asset) as GameObject;
				ResetShader(augmentation);
				yield return null;
				curBundleObj.Unload(false);

				setParameter(augmentation, parent, bundlePosition, bundleRotation, bundleScale);

				mycallback.getInitialName(ojName);
				mycallback.initialStatus("Initiate complete");

				request = null;
			}
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

		/// <summary>
		/// Reset bundle's shader. The shader material must put into "Resourse folder"
		/// </summary>
		/// <param name="obj">GameObject.</param>
		private void ResetShader(UnityEngine.Object obj)
		{
			List<Material> listMat = new List<Material>();
			listMat.Clear();

			if (obj is Material)
			{
				Material m = obj as Material;
				listMat.Add(m);
			}
			else if (obj is GameObject)
			{
				GameObject _go = obj as GameObject;
				Renderer[] rends = _go.GetComponentsInChildren<Renderer>();
				if (null != rends)
				{
					foreach (Renderer item in rends)
					{
						Material[] materialsArr = item.sharedMaterials;
						foreach (Material m in materialsArr)
							listMat.Add(m);
					}
				}
			}

			for (int i = 0; i < listMat.Count; i++)
			{
				Material m = listMat[i];
				if (null == m)
					continue;
				var shaderName = m.shader.name;
				var newShader = Shader.Find(shaderName);
				if (newShader != null)
				{
					m.shader = newShader;
				}
			}
		}
	}
}