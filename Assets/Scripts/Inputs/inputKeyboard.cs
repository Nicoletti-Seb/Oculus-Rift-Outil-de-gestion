using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using fr.unice.miage.og.selector;
using fr.unice.miage.og.actions;

namespace fr.unice.miage.og.flux
{
    public class InputKeyboard : Input
    {
        public float rotateSpeed = 10;
        public float translateSpeed = 10;
        public GameObject instance;

        private GameObject select;
        private Node nodeCourant;
        private MoveAction move;
        private RenameAction rename;
        private bool delete;

        public void Start() {
            // creer et construire l'arbre
            this.nodeCourant = new Node(instance);
            this.nodeCourant.Recursivite();

            // selectionner le centre
            Select(instance);
        }

        public void OnGUI()
        {
            Event ev = Event.current;
            if (ev.isKey)
            {
                onKeyboardEvent(ref ev);
            }
        }

        public void onKeyboardEvent(ref Event e)
        {
            // Rename Action 
            if (UnityEngine.Input.GetKey(KeyCode.F2))
            {
                this.rename = new RenameAction();
                this.select.GetComponentInChildren<TextMesh>();
            }
            // Sinon
            else if(this.rename == null) {
                // Action avec la touche "Controle"
                if (UnityEngine.Input.GetKey(KeyCode.LeftControl))
                {
                    // Selection des objets avec les fleches directionnelles
                    InputLeftControlAction();
                }
                // Sans la touche "Controle"
                // Les racourcis
                else
                {
                    // A: AddAction
                    if (UnityEngine.Input.GetKeyDown(KeyCode.A))
                    {
                        //Create object 
                        //GameObject subject = Instantiate(Resources.Load("Prefab/Sub_Subject")) as GameObject;
                        AddAction addAction = new AddAction("Prefab/Sub_subject", Vector3.zero, Quaternion.identity);
                        base.managerListener.doAction(addAction);
                        print("action: add");
                    }
                    // D: RemoveAction
                    else if (UnityEngine.Input.GetKeyDown(KeyCode.D) && !delete)
                    {
                        // Remove Action
                        RemoveAction removeAction = new RemoveAction(this.select);
                        base.managerListener.doAction(removeAction);
                        print("action: remove");

                        // Supprimer de l'arbre pour ne plus pouvoir le selectionner
                        this.nodeCourant = this.nodeCourant.Remove();

                        // Selectionner le precedent
                        Select(nodeCourant.Gameobject);
                        delete = true;
                    }
                    else if (UnityEngine.Input.GetKeyUp(KeyCode.D) && delete)
                    {
                        delete = false;
                    }
                    // U: UndoAction
                    else if (UnityEngine.Input.GetKeyDown(KeyCode.U))
                    {
                        base.managerListener.undoAction();
                        print("action: undo");
                    }
                    // R: RedoAction
                    else if (UnityEngine.Input.GetKeyDown(KeyCode.R))
                    {
                        base.managerListener.redoAction();
                        print("action: redo");
                    }
                    // Deplacer un objet selectionne avec les fleches directionnelles
                    else {
                        InputMoveAction();
                    }
                }
            }
        }

        private void Select(GameObject gameobject) {
            if (this.select != null) {
                // mettre en BLANC le compo precedent
                MeshRenderer meshRendererBefore = this.select.GetComponent<MeshRenderer>();
                meshRendererBefore.material.color = Color.white;
            }

            // MAJ du compo select
            this.select = gameobject;
            // mettre en VERT le compo select
            MeshRenderer meshRendererAfter = this.select.GetComponent<MeshRenderer>();
            meshRendererAfter.material.color = Color.green;
        }

        private void InputLeftControlAction() {
            // Selectionner le ROOT
            if (UnityEngine.Input.GetKey(KeyCode.M))
            {
                Select(instance);
            }
            // Selectionner le compo PRECEDENT
            if (UnityEngine.Input.GetKey(KeyCode.LeftArrow))
            {
                Select(this.nodeCourant.Previous().Gameobject);
            }
            // Selectionner le compo SUIVANT
            if (UnityEngine.Input.GetKey(KeyCode.RightArrow))
            {
                Select(this.nodeCourant.Next().Gameobject);
            }
            // Selectionner le compo PARENT
            if (UnityEngine.Input.GetKey(KeyCode.UpArrow))
            {
                Select(this.nodeCourant.GetParent().list[0].Gameobject);
                // Remonter dans l'arbre
                this.nodeCourant = this.nodeCourant.GetParent();
            }
            // Selectionner le compo FILS
            if (UnityEngine.Input.GetKey(KeyCode.DownArrow))
            {
                Select(this.nodeCourant.GetSon().list[0].Gameobject);
                // Descendre dans l'arbre
                this.nodeCourant = this.nodeCourant.GetSon();
            }
        }

        private void InputMoveAction() {
            if (UnityEngine.Input.GetKey(KeyCode.LeftArrow))
            {
                if (move == null)
                    move = new MoveAction(this.select);
                this.select.transform.position += Vector3.left * translateSpeed * Time.deltaTime;
            }
            if (UnityEngine.Input.GetKey(KeyCode.RightArrow))
            {
                if (move == null)
                    move = new MoveAction(this.select);
                this.select.transform.position += Vector3.right * translateSpeed * Time.deltaTime;
            }
            if (UnityEngine.Input.GetKey(KeyCode.UpArrow))
            {
                if (move == null)
                    move = new MoveAction(this.select);
                this.select.transform.position += Vector3.up * translateSpeed * Time.deltaTime;
            }
            if (UnityEngine.Input.GetKey(KeyCode.DownArrow))
            {
                if (move == null)
                    move = new MoveAction(this.select);
                this.select.transform.position += Vector3.down * translateSpeed * Time.deltaTime;
            }
            if (!UnityEngine.Input.GetKeyUp(KeyCode.UpArrow) && !UnityEngine.Input.GetKeyUp(KeyCode.DownArrow) &&
                !UnityEngine.Input.GetKeyUp(KeyCode.RightArrow) && !UnityEngine.Input.GetKeyUp(KeyCode.LeftArrow) &&
                this.move != null)
            {

                Vector3 position = this.select.transform.localPosition;
                this.move.NewLocation = new Vector3(position.x, position.y, position.z);
                base.managerListener.doAction(move);
                move = null;
                print("moveaction");
            }
        }
    }
    
}
