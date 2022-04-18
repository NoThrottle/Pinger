using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace XAMLPingNET
{
    public static class extention
    {
        public static async Task TimeoutAfter(this Task task, int millisecondsTimeout)
        {
            if (task == await Task.WhenAny(task, Task.Delay(millisecondsTimeout)))
                await task;
            else
                throw new TimeoutException();
        }

        public static string Pluralizer(this string word, double amount)
        {
            if (amount != 1)
            {
                return word + "s";
            }
            else
            {
                return word;
            }
        }

        public static string CleanName(this string text)
        {
            return text.Replace(".", "").Replace(" ", "").Replace("-", "_");
        }

        public static IEnumerable<IPAddress> GetGateway()
        {
            // stolen from stackoverflow :>
            const int timeout = 5000;
            const int maxTTL = 1;
            const int bufferSize = 32;

            byte[] buffer = new byte[bufferSize];
            new Random().NextBytes(buffer);

            using (var pinger = new Ping())
            {
                for (int ttl = 1; ttl <= maxTTL; ttl++)
                {
                    PingOptions options = new PingOptions(ttl, true);
                    PingReply reply = pinger.Send("1.1.1.1", timeout, buffer, options);

                    if (reply.Status == IPStatus.Success || reply.Status == IPStatus.TtlExpired)
                        yield return reply.Address;

                    if (reply.Status != IPStatus.TtlExpired && reply.Status != IPStatus.TimedOut)
                        break;
                }
            }
        }
    }
}
