using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class RepairScreenManagement : MonoBehaviour
{
    public static RepairScreenManagement _instance;

    private void Awake() {
        _instance = this;
        positionToGridObjects.Clear();
    }

    public const KeyCode NullInput = KeyCode.Alpha0;

    [Header("Orthagonal Input")]
    public List<KeyCode> upDownLeftRight = new List<KeyCode>();

    public static void UpdateOrthagonalKeyCode(int index, KeyCode input)
    {
        _instance.upDownLeftRight[index] = input;
    }

    private static Vector2[] orthagonalDirections = new Vector2[4]
    {
        new Vector2(0, 1),
        new Vector2(0, -1),
        new Vector2(-1, 0),
        new Vector2(1, 0)
    };

    [Header("Rotational Input")]
    public KeyCode LeftRotationalInput = NullInput;
    public KeyCode RightRotationalInput = NullInput;

    public static void UpdateRotationalKey(int index, KeyCode input)
    {
        if(index == 0)
        {
            _instance.LeftRotationalInput = input;
        }
        else
        {
            _instance.RightRotationalInput = input;
        }
    }

    public static Vector2 GetOrthagonalInput()
    {
        Vector2 toReturn = new Vector2();

        for(int i = 0; i < orthagonalDirections.Length; i++)
        {
            if(InputManagement.GetKey(_instance.upDownLeftRight[i]))
            {
                toReturn += orthagonalDirections[i];
            }
        }

        return toReturn;
    }

    public static float GetRotationalInput()
    {
        float toReturn = 0.0f;

        if(InputManagement.GetKey(_instance.LeftRotationalInput))
        {
            toReturn += 1.0f;
        }

        
        if(InputManagement.GetKey(_instance.RightRotationalInput))
        {
            toReturn -= 1.0f;
        }

        return toReturn;
    }

    [Header("Screen Objects Control")]
    public Transform screenParent;
    private static float repairScreenT = 0.0f;
    private static Vector2 startPos = new Vector2(0, -50);
    private static Vector2 endPos = new Vector2(0, 0);
    private bool screenActive;

    //GRID CONTROL
    public static Dictionary<Vector3, GridObject> positionToGridObjects = new Dictionary<Vector3, GridObject>();
    public static Dictionary<Vector3, MovementOutput> positionToOutputs = new Dictionary<Vector3, MovementOutput>();
    public const float cellSize = 1f;

    private List<InputReciever> lineDrawers = new List<InputReciever>();
    private Vector3 mousePosLastFrame;
    private InputReciever currentReciever;
    private Vector3 lastPositionAdded;

    public static void AddGridObject(Vector3 position, GridObject actualObject, bool output = false)
    {
        //Convert position to nearest grid position
        Vector3 gridPos = ConvertPositionToClosestGridPosition(position);

        if(!positionToGridObjects.ContainsKey(gridPos))
        {
            positionToGridObjects.Add(gridPos, actualObject);

            if(output && !positionToOutputs.ContainsKey(gridPos))
            {
                positionToOutputs.Add(gridPos, actualObject as MovementOutput);
            }
        }
    }

    public static Vector3 ConvertPositionToClosestGridPosition(Vector3 inputPos)
    {
        return new Vector3(
            Mathf.RoundToInt(inputPos.x / cellSize),
            Mathf.RoundToInt(inputPos.y / cellSize),
            Mathf.RoundToInt(inputPos.z / cellSize)
        ) * cellSize;
    }

    public static void SetRepairScreenActive(bool _bool)
    {
        _instance.screenActive = _bool;
        repairScreenT = 1.0f;
    }

    private void Update() {
        
        if(InputManagement.GetKeyDown(KeyCode.Escape))
        {
            SetRepairScreenActive(!screenActive);
        }

        if(repairScreenT > 0.0f)
        {
            repairScreenT -= Time.deltaTime * 2.0f;

            if(screenActive)
            {
                screenParent.position = Vector3.Lerp(endPos, startPos, repairScreenT);
            }
            else
            {
                screenParent.position = Vector3.Lerp(startPos, endPos, repairScreenT);
            }
        }

        if(screenActive)
        {
            Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosWorld.z = 0.0f;

            Vector3 mousePos = ConvertPositionToClosestGridPosition(mousePosWorld);

            //Line drawing control
            if(positionToGridObjects.TryGetValue(mousePos, out GridObject obj) && obj is InputReciever)
            {
                InputReciever reciever = obj as InputReciever;

                if(InputManagement.GetMouseButtonDown(0))
                {
                    currentReciever = reciever;
                    //Start drawing line
                    //Add reciever to line drawers list
                    if(lineDrawers.Contains(currentReciever))
                    {
                        //Remove and reset
                        ResetCurrentLineDrawer(false);
                    }

                    lineDrawers.Add(currentReciever);
                }
            }
            else if(currentReciever != null)
            {
                if(InputManagement.GetMouseButtonDown(0))
                {
                    ResetCurrentLineDrawer();
                }
                else if(mousePos != mousePosLastFrame)
                {
                    //Get all positions between this mouse pos and last mouse pos
                    //Using the pathfinding from my standard Unity Additions
                    //Unity does come with pathfing but it wouldn't work in this case
                    List<INode> positions = AStar.FindPath(new GridNode(mousePos), new GridNode(mousePosLastFrame), (x) => {return false;});

                    //Then iterate over them performing the steps below
                    foreach(INode node in positions)
                    {
                        Vector3 position = node.GetPosition();

                        if(position != mousePosLastFrame)
                        {
                            //First check if the current pos is a movement output
                            if(positionToOutputs.ContainsKey(position))
                            {
                                //Notify reciever of connection
                                currentReciever.NotiftyOfMovementOutput(positionToOutputs[position]);

                                currentReciever = null;
                                return;
                            }

                            //Check if there is already a line at this position
                            bool overlappingLine = false;
                            foreach(InputReciever reciever in lineDrawers)
                            {
                                if(reciever.HasPositionInLine(position))
                                {
                                    overlappingLine = true;
                                    break;
                                }
                            }
                            //Or anyother object
                            if(positionToGridObjects.ContainsKey(position) || overlappingLine)
                            {
                                if(position != lastPositionAdded)
                                {
                                    ResetCurrentLineDrawer();
                                }
                            }
                            else
                            {
                                //If not a fail state add position to current line
                                currentReciever.AddPositionToLine(position);
                                lastPositionAdded = position;
                            }
                        }
                    }
                }   
            }

            mousePosLastFrame = mousePos;
        }
    }

    public void ResetCurrentLineDrawer(bool fullReset = true)
    {
        lineDrawers.Remove(currentReciever);
        currentReciever.ResetLine();

        if(fullReset)
        {
            currentReciever = null;
        }
    }

    public class GridNode : INode
    {
        public Vector3 position;

        public GridNode(Vector3 pos)
        {
            position = pos;
        }

        public Vector3 GetPosition()
        {
            return position;
        }

        public List<INode> GetNeighbours()
        {
            List<INode> toReturn = new List<INode>();
            foreach(Vector3 offset in orthagonalDirections)
            {
                toReturn.Add(new GridNode(GetPosition() + offset));
            }

            return toReturn;
        }
    }
}
