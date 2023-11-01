using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


class Program
{
    static async Task Main()
    {
        using (ClientWebSocket clientWebSocket = new ClientWebSocket())
        {
            Uri serverUri = new Uri("wss://socketsbay.com/wss/v2/1/demo/"); // WebSocket sunucu adresi

            try
            {
                await clientWebSocket.ConnectAsync(serverUri, CancellationToken.None);
                Console.WriteLine("WebSocket bağlantısı başarıyla kuruldu.");
                
                Console.Write("Mesaj Girin: ");

                ReceiveDataFromWebSocket(clientWebSocket);
                
                while (true)
                {
                   SendDataToWebSocket(clientWebSocket, "Doga: " + Console.ReadLine());
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"WebSocket bağlantısı sırasında bir hata oluştu: {ex.Message}");
            }
        }
    }
    
    static async Task ReceiveDataFromWebSocket(ClientWebSocket clientWebSocket)
    {
        byte[] buffer = new byte[1024]; // Alınacak verinin depolanacağı tampon boyutu
        
        while (clientWebSocket.State == WebSocketState.Open)
        {
            WebSocketReceiveResult result = await clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Close)
            {
                // WebSocket bağlantısı kapatılıyor, işlemi sonlandırabilirsiniz.
                await clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                break;
            }

            // Veriyi kullanma işlemi burada yapılabilir.
            string receivedData = Encoding.UTF8.GetString(buffer, 0, result.Count);
            Console.WriteLine($"Alınan veri: {receivedData}");
            


        }
    }
    
    static async Task SendDataToWebSocket(ClientWebSocket clientWebSocket, string dataToSend)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(dataToSend);
        await clientWebSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
    }

}