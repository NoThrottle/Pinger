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

namespace XAMLPing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            UpdateAcrylic();

        }

        #region ActiveButton

        bool activebutton_active = false;

        private void ActiveButton_MouseDown(object sender,MouseButtonEventArgs e)
        {
            Console.WriteLine("active down");

            if (!activebutton_active)
            {
                activebutton_active = true;
                ActiveButton_Shadow.Color = Color.FromArgb(255, 164, 255, 196);

            }
            else
            {
                activebutton_active = false;
                ActiveButton_Shadow.Color = Color.FromArgb(255, 110, 110, 110);
            }
        }

        bool ActiveButton_hovactive = false;

        private void ActiveButton_hoverfunc(object sender, MouseEventArgs e)
        {
            Console.WriteLine("ActiveButton hov detected");

            if (activebutton_active == false)
            {
                if (!ActiveButton_hovactive)
                {
                    ActiveButton_BG.Background = new SolidColorBrush(Color.FromArgb(255, 160, 160, 160));
                    ActiveButton_hovactive = true;
                }
                else
                {
                    ActiveButton_BG.Background = new SolidColorBrush(Color.FromArgb(255, 110, 110, 110));
                    ActiveButton_hovactive = false;
                }
            }
            else
            {
                if (!ActiveButton_hovactive)
                {
                    ActiveButton_BG.Background = new SolidColorBrush(Color.FromArgb(255, 194, 255, 216));
                    ActiveButton_hovactive = true;
                }
                else
                {
                    ActiveButton_BG.Background = new SolidColorBrush(Color.FromArgb(255, 164, 255, 196));
                    ActiveButton_hovactive = false;
                }
            }

        }

        #endregion

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
                UpdateAcrylic();

            }
        }

        private void UpdateAcrylic()
        {
            //WindowColor
            AcrylicWindowStyle = SourceChord.FluentWPF.AcrylicWindowStyle.Normal;
            TintColor = Color.FromArgb(255, 12, 12, 12);
            FallbackColor = Color.FromArgb(205, 23, 23, 23);
            NoiseOpacity = (double)0.015;
            AcrylicWindowStyle = SourceChord.FluentWPF.AcrylicWindowStyle.None;
        }

        private void Close_Click(object sender, MouseButtonEventArgs e)
        {
            Close();
            //Minimize in the future to tray instead of closing
        }





    }
}
