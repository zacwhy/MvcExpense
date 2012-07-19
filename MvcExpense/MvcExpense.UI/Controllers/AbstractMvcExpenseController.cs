using MvcExpense.Core;
using Zac.StandardMvc;

namespace MvcExpense.UI.Controllers
{
    public abstract partial class AbstractMvcExpenseController : AbstractStandardController
    {
        //public IMvcExpenseUnitOfWork MvcExpenseUnitOfWork
        //{
        //    get { return (IMvcExpenseUnitOfWork) StandardUnitOfWork; }
        //}

        protected AbstractMvcExpenseController()
            : base()
        {
        }

        protected AbstractMvcExpenseController( IMvcExpenseUnitOfWork unitOfWork )
            //: base( unitOfWork )
        {
        }

    }
}