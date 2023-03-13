using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using OpenAI_API.Models;

public class DropDownPopulator : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _dropdown;

    #region Events
    [SerializeField] private Utils.Events.GameEventSO _newModelSelectedEvent;
    #endregion

    private List<Model> _models = new List<Model>() { Model.DavinciCode,
                    Model.DefaultModel, Model.AdaText, Model.BabbageText, Model.CurieText, Model.DavinciText};

    private void Start()
    {
        PopulateList();
    }

    private void PopulateList()
    {
        List<string> modelsString = new List<string>();
        foreach(Model model in _models)
        {
            modelsString.Add(model);
        }

        _dropdown.AddOptions(modelsString);
    }

    public void ValueChanged(int index)
    {
        _newModelSelectedEvent.Raise(this, _models[index]);
    }

}
