using DigevoUsers.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigevoUsers.Models.Data {

    public class ChannelData : Connection {

        public ChannelData() : base() { }

        /// <summary>
        /// Get all channels availables
        /// </summary>
        public List<Channel> GetAll()
        {
            List<Channel> result = new List<Channel>();
            Dictionary<string, object> attrs = new Dictionary<string, object>();

            return SelectSP<Channel>("SpGetChannels", attrs);

        }

        /// <summary>
        /// Get Channel by ID
        /// </summary>
        /// <param name="idChannel">ID Channel</param>
        /// <returns>Channel object found or null if it doesn't exist</returns>
        public Channel GetChannelById(int idChannel)
        {
            Dictionary<string, object> attrs = new Dictionary<string, object>();
            attrs.Add("idChannel", idChannel);

            List<Channel> res = SelectSP<Channel>("SpFindChannelById", attrs);
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