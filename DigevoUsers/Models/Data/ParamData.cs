using DigevoUsers.Models.Dto;
using System.Collections.Generic;
using System.Linq;

namespace DigevoUsers.Models.Data
{

    public class ParamData : Connection {

        public ParamData() : base() { }

        /// <summary>
        /// Get all channels availables
        /// </summary>
        public List<Param> GetAll()
        {
            List<Param> result = new List<Param>();
            Dictionary<string, object> attrs = new Dictionary<string, object>();

            return SelectSP<Param>("SpGetParams", attrs);
        }

        /// <summary>
        /// Get Param by tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public Param GetByTag(string tag)
        {
            Param output = new Param();

            Dictionary<string, object> attrs = new Dictionary<string, object>();
            attrs.Add("tag", tag);

            List<Param> res = SelectSP<Param>("SpGetParamByTag", attrs);
            if(res.Count > 0)
            {
                output = res.First();
            }
            return output;
        }

    }
}