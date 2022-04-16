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
using System.Diagnostics;
using System.Windows.Threading;

namespace XAMLPingNET
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            Debug.WriteLine("istart");

            InitializeComponent();
            Startup_Load_Loop_Timer("start");
            UpdateAcrylic();
            Tabs(1);
            calltostop_StartupLoad = true;

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
                ActiveButton_Text.Content = "Active";
                intervaltimer = 1;

                Pinger_Timer(true);

            }
            else
            {
                activebutton_active = false;
                ActiveButton_Shadow.Color = Color.FromArgb(255, 110, 110, 110);
                ActiveButton_Text.Content = "Inactive";
                intervaltimer = 5;

                Pinger_Timer(true);

            }
        }

        bool ActiveButton_hovactive = false;

        private void ActiveButton_hoverfunc(object sender, MouseEventArgs e)
        {
            Console.WriteLine("ActiveButton hov detected");

            ActiveButton.ToolTip = "Pinging every " + intervaltimer + " second".Pluralizer(intervaltimer) + ".";

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
            //AcrylicWindowStyle = SourceChord.FluentWPF.AcrylicWindowStyle.Normal;
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

        #region Startup_Load_Loop

        bool running_Startup_Load_Loop_Timer = false;

        static DispatcherTimer timer = null;

        private void Startup_Load_Loop_Timer(string todo)
        {
            if (timer == null)
            {
                timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1 / 30) }; // FPS is 60 so 1/60 or 1 frame every Second at 60FPS;
            }

            timer.Tick += Startup_Load_Loop_Tick;
            if (todo == "start")
            {
                if (!running_Startup_Load_Loop_Timer)
                {

                    timer.Start();
                    running_Startup_Load_Loop_Timer=true;
                }
                else
                {
                    throw new Exception("Load Loop Timer called to start even though it already has.");
                }

            }
            else if(todo == "stop")
            {
                if (running_Startup_Load_Loop_Timer)
                {
                    timer.Stop();
                    timer = null;
                    calltostop_StartupLoad = false;
                    stopping_StartupLoad = false;
                    frame_StartupLoad = 000; 
                    frame_StartupLoad_fade = 0;
                    running_Startup_Load_Loop_Timer = false;
                }
                else
                {
                    throw new Exception("Load Loop Timer called to stop even though it already has.");
                }
            }
            else
            {
                throw new Exception("Wrong Command for Load Loop Timer");
            }
        }

        private void Startup_Load_Loop_Tick(object sender, EventArgs e)
        {
            if (calltostop_StartupLoad == false)
            {
                Startup_Load_Loop();
            }
            else
            {
                stopping_StartupLoad = true;
                Startup_Load_Loop();
            }
        }

        bool calltostop_StartupLoad = false;
        bool stopping_StartupLoad = false;
        int frame_StartupLoad = 0; //59 for loop, 179 for sequence
        int frame_StartupLoad_fade = 0; //15 for 1/4 second at 60fps

        private void Startup_Load_Loop()
        {
            if (stopping_StartupLoad == false)
            {
                if (frame_StartupLoad != 059)
                {
                    frame_StartupLoad++;
                }
                else
                {
                    frame_StartupLoad = 000;
                }
            }
            else
            {
                if (frame_StartupLoad <= 059)
                {
                    frame_StartupLoad++;
                }
                else
                {
                    if (frame_StartupLoad != 179)
                    {
                        frame_StartupLoad++;
                    }
                    else
                    {
                        if(frame_StartupLoad_fade != 30)
                        {
                            frame_StartupLoad_fade++;
                        }
                        else
                        {
                            Startup_Load_Loop_Timer("stop");
                        }
                    }
                }
            }

            Startup_Load.Source = new BitmapImage(new Uri(@"pack://application:,,,/XAMLPingNET;component/resources/Sequence/Loading/load" + frame_StartupLoad.ToString("D3") + ".png"));

            if (frame_StartupLoad_fade != 0)
                {
                    panel_Startup_Load.Opacity = 1 - (frame_StartupLoad_fade/30);
                }

            if (frame_StartupLoad_fade == 30)
                {
                    panel_Startup_Load.Visibility = Visibility.Collapsed;
                }


        }

        #endregion


    }
}
