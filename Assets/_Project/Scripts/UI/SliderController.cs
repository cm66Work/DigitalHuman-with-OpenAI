using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SliderController : MonoBehaviour
{
   

    #region Events
    [SerializeField] private Utils.Events.GameEventSO _sliderValueChangedEvent;
    #endregion

    #region Components
    private Slider _slider;
    [SerializeField] private TMPro.TextMeshProUGUI _valueTextUI;
    #endregion

    private void Awake()
    {
        _slider = this.gameObject.GetComponent<Slider>();
    }

    public void OnValueChanged()
    {
        _sliderValueChangedEvent.Raise(this, (double)_slider.value);
        _valueTextUI.text = _slider.value.ToString("0.00");
    }
}
