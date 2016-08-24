using UnityEngine;
using System.Collections;

public class BoxInteraction : MonoBehaviour {

    //Stage
    public Camera gameplayCamera;
    Vector3 firstMousePosition;
    bool firstClick = false;
    float distance;
    float minDistanceToMove = 5;

    //Platform
    GameObject gameObjectSelected;
    SearchPlatform searchPlatform;
    PlatformRotation platformRotation;
    MovePlatform movePlatform;

    void Start () {}
	
	void Update () {
        if (Input.GetMouseButtonDown(0))
            OnMouseButtonDown();

        if (Input.GetMouseButtonUp(0))
            OnMouseButtonUp();

        if (Input.GetMouseButton(0))
            OnMouseButtonHeldDown();
    }

    void OnMouseButtonDown()
    {
        Ray cameraToStage = new Ray(gameplayCamera.ScreenToWorldPoint(Input.mousePosition), gameplayCamera.transform.forward);
        RaycastHit hit;
        int layerMask = 1 << LayerMask.NameToLayer("Platforms");

        if (Physics.Raycast(cameraToStage, out hit, (float)25, layerMask))
        {
            gameObjectSelected = hit.collider.gameObject;
            SetScripts();
            firstMousePosition = Input.mousePosition;
            SetFirstClick(true);
        }
    }

    void SetScripts()
    {
        searchPlatform = gameObjectSelected.GetComponent<SearchPlatform>();
        platformRotation = gameObjectSelected.GetComponent<PlatformRotation>();
        movePlatform = gameObjectSelected.GetComponent<MovePlatform>();
        movePlatform.SetLastPosition(gameObjectSelected.transform.position);
        movePlatform.SetCamera(gameplayCamera);
    }

    void OnMouseButtonUp()
    {
        if (GetFirstClick())
        {
            if (searchPlatform != null && searchPlatform.GetSearching())
                searchPlatform.StopSearch();

            if (platformRotation != null && distance < minDistanceToMove)
                platformRotation.StartRotation();

            if (movePlatform != null && movePlatform.GetDrag())
            { 
                movePlatform.StopDrag();
                if (searchPlatform != null && searchPlatform.GetFind())
                {
                    searchPlatform.SetFind(false);
                    movePlatform.SetNextPosition(searchPlatform.GetNextPosition());
                    GameObject otherPlatform = searchPlatform.GetOtherPlatform();
                    otherPlatform.GetComponent<MovePlatform>().SetNextPosition(movePlatform.GetLastPosition());
                }
                else
                {
                    movePlatform.StartBack();
                }
            }

            gameObjectSelected = null;
            platformRotation = null;
            searchPlatform = null;
            movePlatform = null;
        }
        SetFirstClick(false);
    }

    void OnMouseButtonHeldDown()
    {
        if(GetFirstClick())
        {
            distance = Vector3.Distance(firstMousePosition, Input.mousePosition);
            if(distance > minDistanceToMove)
                MoveGameObject();
        }
    }

    void MoveGameObject()
    {
        if (movePlatform != null && !movePlatform.GetDrag())
            movePlatform.StartDrag();

        if (searchPlatform != null && !searchPlatform.GetSearching())
            searchPlatform.StartSearch();
    }

    void SetFirstClick(bool value)
    {
        firstClick = value;
    }

    public bool GetFirstClick()
    {
        return firstClick;
    }
}
