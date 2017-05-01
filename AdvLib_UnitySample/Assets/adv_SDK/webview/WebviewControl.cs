using UnityEngine;
using System;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using cqplayart.adv.webview;
using cqplayart.adv.advjson;

public class WebviewControl : MonoBehaviour
{
	webview webView;
	webviewCallback callback;

	advjson myscript;
	mysjsoncallback mycallback;

	private string[] jsonTitle = new string[] { "url", "width", "height", "x", "y", "nav_width", "nav_height", "nav_x", "nav_y" };

	List<string> myJson = new List<String>();

	#region Method

	public void Awake()
	{
		if (Application.platform == RuntimePlatform.Android)
			webView = new WebViewAndroid();
		else if(Application.platform == RuntimePlatform.IPhonePlayer)
			webView = new WebViewIOS();
		else
			webView = null;
	}

	public void Start()
	{
		//initiate the webview
		webView.Init("callback");

		mycallback = new AdvJsoncallback();
		myscript = new AdvJsonBehavior(mycallback);

		StartCoroutine(parseJson());
	}

	private IEnumerator parseJson()
	{
		IEnumerator e_gethtml = myscript.gethtml("https://app.cqplayart.com/webview/webview_json.php");
		while (e_gethtml.MoveNext()) yield return e_gethtml.Current;
		string htmlString = mycallback.getHtmlString;
		JsonData jsonData = myscript.getJsonData(htmlString);
		myJson = myscript.setJsonList(jsonData, jsonTitle);
	}

	public void startWebview_full()
	{
		//load the webview url
		webView.LoadURL(myJson[0]);
		//set webview size
		webView.SetWebviewMargins(Screen.width, Screen.height, 0, 0);
		//set navigation bar size
		webView.SetNavigateBar("cqplayart webview", Screen.width, 50, 0, 0,
							   "file://" + Application.streamingAssetsPath + "/close.png", 50, 50, 0, 0,
								  "file://" + Application.streamingAssetsPath + "/bg.png");
		//set webview visibility
		webView.SetVisibility(true);
	}

	public void startWebview_cus()
	{
		//load the webview url
		webView.LoadURL(myJson[0]);
		//set webview size
		webView.SetWebviewMargins(int.Parse(myJson[1]), int.Parse(myJson[2]), int.Parse(myJson[3]), int.Parse(myJson[4]));
		//set navigation bar size
		webView.SetNavigateBar("cqplayart webview", int.Parse(myJson[5]), int.Parse(myJson[6]), int.Parse(myJson[7]), int.Parse(myJson[8]),
		                       "file://" + Application.streamingAssetsPath + "/close.png", 50, 50, 0, 0,
		                      	"file://" + Application.streamingAssetsPath + "/bg.png");
		//set webview visibility
		webView.SetVisibility(true);
	}

	public void OnDestroy()
	{
		webView.Term();
	}

	public void EvaluateJS(string js)
	{
		webView.EvaluateJS(js);
	}

	public void CallFromJS( string message )
	{
		Debug.Log( "CallFromJS : " + message );
	}

	public void externalWebview()
	{
		Debug.Log(myJson[0]);
		webView.externalWebview(myJson[0]);
	}

	#endregion
}