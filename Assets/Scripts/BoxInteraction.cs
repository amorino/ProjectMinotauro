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
        if (Physics.Raycast(cameraToStage, out hit, (float)25))
        {
            gameObjectSelected = hit.collider.gameObject;
            searchPlatform = gameObjectSelected.GetComponent<SearchPlatform>();
            platformRotation = gameObjectSelected.GetComponent<PlatformRotation>();
            firstMousePosition = Input.mousePosition;
            SetFirstClick(true);
        }
    }

    void OnMouseButtonUp()
    {
        if (GetFirstClick())
        {
            if (searchPlatform != null && searchPlatform.GetSearching())
                searchPlatform.StopSearch();

            if (platformRotation != null && distance < minDistanceToMove)
                platformRotation.StartRotation();

            gameObjectSelected = null;
            platformRotation = null;
            searchPlatform = null;
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
        if(searchPlatform != null && !searchPlatform.GetSearching())
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
