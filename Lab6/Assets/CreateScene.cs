using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateScene : MonoBehaviour
{
    public int sizeOfForest;
    public float forestRadius = 40f;
    public Vector3 forestCenter = new Vector3(30, 0, 0);

    [Range(3, 10)]
    public int pyramidBaseSize;
    public GameObject[] trees;

    public float celestialRotationSpeed = 20f;

    private GameObject celestialSphere;
    private Light sceneLight;
    // Start is called before the first frame update
    void Start()
    {
        CreateGround();
        CreatePyramid();
        CreateForest();
        CreateCelestialObject();
    }

    void Update()
    {
        if (celestialSphere == null || sceneLight == null) return;

        celestialSphere.transform.Rotate(Vector3.right * celestialRotationSpeed * Time.deltaTime);

        sceneLight.transform.rotation = celestialSphere.transform.rotation;

        if (sceneLight.transform.forward.y < 0)
            sceneLight.intensity = 1f;  
        else
            sceneLight.intensity = 0.1f; 
    }


    void CreateGround()
    {
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        if (ground != null)
        {
            ground.transform.localScale = new Vector3(10, 10, 10);
            ground.transform.position = new Vector3(0, -0.5f, 0);
            ground.name = "Ground";
        }
    }

    void CreatePyramid()
    {
        GameObject pyraParent = new GameObject("Pyramid");
        Vector3 currentPoint = Vector3.zero;

        for (int i = 0; i < pyramidBaseSize; i++) // height
        {
            int currentSize = pyramidBaseSize - i;

            for (int j = 0; j < currentSize; j++) // width of current row
            {
                for (int k = 0; k < currentSize; k++) // length of current row
                {
                    currentPoint = new Vector3(j - currentSize / 2f, i, k - currentSize / 2f
                    );

                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = currentPoint;
                    cube.transform.parent = pyraParent.transform;

                    //change color based on which tier we are on
                    Renderer renderer = cube.GetComponent<Renderer>();

                    float t = (float)i / pyramidBaseSize;
                    renderer.material.color = Color.Lerp(Color.yellow, Color.red, t);
                }
            }
        }
    }

    void CreateForest()
    {
        GameObject forestParent = new GameObject("Forest");

        for (int i = 0; i < sizeOfForest; i++)
        {

            GameObject tree = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

            //returns random normalized vector in unit circle so we multiply it to expanmd to our size
            Vector2 randomPos = Random.insideUnitCircle * forestRadius;

            Vector3 finalPosition = new Vector3(randomPos.x, 1, randomPos.y) + forestCenter;

            tree.transform.position = finalPosition;
            tree.transform.localScale = new Vector3(1, Random.Range(2f, 4f), 1);
            tree.transform.parent = forestParent.transform;

            Renderer renderer = tree.GetComponent<Renderer>();
            renderer.material.color = Color.green;
        }
    }

    void CreateCelestialObject()
    {
        celestialSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        celestialSphere.name = "Celestial Object";
        celestialSphere.transform.position = new Vector3(0, 25, 0);
        celestialSphere.transform.localScale = Vector3.one * 3f;

        sceneLight = FindObjectOfType<Light>(); // find the only light in the scene
    }
}
