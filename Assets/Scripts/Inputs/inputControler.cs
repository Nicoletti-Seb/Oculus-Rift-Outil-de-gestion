using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using fr.unice.miage.og.selector;
using fr.unice.miage.og.actions;
using System;
using System.Text;
namespace fr.unice.miage.og.flux
{
    public class InputControler : Input
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
			this.nodeCourant = new Node(instance);
			this.nodeCourant.Recursivite();
			Select(instance);
		}

		public void OnGUI()
		{
			Event ev = Event.current;
			if (ev.isKey)
			{
				onGamePadEvent(ref ev);
			}
		}

		public void onGamePadEvent(ref Event e)
		{
			if (writeMode) {
				updateWriteMode(ref e);
				print("writemode");
				return;
			}
			// Rename Action 
			else if (UnityEngine.Input.GetKey(KeyCode.Joystick1Button1)) // A button
			{
				textMesh = select.GetComponentInChildren<TextMesh>();
				renameAction = new RenameAction(textMesh);
				writeMode = true;
			}
			else if (UnityEngine.Input.GetKey(KeyCode.Joystick1Button6)) { // LT button
				InputLeftControlAction();
			}
			else { 
					if (UnityEngine.Input.GetKeyDown(KeyCode.Joystick1Button3)) // Y button
				{
					AddAction addAction = new AddAction(PrimitiveType.Cube, new Vector3(0, 0, -8));
					base.managerListener.doAction(addAction);
					print("action: add");

					this.nodeCourant.Add(new Node(addAction.GameObject));
				}
					else if (UnityEngine.Input.GetKeyDown(KeyCode.Joystick1Button2) && !deleteMode) // B button
				{
					RemoveAction removeAction = new RemoveAction(ref this.nodeCourant);
					base.managerListener.doAction(removeAction);
					print("action: remove");

					this.nodeCourant = removeAction.NodeCourant;

					if (this.nodeCourant == null) {
						Select(instance);
					} else {
						Select(nodeCourant.Gameobject);
					}
					deleteMode = true;
				}
					else if (UnityEngine.Input.GetKeyUp(KeyCode.Joystick1Button2) && deleteMode) // B button
				{
					deleteMode = false;
				}
					else if (UnityEngine.Input.GetKeyDown(KeyCode.Joystick1Button5)) // RB button
				{
					base.managerListener.undoAction();
					print("action: undo");
				}
					else if (UnityEngine.Input.GetKeyDown(KeyCode.Joystick1Button4)) // LB button
				{
					base.managerListener.redoAction();
					print("action: redo");
				}
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
			if (UnityEngine.Input.GetKey(KeyCode.Joystick1Button0)) // X button 
			{
				Select(instance);
			}
			// Selectionner le compo PRECEDENT
			if (UnityEngine.Input.GetAxis("Horizontal") == 1)
			{
				Select(this.nodeCourant.Previous().Gameobject);
			}
			// Selectionner le compo SUIVANT
			if (UnityEngine.Input.GetAxis("Horizontal") == -1)
			{
				Select(this.nodeCourant.Next().Gameobject);
			}
			// Selectionner le compo PARENT
			if (UnityEngine.Input.GetAxis("Vertical") == 1)
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
		    if (UnityEngine.Input.GetAxis("Vertical") == -1)
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
			// joystick pour deplacer un objet
			if (UnityEngine.Input.GetAxis("Horizontal") == 1)
			{
				if (move == null)
					move = new MoveAction(this.select);
				this.select.transform.position += Vector3.left * translateSpeed * Time.deltaTime;
			}
			if (UnityEngine.Input.GetAxis("Horizontal") == -1)
			{
				if (move == null)
					move = new MoveAction(this.select);
				this.select.transform.position += Vector3.right * translateSpeed * Time.deltaTime;
			}
			if (UnityEngine.Input.GetAxis("Vertical") == 1)
			{
				if (move == null)
					move = new MoveAction(this.select);
				this.select.transform.position += Vector3.up * translateSpeed * Time.deltaTime;
			}
			if (UnityEngine.Input.GetAxis("Vertical") == -1)
			{
				if (move == null)
					move = new MoveAction(this.select);
				this.select.transform.position += Vector3.down * translateSpeed * Time.deltaTime;
			}
			if (UnityEngine.Input.GetAxis("Vertical") != 1 && UnityEngine.Input.GetAxis("Vertical") != -1 &&
				UnityEngine.Input.GetAxis("Horizontal") != 1 && UnityEngine.Input.GetAxis("Horizontal") != -1 &&
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
			if (UnityEngine.Input.GetKey(KeyCode.Joystick1Button7)) {     // RT button
				renameAction.NewName = this.textMesh.text;
				base.managerListener.doAction(renameAction);
				this.writeMode = false;
				this.textMesh = null;                
				return;
			}
			StringBuilder textPanel = new StringBuilder(this.textMesh.text);
			if (UnityEngine.Input.GetKey(KeyCode.Joystick1Button8)) {    // back button
				textPanel.Length = (textPanel.Length > 0) ? textPanel.Length - 1 : 0;
			}
			if (UnityEngine.Input.GetKey(KeyCode.Joystick1Button9))     // start button
			{
				textPanel.Length = 0;
			}
			char c = e.character;
			textPanel.Append(c);
			this.textMesh.text = textPanel.ToString();
		}
	}
		
    }

