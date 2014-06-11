using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Windows.Forms;

using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell;

namespace Run00.GitWorkItems.TeamExplorer
{
	[TeamExplorerNavigationItem(GuidList.WorkItemNavigationItemId, 100)]
	public class WorkItemNavigationItem : ITeamExplorerNavigationItem
	{
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add { _eventHandler += value; }
			remove { _eventHandler -= value; }
		}

		Image ITeamExplorerNavigationItem.Image
		{
			get { return Resources.WorkItemIcon; }
		}

		bool ITeamExplorerNavigationItem.IsVisible
		{
			get 
			{
				return _gitHub.IsLoaded();
			}
		}

		string ITeamExplorerNavigationItem.Text
		{
			get { return "Sample Button"; }
		}

		[ImportingConstructor]
		public WorkItemNavigationItem([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
		{
			_gitHub = serviceProvider.GetService<IGitHub>();
			_teamExplorer = serviceProvider.GetService<ITeamExplorer>();
		}

		void ITeamExplorerNavigationItem.Execute()
		{
			_teamExplorer.NavigateToPage(new Guid(GuidList.WorkItemExplorerPageId), null);
		}

		void ITeamExplorerNavigationItem.Invalidate()
		{
			
		}

		void IDisposable.Dispose()
		{
		}

		private PropertyChangedEventHandler _eventHandler;
		private readonly IGitHub _gitHub;
		private readonly ITeamExplorer _teamExplorer;
	}
}