using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusCamera : MonoBehaviour
{
    [SerializeField] Camera sceneCamera;
    [SerializeField] GameObject centerPoint;
    private GameObject playerOne;
    private GameObject playerTwo;
    private const float ignoreDifferenceFov = 5f;
    private const float ignoreDifferencePosition = 1f;
    private float currentFOV;
    private float distance = 0;

    private float initalZ = 0;
    private Coroutine currentCorutineFOV;
    private Coroutine currentCorutineMovement;

    // Start is called before the first frame update
    void Start()
    {
        currentFOV = sceneCamera.fieldOfView;
        initalZ = sceneCamera.transform.position.z;
    }

    void Update()
    {
        if (playerOne == null) return;
        CalculateCenter();
        CalculateFOV();
        MoveCamera();
    }

    private void CalculateCenter()
    {
        Debug.Log("Calculate Center");
        //get the middle point of the objects
        float xPosition = (playerOne.transform.position.x + playerTwo.transform.position.x) / 2;
        float yPosition = (playerOne.transform.position.y + playerTwo.transform.position.y) / 2;
        // z = 0
        Vector3 CenterPosition = new (xPosition, yPosition, 0);

        //set the middle point to the new middle point
        centerPoint.transform.position = CenterPosition;

        //set the distance from the middle point to each character
        //middle.distance(either character);
        distance = Mathf.Abs(Vector3.Distance(centerPoint.transform.position, playerOne.transform.position));        
    }
    private void MoveCamera()
    {
        Vector3 position = centerPoint.transform.position;
        position.y += 1;
        position.z = initalZ;
        if (Vector3.Distance(position, centerPoint.transform.position) > ignoreDifferencePosition)
        {
            if (currentCorutineMovement != null) StopCoroutine(currentCorutineMovement);
            currentCorutineMovement = StartCoroutine(UpdateCenter(position));
        }
    }

    private void CalculateFOV()
    {
        Debug.Log("Calculate FOV");
        //find what the FOV should be
        float newFOV = distance * 10;
        newFOV = Mathf.Clamp(newFOV, 45, 70);
        newFOV = Mathf.Round(newFOV * 100) / 100;
        //Check if the new FOV is to similar to the current FOV
        if(Mathf.Abs(newFOV - currentFOV) > ignoreDifferenceFov)
        {
            if(currentCorutineFOV != null) StopCoroutine(currentCorutineFOV);
            currentCorutineFOV = StartCoroutine(UpdateFOV(newFOV));
        }
    }

    /// <summary>
    /// Initalize the Camera
    /// </summary>
    /// <param name="player1">Player One, used to find the location of player one</param>
    /// <param name="player2">Player Two, used to find the location of player two</param>
    public IEnumerator Init()
    {
        yield return new WaitForSeconds(1);
        playerOne = GameManager.Instance.playerOneObject;
        playerTwo = GameManager.Instance.playerTwoObject;
        CalculateCenter();
        CalculateFOV();
    }

    public IEnumerator UpdateFOV(float newFOV)
    {
        float multiplier = 1;
        while (newFOV != currentFOV)
        {
            currentFOV += ((newFOV > currentFOV) ? .01f : -.01f) * multiplier;
            if(Mathf.Abs(currentFOV - newFOV) < .01f * multiplier) currentFOV = newFOV;
            sceneCamera.fieldOfView = currentFOV;
            multiplier += .5f;
            yield return new WaitForSeconds(.025f);
        }
        currentCorutineFOV = null;
    }

    public IEnumerator UpdateCenter(Vector3 newCenter)
    {
        bool doneMoving = false;
        float multiplier = 1;
        while (!doneMoving)
        {
            doneMoving = true;
            Vector3 currentPosition = sceneCamera.transform.position;

            if(currentPosition.x != newCenter.x)
            {
                currentPosition.x += ((newCenter.x > currentPosition.x) ? .01f : -.01f) * multiplier;
                if (Mathf.Abs(currentPosition.x - newCenter.x) < .01f * multiplier) currentPosition.x = newCenter.x;
            }

            if (currentPosition.y != newCenter.y)
            {
                currentPosition.y += ((newCenter.y > currentPosition.y) ? .01f : -.01f) * multiplier;
                if(Mathf.Abs(currentPosition.y - newCenter.y) < .01f * multiplier) currentPosition.y = newCenter.y;
            }

            multiplier += .5f;
            sceneCamera.transform.position = currentPosition;
            yield return new WaitForSeconds(.025f);
        }
        currentCorutineMovement = null;
    }
}
