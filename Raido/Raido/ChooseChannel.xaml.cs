using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Threading;
using Raido.Models;

namespace Raido
{
    public partial class ChooseChannel : PhoneApplicationPage
    {

        //RadioList list;
        public ChooseChannel()
        {
            InitializeComponent();

            //RadioList list = new RadioList();
            List<RadioContent> datas = new List<RadioContent>();
            for (int i = 0; i < 100; i++)
            {
                datas.Add(new RadioContent() { RadioName = "beijing" });
            }
            
            //list= new RadioList();
            //longlistAll.ItemsSource = datas;
           

            longlistAll.SelectionChanged += longlistAll_SelectionChanged;
            this.Loaded += ChooseChannel_Loaded;
        }

        void ChooseChannel_Loaded(object sender, RoutedEventArgs e)
        {
            //DispatcherTimer timer = new DispatcherTimer();
            //timer.Tick += (a, b) =>
            //    {
            //longlistAll.ItemsSource = list.GetList();
            //timer.Stop();
            //    };
            //timer.Interval = TimeSpan.FromSeconds(1);
            //timer.Start();
            var viewModel = new RadioListViewModel();
            longlistAll.ItemsSource = viewModel.GroupedRadios;
            
        }

        void longlistAll_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

    }
}