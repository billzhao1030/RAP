
/** Supervision list view
 * 
 *  This file provide the behind-code and control of Supervision list view
 *  
 *  Author: Xunyi Zhao, Michael Skrinnikoff
 */

using System.Windows;
using System.Windows.Controls;
using RAP.Controller;

namespace RAP.View {
    public partial class SupervisionListView : UserControl {
        public SupervisionListView() {
            InitializeComponent();
            DataContext = ResearcherController.selectedResearcher;
        }

        // Close window button clicked
        private void Button_Click(object sender, RoutedEventArgs e) {
            ((MainWindow)Application.Current.MainWindow).FuncWindow.Content = null;
        }
    }
}
