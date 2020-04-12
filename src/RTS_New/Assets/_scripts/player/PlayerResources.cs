public class PlayerResources
{
    private int _gold;
    private int _lumber;
    private int _iron;
    private int _food;

    private int _maxFood;
    
    public PlayerResources()
    {
        _gold = 1000;
        _lumber = 1000;
        _iron = 1000;
        _food = 0;
        _maxFood = 24;
    }

    public ResourceData Data => new ResourceData(_gold,_lumber,_iron,_food);

    public int MaxFood => _maxFood;
    
    public bool CanAffordCost(ResourceData cost)
    {
        var enoughGold = _gold >= cost.Gold;
        var enoughLumber = _lumber >= cost.Lumber;
        var enoughIron = _iron >= cost.Iron;
        var enoughFood = _food + cost.Food <= _maxFood;
        return enoughGold && enoughLumber && enoughIron && enoughFood;
    }

    public void DeductResourceCost(ResourceData cost)
    {
        _gold -= cost.Gold;
        _lumber -= cost.Lumber;
        _iron -= cost.Iron;
        _food += cost.Food;
    }

    public void AddToResources(ResourceType type, int amount)
    {
        switch (type)
        {
            case ResourceType.Gold:
                _gold += amount;
                break;
            case ResourceType.Iron:
                _iron += amount;
                break;
            case ResourceType.Lumber:
                _lumber += amount;
                break;
        }
    }
    
    public void RefundResourceCost(ResourceData cost)
    {
        _gold += cost.Gold;
        _lumber += cost.Lumber;
        _iron += cost.Iron;
        _food -= cost.Food;
    }

    public void ChangeFoodCapacity(int change)
    {
        _maxFood += change;
    }

    public void ChangeFoodUsage(int change)
    {
        _food += change;
    }
    
}