using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ball : MonoBehaviour
{
    public float GameTime = 30.0f;
    public float CubeSpeed = 2.0f;
    public GameObject winTextObject;
    
    private float CubeTime = 0;
    
    // function to generate cube at random location
    void addCube(){
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Collider m_ObjectCollider;
        cube.transform.position = new Vector3(0.0f, Random.Range(0.5f, 2.0f), Random.Range(-5.0f, 5.0f));
        cube.tag = "Collectible";
        m_ObjectCollider = cube.GetComponent<Collider>();
        m_ObjectCollider.isTrigger = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize parameter here
        CubeTime = CubeSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GameTime -= Time.deltaTime;
        CubeTime -= Time.deltaTime;
        // Determine when to generate a new cube
        if (GameTime > 0 && CubeTime <= 0){
            addCube();
            CubeTime = CubeSpeed;
        }
        // if game time < 0: Display Game Over
        if (GameTime <= 0){
            winTextObject.SetActive(true);
            Application.Quit();
        }

    }
}
