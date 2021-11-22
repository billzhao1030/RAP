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

        // override the OnClose, shutdown the whole application
        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        // update the view of researcher's details
        internal void UpdateResearcherDetailsView() {
            if (ResearcherController.selectedResearcher != null) {
                // switch to the selected researcher and enable view
                RDetail.DataContext = ResearcherController.selectedResearcher;
                RDetail.IsEnabled = true;

                // Hide show name button if not a staff or supervise no student
                var supervisionNum = RDetail.supervisionCount.Text;
                RDetail.ShowSupervisions.IsEnabled = (supervisionNum != "") && (supervisionNum != "0");

                // set attributes of the publication list view header
                var PubLView = RDetail.PList;
                PubLView.PublicationList.ItemsSource = PublicationController.LoadPublications();
                PubLView.PubY1.ItemsSource = PubLView.PubY2.ItemsSource = PublicationController.PublicationYears;

                // set year range selection to 0
                PubLView.PubY1.SelectedIndex = PubLView.PubY2.SelectedIndex = 0;

                // clear FuncWindow content
                FuncWindow.Content = null;
            }
        }

        // control the views displayed in the functional area (right)
        internal void SwitchFuncView(FuncView view) {
            switch(view) {
                case FuncView.PublicationDetail:
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
    }
}
