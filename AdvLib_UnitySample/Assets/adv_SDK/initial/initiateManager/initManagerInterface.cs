using System.Collections;

namespace cqplayart.adv.initial
{
	public interface initManager
	{
		void addInitiateList(string type, params object[] args);
		IEnumerator LoadList(string dir);
	}
}