using UnityEngine;

[CreateAssetMenu(fileName = "New UIComponent", menuName = "UI Component", order = 51)]
public class UIComponent : ScriptableObject
{
    [SerializeField] string id;
    [SerializeField] float width;
    [SerializeField] float height;
    [SerializeField] Sprite icon;
    [SerializeField] Color color;
    [SerializeField] Color backgroundColor;

    public string ID
    {
        get
        {
            return id;
        }
    }
    public float Width
    {
        get
        {
            return width;
        }
    }
    public float Height
    {
        get
        {
            return height;
        }
    }
    public Sprite Icon
    {
        get
        {
            return icon;
        }
    }
    public Color Color
    {
        get
        {
            return color;
        }
    }
    public Color BackgroundColor
    {
        get
        {
            return backgroundColor;
        }
    }
}
