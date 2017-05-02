using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Vuforia;

namespace amy.initial
{
	public class initiateDatasetBehavior : MonoBehaviour, datasetInitiate
	{
		datasetInitiateCallback mycallback;

		public void init (datasetInitiateCallback callback)
		{
			mycallback = callback;
		}

		public void loadDataset(string path)
		{
			mycallback.onLoadStart("start load dataset: " + path);
			_loadDataset(null, path, false);
		}

		public void loadDataset(string path, string[] ojName)
		{
			mycallback.onLoadStart("start load dataset: " + path);
			_loadDataset(ojName, path, true);
		}

		private void _loadDataset(string[] _name, string path, bool reName)
		{
			DataSet mDataset = null;
			ObjectTracker tracker = null;

			try
			{
				tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
				mDataset = tracker.CreateDataSet();

				if (mDataset.Load(path, VuforiaUnity.StorageType.STORAGE_ABSOLUTE))
				{
					tracker.ActivateDataSet(mDataset);
					IEnumerable<TrackableBehaviour> tbs = TrackerManager.Instance.GetStateManager().GetTrackableBehaviours();

					int temp = 0;

					foreach (TrackableBehaviour tb in tbs)
					{
						if (tb.name == "New Game Object")
						{
							if (reName)
								tb.gameObject.name = _name[temp++];
							tb.gameObject.AddComponent<DefaultTrackableEventHandler>();
							tb.gameObject.AddComponent<TurnOffBehaviour>();
						}
					}
					mycallback.onLoadFinish("Initiate complete: " + path);
				}
				else
					mycallback.onLoadFailed("Initiate Error");
			}
			catch (Exception e)
			{
				mycallback.onLoadFailed("Initiate Error: " + e);
			}
		}
	}
}
