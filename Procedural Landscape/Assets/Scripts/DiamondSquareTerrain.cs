using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 

Kane Testa - 910748

References
https://en.wikipedia.org/wiki/Diamond-square_algorithm#Description
https://www.youtube.com/watch?v=1HV8GbFnCik&t=699s
https://www.youtube.com/watch?v=iG0Lpp0SQ7U&t=38s

 */

 
public class DiamondSquareTerrain : MonoBehaviour
{

    public int divisions; // Value must only be a power of 2
    public float width;
    public float height;
    private float lowestVertY;
    private float highestVertY;
    private float tempVertY;
    int vertNo;
    public float waterLevel;


    //Shader and Light
    public Shader shader;
    public GameObject Sun;
    public MeshRenderer rend;
    public GameObject Water;


    //Material Percentages
    // Percentages which are lower bound quota for the color. CAN TRY MAKE A GRADIENT BETWEEN THEM BY USING THE CLOSENESS TO EACH LOWER AND UPPER BOUND
    private float Snow = 1f;
    Color SnowCol =  new Color(1.0f,1.0f,1.0f,1.0f);
    private float Stone = 0.95f;
    Color StoneCol = new Color(0.866f,0.866f,0.866f, 1.0f);
    private float DarkGrass = 0.85f;
    Color DarkGrassCol = new Color(0.37f, 0.47f, 0.29f, 1.0f);
    private float Grass = 0.7f;
    Color GrassCol = new Color(0.7f, 0.78f, 0.44f, 1.0f); 
    private float Riverbank = 0.6f;
    Color SandCol = new Color(0.96f, 0.9f, 0.67f, 1.0f);

    private float Sand = 0.55f;
    Color RiverCol = new Color(0.33f, 0.47f, 0.74f, 1.0f);
    


    //Arrays
    Vector3[] verts;
    Color[] terrainVertColor;

    // Start is called before the first frame update
    void Start()
    {
        //Create Terrain and it's collider
        this.GetComponent<MeshCollider>().sharedMesh = CreateTerrain();
        
        //Render Shader
        rend = this.GetComponent<MeshRenderer>();
        rend.material.shader = shader;

    }

    //Render shader
    void Update() {
        rend = this.gameObject.GetComponent<MeshRenderer>();
        rend.material.SetColor("_PointLightColor", Sun.GetComponent<SunOrbit>().color);
        rend.material.SetVector("_PointLightPosition", Sun.GetComponent<SunOrbit>().GetPosition());
    }

    Mesh CreateTerrain()
    {
        //Initialise 2D int[][] array of length of the number of divisions
        vertNo = (divisions+1)*(divisions+1);
        verts = new Vector3[vertNo];

        Vector2[] uvs = new Vector2[vertNo];

        //There are 6 times as many vertice's than squares since squares are 2x3 vertices
        int[] tris = new int[divisions*divisions*6];

        float halfSize = width *0.5f;
        float divisionSize = width/divisions;

        //Create Mesh
        Mesh landscape = new Mesh();
        GetComponent<MeshFilter>().mesh = landscape;

        //Maintains triangle we are up to
        int triangleOffset = 0;
        int vertsOffset = 0;

        //Insert triangles in array representing terrain. Iterate through each coordinate, set up its base and create triangles
        for (int i = 0; i<=divisions; i++){
            for (int j =0; j<=divisions; j++){
                vertsOffset = i*(divisions+1)+j;
                verts[vertsOffset] = new Vector3(-halfSize+j*divisionSize, 0.0f, halfSize-i*divisionSize);
                uvs[vertsOffset] = new Vector2((float)i/divisions, (float)j/divisions);

                if (i < divisions && j < divisions){
                    int topLeft = i*(divisions+1)+j;
                    int botLeft = (i+1)*(divisions+1)+j;
                    int topRight = topLeft+1;;
                    int botRight = botLeft+1;

                    //Triangle 1s
                    tris[triangleOffset] = topLeft;
                    tris[triangleOffset+1] = topRight;
                    tris[triangleOffset+2] = botRight;

                    // Triangle 2
                    tris[triangleOffset+3] = topLeft;
                    tris[triangleOffset+4] = botRight;
                    tris[triangleOffset+5] = botLeft;

                    triangleOffset += 6;                    
                }
            }
        }

        //Initialise corner points
        verts[divisions].y = Random.Range(-height, height);
        verts[0].y = Random.Range(-height, height);
        verts[verts.Length-1].y = Random.Range(-height, height);
        verts[verts.Length-1-divisions].y = Random.Range(-height, height);

        //Start Diamond Square generation
        int numSquares = 1;
        int squareSize = divisions;

        for (int i=0; i<(int)Mathf.Log(divisions, 2); i++){ // Must do DiamondSquare algorithm this many times as it is equal to 2^n divisions
            int row = 0;
            for (int j=0; j<numSquares; j++){
                int col = 0;
                for (int k=0; k<numSquares; k++){
                    tempVertY = DiamondSquare(row, col, squareSize, height);
                    
                    if (tempVertY > highestVertY){
                        highestVertY = tempVertY;
                    } else if (tempVertY < lowestVertY){
                        lowestVertY = tempVertY;
                    }
                    
                    col+= squareSize;
                }
                row += squareSize;
            }
            // Because the number of squares doubles
            numSquares *= 2;
            
            //Because the square size halves
            squareSize /= 2;
            height *= 0.5f;
        }

        //Create mesh
        landscape.vertices = verts;
        landscape.triangles = tris;
        landscape.RecalculateBounds();
        landscape.RecalculateNormals();
        landscape.colors = colourVertices(verts, highestVertY,lowestVertY);
        landscape.uv = uvs;
        
        //Initialise waterMesh values and mesh
        waterLevel = WaterLevelCalc(highestVertY,lowestVertY);
        Water.GetComponent<WaterMesh>().waterLevel = waterLevel;
        Water.GetComponent<WaterMesh>().width = width/2;
        Water.GetComponent<MeshFilter>().mesh = Water.GetComponent<WaterMesh>().CreateWater();

        return landscape;
    }
    //Diamond step - For each square, set midpoint to be average of its 4 corners plus random value
    float DiamondStep(int topLeft, int botLeft, int size, float offset, Vector3[] verts){
        return (verts[topLeft].y + verts[topLeft + size].y + verts[botLeft].y + verts[botLeft+size].y)*0.25f + Random.Range(-offset, offset);
    }

