using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaserPointer : MonoBehaviour
{
    [SerializeField]
    LineRenderer beam;
    [SerializeField]
    RectTransform canvas;
    [SerializeField]
    RectTransform dotUI;
    [SerializeField]
    FloatVariable batteryLevel;
    [SerializeField]
    Vector3Variable hitWorldSpace;
    [SerializeField]
    SphereCollider collider;

    LayerMask mouseHitLayer;
    Vector3 mouseInWorldSpace;

    private void Start()
    {
        beam = GetComponent<LineRenderer>();
        beam.enabled = false;
        collider = GetComponent<SphereCollider>();
        collider.radius = 0;
        dotUI.gameObject.SetActive(false);

    }

    private void Update()
    {
        if (beam.enabled)
        {
            // Ray needs a direction, not a target
            Vector3 laserPointerDirection = mouseInWorldSpace - transform.position;
            // Cast the ray and capture the first impact for use by line renderer
            Physics.Raycast(transform.position, laserPointerDirection, out RaycastHit laserPointerHit, Mathf.Infinity, mouseHitLayer);
            Vector3 dotPosition = laserPointerHit.point;

            // LineRenderer uses world space, but laser pointer position is local
            beam.SetPosition(0, transform.position);
            beam.SetPosition(1, dotPosition);

            collider.center = transform.InverseTransformPoint(dotPosition);

            batteryLevel.ChangeValue(-Time.deltaTime);

            // Update the global variable to cats can chase it
            hitWorldSpace.SetValue(dotPosition);

            // then you calculate the position of the UI element
            // 0,0 for the canvas is at the center of the screen, whereas
            // WorldToViewPortPoint treats the lower left corner as 0,0. Because of this,
            // you need to subtract the height / width of the canvas * 0.5 to get the correct position.
            Vector2 moveTargetPositionInViewport = Camera.main.WorldToViewportPoint(dotPosition);
            Vector2 moveTargetUIPosition = new Vector2(
                (moveTargetPositionInViewport.x * canvas.sizeDelta.x) - (canvas.sizeDelta.x * 0.5f),
                (moveTargetPositionInViewport.y * canvas.sizeDelta.y) - (canvas.sizeDelta.y * 0.5f));
            dotUI.anchoredPosition = moveTargetUIPosition;
        }
    }

    public void Shine(Vector3 mousePosition, LayerMask layer)
    {
        if (batteryLevel.Value > 0)
        {
            beam.enabled = true;
            collider.radius = 6;
            dotUI.gameObject.SetActive(true);

            mouseInWorldSpace = mousePosition;
            mouseHitLayer = layer;
        }
        else
        {
            TurnOff();
        }
    }

    public void TurnOff()
    {
        beam.enabled = false;
        collider.radius = 0;
        dotUI.gameObject.SetActive(false);
        hitWorldSpace.SetValue(Vector3.zero);
    }
}
