using DigevoUsers.Models.Dto;
using System.Collections.Generic;
using System.Linq;

namespace DigevoUsers.Models.Data
{

    public class ProductData : Connection {

        const string TABLE = "Product";

        public ProductData() : base() { }

        /// <summary>
        /// Get Product by ID
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public Product GetProductById(int idProduct)
        {
            Dictionary<string, object> attrs = new Dictionary<string, object>();
            attrs.Add("idProduct", idProduct);

            List<Product> res = SelectSP<Product>("SpFindProductById", attrs);
            if(res.Count > 0)
            {
                return res.First();
            }
            else
            {
                return null;
            }
        }


    }
}