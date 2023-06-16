using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorData", menuName = "ScriptableObjects/SO_ColorData", order = 1)]

public class ScriptableObject_ColorData : ScriptableObject
{
    [SerializeField] private List<ColorData> listColorData;

    public Material GetMaterialByType(ColorType colorType)
    {
        ColorData colorData = listColorData.Find(cd => cd.Type == colorType);
        if (colorData == null)
        {
            Debug.LogError("Didn`t find colorData by type = "+colorType);
            return null;
        }

        return colorData.MaterialForBall;
    }

    public Color GetBackgroundColorByType(ColorType colorType)
    {
        ColorData colorData = listColorData.Find(cd => cd.Type == colorType);
        if (colorData == null)
        {
            Debug.LogError("Didn`t find colorData by type = "+colorType);
            return Color.white;
        }

        return colorData.ColorForBackGround;
    }
}

[Serializable]
public class ColorData
{
    [SerializeField] private ColorType type;
    [SerializeField] private Material materialForBall;
    [SerializeField] private Color colorForBackGround;

    public ColorType Type => type;

    public Material MaterialForBall => materialForBall;

    public Color ColorForBackGround => colorForBackGround;
}

public enum ColorType
{
    YELLOW=0, 
    RED=1,
    BLUE=2,
    ORANGE,
    GREEN
}
