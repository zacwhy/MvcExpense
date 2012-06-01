using System;
using MvcExpense.Models;

namespace MvcExpense.DAL
{
    public interface IOrdinaryExpenseRepository
    {
        OrdinaryExpense GetById( object id );
        void Insert( OrdinaryExpense ordinaryExpense );
        void Delete( object id );
        void Update( OrdinaryExpense ordinaryExpense );
    }
}