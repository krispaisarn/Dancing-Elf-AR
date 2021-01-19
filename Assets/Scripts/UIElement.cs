using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIElement : MonoBehaviour
{



    [SerializeField] private UIComponent _component;
    // Start is called before the first frame update

    [SerializeField] private RectTransform _contentContainer;
    [SerializeField] private RectTransform _backgroundContainer;


    [SerializeField] private Image _contentImage;
    [SerializeField] private Image _backgroundImage;


    public UIComponentOverride _overrideComponent;

    void Start()
    {

        Vector2 size = !_contentContainer.Equals(null) ? _contentContainer.sizeDelta : _backgroundContainer.sizeDelta;
        float width = _overrideComponent._overrideWidth.Equals(0) ? _component.Width : _overrideComponent._overrideWidth;
        float height = _overrideComponent._overrideHeight.Equals(0) ? _component.Height : _overrideComponent._overrideHeight;
        size = new Vector2(width, height);

        if (_contentContainer != null)
            _contentContainer.sizeDelta = size;

        if (_backgroundContainer != null)
            _backgroundContainer.sizeDelta = size;

        if (_contentImage != null)
        {

            if(_overrideComponent._overrideIcon!=null && _component.Icon!=null)
            _contentImage.sprite = _overrideComponent._overrideIcon ?? _component.Icon;

            _contentImage.color = !_overrideComponent._overrideColor.Equals(Color.clear) ? _overrideComponent._overrideColor : _component.Color;
        }

        if (_backgroundImage != null)
            _backgroundImage.color = !_overrideComponent._overrideBackgroundColor.Equals(Color.clear) ? _overrideComponent._overrideBackgroundColor : _component.BackgroundColor;

    }


}

[System.Serializable]
public class UIComponentOverride
{
    public string _overrideId;
    public float _overrideWidth;
    public float _overrideHeight;
    public Sprite _overrideIcon;
    public Color _overrideColor;
    public Color _overrideBackgroundColor;

}
