
/** Researcher details view
 * 
 *  This file provide the behind-code and control of Researcher details view
 *  
 *  Author: Xunyi Zhao, Michael Skrinnikoff
 */

using System.Windows;
using System.Windows.Controls;

namespace RAP.View {
    public partial class ResearcherDetailsView : UserControl {
        public ResearcherDetailsView() {
            InitializeComponent();
        }

        // Show the supervision list if available
        private void ShowSupervisions_Click(object sender, RoutedEventArgs e) {
            ((MainWindow)Application.Current.MainWindow).unselect();
            ((MainWindow)Application.Current.MainWindow).SwitchFuncView(FuncView.SupervisionList);   
        }


        // Show the cumulative count table
        private void CumulativeCount_Click(object sender, RoutedEventArgs e) {
            ((MainWindow)Application.Current.MainWindow).unselect();
            ((MainWindow)Application.Current.MainWindow).SwitchFuncView(FuncView.CumulativeCount);
        }

    }
}
