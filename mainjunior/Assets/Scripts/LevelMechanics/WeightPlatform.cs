using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightPlatform : MonoBehaviour
{
    private bool shouldPlatformRotate;

    private float _platformLength;
    private Transform Player;

    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private float rotationSpeed = 40f;
    [SerializeField] private float rotationMaxDegrees = 30f;
    private BoxCollider2D _platformBoxCollider;
    private float _heightCheckDistance = 0.1f;

  
    private void Awake() {
        _platformBoxCollider = GetComponent<BoxCollider2D>();
    }
    /*private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            shouldPlatformRotate = true;
            Player = col.gameObject.GetComponent<Transform>();
        }

    }
*/
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && !shouldPlatformRotate)
        {
            shouldPlatformRotate = true;
            Player = col.gameObject.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && shouldPlatformRotate)
        {
            //shouldPlatformRotate = false;
        }
    }
    private void Update()
    {
        if (shouldPlatformRotate)
        {
            RaycastHit2D raycastHit2D = Physics2D.BoxCast(_platformBoxCollider.bounds.center, _platformBoxCollider.bounds.size, 0, Vector2.up, _heightCheckDistance, playerLayerMask);

            if (raycastHit2D.collider == null)
            {
                Player = null;
                shouldPlatformRotate = false;
                return;
            }

            Vector3 PlayerRelativePosition = transform.InverseTransformPoint(Player.position);
            float rotationSpeedMultiplier = CalculateRotationMultiplier(PlayerRelativePosition);
            int rotationDirection = PlayerRelativePosition.x < 0 ? 1 : -1;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(rotationMaxDegrees * rotationDirection, Vector3.forward), Time.deltaTime * rotationSpeed * rotationSpeedMultiplier);
        }
        else if (transform.rotation.eulerAngles.z !=0)
        {
            Debug.Log("Current Z Rotation: " + transform.rotation.eulerAngles.z);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * rotationSpeed);
        }

    }
    private float CalculateRotationMultiplier(Vector3 PlayerRelativePosition)
    {

        int rotationDirection = PlayerRelativePosition.x < 0 ? 1 : -1;
        float rotationSpeedMultiplier = Mathf.Abs(Mathf.Clamp((PlayerRelativePosition.x * 2 / _platformLength) * rotationDirection, -1, 1));
        return rotationSpeedMultiplier;
    }
}