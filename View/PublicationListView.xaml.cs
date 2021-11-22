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
using RAP.Controller;

namespace RAP.View {
    /// <summary>
    /// Interaction logic for PublicationListView.xaml
    /// </summary>
    public partial class PublicationListView : UserControl {
        public PublicationListView() {
            InitializeComponent();
        }

        private void FilterYear_Click(object sender, RoutedEventArgs e) {
            int start, end;

            if (ResearcherController.selectedResearcher != null) {
                PublicationList.ItemsSource = PublicationController.FilterByYear(1, 2);
            }
        }

        private void InvertYear_Click(object sender, RoutedEventArgs e) {
            PublicationList.ItemsSource = PublicationController.Invert();
        }

        private void PublicationList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            PublicationController.LoadPublicationDetails(PublicationList.SelectedItem);
            ((MainWindow)Application.Current.MainWindow).SwitchFuncView(FuncView.PublicationDetail);
        }
    }
}
