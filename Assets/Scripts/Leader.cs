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
    Vector3 actionTargetWorldSpace;
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
    FloatVariable slowCatCount;
    [SerializeField]
    FloatVariable averageCatCount;
    [SerializeField]
    FloatVariable fastCatCount;



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
        slowCatCount.Value = 0;
        averageCatCount.Value = 0;
        fastCatCount.Value = 0;
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
        Physics.Raycast(mousePositionRay, out moveRaycastHit, Mathf.Infinity, walkableLayer);

        if (Physics.Raycast(mousePositionRay, out moveRaycastHit, Mathf.Infinity, walkableLayer))
        {
            walkTargetWorldSpace = moveRaycastHit.point;
            walkTargetUI.gameObject.SetActive(true);
            leader.SetDestination(walkTargetWorldSpace);
        }
    }

    void Action()
    {
        // Gather directional vector info
        bool shootable = Physics.Raycast(mousePositionRay, out actionRaycastHit, Mathf.Infinity, walkableLayer);
        actionTargetWorldSpace = actionRaycastHit.point;
        //Debug.Log("Action click location: " + actionTargetWorldSpace);
        if (shootable)
        {
            if (catInventory.Count > 0)
            {
                // Get the first cat in inventory
                Cat catToThrow = catInventory[0];

                // put cat overhead to throw it
                catToThrow.transform.position = catSpawn.position;
                catToThrow.body.velocity = Vector3.zero;

                Vector3 direction = actionTargetWorldSpace - catToThrow.transform.position;
                direction.Normalize();
                //direction.y *= Mathf.Abs(Vector3.Distance(catToThrow.transform.position, actionTargetWorldSpace);
                //Debug.Log("Direction: " + direction);
                catToThrow.body.AddForce(direction * catToThrow.throwSpeed, ForceMode.Impulse);

                catToThrow.ThrowCat(direction);

                AddRemoveCatInventory(catToThrow);
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
            ChangeInventory(cat.catType, 1);

        }
        else
        {
            catInventory.Remove(cat);
            ChangeInventory(cat.catType, -1);
        }
    }

    void ChangeInventory(CatType type, float change)
    {
        if (type == CatType.Slow)
        {
            slowCatCount.Value += change;
        }
        if (type == CatType.Average)
        {
            averageCatCount.Value += change;
        }
        if (type == CatType.Fast)
        {
            fastCatCount.Value += change;
        }
    }
}
