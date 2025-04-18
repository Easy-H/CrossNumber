
using UnityEngine;

[CreateAssetMenu(fileName = "Data_Skin_", menuName = "Custom/SkinData", order = 1)]
public class SkinData : ScriptableObject {

    public enum Type {
        Background,     Unit,       RedLine,        UiPanel,    UiBoard,    UiBoardLine
    };

    public Color back = Color.white;
    public Color unit = Color.white;
    public Color red = Color.white;
    public Color panel = Color.white;
    public Color board = Color.white;
    public Color line = Color.white;

    public Color Get(Type type)
    {
        switch (type)
        {
            case Type.Background:
                return back;
            case Type.Unit:
                return unit;
            case Type.RedLine:
                return red;
            case Type.UiPanel:
                return panel;
            case Type.UiBoard:
                return board;
            case Type.UiBoardLine:
                return line;
            default:
                return Color.white;
        }
    }
}
