using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace fr.unice.miage.og.actions
{
    public class RemoveAction : UserAction
    {

        private GameObject gameObject;


        public RemoveAction(GameObject gameObject) {
            this.gameObject = gameObject;
        }

        public void doAction()
        {
            //Desactivated the object
            /**
                We can not destroy the object because we are an ref
                in Action Objects. So we desacived the object to used it after. 
            **/
            this.gameObject.SetActive(false);
        }

        public void undo()
        {
            this.gameObject.SetActive(true);
        }
    }
}
