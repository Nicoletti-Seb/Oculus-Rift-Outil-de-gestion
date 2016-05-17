using fr.unice.miage.og.actions;


namespace fr.unice.miage.og.Managers
{
    public interface ManagerListener
    {
        void doAction(UserAction userAction);
        void undoAction();
        void redoAction();

    }
}