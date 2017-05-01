using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using System.Net;
using System.Text;
using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace cqplayart.adv.advjson
{
	public class AdvJsonBehavior : MonoBehaviour,advjson
	{
		string jsontxt;
		JsonData jsonvale;

		private static mysjsoncallback mycallback;
		private static List<List<string>> json_target = new List<List<string>>();

		/// <summary>
		/// set callback
		/// </summary>
		/// <param name="callback">Callback.</param>
		public AdvJsonBehavior(mysjsoncallback callback)
		{
			mycallback = callback;
		}

		/// <summary>
		/// check ssl connect
		/// </summary>
		/// <returns><c>true</c>, if remote certificate validation callback was oned, <c>false</c> otherwise.</returns>
		/// <param name="sender">Sender.</param>
		/// <param name="certificate">Certificate.</param>
		/// <param name="chain">Chain.</param>
		/// <param name="sslPolicyErrors">Ssl policy errors.</param>
		private bool OnRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			return true;
		}

		/// <summary>
		/// Parse url and return json string
		/// </summary>
		/// <returns>The json string.</returns>
		/// <param name="url">URL.</param>
		public IEnumerator gethtml(string url)
		{
			mycallback.statusCallback("get html");
			//set ssl
			ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(OnRemoteCertificateValidationCallback);
			WebRequest request = HttpWebRequest.Create(url);
			request.Method = "GET";
			WebResponse resp = null;
			try
			{
				resp = request.GetResponse();
			}
			catch
			{
				resp = null;
		    
			}
			yield return 1;
			if (resp != null)
			{
		    	Stream st = resp.GetResponseStream();
				StreamReader sr = new StreamReader(st, Encoding.GetEncoding("UTF-8"));
				
				string mytxt = sr.ReadToEnd();
				jsontxt = mytxt;
		      
				mycallback.onLoadHtmlText(mytxt);
					
				sr.Close();
				st.Close();
			}
		}

		/// <summary>
		/// Turn json string to json data
		/// </summary>
		/// <param name="myurlstring">Json string.</param>
		public void getJsonData(string myurlstring)
		{
			mycallback.statusCallback("HTML to Json data");
			jsonvale = JsonMapper.ToObject(myurlstring);
			mycallback.onjsonData(jsonvale, jsonvale.Count);
        }

		/// <summary>
		/// Set array json data into one dimension array
		/// </summary>
		/// <param name="jsonvale">Json value.</param>
		public void setArrayList(JsonData jsonvale)
		{
			mycallback.statusCallback("get json array list");
			List<string> targetlist = new List<string>();
			for (int i = 0; i < jsonvale.Count; i++)
			{
				targetlist.Add(jsonvale[i].ToString());

			}
			mycallback.onArrayList(targetlist);
		}

		/// <summary>
		/// Set json data into one dimension array
		/// </summary>
		/// <param name="mjsonvale">Json value.</param>
		/// <param name="mjsonarray">Value's titile array.</param>
		public void setJsonList(JsonData mjsonvale, string[] mjsonarray) 
		{
			mycallback.statusCallback("get json one dimension list");
			int jsoncount = mjsonarray.GetLength(0);
			List<string> targetlist = new List<string>();
			for (int i = 0; i<jsoncount; i++)
			{
				try
				{
					targetlist.Add(mjsonvale[mjsonarray[i]].ToString());
				}
				catch
				{
					mycallback.statusCallback("key :" + mjsonarray[i] + " not found");
				}
			}
			mycallback.onjsonList(targetlist);
		}

		/// <summary>
		/// Set json data into two dimension array
		/// </summary>
		/// <param name="jsonvale">Json value.</param>
		/// <param name="jsonarray">Title's array.</param>
		/// <param name="dataarray">Value's title array.</param>
		public void setJsonTwoList(JsonData jsonvale,string[] jsonarray,string [] dataarray) 
		{
			mycallback.statusCallback("get json two dimension list");
			int jsoncount = jsonarray.GetLength(0); 
			int datacount = dataarray.GetLength(0);
			List<List<string>> m_jsontarget= new List<List<string>>();	

			for (int i = 0; i<jsoncount; i++)
			{
				List<string> targetlist = new List<string>();
				for (int j = 0; j < datacount; j++)
				{
					try
					{
						targetlist.Add(jsonvale[jsonarray[i]][dataarray[j]].ToString());
					}
					catch
					{
						mycallback.statusCallback("key :" + dataarray[j] + " not found");
					}
				}
				
				m_jsontarget.Add(targetlist);	
			}

			mycallback.onjsonTwoList(m_jsontarget);
		}

		/// <summary>
		/// Set json data to three dimension array
		/// </summary>
		/// <param name="jsonvale">Json value.</param>
		/// <param name="jsonarray">Title's array.</param>
		/// <param name="dataarray">Value's title array.</param>
		public void setJsonThreeList(JsonData jsonvale, string[] jsonarray, string[] dataarray)
		{
			mycallback.statusCallback("get json three dimension list");
			int jsoncount = jsonarray.GetLength(0);
			int datacount = dataarray.GetLength(0);
			List<List<List<string>>> m_jsontarget = new List<List<List<string>>>();

			for (int i = 0; i < jsoncount; i++)
			{
				List<List<string>> m_jsontarget_2 = new List<List<string>>(); 
				for (int k = 0; k < jsonvale[i].Count; k++)
				{
					List<string> targetlist = new List<string>();
					for (int j = 0; j < datacount; j++)
					{
						try
						{
							targetlist.Add(jsonvale[jsonarray[i]][k][dataarray[j]].ToString());
						}
						catch
						{
							mycallback.statusCallback("key :" + dataarray[j] + " not found");
						}
					}
					m_jsontarget_2.Add(targetlist);
				}
				m_jsontarget.Add(m_jsontarget_2);
			}

			mycallback.onjsonThreeList(m_jsontarget);
		}
	}
}


