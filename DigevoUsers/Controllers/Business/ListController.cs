using DigevoUsers.Controllers.Libraries;
using DigevoUsers.Models.Data;
using DigevoUsers.Models.Dto;
using DigevoUsers.Models.Enum;
using DigevoUsers.Models.Response;
using DigevoUsers.Models.Response.Common;
using NLog;
using System;
using System.Web.Mvc;

namespace DigevoUsers.Controllers.Business
{

    public class ListController : Controller {

        private static Logger logger = LogManager.GetCurrentClassLogger();
        CommonTasks functions = new CommonTasks();

        /// <summary>
        /// Update client with new list
        /// </summary>
        /// <param name="id_client">ID user master</param>
        /// <param name="id_lista">ID new list</param>
        /// <param name="id_pauta">ID guide (for traceability)</param>
        /// <returns></returns>
        public ActionResponse UpdateClientListAction(decimal id_client, decimal id_lista, decimal id_pauta)
        {
            ActionResponse output = new ActionResponse();

            try
            {

                // Verify if user exists
                ClienteData clientData = new ClienteData();
                Cliente oClient = clientData.GetUserById(id_client);
                if(oClient.id_cliente == null)
                {
                    return functions.Response((int)CodeStatusEnum.NO_CONTENT, "No existe el usuario en el sistema", null);
                }

                ListData listData = new ListData();

                // If everything is ok, try to close it
                int res = listData.UpdateClientList(id_client, id_lista, id_pauta);
                if(res <= 0)
                {
                    return functions.Response((int)CodeStatusEnum.INTERNAL_ERROR, "No se pudo actualizar el estado del usuario/lista", null);
                }

                CloseCustodyResponse response = new CloseCustodyResponse();
                response.updated = DateTime.Now;
                return functions.Response((int)CodeStatusEnum.OK, "OK", response);

            }
            catch(Exception e)
            {
                logger.Fatal(e.Message);
                return functions.Response((int)CodeStatusEnum.INTERNAL_ERROR, e.Message, null);
            }
        }

    }
}