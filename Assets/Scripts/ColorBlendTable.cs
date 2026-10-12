using System.Collections.Generic;
using UnityEngine;

public static class ColorBlendTable
{
    // 1. 기본 7가지 색상의 실제 물감 RGB 정의 (ColorID Enum 명칭과 정확히 일치시킴)
    public static readonly Dictionary<ColorID, Color32> BaseColors = new Dictionary<ColorID, Color32>()
    {
        { ColorID.Red,    new Color32(230, 30,  30,  255) }, // 물감 빨강
        { ColorID.Orange, new Color32(245, 120, 0,   255) }, // 물감 주황
        { ColorID.Yellow, new Color32(255, 225, 0,   255) }, // 물감 노랑
        { ColorID.Green,  new Color32(20,  170, 50,  255) }, // 물감 초록
        { ColorID.Blue,   new Color32(10,  100, 220, 255) }, // 물감 파랑
        
        // 프로젝트의 ColorID Enum 명칭에 맞춰 Navy/Purple 또는 Indigo/Violet 선택
        // 만약 Enum이 Navy/Purple이면 아래 그대로 사용, Indigo/Violet이면 수정 필요
        { ColorID.Navy,   new Color32(15,  30,  110, 255) }, // 물감 남색
        { ColorID.Purple, new Color32(130, 40,  160, 255) }  // 물감 보라
    };

    /// <summary>
    /// 가마솥에 투입된 색상들을 물감 감산 혼합 방식으로 섞어 반환합니다.
    /// </summary>
    public static Color GetBlendedColor(ColorID combinedId)
    {
        if (combinedId == ColorID.None) return Color.white; // 기본 솥/물약 투명-흰색

        List<Color> selectedColors = new List<Color>();

        // 비트 플래그 검사 (단일 비트 단위만 정확히 추출)
        foreach (KeyValuePair<ColorID, Color32> kvp in BaseColors)
        {
            // 해당 색상 비트가 켜져 있는지 확인
            if ((combinedId & kvp.Key) != 0)
            {
                selectedColors.Add(kvp.Value);
                if (selectedColors.Count >= 3) break; // 최대 3개 색상 제한
            }
        }

        if (selectedColors.Count == 0) return Color.white;
        if (selectedColors.Count == 1) return selectedColors[0];

        // 2개 이상의 색상을 다중 물감 혼합
        return MixMultiplePaints(selectedColors);
    }

    private static Color MixMultiplePaints(List<Color> colors)
    {
        int count = colors.Count;

        float totalC = 0f, totalM = 0f, totalY = 0f, totalK = 0f;

        foreach (Color c in colors)
        {
            float k = 1f - Mathf.Max(c.r, Mathf.Max(c.g, c.b));
            float denominator = 1f - k;

            float cComponent = (denominator <= 0.0001f) ? 0f : (1f - c.r - k) / denominator;
            float mComponent = (denominator <= 0.0001f) ? 0f : (1f - c.g - k) / denominator;
            float yComponent = (denominator <= 0.0001f) ? 0f : (1f - c.b - k) / denominator;

            totalC += cComponent;
            totalM += mComponent;
            totalY += yComponent;
            totalK += k;
        }

        float avgC = totalC / count;
        float avgM = totalM / count;
        float avgY = totalY / count;

        float avgK = Mathf.Clamp01((totalK / count) + (count - 1) * 0.05f);

        float r = (1f - avgC) * (1f - avgK);
        float g = (1f - avgM) * (1f - avgK);
        float b = (1f - avgY) * (1f - avgK);

        return new Color(Mathf.Clamp01(r), Mathf.Clamp01(g), Mathf.Clamp01(b), 1.0f);
    }
}