using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Run00.GitWorkItems.TeamExplorer
{
	[ImplementPropertyChanged]
	public class WorkItemAccountProvider
	{
		public string RepositoryPath { get; set; }

		public Uri RepositoryUrl { get; set; }

		public string AccountName { get; set; }

		public string RepositoryName { get; set; }

		public WorkItemAccountProvider([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
		{
			serviceProvider
				.GetService<ITeamExplorer>()
				.GetPropertyValue<object>("TeamExplorerManager")
				.GetPropertyValue<object>("ViewModel")
				.AddEventHandler("PropertyChanged", (PropertyChangedEventHandler)((s, e) => {
					OnTeamExplorerManagerViewModelChanged(s, e);
				}));
		}

		private void OnTeamExplorerManagerViewModelChanged(object sender, PropertyChangedEventArgs e)
		{
			var statusSercice = sender
				.GetPropertyValue<object>("CurrentContextInfoProvider")
				.GetPropertyValue<object>("Instance")
				.GetPropertyValue<object>("StatusService")
				.AddEventHandler("RepositoryPathChanged", (EventHandler)((s, a) => {
					OnRepositoryPathChanged(s, a);
				}));

			if (statusSercice == null)
				return;

			UpdateRepositoryInfo(statusSercice);
		}

		private void OnRepositoryPathChanged(object sender, EventArgs e)
		{
			UpdateRepositoryInfo(sender);
		}

		private void UpdateRepositoryInfo(object statusService)
		{
			RepositoryPath = statusService.GetPropertyValue<string>("RepositoryPath");
			RepositoryUrl = null;
			AccountName = null;
			RepositoryName = null;

			if (string.IsNullOrWhiteSpace(RepositoryPath))
				return;

			var filePath = Path.Combine(RepositoryPath, @".git\config");
			if (File.Exists(filePath) == false)
				return;

			var parser = new Ini(filePath);
			var url = parser.GetValue("url ", "remote \"origin\"");
			if (string.IsNullOrWhiteSpace(url))
				return;

			Uri uri;
			Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri);
			if (uri == null)
				return;

			RepositoryUrl = uri;

			var account = uri.AbsolutePath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
			AccountName = account.First();
			RepositoryName = account.Skip(1).First();
		}

	}
}
