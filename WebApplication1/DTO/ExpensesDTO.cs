using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.DTO
{
    public class ExpensesDTO
    {
        public int ExpensesKey { get; set; }
        public string UserEmail { get; set; }
        public string KindOfExpenses { get; set; }
        public string ExpensesTitle { get; set; }
        public int PricePerOne { get; set; }
        public int NumberOfRepeatExpenses { get; set; }
        public int TotalPriceToPay { get; set; }
    }
}