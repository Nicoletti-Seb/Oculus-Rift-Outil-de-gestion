using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class TubeArrow : MonoBehaviour {

    private readonly float CIRCLE_RADIUS_MAX = 2f * Mathf.PI;
    private readonly float WIDTH_POINTER_ARROW = 2.0f;

    private Mesh mesh;
    private List<Vector3> vertices = new List<Vector3>();

    public List<Vector3> points = new List<Vector3>();
    public float radius = 0.4f;
    public int nbPointCircle = 10;
    public Material material;

    public GameObject objSrc;
    public GameObject objDest;
    private GameObject pointerArrow;

    void Start() {

        if( pointerArrow == null && objDest != null) {
            pointerArrow = objDest.transform.Find("pointer").gameObject;
        }

        initPointCircle();

        foreach (Vector3 point in points) {
            createPointCircle(point);
        }

        initMesh();
        updateMesh();
    }

    private void initPointCircle() {
        createPointCircle(new Vector3(0, 0, 0));

        if (objSrc == null || objDest == null || !objSrc.activeSelf || !objDest.activeSelf) {
            return;
        }

        //Connect the Arrow
        float srcHalfWidth = objSrc.transform.localScale.x / 2f;
        float destHalfWidth = objDest.transform.localScale.x / 2f;
        Vector3 destPosition = this.objDest.transform.localPosition;
        Vector3 srcPosition = this.objSrc.transform.localPosition;

        Vector3 locationSrc= new Vector3(0, 0, srcPosition.z);
        Vector3 locationArrow = new Vector3(0, 0, srcPosition.z);
        Vector3 distCenterCircle = new Vector3(); // distance between the arrow pointer and the target
        Vector3 anglePointerArrow = new Vector3(); // change the pointer orientation 

        //Alert the x axis is inverse. ( positifs values are in the left side)
        if ( destPosition.x  > srcPosition.x )
        {//destination object is left
            locationArrow.x = destPosition.x - destHalfWidth - WIDTH_POINTER_ARROW;
            locationSrc.x = srcPosition.x + srcHalfWidth;
            distCenterCircle.x = locationArrow.x - locationSrc.x;
            anglePointerArrow.z = -90;
        }
        else{//destination object is right
            locationArrow.x = destPosition.x + destHalfWidth + WIDTH_POINTER_ARROW;
            locationSrc.x = srcPosition.x - srcHalfWidth;
            distCenterCircle.x = locationArrow.x - locationSrc.x;
            anglePointerArrow.z = 90;
        }

        locationSrc.y = srcPosition.y;
        locationArrow.y = destPosition.y;
        distCenterCircle.y = destPosition.y - locationSrc.y;

        //update
        pointerArrow.transform.localEulerAngles = anglePointerArrow;
        pointerArrow.transform.position = locationArrow;
        this.transform.position = locationSrc;
        createPointCircle(distCenterCircle);
    }

    private void initMesh() {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null) {
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }
        meshRenderer.material = material;

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }
        meshFilter.mesh = new Mesh();
        mesh = meshFilter.sharedMesh;
    }

    private void updateMesh() {
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = calculateTriangles();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.Optimize();
    }

    private int[] calculateTriangles() {
        List<int> triangles = new List<int>();
        int nbCircles = vertices.Count / nbPointCircle;
        int indexMaxLoop = nbCircles - 1;
        for (int i = 0; i < indexMaxLoop; i++)
        {
            int decalage = i * nbPointCircle;
            for( int j = 0; j < nbPointCircle; j++) { 
                int p1 = decalage + j;
                int p2 = decalage + nbPointCircle + j;
                int p3 = decalage + (j + 1) % nbPointCircle;
                int p4 = decalage + nbPointCircle + (j + 1) % nbPointCircle;

                //first triangle
                triangles.Add(p2);
                triangles.Add(p1);
                triangles.Add(p3);

                //second triangle
                triangles.Add(p3);
                triangles.Add(p4);
                triangles.Add(p2);

            }
        }
        return triangles.ToArray();
    }

    private void createPointCircle(Vector3 center) {
        float angleIncr = CIRCLE_RADIUS_MAX / nbPointCircle;
        float angle = 0;
        for (int i = 0; i < nbPointCircle; i++, angle += angleIncr) {
            float z = center.z + radius * Mathf.Cos(angle);
            float y = center.y + radius * Mathf.Sin(angle);
            float x = center.x;
            vertices.Add(new Vector3(x, y, z));
        }

    }

    public void link(GameObject objSrc, GameObject objDest) {
        this.objSrc = objSrc;
        this.objDest = objDest;
    }

#if UNITY_EDITOR
    void Update()
    {
        if (mesh != null)
        {
            mesh.Clear();
        }

        vertices.Clear();
        Start();
    }
#endif
}
