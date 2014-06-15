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
using Run00.GitWorkItems.Providers;

namespace Run00.GitWorkItems.Controls
{
	[ImplementPropertyChanged]
	[TeamExplorerNavigationItem(GuidList.WorkItemNavigationItemId, 100)]
	public class WorkItemNavigationItem : ITeamExplorerNavigationItem
	{
		public event PropertyChangedEventHandler PropertyChanged;
		
		public bool IsVisible { get; set; }

		public Image Image { get; set; }
	
		public string Text { get; set; }


		[ImportingConstructor]
		public WorkItemNavigationItem([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
		{
			Image = Resources.WorkItemIcon;
			Text = "Work Items";

			_serviceProvider = serviceProvider;

			var accountProvider = _serviceProvider.GetService<WorkItemAccountProvider>() as INotifyPropertyChanged;
			if (accountProvider == null)
				return;

			accountProvider.PropertyChanged += OnAccountInformationChanged;
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

		private void OnAccountInformationChanged(object sender, PropertyChangedEventArgs e)
		{
			var accountProvider = sender as WorkItemAccountProvider;
			if (accountProvider == null)
				return;

			IsVisible = 
				string.IsNullOrWhiteSpace(accountProvider.AccountName) == false &&
				string.IsNullOrWhiteSpace(accountProvider.RepositoryName) == false;
		}

		private readonly IServiceProvider _serviceProvider;
		
	}
}