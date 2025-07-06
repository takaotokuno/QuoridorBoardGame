using System.Collections.Generic;
using System.Linq;
using Quoridor.Player;
using Quoridor.Player.Status;

namespace Quoridor.Command
{
    public class ApplyStatusCommand : ICommand
    {
        private PlayerController _target;
        private List<IStatus> _waitingStatusList;
        private List<IStatus> _activeStatusList;
        private List<IStatus> _backupWaitingStatusList;
        private List<IStatus> _backupActiveStatusList;
        private List<IStatus> _backupExpiredStatusList;
        public ApplyStatusCommand(
            PlayerController target
            , List<IStatus> watingStatusList
            , List<IStatus> activeStatusList
        )
        {
            _target = target;

            _waitingStatusList = watingStatusList;
            _activeStatusList = activeStatusList;

            _backupWaitingStatusList = new List<IStatus>();
            _backupActiveStatusList = new List<IStatus>();
            _backupExpiredStatusList = new List<IStatus>();

            _backupWaitingStatusList.AddRange(_waitingStatusList);
            _backupActiveStatusList.AddRange(_activeStatusList.Where(status => status.RemainTurns > 0));
            _backupExpiredStatusList.AddRange(_activeStatusList.Where(status => status.RemainTurns == 0));
        }

        public void Execute()
        {
            _activeStatusList.AddRange(_waitingStatusList);
            _waitingStatusList.Clear();

            foreach(IStatus status in _activeStatusList)
            {
                if(status.RemainTurns == 0) status.Remove(_target);
                else status.Apply(_target);
            }
            _activeStatusList.RemoveAll(status => status.RemainTurns < 0);
        }

        public void Undo()
        {
            foreach(IStatus status in _backupWaitingStatusList) status.Remove(_target);
            _waitingStatusList.AddRange(_backupWaitingStatusList);

            foreach(IStatus status in _backupActiveStatusList) status.AdjustTurn(1);

            foreach(IStatus status in _backupExpiredStatusList)
            {
                status.Apply(_target);
                status.AdjustTurn(1);
            }
            _activeStatusList.AddRange(_backupExpiredStatusList);
        }
    }
}
