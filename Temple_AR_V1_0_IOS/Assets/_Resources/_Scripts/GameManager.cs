using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;


public class GameManager : MonoBehaviour
{

    [Header("PUBLIC GAMEOBJECTS")]
    public GameObject _mainArRaycastManagerObj;
    public GameObject placementIndicatorObj;

      [Header("PUBLIC ARRAY GAMEOBJECTS")]
    public GameObject[] objectToPlace;

      [Header("PUBLIC UI GAMEOBJECTS")]
    public GameObject versionTxt;
    public GameObject tutorialTxt;

    //  [Header("PUBLIC UI ELEMENTS")]
    //  public Text debugTxt;

    // PRIVATE VARIABLES

    // PRIVATE MIX
    private ARRaycastManager arRaycastMan;
    private Pose placementPose;

    // PRIVATE BOOLS
    private bool placementPoseIsValid = false;

    private bool onlyOnce = true;

    //   PRIVATE INTIGERS OR FLOATS
    private int currentSelectedTemple = 0;

    void Start()
    {

        InitialisationFunc();
    }
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && onlyOnce)
        {
            PlaceObject();
        }
    }


    // FOLLOW FUNCTION WILL DOES ALL VARIABLES INITALISATION
    private void InitialisationFunc()
    {
        arRaycastMan = _mainArRaycastManagerObj.GetComponent<ARRaycastManager>();

        onlyOnce  = true;

        // StartCoroutine(DelayForGameManager("init"));

        // currentSelectedTemple = Random.Range(0, objectToPlace.Length);

        currentSelectedTemple = 0; // 0 is the main temple in the temple array
    }



//  ALL LOGICAL CODE STARTS FROM HERE 111111111111111111111111111111111111111111111111111111111111111
    // FOLLOW FUNCTION WILL GET THE HIT INFO VIA DEVICE'S CAMERA'S DEPTH SENSOR
    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        arRaycastMan.Raycast(screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);


        var xms = UnityEngine.XR.ARSubsystems.TrackableType.Planes;

        placementPoseIsValid = hits.Count > 0;

        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }

    }

    // FOLLOW FUNCTION WILL UPDATE PLACEMENT INDICATOR'S POSITION, ROTATION AND VISIBILITY
    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid && onlyOnce)
        {
            placementIndicatorObj.SetActive(true);
            placementIndicatorObj.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndicatorObj.SetActive(false);
        }
    }

    // FOLLOW FUNCTION WILL SPAWN A GAMEOBJECT
    private void PlaceObject()
    {
        Instantiate(objectToPlace[currentSelectedTemple], placementPose.position, placementPose.rotation);
        onlyOnce = false;
        tutorialTxt.SetActive(false);
    }

//  ALL LOGICAL CODE ENDS TO HERE 111111111111111111111111111111111111111111111111111111111111111




//  ALL BUTTONS FUNCTIONS STARTS FROM HERE 22222222222222222222222222222222222222222222222222222222

public void ReloadBtnFunc()
{
    SceneManager.LoadScene (1);
}

//  ALL BUTTONS FUNCTIONS ENDS TO HERE 22222222222222222222222222222222222222222222222222222222


IEnumerator DelayForGameManager(string purpose)
{
    if (purpose == "init")
    {
        yield return new WaitForSeconds(7f);
        tutorialTxt.SetActive(false);
    }

    yield return new WaitForSeconds(0);
}

}
