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
using System.Windows.Shapes;
using RAP.Controller;

namespace RAP.View {
    /// <summary>
    /// Interaction logic for ReportView.xaml
    /// </summary>
    public partial class ReportView : Window {
        public static ReportView Current { get; private set; }

        public ReportView() {
            Current = this;
            ReportController.LoadAllPerformance();
            InitializeComponent();

            PerformanceLevel.SelectedIndex = 0;
        }

        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
            Current = null;
        }

        private void CopyEmailButton_Click(object sender, RoutedEventArgs e) {
            ReportController.LoadEmail(PerformanceTable.Items);
        }

        private void PerformanceLevel_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            PerformanceTable.ItemsSource = ReportController.GenerateReport(PerformanceLevel.SelectedValue);
        }
    }
}
