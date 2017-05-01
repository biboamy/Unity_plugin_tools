using UnityEngine;
using System.Collections;
using System;
using cqplayart.adv.download;

namespace cqplayart.adv.webview
{
	public class WebViewAndroid : webview
	{
		
		AndroidJavaObject webView = null;
		string inputString = "";

		public void Init(string name)
		{
			webView = new AndroidJavaObject("com.example.mylibrary.webview.WebViewPlugin");
			SafeCall("Init", name);
		}

		public void Term()
		{
			SafeCall("Destroy");
		}

		public void SetWebviewMargins(int width, int height, int x, int y)
		{
			SafeCall("SetMargins", width, height, x, y);
		}

		public void SetNavigateBar(string title, int width, int height, int x, int y, string btnImg, int btnWidth, int btnHeight, int btnX, int btnY, string bgImg)
		{
			SafeCall("SetNavMargins", title, width, height, x, y, btnImg, btnWidth, btnHeight, btnX, btnY, bgImg);
		}

		public void SetVisibility(bool state)
		{
			SafeCall("SetVisibility", state);
		}

		public void LoadURL(string url)
		{
			SafeCall("LoadURL", url);
		}

		public void EvaluateJS(string js)
		{
			SafeCall("LoadURL", "javascript:" + js);
		}

		private void SafeCall(string method, params object[] args)
		{
			if (webView != null)
			{
				webView.Call(method, args);
			}
			else
			{
				Debug.LogError("webview is not created. you check is a call 'Init' method");
			}
		}

		public void externalWebview(string url)
		{
			Application.OpenURL(url);
		}
	}
}