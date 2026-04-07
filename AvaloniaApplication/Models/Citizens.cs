using System;
using System.Collections.Generic;

public interface IWorker
{
    public void DoYouWork(ref KingdomStorage kingdom);
}





public struct Villager : IWorker
{
    private const int FoodProduction = 3;

    public void DoYouWork(ref KingdomStorage storage)
    {
        storage.Food += FoodProduction;
    }
}



public struct Craftman : IWorker
{
    private const int FoodConsume = 1;
    private const int GoodProduction = 1;

    public void DoYouWork(ref KingdomStorage kingdom)
    {
        kingdom.Food -= FoodConsume;
        kingdom.Goods += GoodProduction;
    }
}

public struct Merchant : IWorker
{
    private const int FoodConsume = 2;
    private const int GoodsConsume = 1;
    private const int GoldProducation = 5;

    public void DoYouWork(ref KingdomStorage kingdom)
    {
        kingdom.Food -= FoodConsume;
        kingdom.Goods -= GoodsConsume;
        kingdom.Gold += GoldProducation;
    }
}

public struct Knight : IWorker
{
    public void DoYouWork(ref KingdomStorage kingdom)
    {

    }
}

public struct Bandit : IWorker
{
    public void DoYouWork(ref KingdomStorage kingdom)
    {

    }
}

public struct KingOfTheEnglandAndScotich : IWorker
{
    public void DoYouWork(ref KingdomStorage kingdom)
    {

    }
}



public struct CitizensHome
{
    public List<Villager> Villagers;
    public List<Craftman> Craftmen;
    public List<Merchant> Merchants;
    public List<Knight> Knights;
    public List<Bandit> Bandits;
}