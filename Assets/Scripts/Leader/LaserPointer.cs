using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaserPointer : MonoBehaviour
{
    LineRenderer beam;
    public bool shine;

    [SerializeField]
    RectTransform canvas;
    [SerializeField]
    RectTransform dotUI;
    [SerializeField]
    FloatVariable batteryLevel;
    [SerializeField]
    Vector3Variable hitWorldSpace;

    SphereCollider collider;

    LayerMask walkableLayer;
    Ray mousePositionRay;

    private void Start()
    {
        beam = GetComponent<LineRenderer>();
        collider = GetComponent<SphereCollider>();
        collider.enabled = false;
        dotUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (beam.enabled)
        {
            // Get the mouse coordinates in world space
            Physics.Raycast(mousePositionRay, out RaycastHit mousePositionHit, Mathf.Infinity, walkableLayer);
            // This is where the laser beam should land
            Vector3 laserPointerTargetWorldSpace = mousePositionHit.point;
            // Ray needs a direction, not a target
            Vector3 laserPointerDirection = laserPointerTargetWorldSpace - beam.gameObject.transform.position;
            // Make a new ray to cast
            Ray laserPointerRay = new Ray(beam.gameObject.transform.position, laserPointerDirection);
            // Capture the first impact for use by line renderer
            Physics.Raycast(laserPointerRay, out RaycastHit laserPointerHit, Mathf.Infinity, walkableLayer);
            Vector3 dotPosition = laserPointerHit.point;
            hitWorldSpace.SetValue(dotPosition);

            // TODO Laser point hit is what should catch cat's attention

            // LR uses world space, but position is local
            beam.SetPosition(0, beam.transform.position);
            beam.SetPosition(1, dotPosition);

            collider.center = dotPosition - transform.position;

            batteryLevel.ChangeValue(-Time.deltaTime);



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

    public void Shine(Ray mouse, LayerMask layer)
    {
        beam.enabled = true;
        collider.enabled = true;
        dotUI.gameObject.SetActive(true);
        mousePositionRay = mouse;
        walkableLayer = layer;
    }

    public void TurnOff()
    {
        beam.enabled = false;
        collider.enabled = false;
        dotUI.gameObject.SetActive(false);
    }
}
