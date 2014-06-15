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
	/// Interaction logic for QueryDashboardList.xaml
	/// </summary>
	public partial class QueryDashboardList : UserControl
	{
		public event EventHandler SharedLinkClicked;
		public event EventHandler LocalLinkClicked;

		public QueryDashboardList()
		{
			InitializeComponent();
		}

		private void OnLocalLinkClicked(object sender, RequestNavigateEventArgs e)
		{
			if (LocalLinkClicked == null)
				return;
			LocalLinkClicked.Invoke(this, new EventArgs());
		}

		private void OnSharedLinkClicked(object sender, RequestNavigateEventArgs e)
		{
			if (SharedLinkClicked == null)
				return;
			SharedLinkClicked.Invoke(this, new EventArgs());
		}
	}
}
