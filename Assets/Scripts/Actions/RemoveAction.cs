using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using fr.unice.miage.og.selector;
using UnityEngine;

namespace fr.unice.miage.og.actions
{
    public class RemoveAction : UserAction
    {

        private GameObject gameObject;
        private Node node;
        private Node nodeCourant;
        private Node nodeSelect;

        public Node NodeCourant { get { return nodeCourant; } }

        public RemoveAction(GameObject gameObject) {
            this.gameObject = gameObject;
        }

        public RemoveAction(ref Node node) {
            this.nodeCourant = node;
            this.gameObject = node.GetCurrentNode().Gameobject;
        }

        public void doAction()
        {
            //Desactivated the object
            /**
                We can not destroy the object because we are an ref
                in Action Objects. So we desacived the object to used it after.
            **/
            this.gameObject.SetActive(false);
            this.nodeSelect = this.nodeCourant.GetCurrentNode();
            this.node = this.nodeCourant.Remove();
        }

        public void undo()
        {
            /* Probleme avec le nom du node 
               S'il est modifié, le undo remet l'ancien nom */

            this.gameObject.SetActive(true);
            this.nodeCourant.Add(this.nodeSelect);
        }
    }
}
