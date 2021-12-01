﻿
/** Main window
 * 
 *  This file provide the behind-code of Main window
 *  
 *  Author: Xunyi Zhao, Michael Skrinnikoff, Callum O'Rourke
 */

using System;
using System.Windows;
using RAP.Controller;

namespace RAP.View {

    // Three views switched in the right pane
    internal enum FuncView {
        PublicationDetail,
        SupervisionList,
        CumulativeCount
    };

    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            RDetail.IsEnabled = false;
        }

        // Update the view of researcher's details view
        internal void UpdateResearcherDetailsView() {
            if (ResearcherController.selectedResearcher != null) {
                // Switch to the selected researcher and enable view
                RDetail.DataContext = ResearcherController.selectedResearcher;
                RDetail.IsEnabled = true;

                // Clear FuncWindow content
                FuncWindow.Content = null;

                // Hide show name button if not a staff or supervise no student
                var supervisionNum = RDetail.supervisionCount.Text;
                RDetail.ShowSupervisions.IsEnabled = (supervisionNum != "") && (supervisionNum != "0");

                // Set attributes of the publication list view header
                var PubLView = RDetail.PList;
                PubLView.PublicationList.ItemsSource = PublicationController.LoadPublications();
                PubLView.PubY1.ItemsSource = PubLView.PubY2.ItemsSource = PublicationController.PublicationYears;

                // Set year range selection to 0
                PubLView.PubY1.SelectedIndex = PubLView.PubY2.SelectedIndex = 0;
            }
        }


        // Control the views displayed in the functional area (right)
        internal void SwitchFuncView(FuncView view) {
            switch(view) {
                case FuncView.PublicationDetail:
                    // Make sure not load again
                    if(!(FuncWindow is PublicationDetailsView)) {
                        FuncWindow.Content = new PublicationDetailsView();
                    }
                    break;

                case FuncView.SupervisionList:
                    FuncWindow.Content = new SupervisionListView();
                    break;

                case FuncView.CumulativeCount:
                    FuncWindow.Content = new CumulativeCountView();
                    break;
            }
        }


        // Override the OnClose, shutdown the whole application
        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

    }
}
