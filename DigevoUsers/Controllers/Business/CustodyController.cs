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

    public class CustodyController : Controller {

        private static Logger logger = LogManager.GetCurrentClassLogger();
        CommonTasks functions = new CommonTasks();

        // Models
        CustodyData custodyData = new CustodyData();
        ClienteData clienteData = new ClienteData();
        UserIdentifyData userIdentData = new UserIdentifyData();
        ProductData productData = new ProductData();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="idProduct"></param>
        /// <param name="idCustody"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public ActionResponse GetCustodyInfoAction(string code, int idProduct, int restrictive, int? idCustody = null, string alias = null)
        {
            ActionResponse output = new ActionResponse();

            try
            {

                // Check for product
                Product oProduct = productData.GetProductById(idProduct);
                if(oProduct == null)
                {
                    return functions.Response((int)CodeStatusEnum.NO_CONTENT, "El producto no existe en el sistema", null);
                }

                // Validate code exists
                Custody oCustody = this.FindCustodyByCode(idProduct, code, "long"); // will find custody by code (long version)
                if(oCustody == null)
                {
                    return functions.Response((int)CodeStatusEnum.NO_CONTENT, "No hay ninguna custodia vinculada al código proporcionado", null);
                }

                // If attr 'restrictive' is on, will check for custody integrity
                if(restrictive == 1)
                {
                    ActionResponse isCustodyValid = IsCustodyValid(oCustody);
                    if(isCustodyValid.code != (int)CodeStatusEnum.OK)
                    {
                        return functions.Response(isCustodyValid.code, isCustodyValid.message, null);
                    }
                }

                // OK
                GetCustodyInfoResponse res = new GetCustodyInfoResponse();

                res.idCustody = oCustody.idCustody.Value;
                res.idProduct = oCustody.idProduct.Value;
                res.idClient = (int)oCustody.id_cliente;
                res.value = oCustody.value;
                res.alias = oCustody.alias;
                res.code = oCustody.code;
                res.active = oCustody.active.Value;
                res.expirationDate = oCustody.expirationDate.Value;
                res.creationDate = oCustody.creationDate.Value;

                return functions.Response((int)CodeStatusEnum.OK, "OK", res);

            }
            catch(Exception e)
            {
                logger.Fatal(e.Message);
                return functions.Response((int)CodeStatusEnum.INTERNAL_ERROR, e.Message, null);
            }
        }


        /// <summary>
        /// Create a new custody for a puntual product, reserving any value sent it
        /// </summary>
        /// <param name="idProduct"></param>
        /// <param name="ani"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ActionResponse CustodyRequestFromPlatformAction(int idProduct, string ani, string value)
        {

            ActionResponse output = new ActionResponse();

            try
            {

                // STEP 1: VALIDATE actions

                // Call for platform's sp to get id_user from ani
                decimal id_cliente = clienteData.FindIdUserFromAni(ani);
                if(id_cliente == 0)
                {
                    return functions.Response((int)CodeStatusEnum.NO_CONTENT, "No se ha encontrado el usuario relacionado al ani proporcionado", null);
                }

                // Validate idProduct exists
                Product oProduct = productData.GetProductById(idProduct);
                if(oProduct == null)
                {
                    return functions.Response((int)CodeStatusEnum.NO_CONTENT, "El producto no existe en el sistema", null);
                }

                // STEP 2: CREATE actions

                // Create UserIdentify
                /*int idUserIdentify = userIdentData.CreateUserIdentify(id_cliente, (int)ChannelEnum.ANI, ani);
                if(idUserIdentify == 0)
                {
                    return functions.Response((int)CodeStatusEnum.CONFLICT, "No se pudo crear la identificación del usuario", null);
                }*/

                // Calculate expiration date
                int minutes = Int32.Parse(functions.ConfigItem("MINUTES_CUSTODY_EXPIRATION"));
                DateTime today = DateTime.Now;
                string code = Guid.NewGuid().ToString();

                // Generate alias for code (user could access without link)
                int max = Int32.Parse(functions.ConfigItem("MAX_ALIAS_CUSTODY"));
                string alias = functions.GetUniqueKey(max);

                // Create Custody element
                /*
                attrs.Add("idProduct", idProduct);
                attrs.Add("id_cliente", id_cliente);
                attrs.Add("value", value);
                attrs.Add("code", code);
                attrs.Add("alias", alias);
                attrs.Add("active", active);
                attrs.Add("creationDate", creationDate);
                attrs.Add("expirationDate", expirationDate);
                 */
                int idCustody = custodyData.CreateCustody(idProduct, id_cliente, value, code, alias, true, today, today.AddMinutes(minutes));
                if(idCustody == 0)
                {
                    return functions.Response((int)CodeStatusEnum.CONFLICT, "No fue posible crear la solicitud de custodia", null);
                }

                // URL where user will access to register or whatever needs to do
                /*string link = (oProduct.urlOperatorRegister == null) ? "" : oProduct.urlOperatorRegister;

                string linkCode = "";
                if(link.Contains("?"))
                {
                    linkCode = link + "&code=" + alias;
                }
                else
                {
                    linkCode = link + "?code=" + alias;
                }*/

                // OK
                CreateCustodyResponse res = new CreateCustodyResponse();
                res.idClient = (int)id_cliente;
                res.idCustody = idCustody;
                res.alias = alias;
                res.code = code;
                res.url = oProduct.urlOperatorRegister;
                return functions.Response((int)CodeStatusEnum.OK, "OK", res);

            }
            catch(Exception e)
            {
                logger.Fatal(e.Message);
                return functions.Response((int)CodeStatusEnum.INTERNAL_ERROR, e.Message, null);
            }
        }

        /// <summary>
        /// Will close Custody object
        /// </summary>
        /// <param name="idCustody">ID Custody</param>
        /// <returns></returns>
        public ActionResponse CloseCustodyByIdAction(int idCustody)
        {
            ActionResponse output = new ActionResponse();

            try
            {

                CustodyData custodyData = new CustodyData();

                // Check if custody exists or not
                Custody oCustody = custodyData.GetCustodyById(idCustody);
                if(oCustody == null)
                {
                    return functions.Response((int)CodeStatusEnum.NO_CONTENT, "No existe ninguna custodia asociada al identificador", null);
                }

                // If custody exists, verify if custody is valid or not
                ActionResponse isCustodyValid = IsCustodyValid(oCustody);
                if(isCustodyValid.code != (int)CodeStatusEnum.OK)
                {
                    return functions.Response(isCustodyValid.code, isCustodyValid.message, null);
                }

                // If everything is ok, try to close it
                int res = custodyData.CloseCustody(idCustody);
                if(res <= 0)
                {
                    return functions.Response((int)CodeStatusEnum.INTERNAL_ERROR, "No se pudo actualizar el estado de la custodia", null);
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

        /// <summary>
        /// Verify if Custody object is valid or not
        /// </summary>
        /// <param name="oCustody">Custody object</param>
        /// <returns></returns>
        public ActionResponse IsCustodyValid(Custody oCustody)
        {
            ActionResponse output = new ActionResponse();

            try
            {

                CustodyData custodyData = new CustodyData();

                // If custody exists, verify if custody is valid or not
                if(oCustody.expirationDate != null)
                {
                    DateTime today = DateTime.Now;
                    int compare = DateTime.Compare(today, oCustody.expirationDate.Value);
                    if(compare > 0)
                    {
                        return functions.Response((int)CodeStatusEnum.CONFLICT,
                                                    "El código proporcionado corresponde a una custodia expirada",
                                                    null);
                    }
                }

                if(!oCustody.active.Value)
                {
                    return functions.Response((int)CodeStatusEnum.CONFLICT,
                                                    "La custodia ya fue utilizada o se encuentra inactiva",
                                                    null);
                }

                return functions.Response((int)CodeStatusEnum.OK, "OK", true);

            }
            catch(Exception e)
            {
                logger.Fatal(e.Message);
                return functions.Response((int)CodeStatusEnum.INTERNAL_ERROR, e.Message, null);
            }
        }



        /// <summary>
        /// Find a Custody object in function of code. It checks if code is valid.
        /// </summary>
        /// <param name="code">Code generated previously by custody process</param>
        /// <param name="type">Code type, it means short or long</param>
        /// <returns>Custody object</returns>
        public Custody FindCustodyByCode(int idProduct, string code, string type)
        {
       
            try
            {
                // Find custody object by code
                return custodyData.GetCustodyByCode(idProduct, code, type);
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
                return null;
            }

        }


    }
}