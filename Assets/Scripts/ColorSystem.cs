using System;

// MonoBehaviour를 상속받지 않습니다.
[Flags]
public enum ColorID
{
    None = 0,
    Red = 1 << 0,
    Yellow = 1 << 1,
    Blue = 1 << 2,
    Orange = Red | Yellow,
    Purple = Red | Blue,
    Green = Yellow | Blue,
    Brown = Red | Yellow | Blue
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

    public void AddIngredient(ColorID ingredient)
    {
        _currentMixture |= ingredient;
    }

    public ColorProduct CraftAndClear()
    {
        ColorProduct finalProduct = new ColorProduct(_currentMixture);
        _currentMixture = ColorID.None;
        return finalProduct;
    }

    public void ClearCauldron()
    {
        _currentMixture = ColorID.None;
    }
}