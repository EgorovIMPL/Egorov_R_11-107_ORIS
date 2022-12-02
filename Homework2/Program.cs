namespace Homework2;

internal class Program
{
    private static bool IsRunning = true;
    static void Main()
    {
        using (var server=new HttpServer())
        {
            server.Start();
            while (IsRunning)
                Handler(Console.ReadLine()?.ToLower(),server);
        }
    }
    static void Handler(string command, HttpServer server)
    {
        switch (command.ToLower())
        {
            case "status": 
                Console.WriteLine(server.Status.ToString()); 
                break;
            case "start":
                server.Start();
                break;
            case "restart":
                server.Stop();
                server.Start();
                break;
            case "stop":
                server.Stop();
                break;
            case "exit":
                IsRunning = false;
                break;
        }
    }
}


