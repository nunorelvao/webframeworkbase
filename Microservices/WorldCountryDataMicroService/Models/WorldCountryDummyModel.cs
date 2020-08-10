using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldCountryDataMicroService.Models
{
    public class WorldCountryDummyModel
    {
        public string Code { get; set; }
        public string Code3 { get; set; }
        public string Extcode { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }


        public WorldCountryDummyModel()
        {

        }

        public IEnumerable<WorldCountryDummyModel> GetDumyList()
        {
            for (int i = 1; i < 100; i++)
            {
                WorldCountryDummyModel model = new WorldCountryDummyModel();
                model.Code = i.ToString().PadLeft(1, '0');
                model.Code3 = i.ToString().PadLeft(3, '0');
                model.Extcode = i.ToString().PadLeft(1, '0');
                model.Number = i;
                model.Name = "NAME_" + i;
                model.Domain = "DOMAIN_" + i;
                yield return model;
            }

        }
    }
}
