using Markit.PET.AdminPanel.CommonLibrary;
using Markit.PET.AdminPanel.UI.Models;
using Markit.PET.AdminPanel.UI.RestServiceHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Markit.PET.AdminPanel.UI.Controllers
{
    [RoutePrefix("AdminPanel")]
    public class TeamTaskTypeDetailController : Controller
    {
        // GET: TeamTaskTypeDetail
        public ActionResult Index()
        {
            TeamTaskTypeDetailViewModel _model = new TeamTaskTypeDetailViewModel();
            _model.EmployeeList = ServiceInfo.InvokeGetService<List<EmployeeEntity>>(ServiceInfo.GetAllEmployees, string.Empty);
            Session.Add("AllEmployeeList", _model.EmployeeList);
            _model.TeamList = ServiceInfo.InvokeGetService<List<TeamEntity>>(ServiceInfo.GetAllTeams, string.Empty);
            Session.Add("AllTeamList", _model.TeamList);
            _model.SelectedTeam = new TeamEntity();
            _model.SelectedEmployee = new EmployeeEntity();

            return View(_model);
        }

        [HttpGet]
        public JsonResult SearchEmployee(string Prefix)
        {
            List<EmployeeEntity> ObjList = (List<EmployeeEntity>)Session["AllEmployeeList"];
            try
            {
                if (Prefix.Length < 3)
                {
                    return Json(string.Empty, JsonRequestBehavior.AllowGet);
                }

                var CityName = ObjList.Where(c => c.EmployeeName.ToUpper().Trim().Contains(Prefix.ToUpper()));
                return Json(CityName, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public JsonResult SearchTeam(string Prefix)
        {
            List<TeamEntity> ObjList = (List<TeamEntity>)Session["AllTeamList"];
            if (Prefix.Length < 3)
            {
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
            //Searching records from list using LINQ query
            var CityName = (from N in ObjList
                            where N.TeamName.ToUpper().Trim().Contains(Prefix.ToUpper())
                            select new { N.TeamName, N.TeamID });
            return Json(CityName, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployee(string empName)
        {
            EmployeeTeamDetail _obj = new EmployeeTeamDetail();
            _obj.Employee = GetEmpDetails(empName);

            if(_obj.Employee != null )
            {
                _obj.Team = ServiceInfo.InvokePostService<TeamEntity>(ServiceInfo.GetTeamDetailByEmployeeID, JsonConvert.SerializeObject(_obj.Employee));
            }

            Session.Add("CurrentEmployeeTeamDetail", _obj);

            return Json(_obj, JsonRequestBehavior.AllowGet);
        }

        public EmployeeEntity GetEmpDetails(string empName)
        {
            EmployeeEntity objEmp = new EmployeeEntity();
            try
            {
                if (!string.IsNullOrEmpty(empName))
                {
                    //List<EmployeeEntity> lstOrgs = (List<EmployeeEntity>)Session["AllEmployeeList"];

                    List<EmployeeEntity> lstOrgs = ServiceInfo.InvokeGetService<List<EmployeeEntity>>(ServiceInfo.GetAllEmployees, string.Empty);

                    var org = lstOrgs.FirstOrDefault(p => p.AccountName == empName);
                    if (org != null && !string.IsNullOrEmpty(org.EmployeeUEN) && org.EmployeeUEN != "0")
                    {
                        objEmp.EmployeeUEN = org.EmployeeUEN;
                        objEmp.EmployeeName = org.EmployeeName;
                        objEmp.AccountName = org.AccountName;
                        objEmp.EmployeeEmail = org.EmployeeEmail;
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return objEmp;
        }

        public JsonResult GetTeam(string teamName)
        {
            return Json(GetTeamDetails(teamName), JsonRequestBehavior.AllowGet);
        }

        public TeamEntity GetTeamDetails(string teamName)
        {
            TeamEntity objEmp = new TeamEntity();

            if (!string.IsNullOrEmpty(teamName))
            {
                List<TeamEntity> lstOrgs = (List<TeamEntity>)Session["AllTeamList"];

                var org = lstOrgs.FirstOrDefault(p => p.TeamID.ToString() == teamName);
                if (org != null && !string.IsNullOrEmpty(org.TeamName) && org.TeamID != 0)
                {
                    objEmp.TeamID = org.TeamID;
                    objEmp.TeamName = org.TeamName;
                    objEmp.TeamLogicalID = org.TeamLogicalID;
                }
            }

            return objEmp;
        }

        public ActionResult UpdateTeam(TeamTaskTypeDetailViewModel _model)
        {
            var _currentDetail = (EmployeeTeamDetail)Session["CurrentEmployeeTeamDetail"];

            EmployeeTeamDetailSaveEntity obj = new EmployeeTeamDetailSaveEntity()
            {
                EmployeeUEN = _model.SelectedEmployee.EmployeeUEN.Split(':')[1],
                IsUpdate = _currentDetail == null || _currentDetail.Team == null || _currentDetail.Team.TeamID == 0 ? false : true,
                TeamLogicalID = _model.SelectedTeam.TeamLogicalID.Split(':')[1],
                UserName = "nikhil.verma"
            };
            var _list = ServiceInfo.InvokePostService<TeamEntity>(ServiceInfo.SaveEmployeeTeamDetail, JsonConvert.SerializeObject(obj));
            _model.IsEmployeeTeamUpdated = true;

            List<TeamEntity> _teamList = (List<TeamEntity>)Session["AllTeamList"];
            List<EmployeeEntity> _empList = (List<EmployeeEntity>)Session["AllEmployeeList"];

            _model.SelectedEmployee = _empList.Where(c => c.EmployeeUEN == _model.SelectedEmployee.EmployeeUEN.Split(':')[1]).FirstOrDefault();
            _model.SelectedTeam = _teamList.Where(c => c.TeamLogicalID == _model.SelectedTeam.TeamLogicalID.Split(':')[1]).FirstOrDefault();

            return View("Index", _model);
            //return RedirectToAction("GetEmployee", new { empName = _currentDetail.Employee.EmployeeName });
        }
    }
}