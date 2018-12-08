//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace stock.Services
{
    public static class DataStore
    {
        /*static List<City> cities = new List<City>();

        public static async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await Task.Run(() => {
                if (Application.Current.Properties.ContainsKey("cities"))
                {
                    var _cities = Application.Current.Properties["cities"] as string;
                    cities = JsonConvert.DeserializeObject<List<City>>(_cities);
                }
                return cities;
            });
        }

        public static async Task<City> GetCityAsync(string id)
        {
            return await Task.FromResult(cities.FirstOrDefault(s => s.Id == id));
        }

        public static async Task<bool> AddCityAsync(City city)
        {
            var _city = cities.Where((City arg) => arg.Id == city.Id).FirstOrDefault();
            if (_city != null)
                return await Task.FromResult(false);
            cities.Add(city);
            await SaveChanges();
            return await Task.FromResult(true);
        }

        public static async Task<bool> UpdateCityAsync(City city)
        {
            var _city = cities.Where((City arg) => arg.Id == city.Id).FirstOrDefault();
            if (_city == null)
                return await Task.FromResult(false);
            cities.Remove(_city);
            cities.Add(city);
            await SaveChanges();
            return await Task.FromResult(true);
        }

        public static async Task<bool> DeleteCityAsync(City city)
        {
            var _city = cities.Where((City arg) => arg.Id == city.Id).FirstOrDefault();
            if (_city == null)
                return await Task.FromResult(false);
            cities.Remove(_city);
            await SaveChanges();
            return await Task.FromResult(true);
        }

        public static async Task SaveChanges()
        {
            var _cities = JsonConvert.SerializeObject(cities);
            Application.Current.Properties["cities"] = _cities;
            await Application.Current.SavePropertiesAsync();
        }*/
    }
}