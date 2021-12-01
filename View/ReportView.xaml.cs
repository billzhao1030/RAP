
/** Report window
 * 
 *  This file provide the behind-code and control of Report window
 *  
 *  Author: Xunyi Zhao, Michael Skrinnikoff, Callum O'Rourke
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using RAP.Controller;
using RAP.Research;

namespace RAP.View {
    public partial class ReportView : Window {
        public static ReportView Current { get; private set; }

        public ReportView() {
            Current = this;
            ReportController.LoadAllPerformance();
            InitializeComponent();

            PerformanceLevel.SelectedIndex = 0; //Initiate the selected index
        }

        
        // Copy emails of staffs of selected level 
        private void CopyEmailButton_Click(object sender, RoutedEventArgs e) {
            List<Staff> staffList = PerformanceTable.Items.OfType<Staff>().ToList();

            ReportController.LoadEmail(staffList);
        }


        // Generate report of selected level
        private void PerformanceLevel_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            PerformanceLevel reportLevel = EnumStringConverter.ParseEnum<PerformanceLevel>(PerformanceLevel.SelectedValue.ToString());

            PerformanceTable.ItemsSource = ReportController.GenerateReport(reportLevel);
        }


        // Same as main window, make sure close
        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
            Current = null;
        }
    }
}
