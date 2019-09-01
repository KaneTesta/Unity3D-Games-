using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMesh : MonoBehaviour
{

    public float width;
    public float waterLevel;

    public Shader shader;
    public GameObject Sun;

    public MeshRenderer rend;
    private int offset = 0;
    private MeshCollider collider;


    // Use this for initialization
    void Start() {
        //Render Shader
        collider = this.GetComponent<MeshCollider>();
        rend = this.gameObject.GetComponent<MeshRenderer>();
        rend = this.GetComponent<MeshRenderer>();
        rend.material.shader = shader;
    }

    // Update is called once per frame
    void Update() {
        collider.sharedMesh = this.GetComponent<MeshFilter>().mesh;
        rend.material.SetColor("_PointLightColor", Sun.GetComponent<SunOrbit>().color);
        rend.material.SetVector("_PointLightPosition", Sun.GetComponent<SunOrbit>().GetPosition());
    }

    public Mesh CreateWater() {
        Mesh m = new Mesh();
        int size = ((int)(width*2)*(int)(width*2))*2*3;
        Vector3[] vertices = new Vector3[size];
        int[] triangles = new int[size];
        Color[] colors = new Color[size];

        for (int i = (int)-width; i < width; i++){
            for (int j = (int) -width; j < width; j++) {
                vertices[offset] = new Vector3((float) i, waterLevel, (float) j);
                vertices[offset+1] = new Vector3((float) i, waterLevel, (float) j + 1);
                vertices[offset+2] = new Vector3((float) i + 1, waterLevel, (float) j + 1);
                vertices[offset+3] = new Vector3((float) i, waterLevel, (float) j);
                vertices[offset+4] = new Vector3((float) i + 1, waterLevel, (float) j + 1);
                vertices[offset+5] = new Vector3((float) i + 1, waterLevel, (float) j);
                offset += 6;
            }
        }


        for (int i = 0; i < vertices.Length; i++) {
            triangles[i] = i;
            colors[i] = (new Color(0.0f, 0.35f, 0.81f, 0.25f));
        }
        m.vertices = vertices;
        m.triangles = triangles;
        m.colors = colors;


        m.RecalculateBounds();
        m.RecalculateNormals();

        return m;
    }
}
