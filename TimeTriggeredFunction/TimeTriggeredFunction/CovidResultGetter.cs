using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TimeTriggeredFunction
{
    public static class CovidResultGetter
    {
        static HttpClient client = new HttpClient();
        public async static Task<string> GetResults()
        {
            Console.WriteLine("List of Covid Patients");
            var builder = new UriBuilder("https://services5.arcgis.com/mnYJ21GiFTR97WFg/arcgis/rest/services/PH_masterlist/FeatureServer/0/query");
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["f"] = "json";
            query["where"] = "1=1";
            query["returnGeometry"] = "true";
            query["spatialRel"] = "esriSpatialRelIntersects";
            query["outFields"] = "*";
            query["outSR"] = "102100";
            query["cacheHint"] = "true";
            builder.Query = query.ToString();
            string url = builder.ToString();

            HttpResponseMessage response = await client.GetAsync(url);

            string concat = "";
            int ctr = 0;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var s = JsonConvert.DeserializeObject<response>(result);


                var res = s.features.Select(x => new attributes
                {
                    PH_masterl = x.attributes.PH_masterl,
                    confirmed = x.attributes.confirmed,
                    edad = x.attributes.edad,
                    epi_link = x.attributes.epi_link,
                    facility = x.attributes.facility,
                    FID = x.attributes.FID,
                    kasarian = x.attributes.kasarian,
                    latitude = x.attributes.latitude,
                    longitude = x.attributes.longitude,
                    nationalit = x.attributes.nationalit,
                    petsa = x.attributes.petsa,
                    residence = x.attributes.residence,
                    sequ = x.attributes.sequ,
                    status = x.attributes.status,
                    symptoms = x.attributes.symptoms,
                    travel_hx = x.attributes.travel_hx
                }).ToList();

                foreach (var item in res)
                {
                    concat += string.Format("--------- Patient {16} ---------- \n Name: {0} \n Confirmed: {1} \n Age: {2} /n EPI: {3} \n Facility: {4} \n FID: {5}" +
                        " \n Gender: {6} \n Latitude: {7} \n Longitude: {8} \n Nationality: {9}" +
                        "\n Date: {10} \n Residence: {11} \n SEQU: {12} \n Status:{13} \n Symptoms: {14} \n Travel History: {15} \n -------------------------------- \n\n",
                        item.PH_masterl, item.confirmed, item.edad, item.epi_link,
                        item.facility, item.FID, item.kasarian, item.latitude, item.longitude, item.nationalit, item.petsa, item.residence, item.sequ, item.status,
                    item.symptoms, item.travel_hx, ctr++);
                }
            }
            return concat;
        }
    }
    public class response
    {
        public string objectIdFieldName { get; set; }
        public uniqueIdField uniqueIdField { get; set; }
        public string globalIdFieldName { get; set; }
        public string geometryType { get; set; }
        public spatialReference spatialReference { get; set; }
        public List<fields> fields { get; set; }
        public string exceededTransferLimit { get; set; }
        public IEnumerable<features> features { get; set; }
    }

    public class fields
    {
        public string name { get; set; }
        public string type { get; set; }
        public string alias { get; set; }
        public string sqlType { get; set; }
        public string domain { get; set; }
        public string defaultValue { get; set; }
    }
    public class spatialReference
    {
        public int wkid { get; set; }
        public int latestWkid { get; set; }
    }

    public class uniqueIdField
    {
        public string name { get; set; }
        public bool isSystemMaintained { get; set; }
    }

    public class features
    {
        public attributes attributes { get; set; }
        public geometry geometry { get; set; }
    }

    public class attributes
    {
        public string FID { get; set; }
        public string sequ { get; set; }
        public string PH_masterl { get; set; }
        public string edad { get; set; }
        public string kasarian { get; set; }
        public string nationalit { get; set; }
        public string residence { get; set; }
        public string travel_hx { get; set; }
        public string symptoms { get; set; }
        public string confirmed { get; set; }
        public string facility { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string status { get; set; }
        public string epi_link { get; set; }
        public string petsa { get; set; }
    }

    public class geometry
    {
        public string x { get; set; }
        public string y { get; set; }
    }
}
