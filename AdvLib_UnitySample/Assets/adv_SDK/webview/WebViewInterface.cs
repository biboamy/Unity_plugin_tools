using UnityEngine;

namespace cqplayart.adv.webview
{
	public interface webview
	{

		void Init(string callback);
		void Term();
		void SetWebviewMargins(int width, int height, int x, int y);
		void SetNavigateBar(string title, int width, int height, int x, int y, string btnImg, int btnWidth, int btnHight, int btnX, int btnY, string bgImg);
		void SetVisibility(bool state);
		void LoadURL(string url);
		void EvaluateJS(string js);
		void externalWebview(string url);
	}

	public interface webviewCallback
	{
		void onLoadStart(string url);
		void onLoadFinish(string url);
		void onLoadFail(string url);
	}
}