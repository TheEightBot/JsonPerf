using System;
using System.Collections.Generic;

namespace JsonPerf.Models
{
    public class Location
    {
        // Task Details properties
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public List<Employee> Employees { get; set; }
        public List<Product> Products { get; set; }
    }
}
