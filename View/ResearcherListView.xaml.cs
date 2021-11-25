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
                PositionLevel level = EnumStringConverter.ParseEnum<PositionLevel>(Categories.SelectedItem.ToString());
                ResearcherList.ItemsSource = ResearcherController.FilterBy(SearchBox.Text.ToUpper(), level);
            }
        }

        private void ResearcherList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            object selected = ResearcherList.SelectedItem;
            if (selected != null) {
                Researcher r = (Researcher)selected;
                ResearcherController.LoadCurrentResearcher(r);
            }
            ((MainWindow)(Application.Current.MainWindow)).UpdateResearcherDetailsView();
        }

        private void Categories_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            PositionLevel level = EnumStringConverter.ParseEnum<PositionLevel>(Categories.SelectedItem.ToString());

            ResearcherList.ItemsSource = ResearcherController.FilterBy(SearchBox.Text.ToUpper(), level);
        }

        private void ReportButton_Click(object sender, RoutedEventArgs e) {
            if (ReportView.Current == null) {
                ReportView report = new ReportView();
                report.Show();
            } else {
                ReportView.Current.Activate();
            }
        }
    }
}
