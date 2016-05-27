using System.Collections.Generic;
using UnityEngine;

namespace fr.unice.miage.og.selector
{
    public class Node
    {
        public GameObject gameobject;
        public Node parent;
        public List<Node> list;

        private int index;

        public GameObject Gameobject { get { return gameobject; } set { gameobject = value; } }
        public Node Parent { get { return parent; } set { parent = value; } }

        public Node(GameObject instance) {
            this.gameobject = instance;
            this.parent = null;
            this.list = new List<Node>();
        }

        public void Recursivite() {
            foreach (Transform t in this.gameobject.transform)
            {
                if (t.gameObject.name.Contains("Sub_subject"))
                {
                    Node nodefils = new Node(t.gameObject);
                    Add(nodefils);
                    nodefils.Recursivite();
                }
            }
        }

        public int GetSize() {
            return this.list.Count;
        }

        public void Add(Node node) {
            node.Parent = this;
            this.list.Add(node);
        }

        public Node Remove() {
            list.Remove(GetCurrentNode());
            this.index = 0;
            if(GetSize() != 0) {
                return this;
            } else {
                return GetParent();
            }
        }
        
        public Node GetCurrentNode() {
            if (GetSize() == 0)
            {
                return this;
            }
            else {
                return this.list[index];
            }
        }

        public Node Next() {
            if(index >= GetSize() - 1) {
                index = 0;
            } else {
                index++;
            }
            return GetCurrentNode();
        }

        public Node Previous() {
            if (index == 0) {
                index = GetSize() - 1;
            }
            else {
                index--;
            }
            return GetCurrentNode();
        }

        public Node GetSon() {
            if(GetSize() == 0) {
                return Parent;
            }
            else if (this.list[index].GetSize() == 0)
            {
                return this;
            }
            else {
                return this.list[index];
            }
        }

        public Node GetParent() {
            if (this.parent == null)
            {
                return this;
            }
            else {
                return this.parent;
            }
        }
    }
}
