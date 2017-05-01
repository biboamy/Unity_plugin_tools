using System.Collections;
using System;
using System.IO;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using UnityEngine;

namespace cqplayart.adv.image
{
	public class imageControlBehavior : MonoBehaviour, controlImage
	{
		/// <summary>
		/// Initiate callback
		/// </summary>
		controlImageCallback mycallback;

		/// <summary>
		/// Initiate screen shot asset
		/// </summary>
		public static event Action<Texture2D> OnScreenshotTaken;

		/// <summary>
		/// iOS function - connect to _albumPicker in SDK.mm
		/// </summary>
		/// <param name="callbackName">The name of the gameobject you hang the callback script on. (The callback funtion must call "albumImagePath()")</param>
		[DllImport("__Internal")]
		private static extern void _albumPicker(string callbackName);
		/// <summary>
		/// iOS function - connect to _shareImage in SDK.mm
		/// </summary>
		/// <param name="path">The image path.</param>
		[DllImport("__Internal")]
		private static extern void _shareImage(string path);
		/// <summary>
		/// iOS function - connect to GallerySave in SDK.mm
		/// </summary>
		/// <returns>Save status.</returns>
		/// <param name="path">Path of the image you want to save in the gallery.</param>
		/// <param name="callbackName">The name of the gameobject you hang the callback script on. (The callback funtion must call "galleryPath()")</param>
		[DllImport("__Internal")]
		private static extern int GallerySave(string path, string callbackName);

		/// <summary>
		/// Initializes a new instance of the <see cref="T:cqplayart.adv.image.imageControlBehavior"/> class.
		/// </summary>
		/// <param name="callback">Callback script.</param>
		public imageControlBehavior(controlImageCallback callback)
		{
			mycallback = callback;
		}

		/// <summary>
		/// Screen shot implementation.
		/// </summary>
		/// <returns>Wait for complete.</returns>
		/// <param name="dir">Name of the directory you want to store the screen shot.</param>
		/// <param name="_name">Screen shot image name.</param>
		public IEnumerator screenShot(string dir, string _name)
		{
			mycallback.imageStatus("Start screen shot");

			bool testFinish = false;

			if (Application.platform == RuntimePlatform.Android)
			{
				string path = Application.persistentDataPath + "/" + dir + "/" + _name;
				string dirOnly = Path.GetDirectoryName(path);
				Directory.CreateDirectory(dirOnly);
				Application.CaptureScreenshot("/" + dir + "/" + _name);

				while (!testFinish)
				{
					if (File.Exists(path))
					{
						mycallback.screenShotPath(path);
						mycallback.imageStatus("Screen shot success");
						testFinish = true;
					}
					else
					{
						yield return new WaitForSeconds(0.1f);
					}
				}
			}
			else if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Rect screenArea = new Rect(0, 0, Screen.width, Screen.height);

				Texture2D texture = new Texture2D((int)screenArea.width, (int)screenArea.height, TextureFormat.RGB24, false);
				texture.ReadPixels(screenArea, 0, 0);
				texture.Apply();

				byte[] bytes;
				bytes = texture.EncodeToPNG();

				if (OnScreenshotTaken != null)
					OnScreenshotTaken(texture);
				else
					Destroy(texture);

				string path = Application.persistentDataPath + "/" + dir + "/" + _name;
				string dirOnly = Path.GetDirectoryName(path);
				Directory.CreateDirectory(dirOnly);
				Application.CaptureScreenshot("/" + dir + "/" + _name);

				System.IO.File.WriteAllBytes(path, bytes);

				while (!testFinish)
				{
					if (File.Exists(path))
					{
						mycallback.screenShotPath(path);
						mycallback.imageStatus("Screen shot success");
						testFinish = true;
					}
					else
					{
						yield return new WaitForSeconds(0.1f);
					}
				}
			}
			else
			{
				mycallback.imageStatus("Not in android or ios device");
			}
		}

