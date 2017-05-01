using System.Collections;
namespace cqplayart.amy.download
{
	public interface advWebRequest
	{
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