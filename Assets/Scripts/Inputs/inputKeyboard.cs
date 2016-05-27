using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using fr.unice.miage.og.selector;
using fr.unice.miage.og.actions;
using System;
using System.Text;

namespace fr.unice.miage.og.flux
{
    public class InputKeyboard : Input
    {
        public float rotateSpeed = 10;
        public float translateSpeed = 25;
        public GameObject instance;

        private GameObject select;
        private Node nodeCourant;
        
        private MoveAction move;
        private RenameAction renameAction;
        
        private bool deleteMode;
        private bool writeMode;
        private TextMesh textMesh;

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
            if (writeMode) {
                updateWriteMode(ref e);
                print("writemode");
                return;
            }
            // Rename Action 
            else if (UnityEngine.Input.GetKey(KeyCode.F2))
            {
                textMesh = select.GetComponentInChildren<TextMesh>();
                renameAction = new RenameAction(textMesh);
                writeMode = true;
            }
            // Action avec la touche "Controle"
            else if (UnityEngine.Input.GetKey(KeyCode.LeftControl)) {
                // Selection des objets avec les fleches directionnelles
                InputLeftControlAction();
            }
            // Action sans la touche "Controle"
            else { 
                // A: AddAction
                if (UnityEngine.Input.GetKeyDown(KeyCode.A))
                {
                    //Create object 
                    //GameObject subject = Instantiate(Resources.Load("Prefab/Sub_Subject")) as GameObject;
                    //AddAction addAction = new AddAction("Prefab/Sub_subject", Vector3.zero, Quaternion.identity);
                    AddAction addAction = new AddAction(PrimitiveType.Cube, new Vector3(0, 0, -8), Quaternion.identity);
                    base.managerListener.doAction(addAction);
                    print("action: add");

                    this.nodeCourant.Add(new Node(addAction.GameObject));
                }
                // D: RemoveAction
                else if (UnityEngine.Input.GetKeyDown(KeyCode.D) && !deleteMode)
                {
                    // Remove Action
                    /*RemoveAction removeAction = new RemoveAction(this.select);
                    base.managerListener.doAction(removeAction);
                    print("action: remove");

                    // Supprimer de l'arbre pour ne plus pouvoir le selectionner
                    this.nodeCourant = this.nodeCourant.Remove();
                    */

                    // Remove Action
                    RemoveAction removeAction = new RemoveAction(ref this.nodeCourant);
                    base.managerListener.doAction(removeAction);
                    print("action: remove");

                    this.nodeCourant = removeAction.NodeCourant;

                    // Selectionner le precedent
                    if (this.nodeCourant == null) {
                        Select(instance);
                    } else {
                        Select(nodeCourant.Gameobject);
                    }
                    deleteMode = true;
                }
                else if (UnityEngine.Input.GetKeyUp(KeyCode.D) && deleteMode)
                {
                    deleteMode = false;
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
                // cas special : user monte alors que Center ROOT select
                if (this.nodeCourant.GetParent().GetSize() != 0)
                {
                    Select(this.nodeCourant.GetParent().list[0].Gameobject);
                    // Remonter dans l'arbre
                    this.nodeCourant = this.nodeCourant.GetParent();
                }
            }
            // Selectionner le compo FILS
            if (UnityEngine.Input.GetKey(KeyCode.DownArrow))
            {
                // cas special : user descend alors que Center ROOT n'a pas de fils
                if(this.nodeCourant.GetSon() != null) {
                    Select(this.nodeCourant.GetSon().list[0].Gameobject);
                    // Descendre dans l'arbre
                    this.nodeCourant = this.nodeCourant.GetSon();
                }
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

        private void updateWriteMode(ref Event e) {
            // touche Escape pour valider et sortir de la selection
            if (UnityEngine.Input.GetKey(KeyCode.Escape)) {
                renameAction.NewName = this.textMesh.text;
                base.managerListener.doAction(renameAction);

                // quitte le mode ecriture
                this.writeMode = false;
                this.textMesh = null;                
                return;
            }

            StringBuilder textPanel = new StringBuilder(this.textMesh.text);
            
            // touche effacer pour supprimer le dernier caractere
            if (UnityEngine.Input.GetKey(KeyCode.Backspace)) {
                textPanel.Length = (textPanel.Length > 0) ? textPanel.Length - 1 : 0;
            }
            // touche supp pour supprimer tout le texte
            if (UnityEngine.Input.GetKey(KeyCode.Delete))
            {
                textPanel.Length = 0;
            }
            
            // recupere le char du clavier
            char c = e.character;
            textPanel.Append(c);

            //update the text panel
            this.textMesh.text = textPanel.ToString();
        }
    }
    
}