		/// <summary>
		/// Save image to gallery
		/// </summary>
		/// <returns>Wait for complete.</returns>
		/// <param name="path">Path of the image you want to store in the gallery.</param>
		/// <param name="callbackName">The name of the gameobject you hang the callback script on. (The callback funtion must call "albumImagePath()").</param>
		public IEnumerator saveToGallery(string path, string callbackName)
		{
			mycallback.imageStatus("Start save to gallery");

			bool photoSaved = false;

			if (Application.platform == RuntimePlatform.Android)
			{
				while (!photoSaved)
				{
					string[] _name = path.Split('/');
					string imgName = _name[_name.Length - 1];

					IEnumerator e_downloadBundle = writeToDCIM(path, imgName);
					while (e_downloadBundle.MoveNext()) yield return e_downloadBundle.Current;

					AndroidJavaClass obj = new AndroidJavaClass("com.example.mylibrary.UnityTestActivity");
					photoSaved = obj.CallStatic<bool>("galleryAddPic", Application.persistentDataPath + "/../../../../DCIM/" + imgName, callbackName);
					yield return new WaitForSeconds(0.1f);
				}
				photoSaved = false;
				mycallback.imageStatus("Save to gallery success");
			}
			else if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				while (!photoSaved)
				{
					int saveCallback = GallerySave(path, callbackName);
					if (saveCallback == 1)
						photoSaved = true;
					yield return new WaitForSeconds(0.1f);
				}
				photoSaved = false;
				mycallback.imageStatus("Save to gallery success");
			}
			else
			{
				mycallback.imageStatus("Not in android or ios device");
			}
		}

		/// <summary>
		/// Write your image to DCIM.
		/// </summary>
		/// <returns>Wait for complete.</returns>
		/// <param name="path">Path of your image.</param>
		/// <param name="imgName">Your image name.</param>
		private IEnumerator writeToDCIM(string path, string imgName)
		{
			WWW www = new WWW("file://" + path);
			yield return www;
			string targetPath = Application.persistentDataPath + "/../../../../DCIM/" + imgName;
			File.WriteAllBytes(targetPath, www.bytes);
		}

		/// <summary>
		/// Preview your image on the plane. (Can preview in both UI and 3D plane)
		/// </summary>
		/// <returns>Wait for complete.</returns>
		/// <param name="plane">The plane you want to preview.</param>
		/// <param name="path">The path of your image.</param>
		public IEnumerator previewImage(GameObject plane, string path)
		{
			mycallback.imageStatus("Start preview plane");
			
			WWW localFile = new WWW("file:///" + path);
			yield return localFile;
			//3d plane set texture

			if (plane != null)
			{
				Renderer render = plane.GetComponent<MeshRenderer>();
				RawImage rawImage = plane.GetComponent<RawImage>();
				Image myImage = plane.GetComponent<Image>();
				if (render)
				{
					localFile.LoadImageIntoTexture(render.material.mainTexture as Texture2D);
				}
				else if (rawImage)
				{
					localFile.LoadImageIntoTexture(rawImage.texture as Texture2D);
				}
				else if (myImage)
				{
					Texture2D myImg = localFile.texture;
					myImage.sprite = Sprite.Create(myImg, new Rect(0, 0, myImg.width, myImg.height), Vector2.zero);
				}
				else
				{
					mycallback.imageStatus("Cannot set the image plane");
				}
			}
		}

		/// <summary>
		/// Share the image.
		/// </summary>
		/// <param name="path">Path of your image.</param>
		public void shareImage(string path)
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				AndroidJavaClass ojc = new AndroidJavaClass("com.example.mylibrary.UnityTestActivity");
				bool testShare = ojc.CallStatic<bool>("shareImage", path);
			}
			else if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				_shareImage(path);
			}
			else
			{
				mycallback.imageStatus("Not in android or ios device");
			}
		}

		/// <summary>
		/// Pick a picture from user's DCIM
		/// </summary>
		/// <param name="callbackName">The name of the gameobject you hang the callback script on. (The callback funtion must call "galleryPath()").</param>
		public void albumPicker(string callbackName)
		{
			mycallback.imageStatus("Start album picker");

			if (Application.platform == RuntimePlatform.Android)
			{
				Debug.Log(callbackName);
				AndroidJavaClass ojc = new AndroidJavaClass("com.example.mylibrary.UnityTestActivity");
				bool success = ojc.CallStatic<bool>("TakePhoto", "takeSave", callbackName);
			}
			else if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				_albumPicker(callbackName);
			}
			else
			{
				mycallback.imageStatus("Not in android or ios device");
			}
		}
	}
}
