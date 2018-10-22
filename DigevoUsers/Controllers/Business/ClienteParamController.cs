using DigevoUsers.Controllers.Libraries;
using DigevoUsers.Models.Data;
using DigevoUsers.Models.Dto;
using DigevoUsers.Models.Enum;
using DigevoUsers.Models.Request;
using DigevoUsers.Models.Response;
using DigevoUsers.Models.Response.Common;
using NLog;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DigevoUsers.Controllers.Business
{
    public class ClienteParamController : Controller
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();
        CommonTasks functions = new CommonTasks();
        
        /// <summary>
        /// Allow set any meta-key for user (attributes)
        /// </summary>
        /// <returns></returns>
        public ActionResponse FillDataUserAction(FillUserDataRequest obj)
        {
            ActionResponse output = new ActionResponse();

            try
            {

                // STEP 0.1: Verify if user exists
                ClienteData clientData = new ClienteData();
                Cliente oClient = clientData.GetUserById(obj.idClient.Value);
                if(oClient.id_cliente == null)
                {
                    return functions.Response((int)CodeStatusEnum.NO_CONTENT, "No existe el usuario en el sistema", null);
                }

                ParamData paramData = new ParamData();
                ClienteParamData clientParamData = new ClienteParamData();

                // Detect if meta-key already exists or not
                // tags length is equal to values length
                int l = obj.tags.Length;

                // Try to check and store params objects
                List<Param> paramList = new List<Param>();

                decimal id_cliente = obj.idClient.Value;

                for(int i=0;i<l;i++)
                {
                    string tag = obj.tags[i];

                    // Check if param already exists or not
                    Param param = paramData.GetByTag(tag);
                    if(param.idParam != null)
                    {

                        // Check if meta-key exists
                        ClienteParam cp = clientParamData.GetByIdClienteAndIdParam(id_cliente, param.idParam.Value);
                        if(cp.idClienteParam != null)
                        {
                            return functions.Response((int)CodeStatusEnum.CONFLICT, "Ya existe un valor asociado para el usuario y tag [" + param.tag + "]", null);
                        }

                        paramList.Add(param);
                    } else
                    {
                        return functions.Response((int)CodeStatusEnum.NO_CONTENT, "No existe el tag [" + tag + "]", null);
                    }

                }

                // Start to insert new meta-data
                
                int j = 0;
                foreach(Param o in paramList)
                {
                    string value = obj.values[j];
                    int idParam = o.idParam.Value;

                    int res = clientParamData.FillData(id_cliente, idParam, value);

                    if(res <= 0)
                    {
                        return functions.Response((int)CodeStatusEnum.INTERNAL_ERROR, "Falló la creación de la meta-data para el tag [" + o.tag + "]", null);
                    } 

                }

                FillDataUserResponse response = new FillDataUserResponse();
                response.numberOfItemsInserted = l;
                return functions.Response((int)CodeStatusEnum.OK, "OK", response);
                
            }
            catch(Exception e)
            {
                return functions.Response((int)CodeStatusEnum.INTERNAL_ERROR, "Error desconocido en el sistema", null);
            }

        }

    }
}