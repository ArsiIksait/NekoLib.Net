using System.Net;
using System.Net.Sockets;

namespace NekoLib.Net
{
    public class NekoSocket
    {
        public class NekoSocketServer
        {
            TcpListener? server;
            TcpClient? client;
            NetworkStream? stream;
            IPAddress IPAddress;
            int port;

            public event MessageReceivedEventHandler? MessageReceived;
            public delegate void MessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);
            public class MessageReceivedEventArgs
            {
                public MessageReceivedEventArgs(byte[] data) { Data = data; }
                public byte[] Data { get; }
            }
            ~NekoSocketServer()
            {
                Stop();
            }
            public NekoSocketServer()
            {
                IPAddress = IPAddress.Parse("127.0.0.1");
                port = 2233;
                Start();
            }
            public NekoSocketServer(string iPEndPoint)
            {
                try
                {
                    IPAddress = IPAddress.Parse(iPEndPoint.Split(":")[0]);
                    port = int.Parse(iPEndPoint.Split(":")[1]);
                }
                catch (Exception)
                {
                    throw new Exception("Socket地址字符串格式不正确，示例: 127.0.0.1:2233");
                }

                Start();
            }
            public NekoSocketServer(string iPAddress, int port)
            {
                try
                {
                    IPAddress = IPAddress.Parse(iPAddress);
                    this.port = port;
                }
                catch (Exception)
                {
                    throw new Exception("Socket地址参数不正确，示例参数1: 127.0.0.1:2233 参数2： 2233");
                }

                Start();
            }
            public NekoSocketServer(IPEndPoint iPEndPoint)
            {
                IPAddress = iPEndPoint.Address;
                port = iPEndPoint.Port;
                Start();
            }
            internal NetworkStream? GetNetworkStream()
            {
                return stream;
            }
            internal TcpClient? GetTcpClient()
            {
                return client;
            }
            internal void Start()
            {
                Thread nekoSocketServer = new(() => {
                    server = new TcpListener(IPAddress, port);
                    server.Start();
                    while(true)
                    {
                        try
                        {
                            client = server.AcceptTcpClient();
                            var data = new byte[client.ReceiveBufferSize];
                            stream = client.GetStream();
                            stream.Read(data, 0, data.Length);
                            MessageReceived?.Invoke(this, new(data));
                        }
                        catch (Exception)
                        {
                            Stop();
                        }
                    }
                });
                nekoSocketServer.Start();
            }
            internal void Stop()
            {
                GetNetworkStream()?.Close();
                GetTcpClient()?.Close();
            }
            public void SendRawMessage(byte[] data)
            {
                GetNetworkStream()?.Write(data, 0, data.Length);
                GetNetworkStream()?.Flush();
            }
        }
        public class NekoSocketClinet
        {
            TcpClient? client;
            NetworkStream? stream;
            IPAddress IPAddress;
            int port;

            public event MessageReceivedEventHandler? MessageReceived;
            public delegate void MessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);
            public class MessageReceivedEventArgs
            {
                public MessageReceivedEventArgs(byte[] data) { Data = data; }
                public byte[] Data { get; }
            }
            ~NekoSocketClinet()
            {
                Stop();
            }
            public NekoSocketClinet()
            {
                IPAddress = IPAddress.Parse("127.0.0.1");
                port = 2233;
                Start();
            }
            public NekoSocketClinet(string iPEndPoint)
            {
                try
                {
                    IPAddress = IPAddress.Parse(iPEndPoint.Split(":")[0]);
                    port = int.Parse(iPEndPoint.Split(":")[1]);
                }
                catch (Exception)
                {
                    throw new Exception("Socket地址字符串格式不正确，示例: 127.0.0.1:2233");
                }

                Start();
            }
            public NekoSocketClinet(string iPAddress, int port)
            {
                try
                {
                    IPAddress = IPAddress.Parse(iPAddress);
                    this.port = port;
                }
                catch (Exception)
                {
                    throw new Exception("Socket地址参数不正确，示例参数1: 127.0.0.1:2233 参数2： 2233");
                }

                Start();
            }
            public NekoSocketClinet(IPEndPoint iPEndPoint)
            {
                IPAddress = iPEndPoint.Address;
                port = iPEndPoint.Port;
                Start();
            }
            internal NetworkStream? GetNetworkStream()
            {
                return stream;
            }
            internal TcpClient? GetTcpClient()
            {
                return client;
            }
            internal void Start()
            {
                Thread nekoSocketClient = new(() => {
                    client = new TcpClient(IPAddress.ToString(), port);

                    while (true)
                    {
                        try
                        {
                            var data = new byte[client.ReceiveBufferSize];
                            stream = client.GetStream();
                            stream.Read(data, 0, data.Length);
                            MessageReceived?.Invoke(this, new(data));
                        }
                        catch (Exception)
                        {
                            Stop();
                        }
                    }
                });
                nekoSocketClient.Start();
            }
            internal void Stop()
            {
                GetNetworkStream()?.Close();
                GetTcpClient()?.Close();
            }
            public void SendRawMessage(byte[] data)
            {
                GetNetworkStream()?.Write(data, 0, data.Length);
                GetNetworkStream()?.Flush();
            }
        }
    }
}