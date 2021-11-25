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
using RAP.Research;
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
                if (!int.TryParse(PubY1.Text, out start)) {
                    start = DateTime.MinValue.Year;
                }

                if (!int.TryParse(PubY2.Text, out end)) {
                    end = DateTime.MaxValue.Year;
                }
                

                PublicationList.ItemsSource = PublicationController.FilterByYear(start, end);
            }
        }

        private void InvertYear_Click(object sender, RoutedEventArgs e) {
            PublicationList.ItemsSource = PublicationController.Invert();
        }

        private void PublicationList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            object selected = PublicationList.SelectedItem;
            if (selected != null) {
                Publication p = (Publication)selected;
                PublicationController.LoadPublicationDetails(p);
            }

            ((MainWindow)Application.Current.MainWindow).SwitchFuncView(FuncView.PublicationDetail);
        }
    }
}
