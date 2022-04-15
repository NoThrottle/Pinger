using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace XAMLPingNET
{
    public partial class MainWindow
    {

        string[] game_list = new string[] 
        {
            "Valorant",
            "Apex Legends",
            "League of Legends"
        };

        private void InitializeTab2()
        {

            if (game_combobox.Items.Count == 0)
            {
                foreach (var game in game_list)
                {
                    game_combobox.Items.Add(game);
                }
            }

        }

        private void UninitializeTab2()
        {

        }

        public dynamic Game_Valorant(string obtain)
        {
            //Valorant

            //Test Server IP Addresses
            string[,] game_ip_valorant = new string[,]
            {
            {"103.247.139.18",""},//Asia-Pacific
            {"27.111.229.182",""},
            {"210.176.150.254",""},
            {"195.66.226.186",""},
            {"13.248.220.97","76.223.72.224"},//may be inaccurate
            {"206.81.81.42",""},
            {"206.165.167.42",""},
            {"189.125.250.34",""},
            {"99.83.199.240]","75.2.105.73"}, //may be inaccurate
            {"75.2.66.166","99.83.136.104"}, //may be inaccurate
            };

            //Server Name
            string[,] game_ipname_valorant = new string[,]
            {
            {"HK","Hong Kong"},
            {"SG","Singapore"},
            {"KR","Korea"},
            {"EU","Europe"},
            {"EU-ER","Europe-Eastern Russia"},
            {"NA","North America"},
            {"LATAM","Latin America and Chile"},
            {"BR","Brazil"},
            {"BH","Bahrain"},
            {"Mumbai","Mumbai"},
            };

            //Server Name
            bool[,] game_ipaccuracy_valorant = new bool[,]
            {
            {false},
            {false},
            {false},
            {false},
            {true},
            {false},
            {false},
            {false},
            {true},
            {true},
            };

            int game_tps_valorant = 128;

            switch (obtain)
            {
                case "ip":
                    return game_ip_valorant;

                case "ipname":
                    return game_ipname_valorant;

                case "ipaccuracy":
                    return game_ipaccuracy_valorant;

                case "tps":
                    return game_tps_valorant;

                default:
                    throw new Exception("Incorrect string of obtaining values from game");
            }
        }

        private void tabs2_game_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string curr = this.game_combobox.SelectedItem.ToString();
                UpdateGameUI(curr);
            }
            catch 
            {

                Debug.WriteLine("tabs2_game_combobox_selectionchanged error was suppressed. If everything is working fine, ignore this");

            }
            
        }

        List<RowDefinition> rows = new List<RowDefinition>();

        private void UpdateGameUI(string gamename)
        {

            tab2_GameLogo.Source = new BitmapImage(new Uri(@"pack://application:,,,/XAMLPingNET;component/resources/NonUI/GameLogos/" + gamename.Replace(" ","") + "_logo.png"));
            tab2_GameName.Text = gamename;

            Type thisType = this.GetType();
            MethodInfo theMethod = thisType.GetMethod("Game_" + (gamename.Replace(" ", "")));

            Debug.WriteLine("Game_" + (gamename.Replace(" ", "")));

            string[,] game_ip = theMethod.Invoke(this, new object[] {"ip"}) as string[,];
            string[,] game_ipname = theMethod.Invoke(this, new object[] { "ipname" }) as string[,];
            bool[,] game_ipaccuracy = theMethod.Invoke(this, new object[] { "ipaccuracy" }) as bool[,];

            //leftrow

            int s = 0;
            while (s != game_ipname.GetLength(0))
            {

                var ip = game_ipname[s,1];
                RowDefinition newrow = new RowDefinition();
                newrow.Name = "row_" + gamename + ip.Replace(" ","").Replace("-","_");
                newrow.Height = new GridLength(28);

                rows.Add(newrow);

                game_details_left.RowDefinitions.Add(newrow);

                var g = new Grid { };
                var l = new Label
                {
                    Foreground = new SolidColorBrush(Color.FromRgb(255,255,255)),
                    FontFamily = new System.Windows.Media.FontFamily("Montserrat Regular"),
                    FontSize = 12,
                    Content = ip,                    
                };

                g.Children.Add(l);
                game_details_left.Children.Add(g);
                Grid.SetRow(g, s+1);
                s++;
            }

            s = 0;
            while (s != game_ip.GetLength(0))
            {

                var ip = game_ip[s, 1];
                RowDefinition newrow = new RowDefinition();
                newrow.Name = "row_" + gamename + ip.Replace(".", "_");
                newrow.Height = new GridLength(28);

                rows.Add(newrow);

                game_details_right.RowDefinitions.Add(newrow);

                var g = new Grid { };
                var l = new Label
                {
                    Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                    FontFamily = new System.Windows.Media.FontFamily("Montserrat Regular"),
                    FontSize = 12,
                    Content = "Loading",
                    HorizontalContentAlignment = HorizontalAlignment.Right,
                    HorizontalAlignment = HorizontalAlignment.Right,
                };

                g.Children.Add(l);
                game_details_right.Children.Add(g);
                Grid.SetRow(g, s + 1);
                s++;
            }



            tab2_panel_game.Visibility = Visibility.Visible;
        }

    }

    class PacketLoss
    {
        DispatcherTimer pkloss_timer = null;
        int pkloss_progress = 0;
        int pkloss_total = 0;
        string current_ip = "";

        private int Solve(int tps, string ip)
        {

            int i = 0;
            current_ip = ip;

            pkloss_timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1 / tps) };
            pkloss_timer.Tick += PKLossTimer_Tick;
            pkloss_timer.Start();

            while (pkloss_progress != 1000)
            {
                if (pkloss_progress == 1000)
                {
                    pkloss_timer.Stop();
                    pkloss_timer = null;
                    break;
                }
            }

            return pkloss_total;
        }

        private void PKLossTimer_Tick(object sender, EventArgs e)
        {
            Task.Run(() => PingManager(current_ip));
        }

        private void PingManager(string ip)
        {
            var repl = PingHost(ip);

            if (repl != null)
            {
                if(repl.Result.Status == IPStatus.Success)
                {
                    pkloss_progress++;
                }
                else
                {
                    pkloss_progress++;
                    pkloss_total++;
                }
            }
            else
            {
                pkloss_progress++;
                pkloss_total++;
            }

        }

        private static async Task<PingReply?> PingHost(string address)
        {
            CancellationTokenSource s_cts = new CancellationTokenSource();
            while (s_cts.IsCancellationRequested == false)
            {
                try
                {
                    using (Ping result = new Ping())
                    {
                        try
                        {
                            PingReply? reply = result.Send(address);

                            if (reply.Status == IPStatus.Success)
                            {
                                return reply;
                            }
                            else
                            {
                                return null;
                            }
                        }
                        catch
                        {
                            return null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    return null;
                }
            }

            Debug.WriteLine("Timed out");
            return null;

        }

    }

}
