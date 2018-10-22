using DigevoUsers.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DigevoUsers.Models.Data
{

    public class CustodyData : Connection {

        const string TABLE = "Custody";

        public CustodyData() : base() { }

        /// <summary>
        /// Create new Custody
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public int CreateCustody(int idProduct, decimal id_cliente, string value, string code, string alias, bool active, DateTime creationDate, DateTime expirationDate)
        {
            Dictionary<string, object> attrs = new Dictionary<string, object>();
            attrs.Add("idProduct", idProduct);
            attrs.Add("id_cliente", id_cliente);
            attrs.Add("value", value);
            attrs.Add("code", code);
            attrs.Add("alias", alias);
            attrs.Add("active", active);
            attrs.Add("creationDate", creationDate);
            attrs.Add("expirationDate", expirationDate);

            return InsertSP("SpCreateCustody", attrs);
        }

        /// <summary>
        /// Find Custody by code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Custody GetCustodyByCode(int idProduct, string code, string type)
        {
            
            Dictionary<string, object> attrs = new Dictionary<string, object>();
            attrs.Add("idProduct", idProduct);
            attrs.Add("code", code);
            attrs.Add("codeType", type);

            List<Custody> res = SelectSP<Custody>("SpFindCustodyByCode", attrs);
            if(res.Count > 0)
            {
                return res.First();
            }
            else
            {
                return null;
            }
      
        }

        /// <summary>
        /// Get Custody object by ID
        /// </summary>
        /// <param name="idCustody">ID Custody</param>
        /// <returns></returns>
        public Custody GetCustodyById(int idCustody)
        {

            Dictionary<string, object> attrs = new Dictionary<string, object>();
            attrs.Add("idCustody", idCustody);

            List<Custody> res = SelectSP<Custody>("SpFindCustodyById", attrs);
            if(res.Count > 0)
            {
                return res.First();
            }
            else
            {
                return null;
            }

        }


        /// <summary>
        /// Close Custody object (set active = 0)
        /// </summary>
        /// <param name="idCustody">ID Custody to be closed</param>
        /// <returns></returns>
        public int CloseCustody(int idCustody)
        {
            Dictionary<string, object> attrs = new Dictionary<string, object>();
            attrs.Add("idCustody", idCustody);

            int res = UpdateSP("SpCloseCustody", attrs);

            return res;
        }

    }
}