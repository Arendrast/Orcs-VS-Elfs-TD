using System;

namespace Modules.SharedModule.Scripts.Currencies
{
    public class CurrencyRepositoryService
    {
        public enum SetCurrencyOperation
        {
            Increase,
            Decrease
        }
        
        public int CurrencyNumber { get; private set; }
        
        public event Action<int> UpdatedCurrencyNumber;
        
        public bool MakeOperationOnCurrencyNumber(int number, SetCurrencyOperation operation) 
            // Инкапсулировал и валидировал логику расчёта итогового количества валюты внутрь сервиса
        {
            if (number <= 0)
            {
                return false;
            }

            switch (operation)
            {
                case SetCurrencyOperation.Increase:
                    CurrencyNumber += number;
                    break;
                case SetCurrencyOperation.Decrease:
                {
                    if (CurrencyNumber - number < 0)
                    {
                        return false;
                    }
                
                    CurrencyNumber -= number;
                    break;
                }
            }
            
            UpdatedCurrencyNumber?.Invoke(CurrencyNumber);

            return true;
        }
    }
}