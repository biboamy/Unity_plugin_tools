using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace amy.initial
{
	public interface datasetInitiate
	{
		void init(datasetInitiateCallback callback);
		void loadDataset(string path);
		void loadDataset(string path, string[] ojName);

	}
	public interface datasetInitiateCallback
	{
		void onLoadStart(string msg);
		void onLoadFinish(string msg);
		void onLoadFailed(string msg);
	}
}