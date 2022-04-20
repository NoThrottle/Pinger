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

namespace XAMLPingNET
{
    public partial class MainWindow
    {

        #region HoverHandlers

        bool item1_hovactive = false;

        private void status_item1_hoverfunc(object sender, MouseEventArgs e)
        {
            Console.WriteLine("hov1 detected");

            if (!item1_hovactive)
            {
                status_item1_hoverbg.Visibility = Visibility.Visible;
                item1_hovactive = true;
            }
            else
            {
                status_item1_hoverbg.Visibility = Visibility.Hidden;
                item1_hovactive = false;
            }
        }
        
        private void status_item1_mousedown(object sender, MouseButtonEventArgs e)
        {
            UpdateUI(1);
        }

        //

        bool item2_hovactive = false;

        private void status_item2_hoverfunc(object sender, MouseEventArgs e)
        {
            Console.WriteLine("hov2 detected");

            if (!item2_hovactive)
            {
                status_item2_hoverbg.Visibility = Visibility.Visible;
                item2_hovactive = true;
            }
            else
            {
                status_item2_hoverbg.Visibility = Visibility.Hidden;
                item2_hovactive = false;
            }
        }

        private void status_item2_mousedown(object sender, MouseButtonEventArgs e)
        {
            UpdateUI(2);
        }

        //

        bool item3_hovactive = false;

        private void status_item3_hoverfunc(object sender, MouseEventArgs e)
        {
            Console.WriteLine("hov3 detected");

            if (!item3_hovactive)
            {
                status_item3_hoverbg.Visibility = Visibility.Visible;
                item3_hovactive = true;
            }
            else
            {
                status_item3_hoverbg.Visibility = Visibility.Hidden;
                item3_hovactive = false;
            }
        }

        private void status_item3_mousedown(object sender, MouseButtonEventArgs e)
        {
            UpdateUI(3);
        }

        //

        bool item4_hovactive = false;

        private void status_item4_hoverfunc(object sender, MouseEventArgs e)
        {
            Console.WriteLine("hov4 detected");

            if (!item4_hovactive)
            {
                status_item4_hoverbg.Visibility = Visibility.Visible;
                item4_hovactive = true;
            }
            else
            {
                status_item4_hoverbg.Visibility = Visibility.Hidden;
                item4_hovactive = false;
            }
        }

        private void status_item4_mousedown(object sender, MouseButtonEventArgs e)
        {
            UpdateUI(4);
        }

        //

        bool item5_hovactive = false;

        private void status_item5_hoverfunc(object sender, MouseEventArgs e)
        {
            Console.WriteLine("hov5 detected");

            if (!item5_hovactive)
            {
                status_item5_hoverbg.Visibility = Visibility.Visible;
                item5_hovactive = true;
            }
            else
            {
                status_item5_hoverbg.Visibility = Visibility.Hidden;
                item5_hovactive = false;
            }
        }

        private void status_item5_mousedown(object sender, MouseButtonEventArgs e)
        {
            UpdateUI(5);
        }

        #endregion
    }
}
