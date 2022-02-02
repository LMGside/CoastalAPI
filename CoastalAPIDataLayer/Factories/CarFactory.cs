using CoastalAPIDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoastalAPIDataLayer.Factories
{
    public class CarFactory
    {
        private readonly string dbConnectionString;
        public CarFactory(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public Car Create(Action<Car> initalizer)
        {
            var car = new Car(this.dbConnectionString);
            initalizer(car);
            return car;
        }
    }
}
