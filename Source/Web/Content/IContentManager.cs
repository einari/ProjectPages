using System.Collections.Generic;
using Web.Content.Documentation;

namespace Web.Content
{
	public interface IContentManager
	{
		void Synchronize(string project);
		void DeleteRepository(string project);
		IEnumerable<Group> GetDocumentationStructure(string project);
		string GetFileContentFor(string project, string file);
	}
}

