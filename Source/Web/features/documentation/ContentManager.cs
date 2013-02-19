using System.IO;
using NGit.Api;
using System.Web;
using Bifrost.Execution;
using System.Collections.Generic;
using System.Linq;
using System;
using NGit.Util;

namespace Web.Features.Documentation
{
	[Singleton]
	public class ContentManager : IContentManager
	{
		public static string ContentPath; 
		public static string BifrostPath;
		static FS _fileSystem;

		public static void Initialize(HttpServerUtility server)
		{
			ContentPath = server.MapPath("~/App_Data/Repositories");
			BifrostPath = GetRepositoryPathFor("Bifrost");
			var sitePath = server.MapPath ("~");
			Environment.SetEnvironmentVariable("HOMEPATH", sitePath);
			Environment.SetEnvironmentVariable("USERPROFILE", sitePath);

			ServerVariables.Log ("Home : ",Environment.GetFolderPath (Environment.SpecialFolder.UserProfile).Trim ());

			//_fileSystem = FS.Detect().SetUserHome(server.MapPath ("~"));
		}

		public void Synchronize(string project)
		{
			var repositoryUrl = GetRepositoryUrlFor(project);
			var repositoryPath = GetRepositoryPathFor(project);
			if( !HasRepository(repositoryPath)) 
			{
				Directory.CreateDirectory(repositoryPath);
				var cloneCommand = Git.CloneRepository();

				cloneCommand.SetURI(repositoryUrl);
				cloneCommand.SetDirectory(repositoryPath);
				cloneCommand.Call();
			} 
			else 
			{
				var git = Git.Open (repositoryPath, _fileSystem);
				git.Pull().Call ();
			}
		}


		public IEnumerable<Group> GetStructure (string project)
		{
			var groups = new List<Group>();
			var path = GetRepositoryPathFor(project);
			var git = Git.Open (path);

			var directory = new DirectoryInfo(path);
			foreach( var groupDirectory in directory.GetDirectories() ) 
			{
				if( groupDirectory.Name.StartsWith(".") ) continue;

				var group = new Group
				{
					Name = groupDirectory.Name,
					Elements = groupDirectory.GetFiles().Where(FileIsMarkDown).Select(f => ConvertFileInfoToElement(git, project, f)).ToArray()
				};

				var topics = new List<Topic>();

				foreach( var topicDirectory in groupDirectory.GetDirectories() ) 
				{
					var topic = new Topic 
					{
						Name = topicDirectory.Name,
						Elements = topicDirectory.GetFiles ().Where(FileIsMarkDown).Select(f => ConvertFileInfoToElement(git, project, f)).ToArray()
					};
					topics.Add (topic);
				}
				group.Topics = topics.ToArray ();
				groups.Add (group);
			}

			return groups;
		}

		public string GetFileContentFor(string project, string file)
		{
			var path = string.Format ("{0}{1}{2}",GetRepositoryPathFor(project),Path.DirectorySeparatorChar,file);
			var content = File.ReadAllText (path);
			return content;
		}

		bool FileIsMarkDown(FileInfo fileInfo)
		{
			return fileInfo.Extension == ".md";
		}

		Element ConvertFileInfoToElement(Git git, string project, FileInfo fileInfo)
		{
			var element =  new Element 
			{
				Name = Path.GetFileNameWithoutExtension(fileInfo.Name),
				File = GetRelativePathForFile(project, fileInfo),
				Author = "Unknown",
				LastChanged = "Unknown"
			};

			var logCommand = git.Log ();
			logCommand.AddPath (element.File);
			var result = logCommand.Call ();
			var commit = result.FirstOrDefault();
			if( commit != null ) 
			{
				element.Author = commit.GetCommitterIdent().GetName();
			}
			return element;
		}

		bool HasRepository(string path)
		{
			if( Directory.Exists(path) &&
			    Directory.Exists (string.Format ("{0}{1}{2}", path, Path.DirectorySeparatorChar, ".git")))
				return true;

			return false;
		}

		string GetRelativePathForFile(string project, FileInfo file)
		{
			var repositoryPath = GetRepositoryPathFor(project);
			var path = file.FullName.Substring(repositoryPath.Length+1);
			return path;
		}

		string GetRepositoryUrlFor(string project)
		{
			return string.Format ("https://github.com/dolittle/{0}-Documentation.git", project);
		}

		static string GetRepositoryPathFor(string project)
		{

			var fullPath = string.Format ("{0}{1}{2}", ContentPath, Path.DirectorySeparatorChar, project);
			return fullPath;
		}
	}
}

