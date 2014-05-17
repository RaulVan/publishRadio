using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;

namespace Raido.UseControl
{
    public partial class LongListItemContainer : UserControl
    {
        public LongListItemContainer()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += LongListItemContainer_MouseLeftButtonDown;
            this.MouseLeftButtonUp += LongListItemContainer_MouseLeftButtonUp;
            this.MouseLeave += LongListItemContainer_MouseLeave;
            //this.Width = App.RootFrame.ActualWidth - 24;
        }

        void LongListItemContainer_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if ((sender as UIElement) != null)
                (sender as UIElement).Projection = null;

        }

        void LongListItemContainer_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if ((sender as UIElement) != null)
                (sender as UIElement).Projection = null;

        }

        void LongListItemContainer_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if ((sender as UIElement) != null)
                (sender as UIElement).Projection = new PlaneProjection() { LocalOffsetZ = -30 };
            //this.Background = new SolidColorBrush(Color.FromArgb(0, 153, 204, 255));
        }
    }
}
