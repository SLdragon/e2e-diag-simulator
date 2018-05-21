using CommandLine;

namespace e2e_diag_simulator
{
    class Options
    {
        [Option(shortName:'c',longName:"connection-string", HelpText ="Device connection string")]
        public string ConnectionString { get; set; }

        [Option(shortName: 'r', longName: "sampling-rate", Default=100, HelpText = "Diagnostic sampling rate, which can be [0,100]")]
        public int SamplingRate { get; set; }

        [Option(shortName: 'p', longName: "protocol",Default = "mqtt", HelpText = "Protocol to connect to the IoT Hub, which can be \"mqtt\" or \"amqp\"")]
        public string Protocol { get; set; }
    }
}
