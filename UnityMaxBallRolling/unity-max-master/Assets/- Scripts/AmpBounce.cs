using System.Collections;
using System.Collections.Generic;
using extOSC;
using UnityEngine;

public enum BounceType
{
    Vertical,
    Scale
}

// Basic example of using OSC messages to control a bouncing object
public class AmpBounce : MonoBehaviour
{
    [Header("OSC Settings")]
    public OSCReceiver receiver;

    private const string oscAddress = "/amp";

    public BounceType type = BounceType.Vertical;

    public float height = 5.0f;
    public float speed = 5.0f;

    private Vector3 originalPosition;
    private Vector3 originalScale;

    protected void Start()
    {
        originalPosition = transform.position;
        originalScale = transform.localScale;
        
        // Listen for OSC messages
        receiver.Bind(oscAddress, ReceivedMessage);
    }
    
    private void ReceivedMessage(OSCMessage message)
    {
        //Debug.LogFormat("Received: {0} {1}", message, amp);
        
        var amp = message.Values[0].FloatValue;

        if (type == BounceType.Vertical)
        {
            var destination = originalPosition + Vector3.up * (amp * height);
            
            transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * speed);
        }
        else
        {
            // Bounce the scale, this is a bit more fun
            var destination = originalScale * (amp * height);
            transform.localScale = Vector3.Lerp(transform.localScale, destination, Time.deltaTime * speed);
        }
    }
}