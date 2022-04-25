using System.Collections;
using System.Collections.Generic;
using extOSC;
using UnityEngine;
using TMPro;

// Shows a basic visual equalizer based on OSC data
public class ballRolling : MonoBehaviour
{
    public OSCReceiver receiver;
    public float speed = 5.0f;
    public TextMeshProUGUI countScore;
    
    private const string oscAddress = "/ball";
    private Rigidbody rb;
    private GameObject ball;

    private float maxDistance = 0;
    private float prevFreq;

    private Vector3 movement;
    private int score;
    private int isBanged;
    
    void Start()
    {
        // Create our points to bounce
        rb = GetComponent<Rigidbody>();
        Vector3 position = transform.position;
        // Listen for OSC messages
        receiver.Bind(oscAddress, ReceivedMessage);
        prevFreq = 0;
        movement = new Vector3(0.0f, 0.0f, 0.0f);
        isBanged = 0;
        score = 0;
        countScore.text = "Score: " + score.ToString();
    }

    private void ReceivedMessage(OSCMessage message)
    {
        // initialization
        if(prevFreq == 0){
            prevFreq = message.Values[1].FloatValue;
        }

        var freq = message.Values[1].FloatValue;
        var banged = message.Values[0].IntValue;

        // frequency difference affect ball direction here        
        if (freq - prevFreq > 0){
            var currPosition = transform.position;
            //currPosition.y = 1.5f;
            currPosition.z += 0.5f;
            // transform.position = currPosition;
            transform.position = Vector3.Lerp(transform.position, currPosition, Time.deltaTime*speed);
            
        }
        else if(freq - prevFreq < 0){
            var currPosition = transform.position;
            currPosition.z -= 0.5f;
            transform.position = Vector3.Lerp(transform.position, currPosition, Time.deltaTime*speed);
        }
        // bang triggers the ball to jump:
        else if (banged != isBanged){
            var currPos = transform.position;
            currPos.y += 10.0f;
            transform.position = Vector3.Lerp(transform.position, currPos, Time.deltaTime*speed);
            isBanged = banged;
        }
        prevFreq = freq;
    }

    // Destroy cube objects once the collider is triggered.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible")) 
        {
            Destroy(other.gameObject);
            score += 1;
            countScore.text = "Score: " + score.ToString();
        }
    }
}
