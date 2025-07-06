namespace Quoridor.Command
{
    public interface ICommandInvoker
    {
        public void ExecuteCommand(ICommand command);

        public void UndoLastCommand();
        
        public void Initialize();
    }
}