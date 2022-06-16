using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleGun : MonoBehaviour
{
    public Camera mainCamera;
    public LineRenderer lineRenderer;
    public DistanceJoint2D distanceJoint;
    public Transform bulletSpawn;
    public float grappleRadius;
    public float grappleSpeed;
    Vector3 anchor;
    public Rigidbody2D rb;
    CooldownManager cm;
    public bool isGrappling;

    public LayerMask CanGrapple;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.material.SetColor("_Color", new Color(1f, 1f, 1f, 0.1f));
        distanceJoint.enabled = false;
        cm = CooldownManager.Instance;
        rb = this.gameObject.GetComponent<Rigidbody2D>();

        
    }

    Vector3 perpendicularDirection;
    Collider2D[] hit;

    public void OnDrawGizmos()
    {
        if (Application.isPlaying) 
        {
            Gizmos.color = new Color(255, 0, 0);
            
            Gizmos.DrawLine(transform.position - perpendicularDirection * 5, transform.position + perpendicularDirection * 5);
            Gizmos.color = new Color(0, 255, 0);
            Gizmos.DrawLine(anchor, transform.position);

            Gizmos.DrawWireSphere(transform.position, grappleRadius);
        }
        
    }

    private void Update()
    {
        if (PlayerInput.Instance.pressingGrapple)
        {
            DrawRadius(100, grappleRadius);
            lineRenderer.enabled = true;
            hit = Physics2D.OverlapCircleAll(transform.position, grappleRadius);
            for (int i = 0; i < hit.Length; i++) 
            {
                Debug.Log(hit[i].transform.tag);
                if (hit[i].transform.CompareTag("GrappleNode"))
                {
                    Debug.Log(hit);
                    anchor = hit[i].ClosestPoint(transform.position);
                    //lineRenderer.SetPosition(0, anchor);
                    //lineRenderer.SetPosition(1, bulletSpawn.position);
                    distanceJoint.connectedAnchor = anchor;
                    distanceJoint.enabled = true;
                    
                }
            }
            
        }
        else
        {
            distanceJoint.enabled = false;
            lineRenderer.enabled = false;
        }

        if (distanceJoint.enabled)
        {
            perpendicularDirection = Vector2.Perpendicular((anchor - transform.position).normalized);
            isGrappling = true;
            rb.AddForce(-perpendicularDirection * PlayerInput.Instance.horizontalMovement * grappleSpeed);
            //lineRenderer.SetPosition(1, bulletSpawn.position);
        }
        else 
        {
            isGrappling = false;
        }
   
    }

    void DrawRadius(int steps, float radius)
    {
        lineRenderer.positionCount = steps;

        for (int currentStep = 0; currentStep < steps; currentStep++) 
        {
            float progress = (float)currentStep / (steps - 1);

            float currentRadian = progress * 2 * Mathf.PI;

            float x = Mathf.Cos(currentRadian) * radius;
            float y = Mathf.Sin(currentRadian) * radius;
            float z = 0;

            Vector3 currentPosition = new Vector3(x, y, z );
            Vector3 finalPos = currentPosition + transform.position;

            lineRenderer.SetPosition(currentStep, finalPos);
        }
        
    }
}


