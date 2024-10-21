using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerSelector : MonoBehaviour
{
    // Dropdowns para seleccionar los jugadores
    [SerializeField] private TMP_Dropdown dropdown1 ,dropdown2;
    public TMP_Dropdown Dropdown1 { get { return dropdown1; } }
    public TMP_Dropdown Dropdown2 { get { return dropdown2; } }


    // Referencia al GameManager para acceder a las opciones de los algoritmos
    [SerializeField] private GameManager gameManager;

    // Lista de opciones para los dropdowns
    private List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

    private void Start()
    {
        SetOptionValue();
    }

    private void SetOptionValue()
    {
        // Limpia cualquier opción existente en los dropdowns
        dropdown1.ClearOptions();
        dropdown2.ClearOptions();

        // Recorre la lista de algoritmos del GameManager y añade sus nombres a las opciones
        for (int i = 0; i < gameManager.AlgorithmsOptions.Count; ++i)
        {
            options.Add(new TMP_Dropdown.OptionData(gameManager.AlgorithmsOptions[i].AlgorithmName, null));
        }

        // Añade las opciones generadas a ambos dropdowns
        dropdown1.AddOptions(options);
        dropdown2.AddOptions(options);

        // Refresca el valor mostrado en los dropdowns
        dropdown1.RefreshShownValue();
        dropdown2.RefreshShownValue();
    }
}
