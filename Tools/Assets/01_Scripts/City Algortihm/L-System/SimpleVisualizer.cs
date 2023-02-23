using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleVisualizer : MonoBehaviour
{
    [Header("References")]
    public LSystemGenerator lSystem;
    public GameObject pointPrefab;
    public Material lineStyle;

    [Header("Variables")]
    [SerializeField] private int angle = 90;
    [SerializeField] private float length = 8;
    [SerializeField] private float lengthModifier = -0.5f;
    [SerializeField] private float lineWidth = 0.1f;
    [SerializeField] private Color lineColor = Color.red;

    private List<Vector3> positions = new List<Vector3>();
    public float Length
    {
        get => length > 0 ? length : 1;
        private set => length = value;
    }

    private void Start()
    {
        string sequence = lSystem.GenerateSentence();
        VisualizeSequence(sequence);
    }

    private void VisualizeSequence(string sequence)
    {
        Stack<AgentParameters> savePoints = new Stack<AgentParameters>();
        Vector3 currentPosition = Vector3.zero;

        Vector3 direction = Vector3.forward;
        Vector3 tempPosition = Vector3.zero;

        positions.Add(currentPosition);

        foreach(char letter in sequence)
        {
            EncodingLetters encoding = (EncodingLetters)letter;
            switch (encoding)
            {
                case EncodingLetters.save:
                    SaveAgent(savePoints, currentPosition, direction);
                    break;
                case EncodingLetters.load:
                    if (savePoints.Count > 0)
                    {
                        LoadAgent(savePoints, out currentPosition, out direction);
                    }
                    else
                    {
                        Debug.Log("No more savePoints to load!");
                    }
                    break;
                case EncodingLetters.forward:
                    tempPosition = currentPosition;
                    currentPosition += direction * length;
                    DrawLine(tempPosition, currentPosition, lineColor);
                    length += lengthModifier;
                    positions.Add(currentPosition);
                    break;
                case EncodingLetters.turnRight:
                    direction = TurnAgent(angle, direction);
                    break;
                case EncodingLetters.turnLeft:
                    direction = TurnAgent(-angle, direction);
                    break;
                default:
                    break;
            }
        }

        foreach (Vector3 position in positions)
        {
            Instantiate(pointPrefab, position, Quaternion.identity, transform);
        }
    }

    private Vector3 TurnAgent(float _angle, Vector3 _direction)
    {
        _direction = Quaternion.AngleAxis(_angle, Vector3.up) * _direction;
        return _direction;
    }

    // Why this method here?
    private void DrawLine(Vector3 _start, Vector3 _end, Color _color)
    {
        GameObject line = new GameObject("line");
        line.transform.parent = transform;
        line.transform.position = _start;
        LineRenderer lr = line.AddComponent<LineRenderer>();

        // Line renderer settings
        lr.material = lineStyle;
        lr.startColor = _color;
        lr.endColor = _color;
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        lr.SetPosition(0, _start);
        lr.SetPosition(1, _end);
    }

    private void SaveAgent(Stack<AgentParameters> _savePoints, Vector3 _currentPosition, Vector3 _direction)
    {
        _savePoints.Push(new AgentParameters
        {
            position = _currentPosition,
            direction = _direction,
            currentRoadLength = Length
        });
    }

    // Ignoring load will also create cool results
    private void LoadAgent(Stack<AgentParameters> savePoints, out Vector3 currentPosition, out Vector3 direction)
    {
        AgentParameters agentParameters = savePoints.Pop();
        currentPosition = agentParameters.position;
        direction = agentParameters.direction;
        Length = agentParameters.currentRoadLength;
    }

    public enum EncodingLetters
    {
        unknown = '1',
        save = '[',
        load = ']',
        forward = 'F',
        turnRight = '+',
        turnLeft = '-',
    }
}
