using System.Collections;
using System.Collections.Generic;
using LitJson;

namespace cqplayart.adv.advjson
{
	public interface advjson
	{
		IEnumerator gethtml(string onlinePath);
		void getJsonData(string jsonstring);
		void setArrayList(JsonData jsonvale);
		void setJsonList(JsonData jsonvale, string[] jsonarray);
		void setJsonTwoList(JsonData jsonvale,string[] jsonarray,string [] dataarray);
		void setJsonThreeList(JsonData jsonvale, string[] jsonarray, string[] dataarray);
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