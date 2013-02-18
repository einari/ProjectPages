using System.Collections.Generic;

namespace Web
{
	public interface IContentManager
	{
		void Synchronize(string project);
		IEnumerable<Group> GetStructure(string project);
		string GetFileContentFor(string project, string file);
	}
}
