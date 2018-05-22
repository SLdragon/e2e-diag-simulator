using System;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Azure.Devices.Client;

namespace e2e_diag_simulator
{
    class Program
    {
        private static int MESSAGE_COUNT = int.MaxValue;
        private const int TEMPERATURE_THRESHOLD = 30;
        private static float temperature;
        private static float humidity;
        private static Random rnd = new Random();

        static void Main(string[] args)
        {

            Console.CancelKeyPress += new ConsoleCancelEventHandler(OnExit);
            try
            {
                var parser = new Parser(with => with.EnableDashDash = true);
                var result = parser.ParseArguments<Options>(args);

                if (result is Parsed<Options>)
                {
                    var options = ((Parsed<Options>)result).Value;

                    TransportType protocol = TransportType.Mqtt;

                    if (options.Protocol == "mqtt")
                    {
                        protocol = TransportType.Mqtt;
                    }
                    else if (options.Protocol == "amqp")
                    {
                        protocol = TransportType.Amqp;
                    }

                    DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(options.ConnectionString, protocol);
                    if (deviceClient == null)
                    {
                        Console.WriteLine("Failed to create DeviceClient!");
                    }
                    else
                    {
                        if (options.SamplingRate == -1)
                        {
                            //deviceClient.EnableE2EDiagnosticWithCloudSetting().Wait();
                        }
                        else
                        {
                            deviceClient.DiagnosticSamplingPercentage = options.SamplingRate;
                        }
                        SendEvent(deviceClient).Wait();
                    }

                    Console.WriteLine("Exited!\n");
                }
                else
                {
                    Console.WriteLine(CommandLine.Text.HelpText.AutoBuild(result));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in sample: {0}", ex.Message);
            }
        }

        protected static void OnExit(object sender, ConsoleCancelEventArgs args)
        {
            Console.WriteLine("\tQuit application");
            Environment.Exit(0);
        }

        static async Task SendEvent(DeviceClient deviceClient)
        {
            Console.WriteLine("Device sending {0} messages to IoTHub...\n", MESSAGE_COUNT);
            for (int count = 0; count < MESSAGE_COUNT; count++)
            {
                temperature = rnd.Next(20, 35);
                humidity = rnd.Next(60, 80);
                var dataBuffer = string.Format("{{\"messageId\":{0},\"temperature\":{1},\"humidity\":{2}}}", count, temperature, humidity);
                Message eventMessage = new Message(Encoding.UTF8.GetBytes(dataBuffer));
                eventMessage.Properties.Add("temperatureAlert", (temperature > TEMPERATURE_THRESHOLD) ? "true" : "false");
                eventMessage.Properties.Add("$CreationTimeUtc", "2018-04-18T02:15:58+00:00");
                Console.WriteLine("\t{0}> Sending message: {1}, Data: [{2}]", DateTime.Now.ToLocalTime(), count, dataBuffer);

                await deviceClient.SendEventAsync(eventMessage).ConfigureAwait(false);
                System.Threading.Thread.Sleep(1500);
            }
        }
    }
}
