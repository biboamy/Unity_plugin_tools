using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace cqplayart.amy.download
{
	/// <summary>
	/// 下載序列
	/// </summary>
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

		private static WebRequestCallback mycallback;

		/// <summary>
		/// 建立回傳
		/// </summary>
		/// <param name="callback">Callback.</param>
		public downloadBehavior(WebRequestCallback callback)
		{
			mycallback = callback;
		}

		//SSL check
		private bool OnRemoteCertificateValidationCallback(System.Object sender,X509Certificate certificate,X509Chain chain,SslPolicyErrors sslPolicyErrors)
		{
			return true;  
		}

		//download
		public IEnumerator LoadURL(string onlinePath, string dirname, string fielname)
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
				string path = Application.persistentDataPath + dirname + "/" + fielname;
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
						string[] stringArray = new string[3];
						stringArray[0] = nowmb;
						stringArray[1] = nowprecent;
						stringArray[2] = totalmb;
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

		//check dir
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