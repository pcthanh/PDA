using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicePOS.Model;
using System.Collections;
namespace ServicePOS
{
   public interface IOrderService:IDisposable
    {
       int InsertOrder(OrderDateModel itemOrder);
       int CheckOrderComplete(OrderDateModel idOrder);
       IEnumerable<OrderDateModel> LoadOrder(OrderDateModel idOrder);
       int UpdateOrder(OrderDateModel idOrder);
       OrderDateModel GetOrderByTable(string idTable, int idOrder);
       int CountOrder();
       int CountOrderModifire();
       StatusTable GetStatusTable(string TableID);
       IEnumerable<CardModel> LoadCard();
       int InsertOpenItem(OrderOpenItemModel OpenItem);
       int GetIDLastInsertOpenItem();
       OrderDateModel GetOrderByTKA(string tkaID, string ClientID);
       List< OrderTKAModel> GetStatusOrderTKA();
       int CountTotalEaIn();
       int CountTotalTKA();
       int JoinTable(List<OrderJoinTableModel> lstOrderJoin);
       int DeleteJoinTableAll(OrderDateModel itemOrder);
       int VoidItemHistory(OrderDateModel OrderVoid);
       IEnumerable<OrderDateModel> GetPrevOrder();
       OrderDateModel GetListOrderPrevOrder(string idTable, int idOrder,DateTime ts);
       int CancelOrder(OrderDateModel Order);
       int LastDynID();
       StatusTable GetStatusTablePrinBill(string TableID);
       int DeleteTransferTableAll(OrderDateModel itemOrder);

       int GetOrderID();
       string GetOrderNumber();

       IEnumerable<StatusTable> GetListStatusTable(string TableID);
    }
}
