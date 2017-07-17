using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePOS
{
    public interface IPermissionService:IDisposable
    {
        int GetPermissionByDepartment(int departmentid, int menuid);
    }
}