    //Square step - set midpoint of diamond to be average of 4 corners plus random value
    Vector3[] SquareStep(int topLeft, int botLeft, int size, float offset, Vector3[] verts, int mid, int halfSize){
        verts[topLeft+halfSize].y = (verts[topLeft].y + verts[topLeft+size].y+verts[mid].y)/3 + Random.Range(-offset,offset);
        verts[mid-halfSize].y = (verts[topLeft].y + verts[botLeft].y + verts[mid].y)/3 + Random.Range(-offset, offset);
        verts[mid+halfSize].y = (verts[topLeft+size].y + verts[botLeft+size].y + verts[mid].y)/3 + Random.Range(-offset, offset);
        verts[botLeft+halfSize].y = (verts[botLeft].y + verts[botLeft+size].y + verts[mid].y)/3 + Random.Range(-offset, offset);
        return verts;
    }

    //Work from the middle of each square, perform the diamond step then square step
    float DiamondSquare(int row, int col, int size, float offset){
        int halfSize = (int) (size * 0.5f);
        int topLeft = row * (divisions +1)+col;
        int botLeft = (row+size)*(divisions+1)+col;

        int mid = (int)(row+halfSize)*(divisions+1) +(int)(col+halfSize);

        verts[mid].y = DiamondStep(topLeft, botLeft, size, offset, verts);
        verts = SquareStep(topLeft, botLeft, size, offset, verts,mid,halfSize);
        return verts[mid].y;
    }

    //Find the percentage which a point is inbetween two upper and lower bounds to make a gradient effect
    public float InterpolationPercentage(float maxH, float maxPercent, float minPercent, float actualH){
        return (actualH - (maxH*minPercent))/((maxH*maxPercent)-(maxH*minPercent));
    }

    // Math behind the fade between colors, creating a gradient effect
    public Color colCreator(Color col1, Color col2, float p){
        float r = (col1.r * p) + (col2.r * (1-p));
        float g = (col1.g * p) + (col2.g * (1-p));
        float b = (col1.b * p) + (col2.b * (1-p));
        return new Color(r,g,b);
    }

    //Colour the vertices based on the normalised height
    Color[] colourVertices(Vector3[] verts, float highestVertY, float lowestVertY){

        terrainVertColor = new Color[verts.Length];

        //Normalise the heights so that they start from zero
        float diff = highestVertY - lowestVertY;
        float high = highestVertY + diff;
        float low = lowestVertY + diff;
        float vertHeight = 0;
        float p = 0;

        //For each vertice, find which height it falls between, and its position relevant to the upper and lower bound, then color it
		for (int i = 0; i < verts.Length; i++) {
            vertHeight = verts[i].y + diff;
            float[] upperBounds = {0,Sand,Riverbank,Grass,DarkGrass,Stone,Snow, 1.1f};
            Color[] upperBoundCols = {RiverCol,RiverCol,SandCol,GrassCol,DarkGrassCol,StoneCol,SnowCol};
            
            //Check where about a vertice is positioned
            for (int j = 1; j < upperBounds.Length; j++){
                if (vertHeight <= upperBounds[j]*high && vertHeight > upperBounds[j-1]*high){
                    if (vertHeight >= Snow*high) {
                        terrainVertColor[i] = SnowCol;
                    } else {
                        p = InterpolationPercentage(high, upperBounds[j], upperBounds[j-1], vertHeight);
                        terrainVertColor[i] = colCreator(upperBoundCols[j],upperBoundCols[j-1],p);
                    }
                }
            }
        }
                       
        return terrainVertColor;
    }

    public float WaterLevelCalc(float highestVertY, float lowestVertY){
        float diff = highestVertY - lowestVertY;
        return (highestVertY+diff)*Riverbank - diff;
    }

}