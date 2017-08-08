using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JsonPerf.Data;
using JsonPerf.Models;
using Newtonsoft.Json.Linq;

namespace JsonPerf
{
    public class LocationManager
    {
        public Company PrimaryCompany = new Company();

        int locationId = 0;
        int employeeId = 0;
        int productId = 0;

        readonly int PRODUCT_LIST_SIZE = 50;
        readonly int EMPLOYEE_LIST_SIZE = 500;

        public async Task<JObject> GetLocationJObject(int locationId)
		{
            return await Task.Run(() =>
			{
                var companyJObject = JObject.Parse(RawJson.Company); // parsing to JObject is fast even for large objects
                var locationJObject = companyJObject[nameof(Company.Locations)]?
					.FirstOrDefault(x => (int)x[nameof(Location.Id)] == locationId) as JObject;
				return locationJObject;
			})
		   .ConfigureAwait(false);
		}

        public async Task SaveLocation(JObject locationJObject, int locationId)
		{
			await Task.Run(() =>
			{
                var companyJObject = JObject.Parse(RawJson.Company);

				companyJObject[nameof(Company.Locations)]?
					.FirstOrDefault(x => (decimal)x[nameof(Location.Id)] == locationId)
					.Replace(locationJObject);
			});
		}

        public async Task Seed()
        {
			var rand = new Random();

            var employees = await GetEmployees().ConfigureAwait(false);
            var products = await GetProducts().ConfigureAwait(false);

            PrimaryCompany.Locations =
                await Task.Run(() =>
                    new List<Location>(
                        Enumerable
                            .Range(1, 10)
                            .Select(i =>
                            {
                                var currRand = rand.Next(1, 5);
                                var locationName = string.Empty;
                                var address = string.Empty;

                                switch (currRand)
                                {
                                    case 1:
                                        locationName = "Downtown Storefront";
                                        address = "123 Fake St";
                                        break;
                                    case 2:
                                        locationName = "East Side Storefront";
                                        address = "456 Side E";
                                        break;
                                    case 3:
                                        locationName = "Island Storefront";
                                        address = "789 River Road";
                                        break;
                                    case 4:
                                        locationName = "Main St Location";
                                        address = "101112 Main St";
                                        break;
                                }

                                return new Location
                                {
                                    Id = ++locationId,
                                    Name = locationName,
                                    Address = address,
                                    Employees = i == 3 ? employees : new List<Employee>(),
                                    Products = i == 3 ? products : new List<Product>()
                                };
                }))).ConfigureAwait(false);
        }

        async Task<List<Employee>> GetEmployees()
        {
            return await Task.Run(() =>
            {
                var rand = new Random();
                return new List<Employee>(
                    Enumerable
                        .Range(1, EMPLOYEE_LIST_SIZE)
	                    .Select(i =>
	                    {
	                        var currRand = rand.Next(1, 5);
	                        var name = string.Empty;
	                        var text = string.Empty;

	                        switch (currRand)
	                        {
	                            case 1:
                                    name = "John";
	                                text = "Developer";
	                                break;
	                            case 2:
	                                name = "Jacob";
	                                text = "Business Analyst";
	                                break;
	                            case 3:
	                                name = "Jingleheimer";
	                                text = "Project Manager";
	                                break;
	                            case 4:
	                                name = "Schmidt";
	                                text = "Quality Assurance";
	                                break;
	                        }

	                        return new Employee
	                        {
	                            Id = ++employeeId,
	                            Name = name,
	                            JobTitle = text
	                        };
	                    })
                );
            });
        }

        async Task<List<Product>> GetProducts()
		{
			return await Task.Run(() =>
			{
				var rand = new Random();
				return new List<Product>(
					Enumerable
                        .Range(1, PRODUCT_LIST_SIZE)
						.Select(i =>
						{
							var currRand = rand.Next(1, 5);
		                    var name = string.Empty;
		                    var description = string.Empty;

							switch (currRand)
							{
								case 1:
									name = "Skateboard";
									description = "Fast and danger";
									break;
								case 2:
									name = "Dirtbike";
									description = "Fast and danger again";
									break;
								case 3:
									name = "Hacky Sack";
									description = "5 hit whip";
									break;
								case 4:
									name = "Basketball";
									description = "Slam dunks for all";
									break;
							}

							return new Product
							{
	                            Id = ++productId,
								Name = name,
	                            Description = description
							};
						})
				);
			});
		}
    }
}
