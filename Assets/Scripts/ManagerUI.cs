using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ManagerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text guideText;
    
    [Header("Map Generation Components")] 
    [SerializeField] private GameObject mapGenerationGroup;
    [SerializeField] private TMP_InputField mapWidthInputField;
    [SerializeField] private TMP_InputField mapHeightInputField;
    [SerializeField] private Button generateMapButton;

    [Header("Path Generation Components")]
    [SerializeField] private GameObject pathGenerationGroup;
    [SerializeField] private Button startPathGenerationButton;
    [SerializeField] private Button generatePathButton;
    [SerializeField] private Button launchCharacterButton;
    
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
    public Action OnLaunchCharacterClicked;

    private void Start()
    {
        generateMapButton.onClick.AddListener(OnMapGenerationButtonClicked);
        startPathGenerationButton.onClick.AddListener(StartPathGenerationButtonClicked);
        generatePathButton.onClick.AddListener(OnGeneratePathButtonClicked);
        randomizeObstaclesButton.onClick.AddListener(OnRandomizeObstaclesButtonClicked);
        drawObstaclesButton.onClick.AddListener(OnDrawObstacleButtonClicked);
        finishDrawingButton.onClick.AddListener(OnFinishDrawObstaclesButtonClicked);
        launchCharacterButton.onClick.AddListener(OnLaunchCharacterButtonClicked);
        
        generatePathButton.gameObject.SetActive(false);
        finishDrawingButton.gameObject.SetActive(false);
        launchCharacterButton.gameObject.SetActive(false);

        guideText.text = "Welcome to my demo! Use WASD to move around the map and click some self-explanatory buttons :)";
    }

    public void Init(Vector2Int startMapSize)
    {
        mapWidthInputField.text = startMapSize.x.ToString();
        mapHeightInputField.text = startMapSize.y.ToString();
    }

    public void ProcessPathfindingSequence(PathfindingSequenceUI seq, float generationTime = 0f)
    {
        switch (seq)
        {
            case PathfindingSequenceUI.ChooseStartTile:
                mapGenerationGroup.SetActive(false);
                obstacleGenerationGroup.SetActive(false);
                startPathGenerationButton.gameObject.SetActive(false);
                guideText.text = "Choose Start Tile";
                break;
            case PathfindingSequenceUI.ChooseFinishTile:
                guideText.text = "Choose Finish Tile";
                break;
            case PathfindingSequenceUI.ReadyToFindPath:
                guideText.text = "";
                generatePathButton.gameObject.SetActive(true);
                break;
            case PathfindingSequenceUI.PathFound:
                launchCharacterButton.gameObject.SetActive(true);
                guideText.text = $"Path generated in {generationTime} seconds";
                break;
            case PathfindingSequenceUI.PathNotFound:
                guideText.text = "No path between chosen cells!";
                break;
            case PathfindingSequenceUI.PathReset:
                launchCharacterButton.gameObject.SetActive(false);
                guideText.text = "";
                break;
        }
    }
    
    private void ProcessDrawObstacleSequence(DrawObstaclesSequenceUI seq)
    {
        switch (seq)
        {
            case DrawObstaclesSequenceUI.StartDrawing:
                guideText.text = "Draw some obstacles!";
                mapGenerationGroup.SetActive(false);
                pathGenerationGroup.SetActive(false);
                drawObstaclesButton.gameObject.SetActive(false);
                finishDrawingButton.gameObject.SetActive(true);
                break;
            case DrawObstaclesSequenceUI.FinishDrawing:
                mapGenerationGroup.SetActive(true);
                guideText.text = "";
                pathGenerationGroup.SetActive(true);
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
            Debug.Log("Can't generate a map, enter width and height!");
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

    private void OnLaunchCharacterButtonClicked()
    {
        OnLaunchCharacterClicked?.Invoke();
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
        PathFound,
        PathNotFound,
        PathReset
    }

    public enum DrawObstaclesSequenceUI
    {
        StartDrawing,
        FinishDrawing
    }
}
