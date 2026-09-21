using System.Collections.Generic;
using UnityEngine;

public static class ColorBlendTable
{
    private static readonly Dictionary<ColorID, Color32> BlendDictionary = new Dictionary<ColorID, Color32>()
    {
        // ==========================================
        // 1. 단일 색상 (재료 1개만 넣었을 때)
        // ==========================================
        { ColorID.Red,    new Color32(255, 0, 0, 255) },
        { ColorID.Orange, new Color32(255, 127, 0, 255) },
        { ColorID.Yellow, new Color32(255, 255, 0, 255) },
        { ColorID.Green,  new Color32(0, 255, 0, 255) },
        { ColorID.Blue,   new Color32(0, 0, 255, 255) },
        { ColorID.Navy,   new Color32(0, 0, 128, 255) },
        { ColorID.Purple, new Color32(128, 0, 128, 255) },

        // ==========================================
        // 2. 2가지 색상 혼합
        // ==========================================
        // 빨강 조합
        { ColorID.Red | ColorID.Orange, new Color32(255, 69, 0, 255) },
        { ColorID.Red | ColorID.Yellow, new Color32(255, 165, 0, 255) },
        { ColorID.Red | ColorID.Green,  new Color32(139, 69, 19, 255) },
        { ColorID.Red | ColorID.Blue,   new Color32(128, 0, 128, 255) },
        { ColorID.Red | ColorID.Navy,   new Color32(75, 0, 130, 255) },
        { ColorID.Red | ColorID.Purple, new Color32(199, 21, 133, 255) },

        // 주황 조합
        { ColorID.Orange | ColorID.Yellow, new Color32(255, 191, 0, 255) },
        { ColorID.Orange | ColorID.Green,  new Color32(154, 205, 50, 255) },
        { ColorID.Orange | ColorID.Blue,   new Color32(139, 69, 19, 255) },
        { ColorID.Orange | ColorID.Navy,   new Color32(101, 67, 33, 255) },
        { ColorID.Orange | ColorID.Purple, new Color32(165, 42, 42, 255) },

        // 노랑 조합
        { ColorID.Yellow | ColorID.Green,  new Color32(154, 205, 50, 255) },
        { ColorID.Yellow | ColorID.Blue,   new Color32(0, 128, 0, 255) },
        { ColorID.Yellow | ColorID.Navy,   new Color32(128, 128, 0, 255) },
        { ColorID.Yellow | ColorID.Purple, new Color32(184, 134, 11, 255) },

        // 초록 조합
        { ColorID.Green | ColorID.Blue,   new Color32(0, 128, 128, 255) },
        { ColorID.Green | ColorID.Navy,   new Color32(0, 100, 100, 255) },
        { ColorID.Green | ColorID.Purple, new Color32(128, 128, 96, 255) },

        // 파랑 조합
        { ColorID.Blue | ColorID.Navy,   new Color32(0, 0, 160, 255) },
        { ColorID.Blue | ColorID.Purple, new Color32(75, 0, 130, 255) },

        // 남색 조합
        { ColorID.Navy | ColorID.Purple, new Color32(72, 0, 100, 255) }
    };

    public static Color GetBlendedColor(ColorID combinedId)
    {
        if (BlendDictionary.TryGetValue(combinedId, out Color32 resultColor))
        {
            return resultColor;
        }

        // 정의되지 않은 3가지 이상 혼합이나 에러 시 기본 회색/탁한 색 처리
        return new Color32(120, 120, 120, 255); 
    }
}