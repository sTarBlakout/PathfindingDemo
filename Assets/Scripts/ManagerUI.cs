using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ManagerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text guideText;

    [Header("Records Components")] 
    [SerializeField] private GameObject recordsGroup;
    [SerializeField] private Button clearRecordsButton;
    [SerializeField] private TMP_Text recordsText;
    
    [Header("Map Generation Components")] 
    [SerializeField] private GameObject mapGenerationGroup;
    [SerializeField] private TMP_InputField mapWidthInputField;
    [SerializeField] private TMP_InputField mapHeightInputField;
    [SerializeField] private Button generateMapButton;

    [Header("Path Generation Components")]
    [SerializeField] private GameObject pathGenerationGroup;
    [SerializeField] private GameObject settingsGroup;
    [SerializeField] private Button startPathGenerationButton;
    [SerializeField] private Button generatePathButton;
    [SerializeField] private Button launchCharacterButton;
    [SerializeField] private TMP_Dropdown algorithmDropdown;
    [SerializeField] private TMP_Dropdown heuristicDropdown;
    
    [Header("Obstacle Generation Components")]
    [SerializeField] private GameObject obstacleGenerationGroup;
    [SerializeField] private Button randomizeObstaclesButton;
    [SerializeField] private Button drawObstaclesButton;
    [SerializeField] private Button finishDrawingButton;

    public Action<int, int> OnMapGenerateClicked;
    public Action OnStartPathGenerationClicked;
    public Action<Algorithm, Heuristic> OnGeneratePathClicked;
    public Action OnRandomizeObstaclesClicked;
    public Action OnDrawObstaclesClicked;
    public Action OnFinishDrawObstaclesClicked;
    public Action OnLaunchCharacterClicked;

    private List<string> _recordList = new();

    private void Start()
    {
        generateMapButton.onClick.AddListener(OnMapGenerationButtonClicked);
        startPathGenerationButton.onClick.AddListener(StartPathGenerationButtonClicked);
        generatePathButton.onClick.AddListener(OnGeneratePathButtonClicked);
        randomizeObstaclesButton.onClick.AddListener(OnRandomizeObstaclesButtonClicked);
        drawObstaclesButton.onClick.AddListener(OnDrawObstacleButtonClicked);
        finishDrawingButton.onClick.AddListener(OnFinishDrawObstaclesButtonClicked);
        launchCharacterButton.onClick.AddListener(OnLaunchCharacterButtonClicked);
        clearRecordsButton.onClick.AddListener(OnClearRecordsButtonClicked);
        
        generatePathButton.gameObject.SetActive(false);
        finishDrawingButton.gameObject.SetActive(false);
        launchCharacterButton.gameObject.SetActive(false);
        settingsGroup.gameObject.SetActive(false);

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
                recordsGroup.SetActive(false);
                obstacleGenerationGroup.SetActive(false);
                startPathGenerationButton.gameObject.SetActive(false);
                guideText.text = "Choose Start Tile";
                break;
            case PathfindingSequenceUI.ChooseFinishTile:
                guideText.text = "Choose Finish Tile";
                break;
            case PathfindingSequenceUI.ReadyToFindPath:
                guideText.text = "";
                recordsGroup.SetActive(true);
                settingsGroup.gameObject.SetActive(true);
                generatePathButton.gameObject.SetActive(true);
                break;
            case PathfindingSequenceUI.PathFound:
                launchCharacterButton.gameObject.SetActive(true);
                settingsGroup.gameObject.SetActive(false);
                guideText.text = $"Path generated in {generationTime:F6} seconds";
                AddRecord(generationTime);
                break;
            case PathfindingSequenceUI.PathNotFound:
                settingsGroup.gameObject.SetActive(false);
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
                recordsGroup.SetActive(false);
                break;
            case DrawObstaclesSequenceUI.FinishDrawing:
                mapGenerationGroup.SetActive(true);
                guideText.text = "";
                pathGenerationGroup.SetActive(true);
                finishDrawingButton.gameObject.SetActive(false);
                drawObstaclesButton.gameObject.SetActive(true);
                recordsGroup.SetActive(true);
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

    private void AddRecord(float time)
    {
        var algorithm = (Algorithm) algorithmDropdown.value;
        var heuristic = (Heuristic) heuristicDropdown.value;
        _recordList.Add($"A:{algorithm} H:{heuristic} T:{time:F6}");
        
        recordsText.text = "";
        foreach (var record in _recordList)
        {
            recordsText.text += record + "\n";
        }
    }

    private void OnClearRecordsButtonClicked()
    {
        recordsText.text = "";
        _recordList.Clear();
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
        OnStartPathGenerationClicked?.Invoke();
        ProcessPathfindingSequence(PathfindingSequenceUI.ChooseStartTile);
    }

    private void OnLaunchCharacterButtonClicked()
    {
        OnLaunchCharacterClicked?.Invoke();
    }

    private void OnGeneratePathButtonClicked()
    {
        var algorithm = (Algorithm) algorithmDropdown.value;
        var heuristic = (Heuristic) heuristicDropdown.value;
        OnGeneratePathClicked?.Invoke(algorithm, heuristic);
        
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

    public enum Heuristic
    {
        Manhattan,
        Chebyshev
    }

    public enum Algorithm
    {
        StarA,
        JumpPointSearch
    }
}
