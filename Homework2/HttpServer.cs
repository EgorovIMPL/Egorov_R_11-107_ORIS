using System.Net;
using System.Text;

namespace Homework2;

public enum ServerStatus
{
    Start,
    Stop
}

public class ServerSetting
{
    public readonly int Port = 7700;
    public readonly string Path = Directory.GetCurrentDirectory();
}

public class HttpServer : IDisposable
{
    public ServerStatus Status = ServerStatus.Stop;
    private readonly ServerSetting _serverSetting = new();
    private readonly HttpListener _httpListener;

    public HttpServer()
    {
        _httpListener = new HttpListener();
        _httpListener.Prefixes.Add("http://localhost:" + _serverSetting.Port + "/");
    }

    public void Start()
    {
        if (Status == ServerStatus.Start)
        {
            Console.WriteLine("Сервер уже запущен");
            return;
        }

        Console.WriteLine("Запуск сервера...");
        _httpListener.Start();
        Console.WriteLine("Сервер запущен");
        Status = ServerStatus.Start;
        Listening();
    }

    public void Stop()
    {
        if (Status == ServerStatus.Stop) return;
        Console.WriteLine("Остановка сервера...");
        _httpListener.Stop();
        Status = ServerStatus.Stop;
        Console.WriteLine("Сервер остановлен");
    }

    private void Listening()
    {
        _httpListener.BeginGetContext(ListenerCallBack, _httpListener);
    }

    private void ListenerCallBack(IAsyncResult result)
    {
        if (_httpListener.IsListening)
        {
            var response = _httpListener.EndGetContext(result).Response;

            byte[] buffer;
            if (File.Exists(_serverSetting.Path + "\\Site\\index.html"))
            {
                buffer = GetFile();

                if (buffer is null)
                {
                    response.Headers.Set("Content-Type", "text/plain");
                    response.StatusCode = (int) HttpStatusCode.NotFound;
                    var err = "404 - not found";
                    buffer = Encoding.UTF8.GetBytes(err);
                }
            }
            else
            {
                var err = "File not found";
                buffer = Encoding.UTF8.GetBytes(err);
            }

            var output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
            Listening();
        }
    }

    private byte[] GetFile()
    {
        var filePath = _serverSetting.Path + "\\Site\\index.html";
        byte[] bites;
        try
        {
            bites = File.ReadAllBytes(filePath);
        }
        catch (Exception e)
        {
            bites = null;
        }

        return bites;
    }

    public void Dispose()
    {
        Stop();
    }
}