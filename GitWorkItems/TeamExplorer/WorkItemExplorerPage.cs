using System;
using System.ComponentModel;

using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell.Interop;

namespace Run00.GitWorkItems.TeamExplorer
{
	[TeamExplorerPage(GuidList.WorkItemExplorerPageId)]
	public class SampleTeamExplorerPage : ITeamExplorerPage
	{
		private IServiceProvider serviceProvider;

		private bool isBusy;

		public void Cancel()
		{
		}

		public object GetExtensibilityService(Type serviceType)
		{
			return null;
		}

		public void Initialize(object sender, PageInitializeEventArgs e)
		{
			this.serviceProvider = e.ServiceProvider;
		}

		public bool IsBusy
		{
			get
			{
				return this.isBusy;
			}
			private set
			{
				this.isBusy = value;
				this.FirePropertyChanged("IsBusy");
			}
		}

		public void Loaded(object sender, PageLoadedEventArgs e)
		{
		}

		public object PageContent
		{
			get
			{
				var newPage = new WorkItemExplorerPage();
				newPage.NewWindow.Click += OnNewWindowClick;
				return newPage;
			}
		}

		void OnNewWindowClick(object sender, System.Windows.RoutedEventArgs e)
		{
			var shell = this.serviceProvider.GetService<IVsUIShell>();
			IVsWindowFrame winFrame;
			var guidNo = new Guid("aa1dc5ae-24ea-440a-8268-a6dc65fcd4a0");

			var id = new Random().Next();
			//TODO: Replace id with the name of the query being executed
			if (shell.FindToolWindowEx(0x80000, ref guidNo, uint.Parse(id.ToString()), out winFrame) >= 0 && winFrame != null)
			{
				winFrame.SetProperty((int)__VSFPROPID.VSFPROPID_FrameMode, VSFRAMEMODE.VSFM_MdiChild);
				winFrame.SetProperty((int)__VSFPROPID.VSFPROPID_Caption, id.ToString() + " [" + Resources.ToolWindowTitle + "]");
				winFrame.Show();
			}
		}

		public void Refresh()
		{
		}

		public void SaveContext(object sender, PageSaveContextEventArgs e)
		{
		}

		public string Title
		{
			get
			{
				return "Sample Page";
			}
		}

		public void Dispose()
		{
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void FirePropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}