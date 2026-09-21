using System;

// ColorSystem 클래스 감싸기를 없애고 최외곽에 바로 선언합니다.

[Flags]
public enum ColorID
{
    None   = 0,
    Red    = 1 << 0,
    Orange = 1 << 1,
    Yellow = 1 << 2,
    Green  = 1 << 3,
    Blue   = 1 << 4,
    Navy = 1 << 5,
    Purple = 1 << 6,
    Dummy  = -2
}

public class ColorProduct
{
    public ColorID ID { get; private set; }
    public int Tier { get; private set; }

    public ColorProduct(ColorID combinedId)
    {
        ID = combinedId;
        Tier = CalculateTier(combinedId);
    }

    private int CalculateTier(ColorID id)
    {
        int count = 0, bits = (int)id;
        while (bits > 0) { count += bits & 1; bits >>= 1; }
        return count;
    }
}

public class ColorMixer
{
    private ColorID _currentMixture = ColorID.None;
    public ColorID CurrentMixture => _currentMixture;

    public int IngredientCount
    {
        get
        {
            int count = 0, bits = (int)_currentMixture;
            while (bits > 0) { count += bits & 1; bits >>= 1; }
            return count;
        }
    }

    public bool AddIngredient(ColorID ingredient)
    {
        if (IngredientCount >= 3 && (_currentMixture & ingredient) == 0)
        {
            return false;
        }

        _currentMixture |= ingredient;
        return true;
    }

    public ColorProduct CraftAndClear()
    {
        ColorProduct finalProduct = new ColorProduct(_currentMixture);
        ClearCauldron();
        return finalProduct;
    }

    public void ClearCauldron()
    {
        _currentMixture = ColorID.None;
    }
}