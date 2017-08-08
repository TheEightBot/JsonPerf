using System;
using Xamarin.Forms;
using JsonPerf.Models;
using JsonPerf.Resolvers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace JsonPerf.Pages
{
	public class LocationProducts : ContentPage
	{
		public JObject CurrentLocationJObject { get; set; }
        public Location CurrentLocation { get; set; }

        readonly Lazy<DynamicContractResolver> _locationProductsResolver =
			new Lazy<DynamicContractResolver>(() =>
				new DynamicContractResolver(
					nameof(Models.Location.Id),
                    nameof(Models.Location.Products),
                    nameof(Models.Location.Name)));

        readonly LocationManager _locationManager;

		// Controls
        ListView _products;

        public LocationProducts(LocationManager locationManager)
		{
			_locationManager = locationManager;
			this.BackgroundColor = Color.White;

			SetupUserInterface();
		}

		void SetupUserInterface()
		{
            _products = new ListView();
            Content = _products;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
            RefreshProducts();
		}

		async Task RefreshProducts()
		{
            await GetLocationProducts();

            var productText = 
                CurrentLocation?
                    .Products?
                    .Select(x => x.Name)
                    .ToList();
            
            _products.ItemsSource = new ObservableCollection<string>(productText);
		}

		async Task GetLocationProducts()
		{
			await Task.Run(() =>
			{
                var serializerSettings = new JsonSerializerSettings
				{
					ContractResolver = _locationProductsResolver?.Value
				};

                var productsSerializer =
					JsonSerializer
						.Create(serializerSettings);

				CurrentLocation =
					CurrentLocationJObject?
						.ToObject<Models.Location>(productsSerializer);
			});
		}

		async System.Threading.Tasks.Task SaveTaskDetails()
		{
            CurrentLocationJObject[nameof(Location.Products)] =
                CurrentLocation.Products != null
                    ? JObject.FromObject(CurrentLocation.Products)
					: null;

			await _locationManager.SaveLocation(CurrentLocationJObject, CurrentLocation.Id);
		}
	}
}
