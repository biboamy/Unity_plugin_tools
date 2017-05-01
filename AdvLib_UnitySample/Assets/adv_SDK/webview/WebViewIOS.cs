using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace cqplayart.adv.webview
{
	public class WebViewIOS : MonoBehaviour, webview
	{
		
		private IntPtr instance;
		
		#region Interface Method
		public void Init(string callback )
		{
			instance = _WebViewPlugin_Init( callback );
		}

		public void Term()
		{
			_WebViewPlugin_Destroy( instance );
		} 

		public void SetWebviewMargins( int width, int height, int x, int y )
		{
			_WebViewPlugin_SetMargins( instance, width, height, x, y );
		}

		public void SetNavigateBar(string title, int width, int height, int x, int y, string btnImg, int btnWidth, int btnHight, int btnX, int btnY, string bgImg)
		{
			_WebViewPlugin_SetNavigateBar(instance, title, width, height, x, y, btnImg, btnWidth, btnHight, btnX, btnY, bgImg);
		}

		public void SetVisibility( bool state )
		{
			_WebViewPlugin_SetVisibility( instance, state );
		}

		public void LoadURL( string url )
		{
			_WebViewPlugin_LoadURL( instance, url );
		}

		public void EvaluateJS( string js )
		{
			//_WebViewPlugin_EvaluateJS( instance, js );
		}

		public void externalWebview(string url)
		{
			Application.OpenURL(url);
		}
		#endregion
		
		#region Native Access Method
		[DllImport("__Internal")]
		private static extern IntPtr _WebViewPlugin_Init(string gameObject);
		
		[DllImport("__Internal")]
		private static extern int _WebViewPlugin_Destroy(IntPtr instance);
		
		[DllImport("__Internal")]
		private static extern void _WebViewPlugin_SetMargins(
			IntPtr instance, int left, int top, int right, int bottom);

		[DllImport("__Internal")]
		private static extern void _WebViewPlugin_SetNavigateBar(
			IntPtr instance, string x, int y, int width, int height, int bottom, string btnImg, int btnWidth, int btnHight, int btnX, int btnY, string bgImg);
		
		[DllImport("__Internal")]
		private static extern void _WebViewPlugin_SetVisibility(
			IntPtr instance, bool visibility);
		
		[DllImport("__Internal")]
		private static extern void _WebViewPlugin_LoadURL(
			IntPtr instance, string url);
		
		[DllImport("__Internal")]
		private static extern void _WebViewPlugin_EvaluateJS(
			IntPtr instance, string url);
		
		#endregion
	}
}