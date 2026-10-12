using AdoFilmLibrary.MenuStateMachine.StatePattern;

namespace AdoFilmLibrary;

public class Program {
    private static void Main(string[] args) {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        MenuStateContext.CreateInstance().StartContext();
    }
}