using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Raido.UseControl
{
    public partial class InputUserRadioInfo : UserControl
    {
        public event EventHandler<InputControlEventArgs> OnButtonOKClickChanged;

        //private string radioname;

        //public string Radioname
        //{
        //    get { return radioname; }
        //    set { radioname = value; }
        //}

        //private string radiouri;

        //public string Radiouri
        //{
        //    get { return radiouri; }
        //    set { radiouri = value; }
        //}

        //private string radiotype;

        //public string Radiotype
        //{
        //    get { return radiotype; }
        //    set { radiotype = value; }
        //}

        public InputUserRadioInfo()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
        
            //如果为empty。..焦点..
            if (IsEmpty(txtRadioName) )
            {
                return;
            }
            if (IsEmpty(txtRadioUri))
            {
                return;
            }
            if (IsEmpty(txtRadioType))
            {
                return;
            }


            if (OnButtonOKClickChanged!=null)
            {
                

                //radioname = txtRadioName.Text;
                //radiouri = txtRadioUri.Text;
                //radiotype = txtRadioType.Text;
                RadioContent radio = new RadioContent();
                radio.RadioName = txtRadioName.Text;
                radio.RadioURL = txtRadioUri.Text;
                radio.Type = txtRadioType.Text;

                OnButtonOKClickChanged(this, new InputControlEventArgs() { radio = radio });
            }
            this.CloseMeAsPopup();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.CloseMeAsPopup();
        }

        private bool IsEmpty(PhoneTextBox textBox)
        {
            if (string.IsNullOrEmpty(textBox.Text))
            {
            textBox.Focus();
            return true;
            }
            else
            {
                return false;
            }

        }
    }
}
