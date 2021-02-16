using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Leader : MonoBehaviour
{
    NavMeshAgent leader;
    public LayerMask walkableLayer;
    //List<Vector3> hits = new List<Vector3>();
    [SerializeField]
    public List<Cat> catInventory = new List<Cat>();

    public RectTransform canvas;
    public RectTransform walkTargetUI;
    public RectTransform mousePointer;
    public Vector3 walkTargetWorldSpace;
    public Vector3 actionTargetWorldSpace;
    public Ray clickedRaycastPosition;
    public Ray mousePositionRay;
    public RaycastHit mousePositionRaycastHit;
    public RaycastHit moveRaycastHit;
    public RaycastHit actionRaycastHit;
    Camera camera;
    public GameObject aim;

    public Transform catSpawn;



    private void Start()
    {
        leader = GetComponent<NavMeshAgent>();

        walkTargetUI.gameObject.SetActive(false);
        camera = Camera.main;
    }


    void Update()
    {
        mousePositionRay = camera.ScreenPointToRay(Input.mousePosition);

        // Update mouse aim circle
        Physics.Raycast(mousePositionRay, out mousePositionRaycastHit, Mathf.Infinity, walkableLayer);
        Vector3 mousePositionWorldSpace = mousePositionRaycastHit.point;
        aim.transform.position = new Vector3(mousePositionWorldSpace.x, .6f, mousePositionWorldSpace.z);

        // then you calculate the position of the UI element
        // 0,0 for the canvas is at the center of the screen, whereas
        // WorldToViewPortPoint treats the lower left corner as 0,0. Because of this,
        // you need to subtract the height / width of the canvas * 0.5 to get the correct position.
        Vector2 aimPositionInViewport = camera.WorldToViewportPoint(aim.transform.position);
        Vector2 aimPositionConverted = new Vector2(
            (aimPositionInViewport.x * canvas.sizeDelta.x) - (canvas.sizeDelta.x * 0.5f),
            (aimPositionInViewport.y * canvas.sizeDelta.y) - (canvas.sizeDelta.y * 0.5f));
        mousePointer.anchoredPosition = aimPositionConverted;

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

        // TODO remove walk target when reached
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

        if (transform.position.x == walkTargetWorldSpace.x && transform.position.z == walkTargetWorldSpace.z)
        {
            walkTargetUI.gameObject.SetActive(false);
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
    }
}
