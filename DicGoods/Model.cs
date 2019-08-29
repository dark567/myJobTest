using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicGoods
{
    public class ModelDicGoods
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public static List<ModelDicGoods> _modelDicGoods;

        public ModelDicGoods()
        {
            _modelDicGoods = new List<ModelDicGoods>();
        }

        public ModelDicGoods(string _iD = "n/a", string _nAme = "n/a")
        {
            this.ID = _iD;
            this.Name = _nAme;
        }


        public static ModelDicGoods[] GetDicGoods
        {
            get
            {
                return _modelDicGoods.ToArray();
            }
        }

        public static void AddDicGoods(ModelDicGoods modelSAveGOODS)
        {
            _modelDicGoods.Add(modelSAveGOODS);
        }
    }
}
