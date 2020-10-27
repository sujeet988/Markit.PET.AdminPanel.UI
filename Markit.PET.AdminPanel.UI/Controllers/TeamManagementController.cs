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
    public class TeamManagementController : Controller
    {
        // GET: TeamManagement
        public ActionResult Index()
        {
            TeamManagementViewModel _model = new TeamManagementViewModel();
            _model.TeamList = ServiceInfo.InvokeGetService<List<TeamEntity>>(ServiceInfo.GetAllTeams, string.Empty);
            Session.Add("TeamList", _model.TeamList);
            return View(_model);
        }

        [HttpGet]
        public ActionResult CreateUpdateTeamForm(int teamID)
        {
            TeamManagementViewModel _model = new TeamManagementViewModel();
            _model.TeamList = (List<TeamEntity>)Session["TeamList"];
            _model.OrganisationList = ServiceInfo.InvokeGetService<List<Organisation>>(ServiceInfo.GetAllOrganisation, string.Empty);

            Session.Add("OrganisationList", _model.OrganisationList);
            if (teamID != 0) {
                _model.SelectedTeam = _model.TeamList.Where(c => c.TeamID == teamID).FirstOrDefault();
                _model.SelectedOrganisation = _model.OrganisationList.Where(c => c.LogicalID == _model.SelectedTeam.LogicalOrganisationID).FirstOrDefault();
            }
            else
            {
                if (_model.SelectedTeam == null)
                {
                    _model.SelectedTeam = new TeamEntity() { TeamLogicalID = string.Empty };
                    _model.SelectedOrganisation = new Organisation();
                }
            }

            return View(_model);
        }

        [HttpPost]
        public ActionResult CreateUpdateTeam(TeamManagementViewModel _model)
        {
            _model.SelectedTeam.LogicalFunactionalAreaID = "000001";
            var _lst = _model.SelectedOrganisation.OrgName.Split(':');

            _model.SelectedTeam.LogicalOrganisationID = _lst.Last();
            if (_model.SelectedTeam.TeamID == 0) { _model.SelectedTeam.TeamLogicalID = Guid.NewGuid().ToString(); }
            var _isCreated = ServiceInfo.InvokePostService<bool>(ServiceInfo.CreateTeam, JsonConvert.SerializeObject(_model.SelectedTeam));
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult SearchOrg(string Prefix)
        {
            List<Organisation> ObjList = (List<Organisation>)Session["OrganisationList"];
            
            //Searching records from list using LINQ query
            var CityName = (from N in ObjList
                            where N.OrgName.ToUpper().Trim().Contains(Prefix.ToUpper())
                            select new { N.LogicalID, N.OrgName });
            return Json(CityName, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrg(string orgName)
        {
            return Json(GetOrganisationDetails(orgName), JsonRequestBehavior.AllowGet);
        }

        public Organisation GetOrganisationDetails(string orgName)
        {
            Organisation objEmp = new Organisation();

            if (!string.IsNullOrEmpty(orgName))
            {
                List<Organisation> lstOrgs = (List<Organisation>)Session["OrganisationList"];

                var org = lstOrgs.FirstOrDefault(p => p.LogicalID.ToString() == orgName);
                if (org != null && !string.IsNullOrEmpty(org.OrgName) && org.LogicalID != "0")
                {
                    objEmp.LogicalID = org.LogicalID;
                    objEmp.OrgName = org.OrgName;
                    objEmp.L2_Product = org.L2_Product;
                    objEmp.L1_Product = org.L1_Product;
                    objEmp.Division = org.Division;
                    objEmp.ProductFamily = org.ProductFamily;
                    objEmp.ProductGroup = org.ProductGroup;
                }
            }

             return objEmp;
        }
    }
}