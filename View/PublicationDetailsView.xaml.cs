
/** Publication details view
 * 
 *  This file provide the behind-code and control of Publication details view
 *  
 *  Author: Xunyi Zhao, Michael Skrinnikoff, Callum O'Rourke
 */

using System.Windows;
using System.Windows.Controls;
using RAP.Controller;

namespace RAP.View {

    public partial class PublicationDetailsView : UserControl {
        public PublicationDetailsView() {
            InitializeComponent();
            DataContext = PublicationController.selectedPublication;
        }

        // Close window button clicked
        private void Button_Click(object sender, RoutedEventArgs e) {
            ((MainWindow)Application.Current.MainWindow).unselect();

            ((MainWindow)Application.Current.MainWindow).FuncWindow.Content = null;  
        }
    }
}
