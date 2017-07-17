using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicePOS.Model;

namespace ServicePOS
{
    public interface IMapModifireToProduct : IDisposable
    {
        int SaveMapModifireToProduct(List<ModifireModel> data, int productId, int userid);
    }
}
