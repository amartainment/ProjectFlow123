using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    // Start is called before the first frame update
    BirdController player;
    Vector3 playerPosition;
    Vector3 playerDirection;
    Quaternion playerRotation;
    GameObject lastObstacles;
    GameObject currentObstacles;
    public float difficultySum;
    public GameObject playerObject;
    public GameObject blockPrefab;
    public GameObject loopPrefab;
    public GameObject pathPrefab;
    //difficulty change to be determined by how closely the target was met
    [SerializeField] float difficultyChange;
    //setting up limits for the difficulty parameters
    [SerializeField] float maxThrustFactor;
    [SerializeField] float minThrustFactor;
    public float currentThrust;
    [SerializeField] float startingThrust;
    [SerializeField] float maxLoopSize;
    [SerializeField] float minLoopSize;
    public float currentLoopSize;
    [SerializeField] float startingLoopSize;
    [SerializeField] float maxPathLength; //max no of obstacles
    [SerializeField] float minPathLength; //min no of obstacles
    public float currentPathLength;
    [SerializeField] float startingPathLength;
     float startingDifficulty;
    public float difficulty;
    float pathScore = 0f;
    float idealPathScore = 0f;
    float pathCompletionRatio;

    void Start()
    {
        player = playerObject.GetComponent<BirdController>();
        
        currentLoopSize = startingLoopSize;
        currentPathLength = startingPathLength;
        currentThrust = startingThrust;
        startingDifficulty = startingThrust + startingDifficulty + startingPathLength;
        difficulty = startingDifficulty;
    }

    // Update is called once per frame
    void Update()
    {
        pathScoring();
        playerPosition = playerObject.transform.position;
        playerDirection = playerObject.transform.forward;
        playerRotation = playerObject.transform.rotation;
        //controlling speed based on difficulty
        player.thrustFactor = currentThrust;

        //testing
        if(Input.GetKeyDown(KeyCode.R))
        {
            createLoop(startingLoopSize, 5);

        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            decreaseDifficulty();
        }
    }

    void createLoop(float size, float pathLength)
    {
        GameObject newLoop = Instantiate(loopPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        //+new Vector3(0, 5 * pathLength, -5 * pathLength);
        newLoop.transform.localScale = new Vector3(size, size, size);
        int pathInt = Mathf.RoundToInt(pathLength);

        //  Debug.Log("player at " + playerObject.transform.position + " and trap at" + playerObject.transform.position + playerObject.transform.forward * pathLength * -10 + playerObject.transform.up*pathLength*5);
        newLoop.transform.forward = playerObject.transform.forward;
        newLoop.transform.position = playerObject.transform.position + new Vector3(pathInt*10, pathInt * 10, pathInt*10);

    }
    public void increasePathScore()
    {
        pathScore++;
    }

    void pathScoring()
    {
        if (idealPathScore != 0f)
        {
            pathCompletionRatio = pathScore / idealPathScore;
        }
    }
    public void IncreaseDifficulty()
    {

        
        // new difficulty to be distributed among path(50%), thrust(30%) and loop size (20%)
        difficulty += difficultyChange*pathCompletionRatio;
        //
        if (currentPathLength < maxPathLength) { currentPathLength += 0.5f * difficultyChange; }

        if (currentThrust < maxThrustFactor) { currentThrust += 0.3f * difficultyChange; }
        if (currentLoopSize < maxLoopSize) { currentLoopSize -= 0.2f * difficultyChange; }

        //
        //difficultySum = difficulty;
        //currentLoopSize = Random.Range(minLoopSize, currentLoopSize);
        //difficultySum -= currentLoopSize;
        
        ////Loopsize is being assumed to be most important to difficulty right now.
        //if (maxThrustFactor + minPathLength < difficultySum)
        //{
        //    currentThrust = Random.Range(currentThrust, maxThrustFactor);
        //    difficultySum -= currentThrust;
        //    currentPathLength = Random.Range(difficultySum, maxPathLength);
        //   // difficultySum = 0;
        //} else
        //{
        //    currentThrust = Random.Range(currentThrust, difficultySum - currentLoopSize - minPathLength);
        //    difficultySum -= currentThrust;
        //    currentPathLength = Random.Range(difficultySum, maxPathLength);
        //   // difficultySum = 0;
        //}
        //createLoop(currentLoopSize, currentPathLength);
        createPath(currentPathLength);
        
    }

    public void decreaseDifficulty()
    {
        // new difficulty to be distributed among path(50%), thrust(30%) and loop size (20%)
        difficulty += difficultyChange*pathCompletionRatio;
        //
        if (currentPathLength < maxPathLength) { currentPathLength -= 0.5f * difficultyChange; }

        if (currentThrust < maxThrustFactor) { currentThrust -= 0.3f * difficultyChange; }
        if (currentLoopSize < maxLoopSize) { currentLoopSize += 0.2f * difficultyChange; }

        //difficulty -= difficultyChange;
        //difficultySum = difficulty;
        //currentLoopSize = Random.Range(minLoopSize, currentLoopSize);
        //difficultySum -= currentLoopSize;
        ////Loopsize is being assumed to be most important to difficulty right now.
        //if (maxThrustFactor + minPathLength < difficultySum)
        //{
        //    currentThrust = Random.Range(currentThrust, maxThrustFactor);
        //    difficultySum -= currentThrust;
        //    currentPathLength = Random.Range(difficultySum, maxPathLength);
        //    // difficultySum = 0;
        //}
        //else
        //{
        //    currentThrust = Random.Range(currentThrust, difficultySum - currentLoopSize - minPathLength);
        //    difficultySum -= currentThrust;
        //    currentPathLength = Random.Range(difficultySum, maxPathLength);
        //    // difficultySum = 0;
        //}

        createPath(currentPathLength);
        //createLoop(currentLoopSize, currentPathLength);

    }

    void createPath(float size)
    {
        int convertedSize = Mathf.RoundToInt(size);
        int[,,] airTiles = new int[convertedSize, convertedSize, convertedSize];
        for(int i=0;i<convertedSize;i++)
        {
            for (int j = 0; j < convertedSize; j++)
            {
                for (int k = 0; k < convertedSize; k++)
                {
                    if((i==0&&j==0&&k==0)||(i == convertedSize - 1 && j == convertedSize - 1 && k == convertedSize - 1)) {
                        airTiles[i, j, k] = 1;
                    }

                    else
                    {
                        airTiles[i, j, k] = 0;
                    }
                }
            }
        }
        findPath(convertedSize, airTiles, 0, 0, 0);
       // makePathToEnd(convertedSize,airTiles);
        //randomize approach from start to end


       
    }

   
    int findPath(int convertedSize, int[,,] airTiles, int i,int j,int k)
    {
        
        if(i< convertedSize-1 || j<convertedSize -1 || k<convertedSize -1)
        {
            float randomFloat = Random.Range(0f, 1f);
            if (randomFloat < 0.33f)
            {
                if (i < convertedSize - 1)
                {
                    i++;
                    airTiles[i, j, k] = 1;
                   

                }
            }
            else if (randomFloat > 0.33f && randomFloat < .66f)
            {
                if (j < convertedSize - 1)
                {
                    j++;
                    airTiles[i, j, k] = 1;
                    

                }
            }
            else
            {
                if (k < convertedSize - 1)
                {
                    k++;
                    airTiles[i, j, k] = 1;
                    

                }
            }

            return findPath(convertedSize, airTiles, i, j, k);
        } else
        {
            //airTiles[convertedSize - 1, convertedSize - 1, convertedSize - 1] = 1;
            Debug.Log("path of length " + convertedSize);
            airTiles[convertedSize - 2, convertedSize - 2, convertedSize - 1] = 2;
            printPath(airTiles);
            return 0;
        }
    }

    

    void printPath(int[,,] tiles)
    {
        if (lastObstacles != null)
        {
            Destroy(lastObstacles);
        }
        lastObstacles = null;
        GameObject path = new GameObject();
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1) ; j++)
            {
                for (int k = 0; k < tiles.GetLength(2) ; k++)
                {
                    
                    Vector3 offset = new Vector3(0,0,0);
                    if (tiles[i, j, k] == 0)
                    {
                        GameObject block = Instantiate(blockPrefab, new Vector3(i * 10, j * 10, k * 10), Quaternion.identity);
                        block.transform.localScale = new Vector3(currentLoopSize, currentLoopSize, currentLoopSize);
                        Debug.Log("block of size " + currentLoopSize);
                        block.transform.parent = path.transform;
                    }

                    if (tiles[i, j, k] == 1)
                    {
                        GameObject correctPath = Instantiate(pathPrefab, new Vector3(i * 10, j * 10, k * 10), Quaternion.identity);
                        correctPath.transform.localScale = new Vector3(currentLoopSize, currentLoopSize, currentLoopSize);
                        correctPath.transform.parent = path.transform;
                        idealPathScore++;
                    }

                    if (tiles[i,j,k]==2)
                    {
                        GameObject loop = Instantiate(loopPrefab, new Vector3(i * 10, j * 10, k * 10), Quaternion.identity);
                        loop.transform.localScale = new Vector3(currentLoopSize, currentLoopSize, 0.2f);
                        loop.transform.parent = path.transform;
                    }
                }
            }
        }
        lastObstacles = path;
        path.transform.position = playerPosition + playerDirection * 10;
        path.transform.forward = playerObject.transform.forward;
        //pathScore = 0;
    }
}
