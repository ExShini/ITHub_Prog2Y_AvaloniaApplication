using AvaloniaApplication.Models;
using Lesson2;
using System;
using System.Collections.Generic;
using System.Text;


class KingdomSimulator
{
    const int VilagersStartCount = 30;
    const int CraftmanStartCount = 10;
    const int MerchantStartCount = 3;
    const int KnightStartCount = 3;
    const int BanditStartCount = 3;

    const int StartFoodStoarge = 10;
    const int StartGoldStoarge = 5;
    const int StartGoodStoarge = 2;


    const int VilagersCost = 1;
    const int CraftmanCost = 3;
    const int MerchantCost = 3;
    const int KnightCost = 5;


    public KingdomStorage storage { get; private set; }
    public CitizensHome home { get; private set; }
    int day = 0;


    public void Initialize()
    {
        storage = new KingdomStorage()
        {
            Food = StartFoodStoarge,
            Gold = StartGoldStoarge,
            Goods = StartGoodStoarge,
        };

        home = new CitizensHome()
        {
            Villagers = new List<Villager>(),
            Craftmen = new List<Craftman>(),
            Merchants = new List<Merchant>(),
            Knights = new List<Knight>(),
            Bandits = new List<Bandit>(),
        };


        // создаем жителей
        CreateCitizens(home.Villagers, VilagersStartCount);
        CreateCitizens(home.Craftmen, CraftmanStartCount);
        CreateCitizens(home.Merchants, MerchantStartCount);
        CreateCitizens(home.Knights, KnightStartCount);
        CreateCitizens(home.Bandits, BanditStartCount);
    }




    public void RunSimulation()
    {
        storage.SaveStorageData();

        // игровой цикл

        Console.WriteLine("***********\nDay: " + day);
        PrintCitizens(home);
        storage.ShowStorage();

        // гоним всех на работу
        storage.SaveStorageData();

        var currentStorage = storage;

        GoToYouJob(home.Villagers, ref currentStorage);
        GoToYouJob(home.Craftmen, ref currentStorage);
        GoToYouJob(home.Merchants, ref currentStorage);
        GoToYouJob(home.Knights, ref currentStorage);
        GoToYouJob(home.Bandits, ref currentStorage);

        storage = currentStorage;

        Console.ReadLine();
        day++;
    }



    class IntValue
    {
        public int Value;
    }


    static void DoShomething(ref int data)
    {
        data++;
    }

    static void DoShomething(IntValue data)
    {
        data.Value++;
    }



    public void BuyCitizens(CitezensTypes type, int numToBuy)
    {
        // ЛОГИКА ПОКУПКИ !!!
    }



    private static void HandleMenu(
        ref KingdomStorage storage,
        ref CitizensHome home)
    {
        // состояние меню
        MenuState state = MenuState.MainMenu;
        while (state != MenuState.RunNextDay)
        {
            switch (state)
            {
                case MenuState.MainMenu:
                    MainMenuLogic(ref state);
                    break;

                case MenuState.BuyVillagers:
                    BuyCitizens(home.Villagers, ref storage, VilagersCost);
                    state = MenuState.MainMenu;
                    break;

                case MenuState.BuyCraftmans:
                    BuyCitizens(home.Craftmen, ref storage, CraftmanCost);
                    state = MenuState.MainMenu;
                    break;

                case MenuState.BuyMerchants:
                    BuyCitizens(home.Merchants, ref storage, MerchantCost);
                    state = MenuState.MainMenu;
                    break;
            }
        }
    }

    private static void MainMenuLogic(ref MenuState state)
    {
        Console.WriteLine(
        "Choose your order!\n" +
        "1. buy villagers\n" +
        "2. buy craftmans\n" +
        "3. buy merchants\n" +
        "0. next day");

        var responceStr = Console.ReadLine();

        int responceInt = 0;
        int.TryParse(responceStr, out responceInt);


        switch (responceInt)
        {
            case 0:
                state = MenuState.RunNextDay;
                break;
            // крестьяне
            case 1:
                state = MenuState.BuyVillagers;
                break;
            // ремесленики
            case 2:
                state = MenuState.BuyCraftmans;
                break;
            // купцы
            case 3:
                state = MenuState.BuyMerchants;
                break;

            default:
                break;
        }
    }



    public int GetCitizensMaxNumToBuy(CitezensTypes citType)
    {
        int numCanBuy = 0;

        switch (citType)
        {
            case CitezensTypes.Villager:
                numCanBuy = storage.Gold / VilagersCost;
                break;
            case CitezensTypes.Craftman:
                numCanBuy = storage.Gold / CraftmanCost;
                break;
            case CitezensTypes.Merchant:
                numCanBuy = storage.Gold / MerchantCost;
                break;
        }

        return numCanBuy;
    }






    // покупка жителей
    private static void BuyCitizens<CitisensType>(
        List<CitisensType> citizensCollection,
        ref KingdomStorage storage,
        int cost)

        where CitisensType : struct
    {
        int canBuyNum = storage.Gold / cost;

        Console.WriteLine(string.Format("Cost: {0}, you can buy {1}", cost, canBuyNum));
        Console.WriteLine("Choose the number to buy: ");
        var outputStr = Console.ReadLine();

        int.TryParse(outputStr, out int numberToBuy);

        if (numberToBuy > canBuyNum)
            numberToBuy = canBuyNum;

        //логика покупки
        for (int i = 0; i < numberToBuy; i++)
        {
            var citizenToAdd = new CitisensType();
            citizensCollection.Add(citizenToAdd);
        }
        storage.Gold -= numberToBuy * cost;
    }


    private static void PrintCitizens(CitizensHome home)
    {
        Console.WriteLine(string.Format("Villagers : {0}", home.Villagers.Count));
        Console.WriteLine(string.Format("Craftmans : {0}", home.Craftmen.Count));
        Console.WriteLine(string.Format("Merchants : {0}", home.Merchants.Count));
        Console.WriteLine(string.Format("Knights : {0}", home.Knights.Count));
        Console.WriteLine(string.Format("Bandits : {0}", home.Bandits.Count));
    }





















    private static void CreateCitizens<CitizenType>(
        List<CitizenType> collection,
        int numToCreate)

        where CitizenType : struct
    {
        for (int i = 0; i < numToCreate; i++)
        {
            CitizenType citezen = new CitizenType();
            collection.Add(citezen);
        }
    }








    private void GoToYouJob<WorkerType>(
        List<WorkerType> workers,
        ref KingdomStorage storage)

        where WorkerType : struct, IWorker
    {
        for (int i = 0; i < workers.Count; i++)
        {
            WorkerType worker = workers[i];
            worker.DoYouWork(ref storage);
        }
    }








}



public struct KingdomStorage
{
    private int _goldWas;
    private int _foodWas;
    private int _goodsWas;

    public int Gold;
    public int Food;
    public int Goods;

    public void SaveStorageData()
    {
        _goldWas = Gold;
        _foodWas = Food;
        _goodsWas = Goods;
    }

    public void ShowStorage()
    {
        Console.WriteLine(
            string.Format("\nGold: {0} ({1})", Gold, GetSigned(Gold - _goldWas)));

        Console.WriteLine(
            string.Format("Food: {0} ({1})", Food, GetSigned(Food - _foodWas)));

        Console.WriteLine(
            string.Format("Gold: {0} ({1})", Goods, GetSigned(Goods - _goodsWas)));
    }

    private string GetSigned(int value)
    {
        return value > 0 ? "+" + value : "-" + value;
    }
}
