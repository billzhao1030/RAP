
/** Publication list view
 * 
 *  This file provide the behind-code and control of Publication list view
 *  
 *  Author: Xunyi Zhao, Michael Skrinnikoff, Callum O'Rourke
 */

using System;
using System.Windows;
using System.Windows.Controls;
using RAP.Research;
using RAP.Controller;

namespace RAP.View {
    public partial class PublicationListView : UserControl {
        public PublicationListView() {
            InitializeComponent();
        }


        // Filter according to the year range
        private void FilterYear_Click(object sender, RoutedEventArgs e) {
            int start, end;

            // if no start/end year, system treat it as the smallest/biggest year
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


        // Invert the list 
        private void InvertYear_Click(object sender, RoutedEventArgs e) {
            PublicationList.ItemsSource = PublicationController.Invert();
        }


        // Changes when select a publication, show the details
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
