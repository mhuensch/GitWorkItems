using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Run00.GitWorkItems.Views
{
	/// <summary>
	/// Interaction logic for Explorer.xaml
	/// </summary>
	public partial class Explorer : UserControl
	{
		public event EventHandler NewItemQueryClicked;
		public event EventHandler NewWorkItemClicked;

		public Explorer()
		{
			InitializeComponent();
		}

		private void OnNewItemQueryClicked(object sender, RequestNavigateEventArgs e)
		{
			if (NewItemQueryClicked == null)
				return;
			NewItemQueryClicked.Invoke(this, new EventArgs());
		}

		private void OnNewWorkItemClicked(object sender, RequestNavigateEventArgs e)
		{
			if (NewWorkItemClicked == null)
				return;
			NewWorkItemClicked.Invoke(this, new EventArgs());
		}
	}
}
