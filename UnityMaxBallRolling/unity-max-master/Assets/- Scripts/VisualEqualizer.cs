using UnityEngine;
using extOSC;

// Shows a basic visual equalizer based on OSC data
public class VisualEqualizer : MonoBehaviour
{
    public OSCReceiver receiver;
    
    public float height = 5.0f;
    public float speed = 5.0f;
    public bool speedTweak;
    private const string oscAddress = "/eq";

    private const int POINT_COUNT = 7;
    
    private readonly GameObject[] points = new GameObject[POINT_COUNT];
    private readonly Vector3[] originalPositions = new Vector3[POINT_COUNT];

    private float maxDistance = 0;
    
    void Start()
    {
        // Create our points to bounce
        for (var t = 0; t < POINT_COUNT; t++)
        {
            var point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            point.transform.parent = transform;
            point.transform.localPosition = Vector3.zero + Vector3.up * 4 +  Vector3.right * (t * 2 - POINT_COUNT + 1);
            
            // Use these later for positioning
            originalPositions[t] = point.transform.localPosition;
            
            points[t] = point;
        }
        
        // Listen for OSC messages
        receiver.Bind(oscAddress, ReceivedMessage);
    }

    private void ReceivedMessage(OSCMessage message)
    {
        for (var t = 0; t < POINT_COUNT; t++)
        {
            // Each message is an array of levels
            var level = message.Values[t].FloatValue;
            
            // Figure out how far to bounce up
            var destination = originalPositions[t] + Vector3.up * (level * height);
            var distance = Vector3.Distance(originalPositions[t], destination);
            if (distance > maxDistance)
                maxDistance = distance;

            var time = speed;
            
            // My attempt at making these not so jerky when they are going to move a short distance
            if (speedTweak)
                time = time * (1 - distance/maxDistance);
            
            points[t].transform.localPosition = Vector3.Lerp(points[t].transform.localPosition, destination, Time.deltaTime * time);
        }
    }
}
