using System;
using System.Linq;
using Xamarin.Forms;
using JsonPerf.Models;
using JsonPerf.Resolvers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace JsonPerf.Pages
{
    public class LocationEmployees : ContentPage
    {
		public JObject CurrentLocationJObject { get; set; }
        public Location CurrentLocation { get; set; }

        readonly Lazy<DynamicContractResolver> _locationEmployeesResolver =
			new Lazy<DynamicContractResolver>(() =>
				new DynamicContractResolver(
					nameof(Models.Location.Id),
					nameof(Models.Location.Employees),
                    nameof(Models.Employee.JobTitle),
                    nameof(Models.Employee.Name)));

        readonly LocationManager _locationManager;

        // Controls
        ListView _employees;

        public LocationEmployees(LocationManager locationManager)
		{
			_locationManager = locationManager;
            this.BackgroundColor = Color.White;

			SetupUserInterface();
		}

		void SetupUserInterface()
		{
            _employees = new ListView();
            Content = _employees;
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            RefreshLocationEmployees();
        }

        async Task RefreshLocationEmployees()
		{
            await GetLocationEmployees();

            var jobTitles = 
                CurrentLocation?
                    .Employees?
                    .Select(x => x.JobTitle)
                    .ToList();
            
            _employees.ItemsSource = new ObservableCollection<string>(jobTitles);
		}

		async Task GetLocationEmployees()
		{
			await Task.Run(() =>
            {
                var serializerSettings = new JsonSerializerSettings
				{
					ContractResolver = _locationEmployeesResolver?.Value
				};

                var employeeSerializer =
					JsonSerializer
						.Create(serializerSettings);

				CurrentLocation =
					CurrentLocationJObject?
						.ToObject<Models.Location>(employeeSerializer);
			});
		}
    }
}
