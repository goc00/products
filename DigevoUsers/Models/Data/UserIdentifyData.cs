using DigevoUsers.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigevoUsers.Models.Data
{

    public class UserIdentifyData : Connection
    {

        const string TABLE = "UserIdentify";

        public UserIdentifyData() : base() { }

        /// <summary>
        /// Create a new UserIdentify
        /// </summary>
        /// <param name="id_cliente"></param>
        /// <param name="idChannel"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public int CreateUserIdentify(decimal id_cliente, int idChannel, string value)
        {

            Dictionary<string, object> attrs = new Dictionary<string, object>();
            attrs.Add("id_cliente", id_cliente);
            attrs.Add("idChannel", idChannel);
            attrs.Add("value", value);

            return InsertSP("SpCreateUserIdentify", attrs);

        }

        /// <summary>
        /// Find a specific UserIdentify entity in function of idChannel and value 
        /// </summary>
        /// <param name="idChannel">ID Channel</param>
        /// <param name="value">Any value related to channel</param>
        /// <returns></returns>
        public UserIdentify FindByIdChannelAndValue(int idChannel, string value)
        {
            UserIdentify output = new UserIdentify();

            Dictionary<string, object> attrs = new Dictionary<string, object>();
            attrs.Add("idChannel", idChannel);
            attrs.Add("value", value);

            List<UserIdentify> res = SelectSP<UserIdentify>("SpFindUserIdentifyByIdChannelAndValue", attrs);
            if(res.Count > 0)
            {
                output = res.First();
            }
            return output;
        }

        /// <summary>
        /// Find UserIdentify by id_cliente and idChannel
        /// </summary>
        /// <param name="id_cliente">ID User (id_cliente)</param>
        /// <param name="idChannel">ID Channel</param>
        /// <returns></returns>
        public UserIdentify FindByIdUserAndIdChannel(decimal id_cliente, int idChannel)
        {
            UserIdentify output = new UserIdentify();

            Dictionary<string, object> attrs = new Dictionary<string, object>();
            attrs.Add("id_cliente", id_cliente);
            attrs.Add("idChannel", idChannel);

            List<UserIdentify> res = SelectSP<UserIdentify>("SpFindUserIdentifyByIdUserAndIdChannel", attrs);
            if(res.Count > 0)
            {
                output = res.First();
            }
            return output;
        }

    }
}