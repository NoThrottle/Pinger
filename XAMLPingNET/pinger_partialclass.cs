using System;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Windows.Threading;
using System.Diagnostics;
using System.Threading;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace XAMLPingNET
{
    public partial class MainWindow
    {
        #region Hardcoded Information
        //Google Information
        string google_cloud = @"ch-asia.storage.googleapis.com";
        string google_youtubesite = @"youtube.com";
        string google_site = @"google.com";
        string google_dns1 = @"8.8.8.8";
        string google_dns2 = @"8.8.4.4";

        //AWS Information
        string amazon_cloudfront = @"13.224.163.84";
        string amazon_awssite = @"aws.amazon.com";
        string amazon_aws_ap_se = @"dynamodb.ap-southeast-1.amazonaws.com"; //SG
        string amazon_aws_ap_ne = @"dynamodb.ap-northeast-1.amazonaws.com"; //JP
        string amazon_aws_ap_e = @"dynamodb.ap-east-1.amazonaws.com"; //HK

        //Cloudflare Information
        string cloudflare_site = @"cloudflare.com";
        string cloudflare_dns1 = @"1.1.1.1";
        string cloudflare_dns2 = @"1.0.0.1";
        string cloudflare_ip1 = @"103.21.244.0"; //HK? unsure
        string cloudflare_ip2 = @"104.24.0.0";

        //Akamai Information
        string akamai_site = @"akamai.com";
        string akamai_ip1 = @"1.37.76.216";
        string akamai_dns = @"107.22.235.245";

        //ISP Information
        string isp_globe = @"speedtest.globe.com.ph";
        string isp_pldt = @"119.92.223.158";
        string isp_skyfiber = @"speedtest-manda.skybroadband.com.ph";
        string isp_converge = @"psg-spdtst-srv.convergeict.com";
        string isp_dito = @"1.speedtest.dito.ph.prod.hosts.ooklaserver.net";
        #endregion

        //---------------//

        #region PingReply Sets
        //Google Information
        PingReply google_cloud_result = null;
        PingReply google_youtubesite_result = null;
        PingReply google_site_result = null;
        PingReply google_dns1_result = null;
        PingReply google_dns2_result = null;

        //AWS Information
        PingReply amazon_cloudfront_result = null;
        PingReply amazon_awssite_result = null;
        PingReply amazon_aws_ap_se_result = null;
        PingReply amazon_aws_ap_ne_result = null;
        PingReply amazon_aws_ap_e_result = null;

        //Cloudflare Information
        PingReply cloudflare_site_result = null;
        PingReply cloudflare_dns1_result = null;
        PingReply cloudflare_dns2_result = null;
        PingReply cloudflare_ip1_result = null;
        PingReply cloudflare_ip2_result = null;

        //Akamai Information
        PingReply akamai_site_result = null;
        PingReply akamai_ip1_result = null;
        PingReply akamai_dns_result = null;

        //ISP Information
        PingReply isp_globe_result = null;
        PingReply isp_pldt_result = null;
        PingReply isp_skyfiber_result = null;
        PingReply isp_converge_result = null;
        PingReply isp_dito_result = null;
        #endregion

        //---------------//

        double intervaltimer = 5; //seconds
        double uitimer = 1/4; //15fps
        int setactive = 0; //0 is none

        //---------------//

        private void InitializeTab1()
        {
            PingEachOne();
            UpdateEachOne();

            UI_Timer();
            Pinger_Timer(false);
        }

        private void UninitializeTab1()
        {
            if (ui_timer != null)
            {
                ui_timer.Stop();
                ui_timer = null;
            }

            //if (pinger_timer != null)
            //{
            //    pinger_timer.Stop();
            //    pinger_timer = null;
            //}
        }

        #region Timers
        DispatcherTimer pinger_timer = null;
        DispatcherTimer ui_timer = null;

        private void Pinger_Timer(bool restart)
        {
            if (restart == false)
            {
                pinger_timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(intervaltimer) };
                pinger_timer.Tick += PingerTimer_Tick;
                pinger_timer.Start();
            }
            else
            {
                pinger_timer.Stop();
                pinger_timer = null;

                pinger_timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(intervaltimer) };
                pinger_timer.Tick += PingerTimer_Tick;
                pinger_timer.Start();
            }
        }

        private void UI_Timer()
        {
                ui_timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(uitimer)};
                ui_timer.Tick += UITimer_Tick;
                ui_timer.Start();
            
        }

        private void PingerTimer_Tick(object sender, EventArgs e)
        {
            Task.Run(() => { tab2_panel_connection_update(); });
            PingEachOne();
        }

        private void UITimer_Tick(object sender, EventArgs e)
        {
            UpdateEachOne();
        }

        #endregion

        private string SolvePing(PingReply? reply)
        {

            if (reply != null)
            {

                long ms = reply.RoundtripTime;

                if (reply.Status == IPStatus.Success)
                {
                    if (ms < 1)
                    {
                        return "&lt;1ms";
                    }
                    else
                    {
                        return Math.Round((double)ms).ToString() + "ms";
                    }
                }
                else
                {
                    return "Unavailable";
                }
            }
            else
            {
                return "Unavailable";
            }

        }

        private string Summary(PingReply[] pings)
        {

            int active = 0;
            int total = 0;
            int todivide = 0;

            foreach (PingReply ping in pings)
            {
                total++;

                if ((ping != null) && (ping.Status == IPStatus.Success))
                {
                    active++;
                    todivide += (int)Math.Round((double)ping.RoundtripTime);

                }

            }

            if (active > 0)
            {
                return (todivide / active).ToString() + "ms - " + active + "/" + total + "";
            }
            else
            {
                return "Unavailable - 0/" + total + "";
            }
        }

        private string SolveAddress(PingReply reply)
        {
            if (reply != null)
            {
                try
                {
                    return reply.Address.ToString();
                }
                catch
                {
                    return "Unavailable";
                }
            }
            else
            {
                return "Error";
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
                                //Debug.WriteLine("suc");
                                return reply;
                            }
                            else
                            {
                                //Debug.WriteLine("nosuc");
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

        private void PingEachOne()
        {
            //Debug.WriteLine("eachone");

            #region Google Tasks
            Task.Run(() =>
              {
                google_cloud_result = PingHost(google_cloud).Result;

                google_youtubesite_result =  PingHost(google_youtubesite).Result;

                google_site_result = PingHost(google_site).Result;

                google_dns1_result = PingHost(google_dns1).Result;

                google_dns2_result = PingHost(google_dns2).Result;
            });
            #endregion

            #region AWS Tasks
            Task.Run(() =>
            {
                amazon_cloudfront_result = PingHost(amazon_cloudfront).Result;

                amazon_awssite_result = PingHost(amazon_awssite).Result;

                amazon_aws_ap_se_result = PingHost(amazon_aws_ap_se).Result;

                amazon_aws_ap_ne_result = PingHost(amazon_aws_ap_ne).Result;

                amazon_aws_ap_e_result = PingHost(amazon_aws_ap_e).Result;
            });
            #endregion

            #region Cloudflare Tasks

            Task.Run(() =>
            {

                cloudflare_site_result = PingHost(cloudflare_site).Result;

                cloudflare_dns1_result = PingHost(cloudflare_dns1).Result;

                cloudflare_dns2_result = PingHost(cloudflare_dns2).Result;

                cloudflare_ip1_result = PingHost(cloudflare_ip1).Result;

                cloudflare_ip2_result = PingHost(cloudflare_ip2).Result;
            });

            #endregion

            #region Akamai Tasks

            Task.Run(() =>
            {

                akamai_site_result = PingHost(akamai_site).Result;

                akamai_ip1_result = PingHost(akamai_ip1).Result;

                akamai_dns_result = PingHost(akamai_dns).Result;
            });

            #endregion

            #region ISP Tasks

            Task.Run(() =>
            {

                isp_globe_result = PingHost(isp_globe).Result;

                isp_pldt_result = PingHost(isp_pldt).Result;

                isp_skyfiber_result = PingHost(isp_skyfiber).Result;

                isp_converge_result = PingHost(isp_converge).Result;

                isp_dito_result = PingHost(isp_dito).Result;
            });

            #endregion
        }

        private void UpdateEachOne()
        {
            try
            {
                status_item1ms_text.Content = Summary(new PingReply[] { google_cloud_result, google_youtubesite_result, google_site_result, google_dns1_result, google_dns2_result });
                status_item2ms_text.Content = Summary(new PingReply[] { amazon_cloudfront_result, amazon_awssite_result, amazon_aws_ap_se_result, amazon_aws_ap_ne_result, amazon_aws_ap_e_result });
                status_item3ms_text.Content = Summary(new PingReply[] { cloudflare_site_result, cloudflare_dns1_result, cloudflare_dns2_result, cloudflare_ip1_result, cloudflare_ip2_result });
                status_item4ms_text.Content = Summary(new PingReply[] { akamai_site_result, akamai_ip1_result, akamai_dns_result });
                status_item5ms_text.Content = Summary(new PingReply[] { isp_globe_result, isp_pldt_result, isp_skyfiber_result, isp_converge_result, isp_dito_result});

                switch (setactive)
                {
                    case 0:
                        info_logo.Visibility = Visibility.Hidden;
                        info_brand.Text = "";
                        //left column
                        info_item1_text.Content = "";
                        info_item2_text.Content = "";
                        info_item3_text.Content = "";
                        info_item4_text.Content = "";
                        info_item5_text.Content = "";
                        //right column
                        info_item1ms_text.Content = "";
                        info_item2ms_text.Content = "";
                        info_item3ms_text.Content = "";
                        info_item4ms_text.Content = "";
                        info_item5ms_text.Content = "";
                        break;

                    case 1:
                        info_logo.Source = new BitmapImage (new Uri("/XAMLPingNET;component/Resources/NonUI/Logos/google-logo.png", UriKind.Relative));
                        info_logo.Visibility = Visibility.Visible;
                        info_brand.Text = "Google Cloud Platform"; 
                        //left column
                        info_item1_text.Content = SolveAddress(google_cloud_result);
                        info_item2_text.Content = SolveAddress(google_youtubesite_result);
                        info_item3_text.Content = SolveAddress(google_site_result);
                        info_item4_text.Content = SolveAddress(google_dns1_result);
                        info_item5_text.Content = SolveAddress(google_dns2_result);
                        //right column
                        info_item1ms_text.Content = SolvePing(google_cloud_result);
                        info_item2ms_text.Content = SolvePing(google_youtubesite_result);
                        info_item3ms_text.Content = SolvePing(google_site_result);
                        info_item4ms_text.Content = SolvePing(google_dns1_result);
                        info_item5ms_text.Content = SolvePing(google_dns2_result);
                        break;

                    case 2:
                        info_logo.Source = new BitmapImage(new Uri("/XAMLPingNET;component/Resources/NonUI/Logos/aws-logo.png", UriKind.Relative));
                        info_logo.Visibility = Visibility.Visible;
                        info_brand.Text = "Amazon Web Services";
                        //left column
                        info_item1_text.Content = SolveAddress(amazon_cloudfront_result);
                        info_item2_text.Content = SolveAddress(amazon_awssite_result);
                        info_item3_text.Content = SolveAddress(amazon_aws_ap_se_result);
                        info_item4_text.Content = SolveAddress(amazon_aws_ap_ne_result);
                        info_item5_text.Content = SolveAddress(amazon_aws_ap_e_result);
                        //right column
                        info_item1ms_text.Content = SolvePing(amazon_cloudfront_result);
                        info_item2ms_text.Content = SolvePing(amazon_awssite_result);
                        info_item3ms_text.Content = SolvePing(amazon_aws_ap_se_result);
                        info_item4ms_text.Content = SolvePing(amazon_aws_ap_ne_result);
                        info_item5ms_text.Content = SolvePing(amazon_aws_ap_e_result);
                        break;

                    case 3:
                        info_logo.Source = new BitmapImage(new Uri("/XAMLPingNET;component/Resources/NonUI/Logos/cloudflare-logo.png", UriKind.Relative));
                        info_logo.Visibility = Visibility.Visible;
                        info_brand.Text = "Cloudflare";
                        //left column
                        info_item1_text.Content = SolveAddress(cloudflare_site_result);
                        info_item2_text.Content = SolveAddress(cloudflare_dns1_result);
                        info_item3_text.Content = SolveAddress(cloudflare_dns2_result);
                        info_item4_text.Content = SolveAddress(cloudflare_ip1_result);
                        info_item5_text.Content = SolveAddress(cloudflare_ip2_result);
                        //right column
                        info_item1ms_text.Content = SolvePing(cloudflare_site_result);
                        info_item2ms_text.Content = SolvePing(cloudflare_dns1_result);
                        info_item3ms_text.Content = SolvePing(cloudflare_dns2_result);
                        info_item4ms_text.Content = SolvePing(cloudflare_ip1_result);
                        info_item5ms_text.Content = SolvePing(cloudflare_ip2_result);
                        break;

                    case 4:
                        info_logo.Source = new BitmapImage(new Uri("/XAMLPingNET;component/Resources/NonUI/Logos/akamai-logo.png", UriKind.Relative));
                        info_logo.Visibility = Visibility.Visible;
                        info_brand.Text = "Akamai Technologies";
                        //left column
                        info_item1_text.Content = SolveAddress(akamai_site_result);
                        info_item2_text.Content = SolveAddress(akamai_ip1_result);
                        info_item3_text.Content = SolveAddress(akamai_dns_result);
                        info_item4_text.Content = "";
                        info_item5_text.Content = "";
                        //right column
                        info_item1ms_text.Content = SolvePing(akamai_site_result);
                        info_item2ms_text.Content = SolvePing(akamai_ip1_result);
                        info_item3ms_text.Content = SolvePing(akamai_dns_result);
                        info_item4ms_text.Content = "";
                        info_item5ms_text.Content = "";
                        break;

                    case 5:
                        info_logo.Source = new BitmapImage(new Uri("/XAMLPingNET;component/Resources/NonUI/Logos/router-wireless.png", UriKind.Relative));
                        info_logo.Visibility = Visibility.Visible;
                        info_brand.Text = "Local Networks";
                        //left column
                        info_item1_text.Content = SolveAddress(isp_globe_result);
                        info_item2_text.Content = SolveAddress(isp_pldt_result);
                        info_item3_text.Content = SolveAddress(isp_skyfiber_result);
                        info_item4_text.Content = SolveAddress(isp_converge_result);
                        info_item5_text.Content = SolveAddress(isp_dito_result);
                        //right column
                        info_item1ms_text.Content = SolvePing(isp_globe_result);
                        info_item2ms_text.Content = SolvePing(isp_pldt_result);
                        info_item3ms_text.Content = SolvePing(isp_skyfiber_result);
                        info_item4ms_text.Content = SolvePing(isp_converge_result);
                        info_item5ms_text.Content = SolvePing(isp_dito_result);
                        break;

                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        #region SetActiveTab Methods
        private void SetActiveTab1(object sender, MouseButtonEventArgs e)
        {
            Tabs(1);
        }

        private void SetActiveTab2(object sender, MouseButtonEventArgs e)
        {
            Tabs(2);
        }

        private void SetActiveTab3(object sender, MouseButtonEventArgs e)
        {
            Tabs(3);
        }
        #endregion  
       
    }
   
}
