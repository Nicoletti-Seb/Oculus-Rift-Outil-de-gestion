using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TubeArrow : MonoBehaviour {

    private readonly float CIRCLE_RADIUS_MAX = 2f * Mathf.PI;

    private Mesh mesh;
    private List<Vector3> vertices = new List<Vector3>();

    public List<Vector3> points = new List<Vector3>();
    public float radius = 0.4f;
    public int nbPointCircle = 10;
    public Material material;

    void Start () {
        createPointCircle(new Vector3(0, 0, 0));

        foreach (Vector3 point in points) {
            createPointCircle(point);
        }

        initMesh();
        updateMesh();
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
	
}
