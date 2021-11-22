﻿using System;
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
    /// Interaction logic for SupervisionListView.xaml
    /// </summary>
    public partial class SupervisionListView : UserControl {
        public SupervisionListView() {
            InitializeComponent();
            DataContext = ResearcherController.selectedResearcher;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            ((MainWindow)Application.Current.MainWindow).FuncWindow.Content = null;
        }
    }
}
