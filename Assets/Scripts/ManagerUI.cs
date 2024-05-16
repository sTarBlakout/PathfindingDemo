using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManagerUI : MonoBehaviour
{
    [Header("Map Generation Components")] 
    [SerializeField] private GameObject mapGenerationGroup;
    [SerializeField] private TMP_InputField mapWidthInputField;
    [SerializeField] private TMP_InputField mapHeightInputField;
    [SerializeField] private Button generateMapButton;

    [Header("Path Generation Components")] 
    [SerializeField] private Button startPathGenerationButton;
    [SerializeField] private Button generatePathButton;
    [SerializeField] private TMP_Text pathGenerationGuide;
    
    [Header("Obstacle Generation Components")]
    [SerializeField] private GameObject obstacleGenerationGroup;
    [SerializeField] private Button randomizeObstaclesButton;
    [SerializeField] private Button drawObstaclesButton;
    [SerializeField] private Button finishDrawingButton;

    public Action<int, int> OnMapGenerateClicked;
    public Action OnStartPathGenerationClicked;
    public Action OnGeneratePathClicked;
    public Action OnRandomizeObstaclesClicked;
    public Action OnDrawObstaclesClicked;
    public Action OnFinishDrawObstaclesClicked;

    private void Start()
    {
        generateMapButton.onClick.AddListener(OnMapGenerationButtonClicked);
        startPathGenerationButton.onClick.AddListener(StartPathGenerationButtonClicked);
        generatePathButton.onClick.AddListener(OnGeneratePathButtonClicked);
        randomizeObstaclesButton.onClick.AddListener(OnRandomizeObstaclesButtonClicked);
        drawObstaclesButton.onClick.AddListener(OnDrawObstacleButtonClicked);
        finishDrawingButton.onClick.AddListener(OnFinishDrawObstaclesButtonClicked);
        
        generatePathButton.gameObject.SetActive(false);
        finishDrawingButton.gameObject.SetActive(false);
    }

    public void Init(Vector2Int startMapSize)
    {
        mapWidthInputField.text = startMapSize.x.ToString();
        mapHeightInputField.text = startMapSize.y.ToString();
    }

    public void ProcessPathfindingSequence(PathfindingSequenceUI seq)
    {
        switch (seq)
        {
            case PathfindingSequenceUI.ChooseStartTile:
                mapGenerationGroup.SetActive(false);
                obstacleGenerationGroup.SetActive(false);
                startPathGenerationButton.gameObject.SetActive(false);
                pathGenerationGuide.text = "Choose Start Tile";
                break;
            case PathfindingSequenceUI.ChooseFinishTile:
                pathGenerationGuide.text = "Choose Finish Tile";
                break;
            case PathfindingSequenceUI.ReadyToFindPath:
                pathGenerationGuide.text = "";
                generatePathButton.gameObject.SetActive(true);
                break;
            case PathfindingSequenceUI.PathNotFound:
                pathGenerationGuide.text = "No path between chosen cells!";
                break;
        }
    }
    
    private void ProcessDrawObstacleSequence(DrawObstaclesSequenceUI seq)
    {
        switch (seq)
        {
            case DrawObstaclesSequenceUI.StartDrawing:
                pathGenerationGuide.text = "Click on the tiles that will be obstacles";
                mapGenerationGroup.SetActive(false);
                startPathGenerationButton.gameObject.SetActive(false);
                drawObstaclesButton.gameObject.SetActive(false);
                finishDrawingButton.gameObject.SetActive(true);
                break;
            case DrawObstaclesSequenceUI.FinishDrawing:
                mapGenerationGroup.SetActive(true);
                pathGenerationGuide.text = "";
                startPathGenerationButton.gameObject.SetActive(true);
                finishDrawingButton.gameObject.SetActive(false);
                drawObstaclesButton.gameObject.SetActive(true);
                break;
        }
    }

    private void OnMapGenerationButtonClicked()
    {
        if (int.TryParse(mapWidthInputField.text, out var width) && int.TryParse(mapHeightInputField.text, out var height))
            OnMapGenerateClicked?.Invoke(width, height);
        else
            Debug.LogError("Can't generate a map, enter width and height!");
    }

    private void OnRandomizeObstaclesButtonClicked()
    {
        OnRandomizeObstaclesClicked?.Invoke();
    }

    private void OnDrawObstacleButtonClicked()
    {
        OnDrawObstaclesClicked?.Invoke();
        ProcessDrawObstacleSequence(DrawObstaclesSequenceUI.StartDrawing);
    }

    private void OnFinishDrawObstaclesButtonClicked()
    {
        OnFinishDrawObstaclesClicked?.Invoke();
        ProcessDrawObstacleSequence(DrawObstaclesSequenceUI.FinishDrawing);
    }

    private void StartPathGenerationButtonClicked()
    {
        ProcessPathfindingSequence(PathfindingSequenceUI.ChooseStartTile);
        OnStartPathGenerationClicked?.Invoke();
    }

    private void OnGeneratePathButtonClicked()
    {
        OnGeneratePathClicked?.Invoke();
        generatePathButton.gameObject.SetActive(false);
        mapGenerationGroup.SetActive(true);
        obstacleGenerationGroup.SetActive(true);
        startPathGenerationButton.gameObject.SetActive(true);
    }
    
    public enum PathfindingSequenceUI
    {
        ChooseStartTile,
        ChooseFinishTile,
        ReadyToFindPath,
        PathNotFound
    }

    public enum DrawObstaclesSequenceUI
    {
        StartDrawing,
        FinishDrawing
    }
}
