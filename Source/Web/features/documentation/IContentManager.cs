using System.Collections.Generic;

namespace Web.Features.Documentation
{
	public interface IContentManager
	{
		void Synchronize(string project);
		void DeleteRepository(string project);
		IEnumerable<Group> GetDocumentationStructure(string project);
		string GetFileContentFor(string project, string file);
	}
}

