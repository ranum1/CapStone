using System;

// MonoBehaviour�� ��ӹ��� �ʽ��ϴ�.
[Flags]
public enum ColorID
{
    None = 0,
    Magenta = 1 << 0,
    Yellow = 1 << 1,
    Cyan = 1 << 2,
    Red = Magenta | Yellow,
    Blue = Magenta | Cyan,
    Green = Yellow | Cyan,
    Black = Magenta | Yellow | Cyan,

    Dummy = -2
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