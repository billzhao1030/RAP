
/** Cumulative count view
 * 
 *  This file provide the behind-code and control of Cumulative count view
 *  
 *  Author: Xunyi Zhao, Michael Skrinnikoff
 */


using System.Windows;
using System.Windows.Controls;
using RAP.Controller;

namespace RAP.View {
    public partial class CumulativeCountView : UserControl {
        public CumulativeCountView() {
            InitializeComponent();
            CumulativeCountTable.ItemsSource = PublicationController.CumulativeCount(); // Link the count table
        }


        // Close window button clicked
        private void Button_Click(object sender, RoutedEventArgs e) {
            ((MainWindow)Application.Current.MainWindow).FuncWindow.Content = null;
        }
    }
}
