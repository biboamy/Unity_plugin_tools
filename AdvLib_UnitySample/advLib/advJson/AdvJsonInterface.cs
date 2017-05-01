using System.Collections;
using System.Collections.Generic;
using LitJson;

namespace cqplayart.adv.advjson
{
	public interface advjson
	{
		IEnumerator gethtml(string onlinePath);
		JsonData getJsonData(string jsonstring);
		List<string> setArrayList(JsonData jsonvale);
		List<string> setJsonList(JsonData jsonvale, string[] jsonarray);
		List<List<string>> setJsonTwoList(JsonData jsonvale,string[] jsonarray,string [] dataarray);
		List<List<List<string>>> setJsonThreeList(JsonData jsonvale, string[] jsonarray, string[] dataarray);
	}

	public interface mysjsoncallback
	{
		void onLoadHtmlText(string mtxt);
		void onjsonData(JsonData jsonvale,int num);
		void onArrayList(List<string> array);
		void onjsonList(List<string> json_target);
		void onjsonTwoList(List<List<string>> json_target);
		void onjsonThreeList(List<List<List<string>>> json_target);
		void statusCallback(string status);

		string getHtmlString { get; }
		JsonData getJsonData { get; }
		List<string> getJsonArray { get; }
		List<List<string>> getJsonArray2 { get; }
	}
}