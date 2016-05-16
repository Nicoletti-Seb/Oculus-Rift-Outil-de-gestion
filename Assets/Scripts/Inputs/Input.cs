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

        private List<ManagerListener> listeners;
        

        public void addListener(ManagerListener managerlistener)
        {
            listeners.Add(managerlistener);
        }

        public void removeListener(ManagerListener managerlistener)
        {
            listeners.Remove(managerlistener);
        }

        public void doAction(UserAction userAction) {
            foreach (ManagerListener ml in listeners) {
                ml.doAction(userAction);
            }
        }

        public void undo() {
            foreach (ManagerListener ml in listeners)
            {
                ml.undoAction();
            }
        }

        public void redo() {
            foreach (ManagerListener ml in listeners)
            {
                ml.redoAction();
            }
        }

    }
}
