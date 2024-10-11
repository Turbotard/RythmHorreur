using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    Transform cam; //Main Camera
    Vector3 camStartPos; //Camera's starting position
    float distance; //Distance between camera and object
    GameObject[] backgrounds;
    Material[] mat;
    float[] backSpeed;

    float farthestBack;

    [Range(0.01f, 0.05f)]
    public float parallaxSpeed;

    void Start()
    {
        cam = Camera.main.transform;
        camStartPos = cam.position;

        int backCount = transform.childCount;
        mat = new Material[backCount];
        backSpeed = new float[backCount];
        backgrounds = new GameObject[backCount];

        for (int i = 0; i < backCount; i++)
        {
            backgrounds[i] = transform.GetChild(i).gameObject;
            mat[i] = backgrounds[i].GetComponent<Renderer>().material;
        }
        BackSpeedCalculate(backCount);
    }

    void BackSpeedCalculate(int backCount)
    {
        for (int i = 0; i < backCount; i++)
        {
            if ((backgrounds[i].transform.position.z - cam.position.z) > farthestBack)
            {
                farthestBack = backgrounds[i].transform.position.z - cam.position.z;
            }
        }
        for (int i = 0; i < backCount; i++)
        {
            backSpeed[i] = 1 - (backgrounds[i].transform.position.z - cam.position.z) / farthestBack;
        }
    }

    private void LateUpdate()
    {
        distance = cam.position.x - camStartPos.x;
        /*Debug.Log("Camera Position: " + cam.position);
        Debug.Log("Distance: " + distance);*/

        transform.position = new Vector3(cam.position.x, transform.position.y, 0);

        for (int i = 0; i < backgrounds.Length; i++)
        {
            float speed = backSpeed[i] * parallaxSpeed;
            Vector2 offset = new Vector2(distance, 0) * speed;
            mat[i].SetTextureOffset("_MainTex", offset);
            /*
            Debug.Log("Background " + i + " Offset: " + offset);
            */

            // Check if the material has the _MainTex property
            if (!mat[i].HasProperty("_MainTex"))
            {
                /*
                Debug.LogWarning("Material on background " + i + " does not have a _MainTex property.");
            */
            }
        }
    }
}