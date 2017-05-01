using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace cqplayart.adv.download
{
	public class downloadBehavior : MonoBehaviour, advWebRequest
	{
		HttpWebRequest httpWebRequest;
		HttpWebResponse httpResponse;
		System.IO.Stream dataStream;
		byte[] buffer = new byte[8192];
		int size = 0;
		float downloadMemory = 0;
		bool completeCheck = false;
		FileStream fileStream;

		static List<string> waitList = new List<string>();

		private static WebRequestCallback mycallback;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:cqplayart.adv.download.downloadBehavior"/> class.
		/// </summary>
		/// <param name="callback">Callback script.</param>
		public downloadBehavior(WebRequestCallback callback)
		{
			mycallback = callback;
		}

		//SSL check
		private bool OnRemoteCertificateValidationCallback(System.Object sender,X509Certificate certificate,X509Chain chain,SslPolicyErrors sslPolicyErrors)
		{
			return true;  
		}

		/// <summary>
		/// Add URL to the download list
		/// </summary>
		/// <param name="path">Path of the URL.</param>
		public void addDownloadList(string path)
		{
			waitList.Add(path);
		}

		/// <summary>
		/// Start download the object from the download list
		/// </summary>
		/// <returns>Wait for complete.</returns>
		/// <param name="dir">The name of the directory you want to store the download assets in.</param>
		public IEnumerator LoadList(string dir)
		{
			if(waitList.Count != 0)
			{
				string[] _fileName = waitList[0].Split('/');
				string fileName = _fileName[_fileName.Length - 1];

				IEnumerator e_downloadBundle = LoadURL(waitList[0], dir, fileName);
				while (e_downloadBundle.MoveNext()) yield return e_downloadBundle.Current;

				waitList.Remove(waitList[0]);

				if (waitList.Count != 0)
				{
					IEnumerator e_downloadBundle1 = LoadList(dir);
					while (e_downloadBundle1.MoveNext()) yield return e_downloadBundle1.Current;
				}
			}
			else
			{
				mycallback.onLoadFail("LoadFail: List is empty");
			}
		}

		/// <summary>
		/// Download the asset
		/// </summary>
		/// <returns>Wait for complete.</returns>
		/// <param name="onlinePath">Asset's url.</param>
		/// <param name="dirname">Name of the directory in the lcoal storage.</param>
		/// <param name="filename">Name of the file you want to store.</param>
		public IEnumerator LoadURL(string onlinePath, string dirname, string filename)
		{
			try
			{
				//check ssl
				ServicePointManager.ServerCertificateValidationCallback =new RemoteCertificateValidationCallback(OnRemoteCertificateValidationCallback);
				//creat http
				httpWebRequest = (HttpWebRequest)WebRequest.Create(onlinePath);
				// get http response
				httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				// resopnse to datastrea
				dataStream = httpResponse.GetResponseStream();
				//check dir
				CheckDir(dirname);
				string path = Application.persistentDataPath + dirname + "/" + filename;
				fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
				downloadMemory = 0;
			}
			catch (Exception e)
			{
				mycallback.onLoadFail("LoadFail:"+e);
				Debug.Log(e);
			}

			int size = 0;
			do
			{
				try
				{
					size = dataStream.Read(buffer, 0, buffer.Length);

					if (size > 0)
					{
						fileStream.Write(buffer, 0, size);
						downloadMemory += size;
						string nowmb = (downloadMemory / 1048576).ToString("f1");
						string totalmb = ((float)httpResponse.ContentLength / 1048576).ToString("f1");
						string nowprecent = ((downloadMemory / (float)httpResponse.ContentLength) * 100).ToString("f1");
						string[] stringArray = new string[4];
						stringArray[0] = nowmb;
						stringArray[1] = nowprecent;
						stringArray[2] = totalmb;
						stringArray[3] = filename;
						mycallback.onLoadStatus(stringArray);
					}
					else
					{
						fileStream.Close();
						httpResponse.Close();
						buffer = new byte[8192];

						mycallback.onLoadFinish("loadFinish");
					}
				}
				catch (Exception e)
				{
					mycallback.onLoadFail("LoadFail:"+e);
					Debug.Log(e);
				}
				yield return size;
			} while (size > 0);
		}

		/// <summary>
		/// Check whether the directory exist or not
		/// </summary>
		/// <param name="targetDir">The directory's path.</param>
		public void CheckDir(string targetDir)
		{
			string dirPath = Application.persistentDataPath + targetDir + "/";
			if (!Directory.Exists(dirPath))
			{
				Directory.CreateDirectory(dirPath);
			}
			Debug.Log("dir:" + dirPath);
		}
	}
}