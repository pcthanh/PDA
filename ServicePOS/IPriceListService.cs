using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicePOS.Model;

namespace ServicePOS
{
    public interface IPriceListService : IDisposable
    {
        IEnumerable<PriceListModel> GetDataProductAndModifire(int CurrentPage);
        int GetTotalProductAndModifire();
    }
}
