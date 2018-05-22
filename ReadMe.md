# Command Line Tool to send messages with diagnostic information to Azure IoT Hub

The e2e-diag-simulator offers command line to help you send messages with diagnostic information to Azure IoT Hub.

## Quick Start
```bash
  git clone https://github.com/SLdragon/e2e-diag-simulator.git
  cd e2e-diag-simulator
  dotnet restore
  dotnet publish -c Release -o out
  cd out
  dotnet e2e-diag-simulator.dll -c "Your IoTHub device connection string"
```

## Command Line Reference

```
  dotnet e2e-diag-simulator.dll parameters...

  -c, --connection-string    Device connection string

  -r, --sampling-rate        (Default: 100) Diagnostic sampling rate, which can
                             be [-1,100], if sampling rate is -1, then it will
                             retrieve sampling rate from device twin

  -p, --protocol             (Default: mqtt) Protocol to connect to the IoT
                             Hub, which can be "mqtt" or "amqp"

  --help                     Display help screen.

  --version                  Display version information.
```

### Samples
```bash
  dotnet e2e-diag-simulator.dll  -c "Your IoTHub device connection string" -r 100 -p "amqp"
```


## Docker Usage

```bash
  docker run -it --rm turenlong/e2e-diag-simulator parameters...
```

### Samples

```bash
  docker run -it --rm turenlong/e2e-diag-simulator -c "Your IoTHub device connection string" -r 100 -p "amqp"
```