using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace XAMLPing
{
    public partial class MainWindow
    {

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

        private void pinger(string address)
        {
        }

    }
}
