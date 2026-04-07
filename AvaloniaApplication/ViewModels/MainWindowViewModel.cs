using AvaloniaApplication.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Diagnostics;

namespace AvaloniaApplication.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {

        KingdomSimulator _simulator;

        [ObservableProperty]
        public int _food;

        [ObservableProperty]
        public int _gold;

        [ObservableProperty]
        public int _goods;

        [ObservableProperty]
        public int _villagers;
        
        [ObservableProperty]
        public int _craftmen;
        
        [ObservableProperty]
        public int _merchants;

        [ObservableProperty]
        public int _maxCitizensToBuyNum;

        [ObservableProperty]
        public int _citizensToBuyNum;

        [ObservableProperty]
        public string _toBuyNumInput;



        private CitezensTypes _choosedCitType;
        private int _currentCitizensToBuy;

        public MainWindowViewModel() : base() 
        {
            _simulator = new KingdomSimulator();
            _simulator.Initialize();

            SyncData();
        }

        private void SyncData()
        {
            var storage = _simulator.storage;

            Food = storage.Food;
            Gold = storage.Gold;
            Goods = storage.Goods;

            var home = _simulator.home;

            Villagers = home.Villagers.Count;
            Craftmen = home.Craftmen.Count;
            Merchants = home.Merchants.Count;
        }


        [RelayCommand]
        public void RunNextDay()
        {
            _simulator.RunSimulation();
            SyncData();
        }

        [RelayCommand]
        public void SwitchRadioButtons(string strType)
        {
            var choosedCitWas = _choosedCitType;
            bool succ = Enum.TryParse<CitezensTypes>(strType, out var result);
            if (succ)
            {
                _choosedCitType = result;
            }
            else
            {
                return;
            }

            if(choosedCitWas == _choosedCitType)
                return;

            int canBuyNum = _simulator.GetCitizensMaxNumToBuy(_choosedCitType);
            MaxCitizensToBuyNum = canBuyNum;

            int citizenToBuyNum = canBuyNum / 2;
            CitizensToBuyNum = citizenToBuyNum;
            ToBuyNumInput = (citizenToBuyNum).ToString();

            _currentCitizensToBuy = citizenToBuyNum;

            Debug.WriteLine(_choosedCitType);
        }

        public void OnSliderValueChanged()
        {
            var currentSliderValue = CitizensToBuyNum;

            if (_currentCitizensToBuy == currentSliderValue)
                return;

            ToBuyNumInput = currentSliderValue.ToString();
            _currentCitizensToBuy = currentSliderValue;
        }


        public void OnTextBoxValueChanged()
        {
            var currentText = ToBuyNumInput;

            bool succ = Int32.TryParse(currentText, out int parseResult);
            if(succ == false)
            {
                ToBuyNumInput = "";
                return;
            }

            int maxNumToBuy = _simulator.GetCitizensMaxNumToBuy(_choosedCitType);
            int clampedResult = Math.Clamp(parseResult, 0, maxNumToBuy);

            if(clampedResult != parseResult)
            {
                ToBuyNumInput = clampedResult.ToString();
            }

            if (_currentCitizensToBuy == parseResult)
                return;

            CitizensToBuyNum = clampedResult;
            _currentCitizensToBuy = clampedResult;
        }

        [RelayCommand]
        public void OnBuyCitizens()
        {
            if(_currentCitizensToBuy <= 0)
                return;

            if(_choosedCitType == CitezensTypes.None)
                return;

            _simulator.BuyCitizens(_choosedCitType, _currentCitizensToBuy);
            SyncData();
        }

    }
}
