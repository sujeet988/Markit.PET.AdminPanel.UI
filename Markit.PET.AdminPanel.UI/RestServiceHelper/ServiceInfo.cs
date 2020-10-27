using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Markit.PET.AdminPanel.UI.RestServiceHelper
{
    public class ServiceInfo
    {
        public static string AdminPanel_IsAccessible { get { return BaseServiceUri + "AdminPanel_IsAccessible"; } }
        public static string Dict_GetAllDataBaseObjects { get { return BaseServiceUri + "Dict_GetAllDataBaseObjects"; } }
        public static string Dict_GetDataBaseObjectDetail { get { return BaseServiceUri + "Dict_GetDataBaseObjectDetail"; } }
        public static string Dict_SaveExtendedProperties { get { return BaseServiceUri + "Dict_SaveExtendedProperties"; } }

        public static string GetAllEmployees { get { return BaseServiceUri + "GetAllEmployees"; } }
        public static string GetAllTeams { get { return BaseServiceUri + "GetAllTeams"; } }
        public static string GetTeamDetailByEmployeeID { get { return BaseServiceUri + "GetTeamDetailByEmployeeID"; } }

        public static string SaveEmployeeTeamDetail { get { return BaseServiceUri + "SaveEmployeeTeamDetail"; } }
        public static string CreateTeam { get { return BaseServiceUri + "CreateTeam"; } }

        public static string GetAllOrganisation { get { return BaseServiceUri + "GetAllOrganisation"; } }

        private static string BaseServiceUri
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings.Get("BaseServiceUri");
            }
        }


        internal static T InvokeGetService<T>(string serviceUri, string _data)
        {

            T _returnResponse = default(T);
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceUri);
                request.Headers.Add("Authorization", "Basic ");
                request.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                _returnResponse = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
            }
            catch (Exception ex)
            {

            }
            return _returnResponse;
        }

        internal static T InvokePostService<T>(string serviceUri, string _data)
        {
            T _returnResponse = default(T);
            HttpWebRequest req = null;
            HttpWebResponse res = null;
            try
            {
                string url = serviceUri;
                req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "application/json; charset=utf-8";
                req.Timeout = 600000;
                req.Headers.Add("SOAPAction", url);
                req.ContentLength = _data.Length;
                var sw = new StreamWriter(req.GetRequestStream());

                string result = System.Text.Encoding.UTF8.GetString(Encoding.ASCII.GetBytes(_data));
                sw.Write(result);
                sw.Close();
                res = (HttpWebResponse)req.GetResponse();
                Stream responseStream = res.GetResponseStream();
                var streamReader = new StreamReader(responseStream);
                string responseString = streamReader.ReadToEnd();
                _returnResponse = JsonConvert.DeserializeObject<T>(responseString);

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _returnResponse;
        }
    }
}