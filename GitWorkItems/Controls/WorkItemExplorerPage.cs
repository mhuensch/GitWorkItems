using System;
using System.ComponentModel;

using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell.Interop;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Controls;
using Run00.GitWorkItems.Views;
using Run00.GitWorkItems.Providers;

namespace Run00.GitWorkItems.Controls
{
	[TeamExplorerPage(GuidList.WorkItemExplorerPageId)]
	public class SampleTeamExplorerPage : ITeamExplorerPage
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public bool IsBusy { get; set; }

		public string Title { get; set; }

		public object PageContent { get; set; }

		void ITeamExplorerPage.Initialize(object sender, PageInitializeEventArgs e)
		{
			Title = "Work Items";
			_test = new Query() { Title = "one"};

			_explorer = new Explorer();
			_explorer.DataContext = _test;
			//_explorer.NewWorkItem.Click += OnNewWorkItemClick;
			//newPage.Click += OnNewQueryClick;
			var items = new List<object>()
			{
				new { Name = "test", Value="2"},
				new { Name = "test", Value="3"},
			};
			//newPage.Queries.ItemsSource = items;
			//newPage.Queries.MouseDoubleClick += OnSavedQueryDoubleClick;
			PageContent = _explorer;

			_serviceProvider = e.ServiceProvider;
			var accountProvider = _serviceProvider.GetService<WorkItemAccountProvider>() as INotifyPropertyChanged;
			if (accountProvider == null)
				return;

			accountProvider.PropertyChanged += OnAccountInformationChanged;
			_explorer.Dashboard.SharedLinkClicked += Dashboard_SharedLinkClicked;
			_explorer.Dashboard.LocalLinkClicked += Dashboard_LocalLinkClicked;

			_explorer.NewItemQueryClicked += OnNewItemQueryClicked;
			_explorer.NewWorkItemClicked += OnNewWorkItemClicked;

			//_explorer.SharedQueries.NewItemQueryClicked += OnNewItemQueryClicked;
			//_explorer.LocalQueries.NewItemQueryClicked += OnNewItemQueryClicked;
		}

		private void OnNewWorkItemClicked(object sender, EventArgs e)
		{
			_serviceProvider.OpenNewTabWindow(GuidList.NewWorkItemWindowId, "New Work Item", true); 
		}

		private void OnNewItemQueryClicked(object sender, EventArgs e)
		{
			_serviceProvider.OpenNewTabWindow(GuidList.NewItemQueryWindowId, "New Item Query", true);
		}

		private void Dashboard_LocalLinkClicked(object sender, EventArgs e)
		{
			//_explorer.LocalQueries.Queries.IsExpanded = true;
			//_explorer.SharedQueries.Queries.IsExpanded = false;
		}

		private void Dashboard_SharedLinkClicked(object sender, EventArgs e)
		{
			//_explorer.SharedQueries.Queries.IsExpanded = true;
			//_explorer.LocalQueries.Queries.IsExpanded = false;
		}

		void OnSavedQueryDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			var dep = (DependencyObject)e.OriginalSource;
			while ((dep != null) && !(dep is ListViewItem))
			{
				dep = VisualTreeHelper.GetParent(dep);
			}

			if (dep == null)
				return;

			var item = ((ListView)sender).ItemContainerGenerator.ItemFromContainer(dep);

			_serviceProvider.OpenNewTabWindow(GuidList.QueryResultsWindowId, item.GetPropertyValue<string>("Name"));
			//var item = (MyDataItemType)MyListView.ItemContainerGenerator.ItemFromContainer(dep);
		}

		void ITeamExplorerPage.Cancel()
		{
		}

		object ITeamExplorerPage.GetExtensibilityService(Type serviceType)
		{
			return null;
		}
		void ITeamExplorerPage.Loaded(object sender, PageLoadedEventArgs e)
		{
		}

		void ITeamExplorerPage.Refresh()
		{
		}

		void ITeamExplorerPage.SaveContext(object sender, PageSaveContextEventArgs e)
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

			_test.Title = "two";
		}

		private void OnNewWorkItemClick(object sender, System.Windows.RoutedEventArgs e)
		{
			_serviceProvider.OpenNewTabWindow(GuidList.NewWorkItemWindowId, "New Work Item", true);
		}

		private IServiceProvider _serviceProvider;
		private Query _test;
		private Explorer _explorer;
	}
}