using Markit.PET.AdminPanel.CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Markit.PET.AdminPanel.UI.Models
{
    public class ExtendedPropertyViewModel
    {
        public string SelectedTable { get; set; }
        public string SelectedDatabase { get; set; }

        public List<KeyValuePair<string, string>> DatabaseList
        {
            get
            {
                List<KeyValuePair<string, string>> _databaseList = new List<KeyValuePair<string, string>>();

                _databaseList.Add(new KeyValuePair<string, string>("SBM_Reporting", "SBM Reporting"));
                _databaseList.Add(new KeyValuePair<string, string>("TimeUpdater", "TimeUpdater"));
                _databaseList.Add(new KeyValuePair<string, string>("Project_Lite", "Project Lite"));

                return _databaseList;
            }
        }

        public List<DatabaseObjects> DatabaseObject { get; set; }

        public List<ExtendedPropertyList> PropertiesValues { get; set; }
    }
}