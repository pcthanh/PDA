using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicePOS.Model;
namespace ServicePOS
{
   public interface IInvoiceService:IDisposable
    {
       int CountInvoie();
       int InsertInvoice(OrderDateModel Order);
       int UpdateOrderInvice(OrderDateModel Order);
    }
}
