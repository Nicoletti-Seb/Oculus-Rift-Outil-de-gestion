using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace fr.unice.miage.og.actions
{
    public class RenameAction : UserAction
    {
        private String newName;
        public String NewName { get{return newName;} set{newName = value;} }

        private String oldName;
        public String OldName { get { return newName; } }

        private TextMesh textMesh;

        public RenameAction(TextMesh textMesh) {
            this.textMesh = textMesh;
            this.oldName = textMesh.text;
        }

        public void doAction()
        {
            this.textMesh.text = newName;
        }

        public void undo()
        {
            this.textMesh.text = oldName;
        }
    }
}
