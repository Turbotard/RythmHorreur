
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    Transform cam; //Main Camera
    Vector3 camStartPos; //Camera's starting position
    float distance; //Distance between camera and object
    GameObject[] background;
    Material[] amt;
    float[] backSpeed;

    float fathestBack;

    [Range(0.01f, 0.05f)]
    public float parallaxSpeed;

    void Start()
    {
        cam=camera.main.transform;
        camStartPose=cam.position;

        int backCount = transform.childCount;
        mat= new Material[backCount];
        backSpeed = new float [backCount];
        background = new GameObject[backCount];

        for (int i = 0; i < backCount; i++)
        {
            background[i] = transform.GetChild(i).gameObject;
            mat[i] = backgroundsp[i].GetComponent<Renderer>().material;
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

        for (int i = 0; i < backgrounds.Length; i++)
        {
            float speed = backSpeed[i] * parallaxSpeed;
            mat[i].SetTextureOffset("_Maintex", new Vector2(distance, 0) * speed);
        }
    }

}
