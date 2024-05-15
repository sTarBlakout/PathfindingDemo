using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManagerUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField mapWidthInputField;
    [SerializeField] private TMP_InputField mapHeightInputField;
    [SerializeField] private Button generateMapButton;

    public Action<int, int> OnMapGenerateClicked;

    private void Start()
    {
        generateMapButton.onClick.AddListener(OnMapGenerateButtonClicked);
    }

    private void OnMapGenerateButtonClicked()
    {
        if (int.TryParse(mapWidthInputField.text, out var width) && int.TryParse(mapHeightInputField.text, out var height))
            OnMapGenerateClicked?.Invoke(width, height);
        else
            Debug.LogError("Can't generate a map, enter width and height!");
    }
}
