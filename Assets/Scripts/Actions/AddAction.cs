using System;
using UnityEngine;

namespace fr.unice.miage.og.actions
{
    public class AddAction : UserAction
    { 
        // build object
        private GameObject gameObject;
        public GameObject GameObject { get { return gameObject; } }

        private Vector3 position;
        public Vector3 Position { get { return position; } }

        private Material material;
        public Material Material { get { return material; } set { material = value; } }

        //to load the ressource
        private PrimitiveType? primitiveType;
        private String path;


        public AddAction(String path, Vector3 position) {
            this.path = path;
            this.position = position;
        }

        public AddAction(PrimitiveType primitiveType, Vector3 position)
        {
            this.primitiveType = primitiveType;
            this.position = position;
        }

        public void doAction()
        {
            // Create object only if not already create
            if (gameObject == null)
            {
                if (primitiveType != null)
                {
                    gameObject = GameObject.CreatePrimitive((PrimitiveType) primitiveType);
                }
                else {
                    gameObject = GameObject.Instantiate(Resources.Load(path) as GameObject);
                }

                gameObject.transform.localPosition = this.position;
                
            }
            else {
                //Reactived the object
                gameObject.SetActive(true);
            }

            //Set material
            if (material == null) {
                return;
            }

            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            if (meshRenderer == null)
            {
                meshRenderer = gameObject.AddComponent<MeshRenderer>();
            }
            meshRenderer.material = material;
        }

        public void undo()
        {
            //Desactivated the object
            /**
                We can not destroy the object because we are an ref
                in Action Objects. So we desacived the object to used it after. 
            **/
            gameObject.SetActive(false);
        }
    }
}
