using Assets.Scripts.Actions;
using System;
using UnityEngine;


namespace Assets.Scripts.ManagerAction
{
    public interface ManagerListener
    {
        void doAction(UserAction userAction);
        void undoAction();
        void redoAction();

    }
}