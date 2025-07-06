namespace Quoridor.Command
{
    public class EmptyCommand : ICommand
    {
        public EmptyCommand(){}
        
        public void Execute(){}

        public void Undo(){}
    }
}
