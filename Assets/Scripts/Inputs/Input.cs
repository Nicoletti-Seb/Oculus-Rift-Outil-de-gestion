using fr.unice.miage.og.actions;
using fr.unice.miage.og.Managers;
using UnityEngine;

namespace fr.unice.miage.og.flux
{
    /**

        This class allow to represente an input.

    */
    public abstract class Input : MonoBehaviour
    {
        protected ManagerListener managerListener = ManagerAction.getInstance();

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
