/**
* UniMove API - A Unity plugin for the PlayStation Move motion controller
* Copyright (C) 2012, 2013, Copenhagen Game Collective (http://www.cphgc.org)
* 					         Patrick Jarnfelt
* 					         Douglas Wilson (http://www.doougle.net)
*
*
* All rights reserved.
*
* Redistribution and use in source and binary forms, with or without
* modification, are permitted provided that the following conditions are met:
*
*    1. Redistributions of source code must retain the above copyright
*       notice, this list of conditions and the following disclaimer.
*
*    2. Redistributions in binary form must reproduce the above copyright
*       notice, this list of conditions and the following disclaimer in the
*       documentation and/or other materials provided with the distribution.
*
* THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
* AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
* IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
* ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE
* LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
* CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
* SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
* INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
* CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
* ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
* POSSIBILITY OF SUCH DAMAGE.
**/

using UnityEngine;
using System;
using System.Collections.Generic;
using fr.unice.miage.og.actions;
using System.Text;

namespace fr.unice.miage.og.flux
{
    public class InputPsMove : Input
    {
        private readonly String ALPHABET = " abcdefghijklmnopqrstuvwxyz0123456789";

        private readonly float VISUALISATION_DIST = -10;

        private Material CUBE_MATERIAL;

        // This is the (3d object prototype in the scene)
        private GameObject moveControllerPrefab;

        // We save a list of Move controllers.
        private List<UniMoveController> moves = new List<UniMoveController>();
        // This is a list of graphical representations of move controllers (3d object)
        private List<MoveController> moveObjs = new List<MoveController>();

        //Object Selection
        private GameObject collisionObject;

        //Visualisation mode
        private bool visualisationMode;
        private Vector3 positionSave;
        private Quaternion rotationSave;

        //Write mode
        private bool writeMode;
        private TextMesh textMesh;
        private int indexAlphabet;
        private RenameAction renameAction;

        //Actions
        private MoveAction moveAction;

        void Start()
        {
            //Load DATA
            CUBE_MATERIAL  = Resources.Load("Materials/color") as Material;

            /* NOTE! We recommend that you limit the maximum frequency between frames.
		     * This is because the controllers use Update() and not FixedUpdate(),
		     * and yet need to update often enough to respond sufficiently fast.
		     * Unity advises to keep this value "between 1/10th and 1/3th of a second."
		     * However, even 100 milliseconds could seem slightly sluggish, so you
		     * might want to experiment w/ reducing this value even more.
		     * Obviously, this should only be relevant in case your framerare is starting
		     * to lag. Most of the time, Update() should be called very regularly.
		     */
            Time.maximumDeltaTime = 0.1f;

            moveControllerPrefab = GameObject.Find("MoveController");
            Destroy(moveControllerPrefab);
            if (moveControllerPrefab == null || moveControllerPrefab.GetComponent<MoveController>() == null)
                Debug.LogError("GameObject with object named \"MoveController\" with script MoveController is missing from the scene");



            int count = UniMoveController.GetNumConnected();

            // Iterate through all connections (USB and Bluetooth)
            for (int i = 0; i < count; i++)
            {
                UniMoveController move = gameObject.AddComponent<UniMoveController>();  // It's a MonoBehaviour, so we can't just call a constructor


                // Remember to initialize!
                if (!move.Init(i))
                {
                    Destroy(move);  // If it failed to initialize, destroy and continue on
                    continue;
                }

                // This example program only uses Bluetooth-connected controllers
                PSMoveConnectionType conn = move.ConnectionType;
                if (conn == PSMoveConnectionType.Unknown || conn == PSMoveConnectionType.USB)
                {
                    Destroy(move);
                }
                else
                {
                    moves.Add(move);

                    
                    move.OnControllerDisconnected += HandleControllerDisconnected;

                    move.InitOrientation();
                    move.ResetOrientation();

                    // Start all controllers with a white LEDC:\Users\Seb\Desktop\Projet_oculus_miage\Oculus-Rift-Outil-de-gestion\Assets\Scripts\Inputs\inputPsMove.cs
                    move.SetLED(Color.white);

                    // adding the MoveController Objects on screen
                    GameObject moveController = GameObject.Instantiate(moveControllerPrefab,
                        Vector3.right * count * 2 + Vector3.left * i * 4, Quaternion.identity) as GameObject;
                    MoveController moveObj = moveController.GetComponent<MoveController>();
                    moveObjs.Add(moveObj);
                    moveObj.SetLED(Color.white);

                }
            }
        }


