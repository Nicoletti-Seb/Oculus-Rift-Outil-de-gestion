using Assets.Scripts.Actions;
using Assets.Scripts.ManagerAction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Inputs
{
    /**

        This class allow to represente an input.

    */
    public abstract class Input
    {

        private ManagerListener managerListener;

        public Input(ManagerListener managerListener)
        {
            this.managerListener = managerListener;
        }

        public void doAction(UserAction userAction) {
            managerListener.doAction(userAction);
        }

        public void undo() {
            managerListener.undoAction();
        }

        public void redo() {
            managerListener.redoAction();
        }

    }
}
