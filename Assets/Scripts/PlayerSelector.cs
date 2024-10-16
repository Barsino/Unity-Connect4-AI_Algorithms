using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerSelector : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown1 ,dropdown2;
    public TMP_Dropdown Dropdown1 { get { return dropdown1; } }
    public TMP_Dropdown Dropdown2 { get { return dropdown2; } }

    [SerializeField] private List<Algorithm> algoritnmOptions = new List<Algorithm>();
    public List<Algorithm> AlgorithmOptions { get { return algoritnmOptions; } }

    private List<TMP_Dropdown.OptionData> options1 = new List<TMP_Dropdown.OptionData>(), 
                                          options2 = new List<TMP_Dropdown.OptionData>();

    private void Start()
    {
        SetOptionValue();
    }

    private void SetOptionValue()
    {
        dropdown1.ClearOptions();
        dropdown2.ClearOptions();


        for (int i = 0; i < algoritnmOptions.Count; ++i)
        {
            if (algoritnmOptions[i].Turn == 1)
            {
                options1.Add(new TMP_Dropdown.OptionData(algoritnmOptions[i].AlgorithmName, null));
            }
            else
            {
                options2.Add(new TMP_Dropdown.OptionData(algoritnmOptions[i].AlgorithmName, null));
            }
        }

        dropdown1.AddOptions(options1);
        dropdown2.AddOptions(options2);

        dropdown1.RefreshShownValue();
        dropdown2.RefreshShownValue();
    }
}
