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
    public class ParamController : Controller
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
                ParamData paramData = new ParamData();
  
                List<Param> list = paramData.GetAll();

                if(list.Count > 0) { return functions.Response(200, "OK", list); }
                else { return functions.Response(404, "No se encontraron parámetros usuario disponibles", null); }
            }
            catch(Exception e)
            {
                return functions.Response(500, "Error desconocido en el sistema", null);
            }

        }

    }
}