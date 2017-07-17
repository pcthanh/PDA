using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicePOS.Model;
namespace ServicePOS
{
   public interface ICustomerService:IDisposable
    {
       int InsertCustomer(CustomerModel item);
       IEnumerable<CustomerModel> GetCustomer();
       IEnumerable<CustomerModel> GetCustomerSearch(string Name);
    }
}
