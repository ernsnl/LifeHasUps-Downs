using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public float xMargin = 1f;
    public float yMargin = 1f;
    public float xSmooth = 8f;
    public float ySmooth = 8f;

    public Vector2 maxXandY;
    public Vector2 minXandY;

    private Transform player;

    private bool checkXMargin()
    {
        return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
    }

    private bool checkYMargin()
    {
        return Mathf.Abs(transform.position.y - player.position.y) > yMargin;
    }

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        TrackPlayer();
    }

    private void TrackPlayer()
    {
        float targetX = transform.position.x;
        float targetY = transform.position.y;

        if (checkXMargin())
            targetX = Mathf.Lerp(transform.position.x, player.position.x, xSmooth * Time.deltaTime);
        if (checkYMargin())
            targetY = Mathf.Lerp(transform.position.y, player.position.y, ySmooth * Time.deltaTime);

        targetX = Mathf.Clamp(targetX, minXandY.x, maxXandY.x);
        targetY = Mathf.Clamp(targetY, minXandY.y, maxXandY.y);

        transform.position = new Vector3(targetX, targetY, transform.position.z);
       
    }
}
