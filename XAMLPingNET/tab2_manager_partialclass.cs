using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
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

            if ((active_iplist != null))
            {

                Game_Pinger_Timer_func("start");
            }

        }

        private void UninitializeTab2()
        {
            Game_Pinger_Timer_func("stop");
        }

        private void Tab2_ResetToDefault()
        {
            if (game_details_right.RowDefinitions.Count != 1)
            {
                game_details_left.Children.RemoveRange(1, game_details_left.Children.Count - 1);
                game_details_right.Children.RemoveRange(1, game_details_right.Children.Count - 1);
                game_details_right.RowDefinitions.RemoveRange(1, game_details_right.RowDefinitions.Count() - 1);
                game_details_left.RowDefinitions.RemoveRange(1, game_details_left.RowDefinitions.Count() - 1);
            }

            foreach (var grid in active_gridlist)
            {
                UnregisterName(grid.Name);
            }

            foreach (var label in listlabelsping)
            {
                UnregisterName(label.Name);
            }

            active_ipaccuracy = null;
            active_iplist = null;
            rows.Clear();
            rows2.Clear();
            listlabelsping.Clear();
            active_gridlist.Clear();

        }

        public dynamic Game_Valorant(string obtain)
        {
            //Valorant

            //Test Server IP Addresses
            string[,] game_ip = new string[,]
            {
            {"103.247.139.18",""},//Asia-Pacific
            {"27.111.229.182",""},
            {"210.176.150.254",""},
            {"195.66.226.186",""},
            {"13.248.220.97","76.223.72.224"},//may be inaccurate
            {"206.81.81.42","120.28.10.150"}, //may be inaccurate
            {"206.165.167.42",""},
            {"189.125.250.34",""},
            {"99.83.199.240","75.2.105.73"}, //may be inaccurate
            {"75.2.66.166","99.83.136.104"}, //may be inaccurate
            };

            //Server Name
            string[,] game_ipname = new string[,]
            {
            {"Hong Kong","HK"},
            {"Singapore","SG"},
            {"Korea","KR"},
            {"Europe","EE"},
            {"Europe-Eastern Russia","EU-ER"},
            {"North America","NA"},
            {"Latin America and Chile","LATAM"},
            {"Brazil","BR"},
            {"Bahrain","BH"},
            {"Mumbai","Mumbai"},
            };

            //Server Name
            bool[,] game_ipaccuracy = new bool[,]
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

            int game_tps = 128;

            switch (obtain)
            {
                case "ip":
                    return game_ip;

                case "ipname":
                    return game_ipname;

                case "ipaccuracy":
                    return game_ipaccuracy;

                case "tps":
                    return game_tps;

                default:
                    throw new Exception("Incorrect string of obtaining values from game");
            }
        }

        public dynamic Game_ApexLegends(string obtain)
        {

            //Test Server IP Addresses
            string[,] game_ip = new string[,]
            {
                {"107.182.233.168","","","","",""},
                {"104.198.102.93","104.198.101.253","52.40.240.176","52.42.44.79","",""},
                {"209.239.121.82","","","","",""},
                {"63.251.239.123","","","","",""},
                {"130.211.193.234","104.197.17.180","104.197.136.10","104.197.42.178","",""},
                {"107.150.147.67","","","","",""},
                {"104.196.43.45","35.196.104.104","104.196.8.33","","",""},
                {"52.6.64.33","52.86.226.95","","","",""},

                {"177.54.152.31","52.67.92.122","52.67.31.204","","",""},
                {"217.147.89.101","","","","",""},
                {"64.95.100.189","","","","",""},
                {"146.148.120.92","104.155.80.155","130.211.51.110","","",""},
                {"52.58.81.34","52.59.121.244","","","",""},
                {"69.88.135.37","","","","",""},
                {"104.155.233.79","104.199.182.138","","","",""},
                {"72.5.161.228","35.185.189.243","35.185.189.104","","",""},
                {"161.202.72.179","104.198.82.36","104.198.82.65","104.198.88.214","52.69.157.152","52.197.77.217"},
                {"27.50.72.162","35.197.166.13","35.201.19.135","52.63.136.88","52.62.160.212",""}
            };

            //Server Name
            string[,] game_ipname = new string[,]
            {
                {"NA - Salt Lake City"},
                {"NA - Oregon"},
                {"NA - St.Louis"},
                {"NA - Dallas"},
                {"NA - Iowa"},
                {"NA - New York"},
                {"NA - South Carolina"},
                {"NA - Virginia"},

                {"Brazil"},
                {"UK"},
                {"Amsterdam"},
                {"Belgium"},
                {"Germany"},
                {"Hong Kong"},
                {"Taiwan"},
                {"Singapore"},
                {"Tokyo"},
                {"Sydney"}
            };

            //Server Name
            bool[,] game_ipaccuracy = new bool[,]
            {
                {false},
                {false},
                {false},
                {false},
                {false},
                {false},
                {false},
                {false},
                {false},
                {false},
                {false},
                {false},
                {false},
                {false},
                {false},
                {false},
                {false},
                {false},
            };

            int game_tps = 20;

            switch (obtain)
            {
                case "ip":
                    return game_ip;

                case "ipname":
                    return game_ipname;

                case "ipaccuracy":
                    return game_ipaccuracy;

                case "tps":
                    return game_tps;

                default:
                    throw new Exception("Incorrect string of obtaining values from game");
            }
        }

        private void tabs2_game_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            game_combobox_default.Visibility = Visibility.Collapsed;
            Game_Pinger_Timer_func("stop");

            try
            {
                Tab2_ResetToDefault();
                string curr = this.game_combobox.SelectedItem.ToString();

                Type thisType = this.GetType();
                MethodInfo theMethod = thisType.GetMethod("Game_" + (curr.Replace(" ", "")));

                Debug.WriteLine("Game_" + (curr.Replace(" ", "")));

                string[,] game_ip = theMethod.Invoke(this, new object[] { "ip" }) as string[,];
                string[,] game_ipname = theMethod.Invoke(this, new object[] { "ipname" }) as string[,];
                bool[,] game_ipaccuracy = theMethod.Invoke(this, new object[] { "ipaccuracy" }) as bool[,];

                active_iplist = game_ip;
                active_ipaccuracy = game_ipaccuracy;
                active_ipname = game_ipname;

                UpdateGameUI(curr);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                Debug.WriteLine("tabs2_game_combobox_selectionchanged error was suppressed. If everything is working fine, ignore this");

            }

        }

        private void tab2_panel_connection_update()
        {
            Dispatcher.Invoke(() => 
            {
                tab2_panel_connection_gatewayresult.Content = (NetworkInterface.GetIsNetworkAvailable()) ? "Connected" : "Disconnected";

                try
                {
                    tab2_panel_connection_routerping.Content = (extention.GetGateway() != null) ? PingHost(extention.GetGateway().First(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString()).Result.RoundtripTime + "ms" : "-";
                }
                catch
                {
                    tab2_panel_connection_routerping.Content = "-";
                }
            });

        }

        //---------------//

        List<RowDefinition> rows = new List<RowDefinition>();
        List<RowDefinition> rows2 = new List<RowDefinition>();
        List<Label> listlabelsping = new List<Label>();
        List<Grid> active_gridlist = new List<Grid>();

        private void UpdateGameUI(string gamename)
        {

            tab2_GameLogo.Source = new BitmapImage(new Uri(@"pack://application:,,,/XAMLPingNET;component/resources/NonUI/GameLogos/" + gamename.Replace(" ", "") + "_logo.png"));
            tab2_GameName.Text = gamename;

            var game_ipname = active_ipname;

            //leftrow
            int s = 0;
            while (s != game_ipname.GetLength(0))
            {

                var ip = game_ipname[s, 0];
                RowDefinition newrow = new RowDefinition();
                newrow.Name = "column_" + gamename.CleanName() + ip.CleanName();
                newrow.Height = new GridLength(28);

                rows.Add(newrow);
                game_details_left.RowDefinitions.Add(newrow);

                var g = new Grid { };
                var l = new Label
                {
                    Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                    FontFamily = new System.Windows.Media.FontFamily("Montserrat Regular"),
                    FontSize = 12,
                    Content = ip,
                };

                g.Children.Add(l);
                game_details_left.Children.Add(g);
                Grid.SetRow(g, s + 1);

                //----------------//

                RowDefinition newrow2 = new RowDefinition();
                newrow2.Name = "column2_" + gamename.CleanName() + ip.CleanName();
                newrow2.Height = new GridLength(28);

                rows2.Add(newrow2);
                game_details_right.RowDefinitions.Add(newrow2);

                var g2 = new Grid { Name = "column2_" + gamename.CleanName() + ip.CleanName() + "_grid", };
                active_gridlist.Add(g2);
                var l2 = new Label
                {
                    Name = "column2_" + gamename.CleanName() + ip.CleanName() + "_text",
                    Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                    FontFamily = new System.Windows.Media.FontFamily("Montserrat Regular"),
                    FontSize = 12,
                    Content = "Loading",
                    HorizontalContentAlignment = HorizontalAlignment.Right,
                    HorizontalAlignment = HorizontalAlignment.Right,
                };

                listlabelsping.Add(l2);

                g2.Children.Add(l2);
                game_details_right.Children.Add(g2);
                Grid.SetRow(g2, s + 1);

                RegisterName(l2.Name,l2);
                RegisterName(g2.Name,g2);

                s++;
            }

            Task.Run(() =>
            {

                UpdatePingRead(active_iplist, active_ipaccuracy, listlabelsping, active_gridlist.ToArray());

            });

            game_dyngrid.Visibility = Visibility.Visible;
            game_gameinfo.Visibility = Visibility.Visible;

            tab2_panel_game.ScrollToVerticalOffset(100);
            Game_Pinger_Timer_func("start");
        }

        #region Game_Pinger

        DispatcherTimer game_pinger_timer = null;
        private void Game_Pinger_Timer_func(string state)
        {

            switch (state)
            {
                case "start":

                    if (game_pinger_timer != null)
                    {
                        game_pinger_timer.Start();
                    }
                    else
                    {
                        game_pinger_timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
                        game_pinger_timer.Tick += UpdatePingRead_tick;

                        game_pinger_timer.Start();
                    }
                    break;

                case "stop":

                    if (game_pinger_timer != null)
                    {
                        game_pinger_timer.Stop();
                    }
                    else
                    {
                        Debug.WriteLine("Game_Pinger_Timer is requested to be stopped but has not started ever.");
                    }
                    break;
            }
        }

        string[,] active_iplist = null;
        string[,] active_ipname = null;
        bool[,] active_ipaccuracy = null;

        private void UpdatePingRead_tick(object sender, EventArgs e)
        {
            Task.Run(() =>
            {

                UpdatePingRead(active_iplist, active_ipaccuracy, listlabelsping, active_gridlist.ToArray());

            });
        }

        private void UpdatePingRead(string[,] iplist, bool[,] ipaccuracy, List<Label> labellist, Grid[] gridlist)
        {
            for(int i = 0; i < iplist.GetLength(0); i++)
            {
                List<PingReply?> reply = new List<PingReply?>();
                List<string> tooltip = new List<string>();
                string toreturn = "";

                if (ipaccuracy[i, 0] == true)
                {
                    tooltip.Add("The accuracy of results from this" + Environment.NewLine + "IP test may be questionable." + Environment.NewLine);
                };

                for (int a = 0; a < iplist.GetLength(1); a++)
                {
                    if (iplist[i, a] != "")
                    {
                        var z = PingHost(iplist[i, a]).Result;
                        reply.Add(z);
                        tooltip.Add(iplist[i, a] + ": - " + SolvePing(z));
                    }
                }

                if (reply.Count > 1)
                {
                    toreturn = Summary(reply.ToArray());
                }
                else if (reply.Count == 1)
                {
                    toreturn = SolvePing(reply[0]);
                }
                else
                {
                    toreturn = "Error";
                }

                Dispatcher.Invoke(new Action(() =>
                {

                    try
                    {
                        var label = (Label)gridlist[i].FindName(labellist[i].Name);
                        label.Content = (ipaccuracy[i, 0] == true ? @"ⓘ " : "") + toreturn;
                        label.ToolTip = string.Join(Environment.NewLine, tooltip);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Null on Ping");
                    }

                }));
            }
        }

        #endregion
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
