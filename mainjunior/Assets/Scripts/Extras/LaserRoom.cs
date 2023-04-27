using UnityEngine;

public class LaserRoom : MonoBehaviour
{
      public float speed = 5f;
    public float amplitude = 1f;
    public bool isHorizontal = true;

    private float _startTime;
    private Vector3 _startPosition;

    private void Start()
    {
        _startTime = Time.time;
        _startPosition = transform.position;
    }

    private void Update()
    {
        float t = (Time.time - _startTime) * speed;
        float x = isHorizontal ? Mathf.Cos(t) : 0f;
        float y = isHorizontal ? 0f : Mathf.Cos(t);
        transform.position = _startPosition + new Vector3(x, y, 0f) * amplitude;
    }
}