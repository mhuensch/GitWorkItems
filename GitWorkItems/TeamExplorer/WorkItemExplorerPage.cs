using System;
using System.ComponentModel;

using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell.Interop;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Controls;

namespace Run00.GitWorkItems.TeamExplorer
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

			var newPage = new WorkItemExplorerView();
			newPage.DataContext = _test;
			newPage.NewWorkItem.Click += OnNewWorkItemClick;
			newPage.NewQuery.Click += OnNewQueryClick;
			var items = new List<object>()
			{
				new { Name = "test", Value="2"},
				new { Name = "test", Value="3"},
			};
			newPage.Queries.ItemsSource = items;
			newPage.Queries.MouseDoubleClick += OnSavedQueryDoubleClick;
			PageContent = newPage;

			_serviceProvider = e.ServiceProvider;
			var accountProvider = _serviceProvider.GetService<WorkItemAccountProvider>() as INotifyPropertyChanged;
			if (accountProvider == null)
				return;

			accountProvider.PropertyChanged += OnAccountInformationChanged;
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

			OpenNewTabWindow(GuidList.QueryResultsWindowId, item.GetPropertyValue<string>("Name"));
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
			OpenNewTabWindow(GuidList.NewWorkItemWindowId, "New Work Item");
		}

		private void OnNewQueryClick(object sender, System.Windows.RoutedEventArgs e)
		{
			OpenNewTabWindow(GuidList.NewWorkItemWindowId, "New Query");
		}

		private void OpenNewTabWindow(string guid, string title)
		{
			var shell = _serviceProvider.GetService<IVsUIShell>();
			IVsWindowFrame winFrame;
			var guidNo = new Guid(guid);

			var id = new Random().Next();
			//TODO: Replace id with the name of the query being executed
			if (shell.FindToolWindowEx(0x80000, ref guidNo, uint.Parse(id.ToString()), out winFrame) >= 0 && winFrame != null)
			{
				winFrame.SetProperty((int)__VSFPROPID.VSFPROPID_FrameMode, VSFRAMEMODE.VSFM_MdiChild);
				winFrame.SetProperty((int)__VSFPROPID.VSFPROPID_Caption, title);
				winFrame.Show();
			}
		}

		private IServiceProvider _serviceProvider;
		private Query _test;
		
	}
}