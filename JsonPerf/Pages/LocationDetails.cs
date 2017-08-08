using System;
using System.Threading.Tasks;
using JsonPerf.Resolvers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace JsonPerf.Pages
{
    public class LocationDetails : ContentPage
    {
		public JObject CurrentLocationJObject { get; set; }
        public Models.Location CurrentLocation { get; set; }

        readonly Lazy<DynamicContractResolver> _locationDetailsResolver =
			new Lazy<DynamicContractResolver>(() =>
				new DynamicContractResolver(
					nameof(Models.Location.Name),
					nameof(Models.Location.Address),
					nameof(Models.Location.Id)));

        readonly LocationManager _locationManager;

        Entry _name, _description;

        Button _save;

        public LocationDetails(LocationManager locationManager)
		{
			_locationManager = locationManager;
            this.BackgroundColor = Color.White;

			SetupUserInterface();
		}

		void SetupUserInterface()
		{
            _name = new Entry() { Placeholder = "Location Name" };
            _description = new Entry() { Placeholder = "Location Description" };
            _save = new Button { Text = "Save" };

            var layout = new StackLayout() { VerticalOptions = LayoutOptions.CenterAndExpand };
            layout.Children.Add(_name);
            layout.Children.Add(_description);
            layout.Children.Add(_save);

            _save.Clicked += (sender, e) => SaveLocationDetails();

            Content = layout;
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            RefreshLocationDetails();
        }

		async Task RefreshLocationDetails()
		{
            await GetLocationDetails();
			_name.Text = CurrentLocation.Name;
			_description.Text = CurrentLocation.Address;
		}

        async Task GetLocationDetails()
		{
			await Task.Run(() =>
			{
                var serializerSettings = new JsonSerializerSettings
				{
					ContractResolver = _locationDetailsResolver?.Value
				};

                var locationDetailsSerializer =
					JsonSerializer
						.Create(serializerSettings);

				CurrentLocation =
					CurrentLocationJObject?
						.ToObject<Models.Location>(locationDetailsSerializer);
			});
		}

		async Task SaveLocationDetails()
		{
            CurrentLocation.Name = _name.Text;
            CurrentLocation.Address = _description.Text;

			CurrentLocationJObject[nameof(Models.Location.Name)] = CurrentLocation.Name;
			CurrentLocationJObject[nameof(Models.Location.Address)] = CurrentLocation.Address;

			await _locationManager.SaveLocation(CurrentLocationJObject, CurrentLocation.Id);
		}
    }
}
