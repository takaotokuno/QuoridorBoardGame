using System.Collections.Generic;

namespace Quoridor.Command
{
    public class NormalCommandInvoker : PlaneCommandInvoker
    {
        private Stack<ICommand> _redoHistory;
        public NormalCommandInvoker() : base()
        => _redoHistory = new Stack<ICommand>();

        public override void ExecuteCommand(ICommand command)
        {
            base.ExecuteCommand(command);
            _redoHistory.Clear();
        }

        public override void UndoLastCommand()
        {
            if(_commandHistory.Count > 0)
            {
                ICommand command = _commandHistory.Pop();
                command.Undo();
                _redoHistory.Push(command);
            }
        }

        public void RedoLastCommand()
        {
            if(_redoHistory.Count > 0)
            {
                ICommand command = _redoHistory.Pop();
                command.Execute();
                _commandHistory.Push(command);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            _redoHistory.Clear();
        }
    }
}