using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerSelector : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;

    [SerializeField] private List<Algorithm> optionsName = new List<Algorithm>();

    private List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

    private void Start()
    {
        SetOptionValue();
    }

    private void SetOptionValue()
    {
        dropdown.ClearOptions();

        for(int i = 0; i < optionsName.Count; ++i)
        {
            options.Add(new TMP_Dropdown.OptionData(optionsName[i].AlgorithmName, null));
        }

        dropdown.AddOptions(options);

        dropdown.RefreshShownValue();
    }
}
