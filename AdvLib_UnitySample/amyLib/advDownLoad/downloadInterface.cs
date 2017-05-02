using System.Collections;
namespace amy.download
{
	public interface advWebRequest
	{
		void addDownloadList(string path);
		IEnumerator LoadList(string dir);
		IEnumerator LoadURL(string onlinePath, string dirname, string fielname);
	}

	public interface WebRequestCallback
	{
		void onLoadStart(string url);
		void onLoadFinish(string url);
		void onLoadFail(string url);
		void onLoadStatus(string[] statu);
	}
}