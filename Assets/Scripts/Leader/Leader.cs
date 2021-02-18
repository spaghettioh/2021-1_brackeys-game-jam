using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;

public class Leader : MonoBehaviour
{
    [SerializeField]
    LayerMask walkableLayer;
    [SerializeField]
    LayerMask mouseHitLayer;

    [SerializeField]
    float moveSpeed;

    [Header("Inventory")]
    [SerializeField]
    Transform catSpawn;
    [SerializeField]
    FloatVariable laserPointerBattery;
    [SerializeField]
    FloatVariable catsInInventory;
    List<Cat> catInventory = new List<Cat>();

    // Laser pointer
    LaserPointer laserPointer;
    [SerializeField]
    Vector3Variable laserPointerBeamPoint;

    [Header("UI elements")]
    [SerializeField]
    RectTransform canvas;

    [SerializeField]
    RectTransform mousePointerUI;

    Ray mousePositionRay;
    Camera camera;
    Vector3 mouseInWorldSpace;

    private void Start()
    {
        /// Grab some components
        camera = Camera.main;
        laserPointer = GetComponentInChildren<LaserPointer>();

        // Turn off some visual stuff
        Cursor.visible = false;

        // Reset some global stuff
        catsInInventory.SetValue(0);
        laserPointerBattery.SetValue(100);
        laserPointerBeamPoint.SetValue(Vector3.zero);
    }


    void Update()
    {
        mousePointerUI.position = Input.mousePosition;
        mousePositionRay = camera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(mousePositionRay, out RaycastHit mouseRayArenaHit, Mathf.Infinity, mouseHitLayer);
        mouseInWorldSpace = mouseRayArenaHit.point;

        Move();

        // Throw a cat
        if (Input.GetMouseButtonDown(0))
        {
            ThrowCatFromInventory();
        }

        // Point the laser while holding the button
        if (Input.GetMouseButton(1))
        {
            laserPointer.Shine(mouseInWorldSpace, mouseHitLayer);
        }
        if (Input.GetMouseButtonUp(1))
        {
            laserPointer.TurnOff();
        }
    }

    void Move()
    {
        // Look towards the mouse position
        Vector3 lookDirection = mouseInWorldSpace - transform.position;
        float rotation = Mathf.Atan2(lookDirection.x, lookDirection.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, rotation, 0);

        // Move the player based on input
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 moveDirection = new Vector3(h, 0, v);
        transform.position = Vector3.MoveTowards(transform.position, transform.position + moveDirection, Time.deltaTime * moveSpeed);
    }

    void ThrowCatFromInventory()
    {
        // Gather directional vector info
        bool shootable = Physics.Raycast(mousePositionRay, out RaycastHit actionRaycastHit, Mathf.Infinity, mouseHitLayer);
        Vector3 actionTargetWorldSpace = actionRaycastHit.point;

        if (shootable)
        {
            if (catInventory.Count > 0)
            {
                // Get the first cat in inventory
                Cat catToThrow = catInventory[0];

                // Set direction based on mouse input
                Vector3 direction = actionTargetWorldSpace - catToThrow.transform.position;
                direction.Normalize();

                catToThrow.ThrowCat(catSpawn.position, direction);
            }
        }
    }

    public void AddRemoveCatInventory(Cat cat)
    {
        if (!catInventory.Contains(cat))
        {
            catInventory.Add(cat);
        }
        else
        {
            catInventory.Remove(cat);
        }
        catsInInventory.SetValue(catInventory.Count);
    }
}
