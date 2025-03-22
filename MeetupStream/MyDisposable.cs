namespace MeetupStream;

public class MyDisposable : IDisposable
{
    private bool _disposed = false;

    public void DoSomething()
    {
        if (_disposed)
        { 
            throw new ObjectDisposedException(nameof(MyDisposable));
        }

        Console.WriteLine("Benutze Ressource...");
    }
    
    public void Dispose()
    {
        if (!_disposed)
        {
            Console.WriteLine("Ressourcen werden aufgeräumt..."); 
            _disposed = true;
        }
    }
}