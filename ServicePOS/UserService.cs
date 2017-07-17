using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemLog;
using ModelPOS.ModelEntity;
using ServicePOS.Model;

namespace ServicePOS
{
    public class UserService : IUserService
    {
        #region Variables and Constructors

        private POSEZ2UEntities _context;
        public UserService()
        {
            _context = new POSEZ2UEntities();
        }

        public UserService(POSEZ2UEntities context)
        {
            _context = context;
        }

        public void SetProxyCreationEnabled(bool proxyCreationEnabled)
        {
            _context.Configuration.ProxyCreationEnabled = proxyCreationEnabled;
        }
        #endregion

        #region Function

        #region Department

        public IEnumerable<DepartmentModel> GetListDepartment()
        {
            var data = _context.DEPARTMENTs.Where(x => x.Status == 1).Select(x => new DepartmentModel
            {
                DepartmentID = x.DepartmentID,
                DepartmentName = x.DepartmentName,
                Status = x.Status
            });

            return data;
        }

        public int SaveDataDeparment(DepartmentModel data)
        {
            try
            {
                if (data.DepartmentID == 0)
                {
                    var departmentcheck = _context.DEPARTMENTs.Where(x => x.DepartmentName == data.DepartmentName).ToList();
                    if (departmentcheck.Count > 0)
                    {
                        return -1;
                    }
                    var department = new DEPARTMENT();
                    department.DepartmentName = data.DepartmentName;
                    department.Status = 1;
                    department.CreateBy = data.UpdateBy;
                    department.CreateDate = DateTime.Now;
                    department.UpdateBy = data.UpdateBy;
                    department.UpdateDate = DateTime.Now;

                    _context.Entry(department).State = EntityState.Added;
                    _context.SaveChanges();
                    return 1;
                }
                else
                {
                    var departmentcheck = _context.DEPARTMENTs.Where(x => x.DepartmentName == data.DepartmentName && x.DepartmentID!=data.DepartmentID).ToList();
                    if (departmentcheck.Count > 0)
                    {
                        return -1;
                    }
                    var department = _context.DEPARTMENTs.Find(data.DepartmentID);
                    if (department != null)
                    {
                        department.DepartmentName = data.DepartmentName;
                        department.UpdateBy = data.UpdateBy;
                        department.UpdateDate = DateTime.Now;
                        _context.Entry(department).State = EntityState.Modified;
                        _context.SaveChanges();
                        return 1;
                    }
                    return 0;
                }
            }
            catch (Exception ex)
            {
                LogPOS.WriteLog("SaveDataDeparment :::::::::::::::::::::::::" + ex.Message);
                return 0;
            }
        }

