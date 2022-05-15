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
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;

using IslamovAIDBClientWPF;
using System.IO;
using System.Data.SqlClient;

namespace WpfApp2
{


    public partial class MainWindow : Window
    {
        private string _SQLCommand;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = WorkingMySQLDBHelper.MainViewModel;
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WorkingMySQLDBHelper.MainViewModel.TryConnect();


            //заполнение справочных таблиц
            DataTable dt1 = WorkingMySQLDBHelper.MainViewModel.discipline;
            DataTable dt2 = WorkingMySQLDBHelper.MainViewModel.group;
            DataTable dt3 = WorkingMySQLDBHelper.MainViewModel.teacher;

            if (dt1 != null)
            {
                grid_discipline.ItemsSource = dt1.DefaultView;
            }
            if (dt2 != null)
            {
                grid_group.ItemsSource = dt2.DefaultView;
            }
            if (dt3 != null)
            {
                grid_teacher.ItemsSource = dt3.DefaultView;
            }
        }

        private void keyboardEnter1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _password.Focus(); 
            }
        }

        private void keyboardEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Button_Click(sender, e);
        }

        private void keyboardEnter2(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                StartButton_Click(sender, e);
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = WorkingMySQLDBHelper.MainViewModel.DoSqlQuery(SQLQuery.Text);
            if (dt != null)
            {
                grid_1.ItemsSource = dt.DefaultView;
            }
        }


        private void _groups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
         
        }


        private void studentButton_Click(object sender, RoutedEventArgs e)
        {
            
          
        }

        private void dictionaries()
        {
           
        }

    }
}
