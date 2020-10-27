using Markit.PET.AdminPanel.CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Markit.PET.AdminPanel.UI.Models
{
    public class TeamTaskTypeDetailViewModel
    {
        public List<EmployeeEntity> EmployeeList { get; set; }
        public List<TeamEntity> TeamList { get; set; }

        public EmployeeEntity SelectedEmployee { get; set; }

        public TeamEntity SelectedTeam { get; set; }

        public EmployeeTeamDetail EmployeeTeamDetail { get; set; }

        public bool IsEmployeeTeamUpdated { get; set; }
    }
}