        public int RemoveDepartment(int departmentid, int userid)
        {
            try
            {
                var data = _context.DEPARTMENTs.Find(departmentid);
                if (data != null)
                {
                    data.Status = 0;
                    data.UpdateBy = userid;
                    data.UpdateDate = DateTime.Now;

                    _context.Entry(data).State = EntityState.Modified;
                    _context.SaveChanges();

                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                LogPOS.WriteLog("RemoveDepartment :::::::::::::::::::::::::" + ex.Message);
                return 0;
            }
        }

        public IEnumerable<SubMenuModel> GetSubMenuByDepartment(int departmentid)
        {

            var data = _context.PERMISSIONs.Join(_context.SUB_MENU, pe => pe.SubMenuID,
                sb => sb.SubMenuID, (pe, sb) => new { pe, sb })
                .Where(x => x.pe.DepartmentID == departmentid && x.pe.Status == 1)
                .Select(x => new SubMenuModel()
                {
                    SubMenuID = x.sb.SubMenuID,
                    SubMenuName = x.sb.SubMenuName,
                    Status = x.sb.Status
                });

            return data;
        }


        public IEnumerable<SubMenuModel> GetAllListSubMenuByDepartment(int departmentid)
        {
            var data = _context.Database.SqlQuery<SubMenuModel>("pos_th_GetAllListPermissionByDepartmentID @departmentid",
              new SqlParameter("departmentid", departmentid)
            ).ToList();
            return data;
        }

        public int SaveMapPermission(List<SubMenuModel> data, int departmentid, int userid)
        {
            try
            {
                var tabletemp = new DataTable();
                tabletemp.Columns.Add("Value");

                foreach (var item in data)
                {
                    tabletemp.Rows.Add(item.SubMenuID);
                }

                var tableSubMenutemp = new SqlParameter("tablesubmenu", SqlDbType.Structured);
                tableSubMenutemp.Value = tabletemp;
                tableSubMenutemp.TypeName = "dbo.TableTemp";

                var result = _context.Database.ExecuteSqlCommand("pos_th_SaveDataPermissionByDepartmet @departmentid,@userid,@tablesubmenu",
                  new SqlParameter("departmentid", departmentid),
                  new SqlParameter("userid", userid),
                  tableSubMenutemp
               );


                return 1;
            }
            catch (Exception ex)
            {
                LogPOS.WriteLog("SaveMapPermission :::::::::::::::::::::::::" + ex.Message);
                return 0;
            }

        }

        #endregion


        #region Staff

        public IEnumerable<StaffModel> GetListStaff()
        {
            var data = _context.STAFFs.Where(x => x.Status == 1).Select(x => new StaffModel
            {
                StaffID = x.StaffID,
                Fname = x.Fname,
                Lname = x.Lname,
                UserName = x.UserName,
                Password = x.Password,
                DepartmentID = x.DepartmentID,
                Status = x.Status
            });

            return data;
        }

        public int SaveDataStaff(StaffModel data)
        {
            try
            {
                if (data.StaffID == 0)
                {
                    var staffcheck = _context.STAFFs.Where(x => x.UserName == data.UserName).ToList();
                    if (staffcheck.Count > 0)
                    {
                        return -1;
                    }
                    var staff = new STAFF();
                    staff.Status = 1;
                    staff.Fname = data.Fname;
                    staff.Lname = data.Lname;
                    staff.UserName = data.UserName;
                    staff.Password = StaffModel.Encrypt(data.Password);
                    staff.CreateBy = data.UpdateBy;
                    staff.CreateDate = DateTime.Now;
                    staff.UpdateBy = data.UpdateBy;
                    staff.UpdateDate = DateTime.Now;
                    staff.DepartmentID = data.DepartmentID;

                    _context.Entry(staff).State = EntityState.Added;
                    _context.SaveChanges();

                    return 1;
                }
                else
                {
                    var staffcheck = _context.STAFFs.Where(x => x.UserName == data.UserName && x.StaffID!=data.StaffID).ToList();
                    if (staffcheck.Count > 0)
                    {
                        return -1;
                    }

                    var staff = _context.STAFFs.Find(data.StaffID);
                    if (staff != null)
                    {
                        staff.Fname = data.Fname;
                        staff.Lname = data.Lname;
                        staff.UserName = data.UserName;
                        staff.Password = StaffModel.Encrypt(data.Password);
                        staff.UpdateBy = data.UpdateBy;
                        staff.UpdateDate = DateTime.Now;
                        staff.DepartmentID = data.DepartmentID;

                        _context.Entry(staff).State = EntityState.Modified;
                        _context.SaveChanges();

                        return 1;
                    }
                    return 0;
                }
            }
            catch (Exception ex)
            {
                LogPOS.WriteLog("SaveDataStaff :::::::::::::::::::::::::" + ex.Message);
                return 0;
            }
        }

        public int RemoveStaff(int staffid, int userid)
        {
            try
            {
                var data = _context.STAFFs.Find(staffid);
                if (data != null)
                {
                    data.UserName = data.UserName+DateTime.Now.ToShortDateString();
                    data.Status = 0;
                    data.UpdateBy = userid;
                    data.UpdateDate = DateTime.Now;

                    _context.Entry(data).State = EntityState.Modified;
                    _context.SaveChanges();

                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                LogPOS.WriteLog("RemoveStaff :::::::::::::::::::::::::" + ex.Message);
                return 0;
            }
        }

        #endregion

        #endregion

        #region Dispose
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // code is here
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion


        public IEnumerable<StaffModel> GetStaffByUserName(string UserName)
        {
            var User = _context.STAFFs.Where(x => x.UserName == UserName && x.Status == 1)
                .Select(x => new StaffModel { 
                    Password = x.Password,
                    UserName = x.UserName
                }
                );
            return User;
        }
    }
}
