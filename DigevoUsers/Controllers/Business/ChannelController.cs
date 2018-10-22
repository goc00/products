using DigevoUsers.Controllers.Libraries;
using DigevoUsers.Models.Data;
using DigevoUsers.Models.Dto;
using DigevoUsers.Models.Response.Common;
using NLog;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DigevoUsers.Controllers.Business
{
    public class ChannelController : Controller
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();
        CommonTasks functions = new CommonTasks();
        
        /// <summary>
        /// Get all channels available
        /// </summary>
        /// <returns></returns>
        public ActionResponse GetAllAction()
        {
            ActionResponse output = new ActionResponse();

            try
            {

                // Get all channels
                ChannelData channelData = new ChannelData();
  
                List<Channel> list = channelData.GetAll();

                if(list.Count > 0) { return functions.Response(200, "OK", list); }
                else { return functions.Response(404, "No se encontraron canales disponibles", null); }
            }
            catch(Exception e)
            {
                return functions.Response(500, "Error desconocido en el sistema", null);
            }

        }

    }
}