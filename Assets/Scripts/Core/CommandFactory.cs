using System;
public static class CommandFactory
{
    public static ICommand Create(string type)
    {
        switch(type)
        {
            case "Forward":   return new ForwardCommand();
            case "TurnRight": return new TurnRightCommand();
            case "TurnLeft":  return new TurnLeftCommand();
            default:
                throw new ArgumentException($"Unknown command: {type}");
        }
    }
}
