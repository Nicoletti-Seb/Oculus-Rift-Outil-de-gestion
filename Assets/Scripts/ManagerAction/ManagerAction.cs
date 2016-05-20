using fr.unice.miage.og.actions;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

namespace fr.unice.miage.og.Managers
{
    /**
        This class allow to manage actions users. 
    */
    public class ManagerAction : ManagerListener
    {

        private int countUndo;

        private LinkedList<UserAction> actionList;
        public List<UserAction> ActionList { get { return actionList.ToList(); } }

        private LinkedList<UserAction> actionUndoList;
        public List<UserAction> ActionUndoList { get { return actionUndoList.ToList(); } }

        //Singleton
        private static readonly ManagerAction instance = new ManagerAction();

        public ManagerAction() {
            actionList = new LinkedList<UserAction>();
            actionUndoList = new LinkedList<UserAction>();
        }

        public static ManagerAction getInstance() {
            return instance;
        }

        //Methods
        public void doAction(UserAction userAction)
        {
            userAction.doAction();
            actionList.AddFirst(userAction);

            //cancel old actions
            if (countUndo > 0)
            {
                countUndo = 0;
                actionUndoList.Clear();
            }
        }

        public void undoAction()
        {
            if (actionList.Count <= 0)
            {
                return;
            }

            //pop the first element in actionList
            UserAction action = actionList.First();
            actionList.RemoveFirst();

            //action undo
            action.undo();
            actionUndoList.AddFirst(action);

            countUndo++;
        }

        public void redoAction()
        {
            if (actionUndoList.Count <= 0) {
                return;
            }

            //pop the first element in undo list
            UserAction action = actionUndoList.First();
            actionUndoList.RemoveFirst();

            //action redo
            action.doAction();
            actionList.AddFirst(action);

            countUndo--;
        }
    }
}
