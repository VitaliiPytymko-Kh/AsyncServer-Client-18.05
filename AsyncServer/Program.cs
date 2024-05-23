using System.Net.Sockets;
using System.Net;
using System.Text;

namespace AsyncServer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var endPoint = new IPEndPoint(IPAddress.Loopback, 1234);
            socket.Bind(endPoint);
            socket.Listen(10);
            Console.WriteLine("Server has started");

            try
            {

                while (true)
                {
                    var client = socket.Accept();
                    _ = HandleClientAsync(client);
                }
            }
            catch (SocketException se)
            {
                Console.WriteLine($"SocketException: {se.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            finally
            {
                socket.Close();
            }
         
        }

        private static async Task HandleClientAsync(Socket client)
        {
            var buf = new byte[1024];
            try
            {
                int bytesRead = await client.ReceiveAsync(buf, SocketFlags.None);
                string receivedMessage = Encoding.UTF8.GetString(buf, 0, bytesRead);
                Console.WriteLine($"o {DateTime.Now:HH:mm}" +
                                  $" від {client?.RemoteEndPoint?.ToString()}" +
                                  $" отримано рядок:{receivedMessage}");

                string responseMessage = "Привіт, клієнте! ";
                byte[] responseBytes = Encoding.UTF8.GetBytes(responseMessage);
                await client?.SendAsync(responseBytes, SocketFlags.None);
                client?.Shutdown(SocketShutdown.Both);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in HandleClientAsync: {ex.Message}");
            }

            finally { client?.Close(); }

         

        }
    }
}

        
          
        

    

