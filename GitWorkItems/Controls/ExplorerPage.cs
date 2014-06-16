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
	[TeamExplorerPage(GuidList.ExplorerPageId)]
	public class ExplorerPage : ITeamExplorerPage
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

			var items = new List<object>()
			{
				new { Name = "test", Value="2"},
				new { Name = "test", Value="3"},
			};

			PageContent = _explorer;

			_serviceProvider = e.ServiceProvider;
			var accountProvider = _serviceProvider.GetService<AccountProvider>() as INotifyPropertyChanged;
			if (accountProvider == null)
				return;

			accountProvider.PropertyChanged += OnAccountInformationChanged;

			_explorer.NewQueryLink.RequestNavigate += OnNewItemQueryClicked;
			_explorer.NewItemLink.RequestNavigate += OnNewWorkItemClicked;
			_explorer.CreateQueryLink.RequestNavigate += OnCreateQueryClicked;
			_explorer.AddQueryLink.RequestNavigate += OnAddQueryClicked;
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

		private void OnNewWorkItemClicked(object sender, EventArgs e)
		{
			_serviceProvider.OpenNewTabWindow(GuidList.NewItemPaneId, "New Work Item", true);
		}

		private void OnNewItemQueryClicked(object sender, EventArgs e)
		{
			_serviceProvider.OpenNewTabWindow(GuidList.NewQueryPaneId, "New Item Query", true);
		}

		private void OnCreateQueryClicked(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
		{
			_serviceProvider.OpenNewTabWindow(GuidList.NewQueryPaneId, "New Item Query", true);
		}
		private void OnAddQueryClicked(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
		{
			//throw new NotImplementedException();
		}

		private void OnAccountInformationChanged(object sender, PropertyChangedEventArgs e)
		{

			var accountProvider = sender as AccountProvider;
			if (accountProvider == null)
				return;

			_test.Title = "two";
		}

		//void OnSavedQueryDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		//{
		//	var dep = (DependencyObject)e.OriginalSource;
		//	while ((dep != null) && !(dep is ListViewItem))
		//	{
		//		dep = VisualTreeHelper.GetParent(dep);
		//	}

		//	if (dep == null)
		//		return;

		//	var item = ((ListView)sender).ItemContainerGenerator.ItemFromContainer(dep);

		//	_serviceProvider.OpenNewTabWindow(GuidList.QueryResultsPaneId, item.GetPropertyValue<string>("Name"));
		//	//var item = (MyDataItemType)MyListView.ItemContainerGenerator.ItemFromContainer(dep);
		//}

		private IServiceProvider _serviceProvider;
		private Query _test;
		private Explorer _explorer;
	}
}