        void Update()
        {
            for (int i = 0; i < moves.Count; i++)
            {
                UniMoveController move = moves[i];
                MoveController moveObj = moveObjs[i];

                // Instead of this somewhat kludge-y check, we'd probably want to remove/destroy
                // the now-defunct controller in the disconnected event handler below.
                if (move.Disconnected) continue;



                if (writeMode) {
                    updateWriteMode(ref moveObj, ref move);
                    return;
                }


                // Change the colors of the LEDs based on which button has just been pressed:
                if (move.GetButtonDown(PSMoveButton.Circle)) {
                    moveObj.SetLED(Color.cyan); move.SetLED(Color.cyan);

                    if (this.collisionObject != null && this.collisionObject.GetComponentInChildren<TextMesh>() != null ) {
                        textMesh = this.collisionObject.GetComponentInChildren<TextMesh>();
                        textMesh.text += " "; // the space to show the choice letter
                        renameAction = new RenameAction(textMesh);
                        writeMode = true;
                    }

                    
                }
                else if (move.GetButtonDown(PSMoveButton.Cross)) {
                    moveObj.SetLED(Color.red); move.SetLED(Color.red);

                    //Remove object
                    if (this.collisionObject != null) {
                        RemoveAction removeAction = new RemoveAction(this.collisionObject);
                        base.managerListener.doAction(removeAction);
                        deselectCollisionObject();
                    }
                    

                }
                else if (move.GetButtonDown(PSMoveButton.Square)) {
                    moveObj.SetLED(Color.yellow); move.SetLED(Color.yellow);

                    if (collisionObject != null) {

                        //Create object 
                        AddAction addAction = new AddAction("Prefab/Panel", new Vector3(0, 0, -8));
                        base.managerListener.doAction(addAction);
                        GameObject objectCreated = addAction.GameObject;

                        //create tuve
                        GameObject tubeObject = GameObject.Instantiate(Resources.Load("Prefab/Tube") as GameObject);
                        TubeArrow tube = tubeObject.GetComponent<TubeArrow>();
                        tube.link(collisionObject, objectCreated);
                    }

                }
                else if (move.GetButtonDown(PSMoveButton.Triangle))
                {
                    moveObj.SetLED(Color.magenta); move.SetLED(Color.magenta);

                    if (this.collisionObject != null && !visualisationMode)
                    {
                        visualisationMode = true;
                        positionSave = this.collisionObject.transform.localPosition;
                        rotationSave = this.collisionObject.transform.localRotation;

                        Vector3 location = this.collisionObject.transform.localPosition;
                        location.Set(moveObj.transform.localPosition.x, moveObj.transform.localPosition.y, VISUALISATION_DIST);
                        this.collisionObject.transform.localPosition = location;
                    }
                    else if (this.collisionObject != null)
                    {
                        visualisationMode = false;
                        this.collisionObject.transform.localPosition = positionSave;
                        this.collisionObject.transform.localRotation = rotationSave;
                        deselectCollisionObject();
                    }
                }
                else if (move.GetButtonDown(PSMoveButton.PS))
                {
                    //Reset orientation
                    move.ResetOrientation();
                    moveObj.SetLED(Color.black);
                    move.SetLED(Color.black);
                }
                else if (move.GetButtonDown(PSMoveButton.Start)) {
                    base.managerListener.redoAction();
                }
                else if (move.GetButtonDown(PSMoveButton.Select))
                {
                    base.managerListener.undoAction();
                }

                // Set the rumble based on how much the trigger is down
                moveObj.gameObject.transform.localRotation = new Quaternion(move.Orientation.x, -move.Orientation.y, -move.Orientation.z, move.Orientation.w);


                if (visualisationMode)
                {
                    visualisationModeUpdate(ref moveObj);
                }
                else
                {
                    rayUpdate(ref move, ref moveObj);
                }

            }
        }

