using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer.Framework;
using PropertyChanged;
using System.IO;

namespace Run00.GitWorkItems.TeamExplorer
{
	[ImplementPropertyChanged]
	[TeamExplorerNavigationItem(GuidList.WorkItemNavigationItemId, 100)]
	public class WorkItemNavigationItem : ITeamExplorerNavigationItem
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public string RepositoryPath { get; set; }
		public Uri RepositoryUrl { get; set; }
		public string Account { get; set; }
		public string RepositoryName { get; set; }
		public bool IsVisible { get; set; }

		Image ITeamExplorerNavigationItem.Image
		{
			get { return Resources.WorkItemIcon; }
		}

		string ITeamExplorerNavigationItem.Text
		{
			get { return "Work Items"; }
		}

		[ImportingConstructor]
		public WorkItemNavigationItem([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
		{
			serviceProvider
				.GetService<ITeamExplorer>()
				.GetPropertyValue<object>("TeamExplorerManager")
				.GetPropertyValue<object>("ViewModel")
				.AddEventHandler("PropertyChanged", (PropertyChangedEventHandler)((s, e) => { 
					OnTeamExplorerManagerViewModelChanged(s, e);
				}));

			_serviceProvider = serviceProvider;

			this.PropertyChanged += OnMyPropertyChanged;
		}

		void ITeamExplorerNavigationItem.Execute()
		{
			var teamExplorer = _serviceProvider.GetService<ITeamExplorer>();
			if (teamExplorer == null)
				return;

			teamExplorer.NavigateToPage(new Guid(GuidList.WorkItemExplorerPageId), null);
		}

		void ITeamExplorerNavigationItem.Invalidate()
		{
		}

		void IDisposable.Dispose()
		{
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

			RepositoryPath = GetPathFromService(statusSercice);
		}

		private void OnRepositoryPathChanged(object sender, EventArgs e)
		{
			RepositoryPath = GetPathFromService(sender);
		}

		private void OnMyPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			IsVisible = string.IsNullOrWhiteSpace(Account) == false && string.IsNullOrWhiteSpace(RepositoryName) == false;

			if (e.PropertyName == "RepositoryPath")
			{
				UpdateRepositoryInfo(sender.GetPropertyValue<string>(e.PropertyName));
				return;
			}
		}

		private void UpdateRepositoryInfo(string path)
		{
			RepositoryUrl = null;
			Account = null;
			RepositoryName = null;

			if (string.IsNullOrWhiteSpace(path))
				return;

			var filePath = Path.Combine(path, @".git\config");
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
			Account = account.First();
			RepositoryName = account.Skip(1).First();
		}

		private string GetPathFromService(object service)
		{
			return service.GetPropertyValue<string>("RepositoryPath");
		}

		private readonly IServiceProvider _serviceProvider;
		
	}
}