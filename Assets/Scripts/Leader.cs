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

        Vector2 aimPositionInViewport = camera.WorldToViewportPoint(aim.transform.position);
        Vector2 aimPositionConverted = new Vector2(
            (aimPositionInViewport.x * canvas.sizeDelta.x) - (canvas.sizeDelta.x * 0.5f),
            (aimPositionInViewport.y * canvas.sizeDelta.y) - (canvas.sizeDelta.y * 0.5f));
        mousePointer.anchoredPosition = aimPositionConverted;

        // Move to right click location
        if (Input.GetMouseButtonDown(1))
        {
            Physics.Raycast(mousePositionRay, out moveRaycastHit, Mathf.Infinity, walkableLayer);

            if (Physics.Raycast(mousePositionRay, out moveRaycastHit, Mathf.Infinity, walkableLayer))
            {
                walkTargetWorldSpace = moveRaycastHit.point;
                walkTargetUI.gameObject.SetActive(true);
                leader.SetDestination(walkTargetWorldSpace);
            }
        }

        Vector2 movePositionInViewport = camera.WorldToViewportPoint(walkTargetWorldSpace);
        Vector2 movePositionConverted = new Vector2(
            (movePositionInViewport.x * canvas.sizeDelta.x) - (canvas.sizeDelta.x * 0.5f),
            (movePositionInViewport.y * canvas.sizeDelta.y) - (canvas.sizeDelta.y * 0.5f));
        walkTargetUI.anchoredPosition = movePositionConverted;

        if (transform.position.x == walkTargetWorldSpace.x && transform.position.z == walkTargetWorldSpace.z)
        {
            walkTargetUI.gameObject.SetActive(false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(mousePositionRay, out actionRaycastHit, Mathf.Infinity, walkableLayer))
            {
                if (catInventory.Count > 0)
                {
                    Cat catToThrow = catInventory[0];

                    catToThrow.transform.position = catSpawn.position;

                    Debug.Log(mousePositionRay.origin);
                    Debug.Log(actionRaycastHit.point);

                    Vector3 throwToward = mousePositionRay.origin;
                    Vector2 direction = (Vector2)(Input.mousePosition - throwToward);
                    direction.Normalize();


                    catToThrow.ThrowCat(direction);
                    //body.isKinematic = false;
                    catToThrow.body.AddForce(direction * catToThrow.throwSpeed, ForceMode.Impulse);

                    AddRemoveCatInventory(catToThrow);
                }

            }
        }









        //UpdateWalkTargetUIPosition();
    }

    void Move()
    {


        // TODO remove walk target when reached
    }

    void UpdateWalkTargetUIPosition()
    {
        Vector3 targetPositionScreenPoint = camera.WorldToViewportPoint(walkTargetWorldSpace);
        //Debug.Log("Walk target: " + walkTargetWorldSpace);
        //Debug.Log("TargetScreenPoint: " + targetPositionScreenPoint);
        walkTargetUI.anchoredPosition = targetPositionScreenPoint;
    }
    void Action()
    {
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