        private void updateWriteMode(ref MoveController moveObj, ref UniMoveController move) {
            //update psmove rotation
            moveObj.gameObject.transform.localRotation = new Quaternion(move.Orientation.x, -move.Orientation.y, -move.Orientation.z, move.Orientation.w);

            if ( move.GetButtonDown(PSMoveButton.Circle) ) {
                //We remove the last letter because it's only a letter to show the next letter to add.
                String newText = this.textMesh.text.Substring(0, this.textMesh.text.Length - 1);
                renameAction.NewName = newText;
                base.managerListener.doAction(renameAction);

                this.writeMode = false;
                this.textMesh = null;
                deselectCollisionObject();
                return;
            }

            StringBuilder textPanel = new StringBuilder(this.textMesh.text);

            if (move.GetButtonDown(PSMoveButton.Square))
            {
                // remove last letter
                textPanel.Length = (textPanel.Length > 1) ? textPanel.Length - 1 : 1;
            }
            else if (move.GetButtonDown(PSMoveButton.Triangle))
            {
                // add letter
                textPanel.Append(ALPHABET[indexAlphabet]);
            }
            else {
                //change last letter
                float z = moveObj.transform.rotation.eulerAngles.z;
                int step = 360 / ALPHABET.Length;
                this.indexAlphabet = (int) (z / step);

                if (indexAlphabet >= ALPHABET.Length) {
                    indexAlphabet = 0;
                }
                textPanel[textPanel.Length - 1] = ALPHABET[indexAlphabet];
            }

            //update the text panel
            this.textMesh.text = textPanel.ToString();

        }

        private void visualisationModeUpdate(ref MoveController moveObj)
        {
            this.collisionObject.transform.localRotation = moveObj.gameObject.transform.localRotation;
        }

        private void rayUpdate(ref UniMoveController move, ref MoveController moveObj)
        {

            Transform sphereTransform = moveObj.transform.Find("Sphere");
            Transform capsuleTransform = moveObj.transform.Find("Capsule");
            Vector3 heading = sphereTransform.position - capsuleTransform.transform.position;
            Vector3 direction = heading.normalized;
            RaycastHit hit;
            Ray ray = new Ray(sphereTransform.position, direction);

            Boolean haveCollision = Physics.Raycast(ray, out hit);
            if (haveCollision && move.GetButtonDown(PSMoveButton.Trigger))
            {
                GameObject obj = hit.collider.gameObject;
                selectCollisionObject(ref obj);
                this.moveAction = new MoveAction(obj);
            }

            if (move.GetButtonUp(PSMoveButton.Trigger))
            {
                //set the position
                if (this.collisionObject != null) { 
                    Vector3 position = this.collisionObject.transform.localPosition;
                    this.moveAction.NewLocation = new Vector3(position.x, position.y, position.z);
                    base.managerListener.doAction(this.moveAction);
                }

                deselectCollisionObject();
            }

            if (this.collisionObject != null)
            {
                float distance = Vector3.Distance(moveObj.transform.position, this.collisionObject.transform.position);
                float z = this.collisionObject.transform.localPosition.z;
                Vector3 location = moveObj.transform.position + direction * distance;
                location.z = z;
                this.collisionObject.transform.localPosition = location;
            }

            Debug.DrawRay(sphereTransform.position, direction * 10, Color.green);
        }

        private void selectCollisionObject(ref GameObject collisionObject)
        {
            this.collisionObject = collisionObject;
            Renderer r = this.collisionObject.GetComponent<Renderer>();
            r.material.SetColor("_Color", Color.blue);
        }

        private void deselectCollisionObject()
        {
            if (this.collisionObject == null)
            {
                return;
            }

            Renderer r = this.collisionObject.GetComponent<Renderer>();
            r.material.SetColor("_Color", Color.gray);
            this.collisionObject = null;
        }

        void HandleControllerDisconnected(object sender, EventArgs e)
        {
            // TODO: Remove this disconnected controller from the list and maybe give an update to the player
        }

        void OnGUI()
        {
            string display = "";

            if (moves.Count > 0)
            {
                for (int i = 0; i < moves.Count; i++)
                {
                    display += string.Format("PS Move {0}: ax:{1:0.000}, ay:{2:0.000}, az:{3:0.000} gx:{4:0.000}, gy:{5:0.000}, gz:{6:0.000}\n",
                        i + 1, moves[i].Acceleration.x, moves[i].Acceleration.y, moves[i].Acceleration.z,
                        moves[i].Gyro.x, moves[i].Gyro.y, moves[i].Gyro.z);
                }
            }
            else display = "No Bluetooth-connected controllers found. Make sure one or more are both paired and connected to this computer.";

            GUI.Label(new Rect(10, Screen.height - 100, 500, 100), display);
        }
    }
}