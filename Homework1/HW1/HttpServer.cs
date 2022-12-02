using System.Net;
using System.Text;

public class HttpServer
{
    private HttpListener listener;
    public void Start()
    {
        listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:8888/connection/");
        listener.Start();
        Console.WriteLine("Ожидание подключений...");
        Processing();
        Console.Read();
    }
    
    private void Processing()
    {
        while(true)
        {
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            byte[] buffer = new byte[0];
            try
            {
                string responseString = File.ReadAllText(@"C:\Users\Impl\RiderProjects\Egorov_R_11-107_ORIS\Homework1\HW1\index.html");
                buffer = Encoding.UTF8.GetBytes(responseString);
            }
            catch (Exception e)
            {
                Stop();
                Console.WriteLine("Для перезапуска введите: Restart");
                Console.WriteLine("Для остановки введите: Stop");
                while (true)
                {
                    string input = Console.ReadLine();
                    if(input == "Restart")
                        Start();
                    else if(input == "Stop")
                        break;
                }
            }
            response.ContentLength64 = buffer.Length;
            Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }
    }
    public void Stop()
    {
        listener.Stop();
        Console.WriteLine("Обработка подключений завершена");
    }
}