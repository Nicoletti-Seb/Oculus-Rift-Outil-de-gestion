using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Actions;

namespace Assets.Scripts.ManagerAction
{
    /**
        This class allow to manage actions users. 
    */
    public class ManagerAction : ManagerListener
    {

        private LinkedList<UserAction> actionList;
        public List<UserAction> ActionList { get { return actionList.ToList(); } }

        private LinkedList<UserAction> actionUndoList;
        public List<UserAction> ActionUndoList { get { return actionUndoList.ToList(); } }

        public ManagerAction() {
            actionList = new LinkedList<UserAction>();
            actionUndoList = new LinkedList<UserAction>();
        }

        public void doAction(UserAction userAction)
        {
            userAction.doAction();
            actionList.AddFirst(userAction);
        }

        public void undoAction()
        {
            //pop the first element in actionList
            UserAction action = actionList.First();
            actionList.RemoveFirst();

            //action undo
            action.undo();
            actionUndoList.AddFirst(action);
        }

        public void redoAction()
        {
            //pop the first element in undo list
            UserAction action = actionUndoList.First();
            actionUndoList.RemoveFirst();

            //action redo
            action.doAction();
            actionList.AddFirst(action);
        }

    }
}
