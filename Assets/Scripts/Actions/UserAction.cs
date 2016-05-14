using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Actions
{
    interface UserAction
    {
        void doAction(GameObject g);
        void undo(GameObject g);
    }
}
