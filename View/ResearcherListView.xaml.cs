
/** Reseacher list view
 * 
 *  This file provide the behind-code and control of Researcher list view
 *  
 *  Author: Xunyi Zhao, Michael Skrinnikoff
 */

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RAP.Controller;
using RAP.Research;

namespace RAP.View {
    public partial class ResearcherListView : UserControl {
        public ResearcherListView() {
            
            InitializeComponent();
            ResearcherList.ItemsSource = ResearcherController.LoadResearchers();
            Categories.SelectedIndex = 0;
        }


        // When the 'enter' key is up, update the list
        private void SearchBox_KeyUp(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                PositionLevel level = EnumStringConverter.ParseEnum<PositionLevel>(Categories.SelectedItem.ToString());
                ResearcherList.ItemsSource = ResearcherController.FilterBy(SearchBox.Text.ToUpper(), level);
            }
        }


        // When select a level, update the list
        private void Categories_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            PositionLevel level = EnumStringConverter.ParseEnum<PositionLevel>(Categories.SelectedItem.ToString());
            ResearcherList.ItemsSource = ResearcherController.FilterBy(SearchBox.Text.ToUpper(), level);
        }


        // When select a researcher update the researcher details view
        private void ResearcherList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            object selected = ResearcherList.SelectedItem;

            if (selected != null) {
                Researcher r = (Researcher)selected;
                ResearcherController.LoadCurrentResearcher(r);
            }

            ((MainWindow)(Application.Current.MainWindow)).UpdateResearcherDetailsView();
        }
    }
}
