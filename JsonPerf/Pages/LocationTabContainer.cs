using System;
using Xamarin.Forms;
using Newtonsoft.Json;
using JsonPerf.Data;
using JsonPerf.Resolvers;
using JsonPerf;

namespace JsonPerf.Pages
{
    public class LocationTabContainer : TabbedPage
    {
        // Pages
        LocationDetails _locationDetails;
        LocationEmployees _locationEmployees;
        LocationProducts _locationProducts;

        // Data Access
        readonly LocationManager _locationManager = new LocationManager();
        readonly int LOCATION_WITH_DATA = 3;

        public LocationTabContainer()
        {
            InitializeData();
        }

        async System.Threading.Tasks.Task InitializeData()
        {
            // You can use these to seed diff types of graphs, then copy the raw JSON into RawJson
            // and apply optimizations against it
            //await _locationManager?.Seed(); 
            //var companyJson = JsonConvert.SerializeObject(_locationManager.PrimaryCompany);

            try
            {
                var locationJObject = await _locationManager.GetLocationJObject(LOCATION_WITH_DATA);

                _locationDetails = new LocationDetails(_locationManager) { Title = "Details", CurrentLocationJObject = locationJObject };
                _locationEmployees = new LocationEmployees(_locationManager) { Title = "Employees", CurrentLocationJObject = locationJObject };
                _locationProducts = new LocationProducts(_locationManager) { Title = "Products", CurrentLocationJObject = locationJObject };

                Children.Add(_locationDetails);
                Children.Add(_locationEmployees);
                Children.Add(_locationProducts);
            }
            catch { }

            // you can test how long it takes to deserialize the entire graph here
            //var company = JsonConvert.DeserializeObject<Models.Company>(RawJson.Company);
        }
    }
}
