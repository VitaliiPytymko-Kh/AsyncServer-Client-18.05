using System.Net.Sockets;
using System.Net;
using System.Text;

namespace AsyncClient
{
    internal class Program
    {

        static async Task  Main(string[] args)
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var endPoint = new IPEndPoint(IPAddress.Loopback, 1234);
            try
            {
                Console.WriteLine("Client has started");
                await socket.ConnectAsync(endPoint);
                string message = "Привіт, сервере!";
                await socket.SendAsync(Encoding.UTF8.GetBytes(message), SocketFlags.None);

                var buf = new byte[1024];
                int bytesRead = await socket.ReceiveAsync(buf, SocketFlags.None);
                string responseMessage = Encoding.UTF8.GetString(buf, 0, bytesRead);
                Console.WriteLine($"o {DateTime.Now:HH:mm} " +
                                  $"від {socket?.RemoteEndPoint?.ToString()} " +
                                  $"отримано рядок : {responseMessage}");
                socket?.Shutdown(SocketShutdown.Both);
            }
            catch (SocketException se)
            {
                await Console.Out.WriteLineAsync($"SocketException:{se.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            finally
            {
                socket.Close();
            }
            Console.WriteLine("\nНатисніть будь-яку клавішу для завершення...");
            Console.ReadKey();
        }
    }
}

