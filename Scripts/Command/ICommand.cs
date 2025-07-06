namespace Quoridor.Command
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}