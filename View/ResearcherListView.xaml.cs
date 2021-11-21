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
using RAP.Research;

namespace RAP.View {
    public partial class ResearcherListView : UserControl {
        public ResearcherListView() {
            ResearcherController.LoadResearchers();
            InitializeComponent();

            Categories.SelectedIndex = 0;
        }

        private void SearchBox_KeyUp(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                ResearcherList.ItemsSource = ResearcherController.FilterBy(SearchBox.Text, Categories.SelectedItem);
            }
        }

        private void ResearcherList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            ResearcherController.LoadCurrentResearcher(ResearcherList.SelectedItem);
            ((MainWindow)(Application.Current.MainWindow)).UpdateResearcherDetailsView();
        }

        private void Categories_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            ResearcherList.ItemsSource = ResearcherController.FilterBy(SearchBox.Text, Categories.SelectedItem);
        }

        private void Button_Click(object sender, RoutedEventArgs e) {

        }
    }
}
