using Markit.PET.AdminPanel.CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Markit.PET.AdminPanel.UI.Models
{
    public class TeamManagementViewModel
    {
        public List<TeamEntity> TeamList { get; set; }
        public TeamEntity SelectedTeam { get; set; }

        public List<Organisation> OrganisationList { get; set; }
        public Organisation SelectedOrganisation { get; set; }
    }
}