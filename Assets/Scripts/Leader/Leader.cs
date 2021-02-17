using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Leader : MonoBehaviour
{
    [SerializeField]
    LayerMask walkableLayer;

    [Header("Inventory")]
    [SerializeField]
    Transform catSpawn;
    [SerializeField]
    FloatVariable catsInInventory;
    [SerializeField]
    List<Cat> catInventory = new List<Cat>();


    [Header("UI elements")]
    [SerializeField]
    RectTransform canvas;
    [SerializeField]
    RectTransform walkTargetUI;
    [SerializeField]
    RectTransform mousePointerUI;

    NavMeshAgent agent;
    Vector3 moveTargetWorldSpace;
    Ray mousePositionRay;
    Camera camera;

    private void Start()
    {
        // Grab some components
        agent = GetComponent<NavMeshAgent>();
        camera = Camera.main;

        // Turn off the UI elements
        walkTargetUI.gameObject.SetActive(false);
        //Set mouse cursor to not be visible
        Cursor.visible = false;

        // Reset some global stuff
        catsInInventory.Value = 0;
    }


    void Update()
    {
        mousePointerUI.position = Input.mousePosition;
        mousePositionRay = camera.ScreenPointToRay(Input.mousePosition);

        // Move to right click location
        if (Input.GetMouseButtonDown(1))
        {
            Move();
        }

        // Right click to perform an action
        if (Input.GetMouseButtonDown(0))
        {
            Action();
        }

        // Keep the walk UI updated as the leader moves
        UpdateWalkTargetUIPosition();
    }

    void Move()
    {
        // Check to see if the click was on the nav mesh
        bool walkable = Physics.Raycast(mousePositionRay, out RaycastHit moveRaycastHit, Mathf.Infinity, walkableLayer);

        if (walkable)
        {
            moveTargetWorldSpace = moveRaycastHit.point;
            agent.SetDestination(moveTargetWorldSpace);

            // Turn on the walk target UI
            walkTargetUI.gameObject.SetActive(true);
        }
    }

    void Action()
    {
        // Gather directional vector info
        bool shootable = Physics.Raycast(mousePositionRay, out RaycastHit actionRaycastHit, Mathf.Infinity, walkableLayer);
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
        Vector2 moveTargetPositionInViewport = camera.WorldToViewportPoint(moveTargetWorldSpace);
        Vector2 moveTargetUIPosition = new Vector2(
            (moveTargetPositionInViewport.x * canvas.sizeDelta.x) - (canvas.sizeDelta.x * 0.5f),
            (moveTargetPositionInViewport.y * canvas.sizeDelta.y) - (canvas.sizeDelta.y * 0.5f));
        walkTargetUI.anchoredPosition = moveTargetUIPosition;

        // Remove target when reached
        if (transform.position.x == moveTargetWorldSpace.x && transform.position.z == moveTargetWorldSpace.z)
        {
            walkTargetUI.gameObject.SetActive(false);
        }
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
