using System.Collections.Generic;

namespace Quoridor.Command
{
    public class PlaneCommandInvoker : ICommandInvoker
    {
        protected Stack<ICommand> _commandHistory;
        public PlaneCommandInvoker()
        => _commandHistory = new Stack<ICommand>();

        public virtual void ExecuteCommand(ICommand command)
        {
            command.Execute();
            _commandHistory.Push(command);
        }

        public virtual void UndoLastCommand()
        {
            if(_commandHistory.Count > 0) _commandHistory.Pop().Undo();
        }
        
        public virtual void Initialize() => _commandHistory.Clear();
    }
}