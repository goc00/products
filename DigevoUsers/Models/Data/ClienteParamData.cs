using DigevoUsers.Models.Dto;
using System.Collections.Generic;
using System.Linq;

namespace DigevoUsers.Models.Data
{

    public class ClienteParamData : Connection {

        public ClienteParamData() : base() { }

        /// <summary>
        /// Insert meta-data for user
        /// </summary>
        public int FillData(decimal id_cliente, int idParam, string value)
        {
            Dictionary<string, object> attrs = new Dictionary<string, object>();
            attrs.Add("id_cliente", id_cliente);
            attrs.Add("idParam", idParam);
            attrs.Add("value", value);

            return InsertSP("SpFillData", attrs);
        }

        /// <summary>
        /// Get meta-key in function of cliente and param (ids)
        /// </summary>
        /// <param name="id_cliente">ID cliente</param>
        /// <param name="idParam">ID param</param>
        /// <returns>ClienteParam object</returns>
        public ClienteParam GetByIdClienteAndIdParam(decimal id_cliente, int idParam)
        {
            ClienteParam output = new ClienteParam();

            Dictionary<string, object> attrs = new Dictionary<string, object>();
            attrs.Add("id_cliente", id_cliente);
            attrs.Add("idParam", idParam);

            List<ClienteParam> res = SelectSP<ClienteParam>("SpGetByIdClienteAndIdParam", attrs);
            if(res.Count > 0)
            {
                output = res.First();
            }
            return output;
        }


    }
}