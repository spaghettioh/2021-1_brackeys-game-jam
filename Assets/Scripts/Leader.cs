using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Leader : MonoBehaviour
{
    NavMeshAgent leader;
    [SerializeField]
    LayerMask walkableLayer;
    //List<Vector3> hits = new List<Vector3>();
    [SerializeField]
    List<Cat> catInventory = new List<Cat>();

    [SerializeField]
    RectTransform canvas;
    [SerializeField]
    RectTransform walkTargetUI;
    [SerializeField]
    RectTransform mousePointer;

    Vector3 walkTargetWorldSpace;
    Ray mousePositionRay;
    RaycastHit mousePositionRaycastHit;
    RaycastHit moveRaycastHit;
    RaycastHit actionRaycastHit;
    Camera camera;

    [SerializeField]
    GameObject aim;
    [SerializeField]
    Transform catSpawn;

    [SerializeField]
    FloatVariable catsInInventory;



    private void Start()
    {
        // Grab some components
        leader = GetComponent<NavMeshAgent>();
        camera = Camera.main;

        // Turn off the UI elements
        walkTargetUI.gameObject.SetActive(false);
        //Set Cursor to not be visible
        Cursor.visible = false;

        // Reset some global stuff
        catsInInventory.Value = 0;
    }


    void Update()
    {
        mousePositionRay = camera.ScreenPointToRay(Input.mousePosition);

        UpdateMouseCursorPosition();

        // Move to right click location
        if (Input.GetMouseButtonDown(1))
        {
            Move();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Action();
        }

        UpdateWalkTargetUIPosition();
    }

    void Move()
    {
        bool walkable = Physics.Raycast(mousePositionRay, out moveRaycastHit, Mathf.Infinity, walkableLayer);

        if (walkable)
        {
            walkTargetWorldSpace = moveRaycastHit.point;
            leader.SetDestination(walkTargetWorldSpace);

            // Turn on the walk target
            walkTargetUI.gameObject.SetActive(true);
        }
    }

    void Action()
    {
        // Gather directional vector info
        bool shootable = Physics.Raycast(mousePositionRay, out actionRaycastHit, Mathf.Infinity, walkableLayer);
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

    void UpdateWalkTargetUIPosition()
    {
        // then you calculate the position of the UI element
        // 0,0 for the canvas is at the center of the screen, whereas
        // WorldToViewPortPoint treats the lower left corner as 0,0. Because of this,
        // you need to subtract the height / width of the canvas * 0.5 to get the correct position.
        Vector2 movePositionInViewport = camera.WorldToViewportPoint(walkTargetWorldSpace);
        Vector2 movePositionConverted = new Vector2(
            (movePositionInViewport.x * canvas.sizeDelta.x) - (canvas.sizeDelta.x * 0.5f),
            (movePositionInViewport.y * canvas.sizeDelta.y) - (canvas.sizeDelta.y * 0.5f));
        walkTargetUI.anchoredPosition = movePositionConverted;

        // Remove target when reached
        if (transform.position.x == walkTargetWorldSpace.x && transform.position.z == walkTargetWorldSpace.z)
        {
            walkTargetUI.gameObject.SetActive(false);
        }
    }

    void UpdateMouseCursorPosition()
    {
        // Update mouse aim circle
        Physics.Raycast(mousePositionRay, out mousePositionRaycastHit, Mathf.Infinity, walkableLayer);
        Vector3 mousePositionWorldSpace = mousePositionRaycastHit.point;
        //aim.transform.position = new Vector3(mousePositionWorldSpace.x, .6f, mousePositionWorldSpace.z);
        // then you calculate the position of the UI element
        // 0,0 for the canvas is at the center of the screen, whereas
        // WorldToViewPortPoint treats the lower left corner as 0,0. Because of this,
        // you need to subtract the height / width of the canvas * 0.5 to get the correct position.
        Vector2 aimPositionInViewport = camera.WorldToViewportPoint(mousePositionWorldSpace);
        Vector2 aimPositionConverted = new Vector2(
            (aimPositionInViewport.x * canvas.sizeDelta.x) - (canvas.sizeDelta.x * 0.5f),
            (aimPositionInViewport.y * canvas.sizeDelta.y) - (canvas.sizeDelta.y * 0.5f));
        mousePointer.anchoredPosition = aimPositionConverted;
    }

    public void AddRemoveCatInventory(Cat cat)
    {
        if (!catInventory.Contains(cat))
        {
            catInventory.Add(cat);
            catsInInventory.Value += 1;

        }
        else
        {
            catInventory.Remove(cat);
            catsInInventory.Value -= 1;
        }
    }
}
