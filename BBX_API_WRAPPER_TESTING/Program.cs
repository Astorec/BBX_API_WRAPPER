namespace BBX_API_WRAPPER;

public class Program
{
    static async Task Main(string[] args)
    {
        var start = new Start();
        await start.StartTask();
    }
}
