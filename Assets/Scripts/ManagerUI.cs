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

    public Action<int, int> OnMapGenerateClicked;
    public Action OnStartPathGenerationClicked;
    public Action OnGeneratePathClicked;

    private void Start()
    {
        generateMapButton.onClick.AddListener(OnMapGenerationButtonClicked);
        startPathGenerationButton.onClick.AddListener(StartPathGenerationButtonClicked);
        generatePathButton.onClick.AddListener(OnGeneratePathButtonClicked);
        
        generatePathButton.gameObject.SetActive(false);
    }

    public void ProgressPathfindingSequence(PathfindingSequenceUI seq)
    {
        switch (seq)
        {
            case PathfindingSequenceUI.ChooseStartTile:
                mapGenerationGroup.SetActive(false);
                startPathGenerationButton.gameObject.SetActive(false);
                pathGenerationGuide.text = "Choose Start Tile";
                break;
            case PathfindingSequenceUI.ChooseFinishTile:
                pathGenerationGuide.text = "Choose Finish Tile";
                break;
            case PathfindingSequenceUI.FindPath:
                pathGenerationGuide.text = "";
                generatePathButton.gameObject.SetActive(true);
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

    private void StartPathGenerationButtonClicked()
    {
        ProgressPathfindingSequence(PathfindingSequenceUI.ChooseStartTile);
        OnStartPathGenerationClicked?.Invoke();
    }

    private void OnGeneratePathButtonClicked()
    {
        OnGeneratePathClicked?.Invoke();
        generatePathButton.gameObject.SetActive(false);
        mapGenerationGroup.SetActive(true);
        startPathGenerationButton.gameObject.SetActive(true);
    }
    
    public enum PathfindingSequenceUI
    {
        ChooseStartTile,
        ChooseFinishTile,
        FindPath
    }
}
