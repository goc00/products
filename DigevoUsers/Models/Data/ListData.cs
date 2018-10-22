using NLog;
using System.Collections.Generic;

namespace DigevoUsers.Models.Data
{

    public class ListData : Connection {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ListData() : base() { }

        /// <summary>
        /// Update client list in subscription module
        /// </summary>
        /// <param name="id_cliente">ID user master</param>
        /// <param name="id_lista">ID list (new)</param>
        /// <param name="id_pauta">ID guide (0 by default)</param>
        /// <returns></returns>
        public int UpdateClientList(decimal id_cliente, decimal id_lista, decimal id_pauta)
        {

            Dictionary<string, object> attrs = new Dictionary<string, object>();
            attrs.Add("id_cliente", id_cliente);
            attrs.Add("id_lista", id_lista);
            attrs.Add("id_pauta", id_pauta);

            int res = UpdateSP("pa_UpdateClienteLista_idp", attrs);

            return res;

        }

      
    }
}