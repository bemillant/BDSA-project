namespace GitInsight;

using static Modes;
public class Control
{
    private Data? _repo;
    private Modes _mode;
    private Boolean _isLoaded;
    public void Run()
    {
        var isRunning = true;
        _isLoaded = false;

        Console.WriteLine("Welcome to GitInsight. Type 'newpath' to continue. \n! - ! Remember to exit using the command 'exit' ! - !");
        while (isRunning)
        {
            var input = Console.ReadLine();
            switch (input)
            {
                case "author":
                    SetMode(Modes.AUTHOR);
                    Console.WriteLine("Current Mode: AUTHOR");
                    break;
                case "frequency":
                    SetMode(Modes.FREQUENCY);
                    Console.WriteLine("Current Mode: FREQUENCY");
                    break;
                case "print":
                    if (!_isLoaded)
                    {
                        Console.WriteLine("no dataset loaded");
                    }
                    else
                    {
                        _repo!.Print(_mode);
                    }
                    break;
                case "newpath":
                    Console.WriteLine("Insert path:");
                    var path = Console.ReadLine();
                    Console.WriteLine("--- Loading Repository ---");
                    _repo = new Data(path!);
                    Console.WriteLine("--- Repository has been loaded ---");
                    _isLoaded = true;
                    break;
                case "help":
                    Console.WriteLine("'newpath' to set new repository path.");
                    Console.WriteLine("'author' to switch to AUTHOR mode.");
                    Console.WriteLine("'frequency' to switch to FREQUENCY mode.");
                    Console.WriteLine("'print' to print repository details.");
                    Console.WriteLine("'exit' to exit program");
                    break;
                case "exit":
                    _repo!.Shutdown();
                    isRunning = false;
                    break;
                default:
                    Console.WriteLine("Command doesn't exist. Type 'help' for commandlist");
                    break;
            }
        }
    }

    public Modes GetMode()
    {
        return _mode;
    }

    public void SetMode(Modes mode)
    {
        _mode = mode;
    }
}