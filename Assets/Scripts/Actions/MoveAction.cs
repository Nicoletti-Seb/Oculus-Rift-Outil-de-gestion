using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace fr.unice.miage.og.actions
{
    public class MoveAction : UserAction
    {
        private GameObject obj;
        public GameObject ObjectUpdate { get { return obj; } }

        private Vector3 oldLocation;
        public Vector3 OldLocation { get { return oldLocation; } }

        private Vector3 newLocation;
        public Vector3 NewLocation { get { return newLocation; } set { newLocation = value; } }

        public MoveAction(GameObject obj) {
            this.obj = obj;
            this.oldLocation = new Vector3(obj.transform.localPosition.x,
                obj.transform.localPosition.y, obj.transform.localPosition.z);
        }

        public void doAction()
        {
            obj.transform.localPosition = newLocation;
        }

        public void undo()
        {
            obj.transform.localPosition = oldLocation;
        }
    }
}
