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
			get { return true; }
		}

		string ITeamExplorerNavigationItem.Text
		{
			get { return "Sample Button"; }
		}

		[ImportingConstructor]
		public WorkItemNavigationItem([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		void ITeamExplorerNavigationItem.Execute()
		{
			var service = _serviceProvider.GetService<ITeamExplorer>();
			if (service == null)
			{
				return;
			}
			service.NavigateToPage(new Guid(GuidList.WorkItemExplorerPageId), null);
		}

		void ITeamExplorerNavigationItem.Invalidate()
		{
			
		}

		void IDisposable.Dispose()
		{
			_eventHandler = null;
			_serviceProvider = null;
		}

		private IServiceProvider _serviceProvider;
		private PropertyChangedEventHandler _eventHandler;
	}
}