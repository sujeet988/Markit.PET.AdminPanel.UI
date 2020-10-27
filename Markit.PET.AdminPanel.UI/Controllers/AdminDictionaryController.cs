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
    public class AdminDictionaryController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FillDatabaseObjects(string databaseID)
        {
            DatabaseEntity _entity = new DatabaseEntity() { DatabaseName = databaseID };
            var _list = ServiceInfo.InvokePostService<List<DatabaseObjects>>(ServiceInfo.Dict_GetAllDataBaseObjects, JsonConvert.SerializeObject(_entity));
            return Json(_list, JsonRequestBehavior.AllowGet);
        }

        // GET: AdminDictionary
        public ActionResult ExtendedProperties()
        {
            ExtendedPropertyViewModel _model = new ExtendedPropertyViewModel();
            _model.DatabaseObject = new List<DatabaseObjects>();
            return View(_model);
        }

        [HttpPost]
        public ActionResult ChangeDatabase(ExtendedPropertyViewModel _model)
        {
            DatabaseEntity _entity = new DatabaseEntity() { DatabaseName = _model.SelectedDatabase };
            _model.DatabaseObject = ServiceInfo.InvokePostService<List<DatabaseObjects>>(ServiceInfo.Dict_GetAllDataBaseObjects, JsonConvert.SerializeObject(_entity));
            return View("ExtendedProperties", _model);
        }

        [HttpPost]
        public ActionResult ExtendedPropertiesInfo(ExtendedPropertyViewModel _selectedTableName)
        {
            ExtendedPropertyViewModel _model = new ExtendedPropertyViewModel();

            DatabaseEntity _entity = new DatabaseEntity() { DatabaseName = _selectedTableName.SelectedDatabase };
            _model.DatabaseObject = ServiceInfo.InvokePostService<List<DatabaseObjects>>(ServiceInfo.Dict_GetAllDataBaseObjects, JsonConvert.SerializeObject(_entity));
            DatabaseDetailObject _data = new DatabaseDetailObject() { ObjectName = _selectedTableName.SelectedTable, DatabaseName = _selectedTableName.SelectedDatabase };

            _model.PropertiesValues = ServiceInfo.InvokePostService<List<ExtendedPropertyList>>(ServiceInfo.Dict_GetDataBaseObjectDetail, JsonConvert.SerializeObject(_data));
            _model.SelectedTable = _selectedTableName.SelectedTable;
            _model.SelectedDatabase = _selectedTableName.SelectedDatabase;
            return View("ExtendedProperties", _model);
        }

        public ActionResult SaveExtendedProperties(ExtendedPropertyViewModel _model)
        {
            ExtendedPropertyEntities _obj = new ExtendedPropertyEntities() { DatabaseName = _model.SelectedDatabase, PropertiesList = _model.PropertiesValues };
            if (_model.PropertiesValues != null && _model.PropertiesValues.Count > 0)
            {
                var PropertiesValues = ServiceInfo.InvokePostService<DatabaseObjects>(ServiceInfo.Dict_SaveExtendedProperties, JsonConvert.SerializeObject(_obj));
                _model = UpdateModelObject(_model);
            }
            return View("ExtendedProperties", _model);
        }

        private ExtendedPropertyViewModel UpdateModelObject(ExtendedPropertyViewModel _model)
        {
            DatabaseEntity _entity = new DatabaseEntity() { DatabaseName = _model.SelectedDatabase };
            _model.DatabaseObject = ServiceInfo.InvokePostService<List<DatabaseObjects>>(ServiceInfo.Dict_GetAllDataBaseObjects, JsonConvert.SerializeObject(_entity));
            DatabaseDetailObject _data = new DatabaseDetailObject() { ObjectName = _model.SelectedTable, DatabaseName = _model.SelectedDatabase };
            _model.PropertiesValues = ServiceInfo.InvokePostService<List<ExtendedPropertyList>>(ServiceInfo.Dict_GetDataBaseObjectDetail, JsonConvert.SerializeObject(_data));
            return _model;
        }
    }
}