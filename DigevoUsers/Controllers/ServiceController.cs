using DigevoUsers.Controllers.Business;
using DigevoUsers.Controllers.Exceptions;
using DigevoUsers.Controllers.Libraries;
using DigevoUsers.Models.Dto;
using DigevoUsers.Models.Enum;
using DigevoUsers.Models.Request;
using DigevoUsers.Models.Response.Common;
using NLog;
using System;
using System.Web.Http;

namespace DigevoUsers.Controllers
{

    [RoutePrefix("api/service")]
    public class ServiceController : ApiController {

        CommonTasks functions = new CommonTasks();

        private static Logger logger = LogManager.GetCurrentClassLogger();



        /// <summary>
        /// Expose service to update user's list (recurrence)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateClientList([FromBody]UpdateClientListRequest obj)
        {

            try
            {
                // Send object to business layer
                ListController core = new ListController();

                // Verify at least object arrives with data
                if(obj == null) throw new NotEnoughAttributesException("No se ha recibido ningún parámetro");

                // Verify for parameters needed
                if(obj.idClient == null || obj.idList == null)
                {
                    throw new NotEnoughAttributesException("No se han recibido todos los parámetros requeridos");
                }

                // Check params's integrity
                if(obj.idClient.GetType() != typeof(int)
                    || obj.idList.GetType() != typeof(int))
                {
                    throw new NotEnoughAttributesException("Los tipos de datos no coinciden");
                }

                // ID guide is optional, if it's setted, we will check it
                int idGuide = 0;
                if(obj.idGuide != null)
                {

                    int Num;
                    bool isNum = int.TryParse(obj.idGuide.ToString(), out Num);

                    if(!isNum)
                    {
                        idGuide = 0;
                    }
                    else
                    {
                        //idGuide = Convert.ToInt32(obj.idGuide);
                        idGuide = obj.idGuide.Value;
                    }

                }

                ActionResponse action = core.UpdateClientListAction(obj.idClient.Value, obj.idList.Value, idGuide);

                if(action.code == (int)CodeStatusEnum.OK) { return ResponseOk(action.data); }  // OK
                else { return ResponseError(action.code, action.message); } // NOK
            }
            catch(NotValidDataException e)
            {
                logger.Error(e.Message);
                return ResponseError((int)CodeStatusEnum.BAD_REQUEST, e.Message);
            }
            catch(NotEnoughAttributesException e)
            {
                logger.Error(e.Message);
                return ResponseError((int)CodeStatusEnum.BAD_REQUEST, e.Message);
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
                return ResponseError((int)CodeStatusEnum.INTERNAL_ERROR, "Error desconocido en el sistema");
            }
        }

        /// <summary>
        /// Get all info related to custody (it can validate custody integrity)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getcustodyinfo/{code}/{idProduct:int}/{restrictive}")]
        public IHttpActionResult GetCustodyInfo([FromUri]GetCustodyInfoRequest obj)
        {

            try
            {
                // Send object to business layer
                CustodyController core = new CustodyController();

                // Verify at least object arrives with data
                if(obj == null) throw new NotEnoughAttributesException("No se ha recibido ningún parámetro");

                // Verify for parameters needed
                if(obj.code == null || obj.idProduct == null || obj.restrictive == null)
                {
                    throw new NotEnoughAttributesException("No se han recibido todos los parámetros requeridos");
                }

                // Check params's integrity
                if(obj.code.GetType() != typeof(string)
                    || obj.idProduct.GetType() != typeof(int)
                    || obj.restrictive.GetType() != typeof(int))
                {
                    throw new NotEnoughAttributesException("Los tipos de datos no coinciden");
                }

                // Call action
                ActionResponse action = core.GetCustodyInfoAction(obj.code, obj.idProduct.Value, obj.restrictive.Value);

                if(action.code == (int)CodeStatusEnum.OK) { return ResponseOk(action.data); }  // OK
                else { return ResponseError(action.code, action.message); } // NOK
            }
            catch(NotValidDataException e)
            {
                logger.Error(e.Message);
                return ResponseError((int)CodeStatusEnum.BAD_REQUEST, e.Message);
            }
            catch(NotEnoughAttributesException e)
            {
                logger.Error(e.Message);
                return ResponseError((int)CodeStatusEnum.BAD_REQUEST, e.Message);
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
                return ResponseError((int)CodeStatusEnum.INTERNAL_ERROR, "Error desconocido en el sistema");
            }
        }


        /// <summary>
        /// Start process for custody from platform
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult CustodyRequestFromPlatform() {

            try
            {

                // Parameters sent by platform (GET)
                var request = Request;

                string ani = functions.GetQueryString(request, "ani");
                string text = functions.GetQueryString(request, "text");
                string nc = functions.GetQueryString(request, "nc");
                string op = functions.GetQueryString(request, "op");
                string id = functions.GetQueryString(request, "id");
                string cfield1 = functions.GetQueryString(request, "cfield1");
                string lista = functions.GetQueryString(request, "lista");
                string campEnvio1 = functions.GetQueryString(request, "campEnvio1");
                string campEnvio2 = functions.GetQueryString(request, "campEnvio2");
                string campCobro = functions.GetQueryString(request, "campCobro");

                // Custom params
                string idProduct = functions.GetQueryString(request, "idProduct");
                string value = functions.GetQueryString(request, "value"); // value to be stored


                // Check for vital data
                if(idProduct == null || ani == null || value == null)
                {
                    throw new NotEnoughAttributesException("No se han recibido todos los parámetros requeridos");
                }
                    

                // Create object to send it to business layer and process it
                CustodyRequest req = new CustodyRequest();
                req.idProduct = (int)Int32.Parse(idProduct);
                req.value = value;
                req.valChannel = ani;
                req.sendSms = true;

                // Business's logic controllers, core
                CustodyController core = new CustodyController();
                ActionResponse action = core.CustodyRequestFromPlatformAction(req.idProduct.Value, ani, value);

                if(action.code == (int)CodeStatusEnum.OK) { return ResponseOk(action.data); }  // OK
                else { return ResponseError(action.code, action.message); } // NOK
            }
            catch(NotValidDataException e)
            {
                logger.Error(e.Message);
                return ResponseError((int)CodeStatusEnum.BAD_REQUEST, e.Message);
            }
            catch(NotEnoughAttributesException e)
            {
                logger.Error(e.Message);
                return ResponseError((int)CodeStatusEnum.BAD_REQUEST, e.Message);
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
                return ResponseError((int)CodeStatusEnum.INTERNAL_ERROR, "Error desconocido en el sistema");
            }

        }

        /// <summary>
        /// Close custody
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult CloseCustody([FromBody]CloseCustodyRequest obj)
        {

            try
            {
                // Send object to business layer
                CustodyController core = new CustodyController();

                // Verify at least object arrives with data
                if(obj == null) throw new NotEnoughAttributesException("No se ha recibido ningún parámetro");

                // Verify for parameters needed
                if(obj.idCustody == null)
                    throw new NotEnoughAttributesException("No se han recibido todos los parámetros requeridos");

                // Check params's integrity
                if(obj.idCustody.GetType() != typeof(int)) throw new NotEnoughAttributesException("Los tipos de datos no coinciden");

                ActionResponse action = core.CloseCustodyByIdAction(obj.idCustody.Value);

                if(action.code == (int)CodeStatusEnum.OK) { return ResponseOk(action.data); }  // OK
                else { return ResponseError(action.code, action.message); } // NOK
            }
            catch(NotValidDataException e)
            {
                logger.Error(e.Message);
                return ResponseError((int)CodeStatusEnum.BAD_REQUEST, e.Message);
            }
            catch(NotEnoughAttributesException e)
            {
                logger.Error(e.Message);
                return ResponseError((int)CodeStatusEnum.BAD_REQUEST, e.Message);
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
                return ResponseError((int)CodeStatusEnum.INTERNAL_ERROR, "Error desconocido en el sistema");
            }
        }


        

        /// <summary>
        /// Get Custody object by code. If flow comes here, it means the code is valid and must to be completed
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetCustodyByCode([FromBody]RegisterRequest obj)
        {

 
            try
            {

                CustodyController oCustody = new CustodyController();

                // Check for data
                if(obj == null) { throw new NotEnoughAttributesException("No se ha recibido ningún parámetro"); }

                // Find Custody object linked to code received
                Custody custody = oCustody.FindCustodyByCode(obj.idProduct.Value, obj.code, obj.codeType);
                if(custody == null) { throw new NotValidDataException("No hay ninguna custodia vinculada al código proporcionado"); }

                // OK
                /*output.code = 0;
                output.message = "OK";
                output.result = custody.value;*/
                ActionResponse output = new ActionResponse();

                return Ok(output);
            }
            catch(NotValidDataException e)
            {
                logger.Error(e.Message);
                return ResponseError((int)CodeStatusEnum.BAD_REQUEST, e.Message);
            }
            catch(NotEnoughAttributesException e)
            {
                logger.Error(e.Message);
                return ResponseError((int)CodeStatusEnum.BAD_REQUEST, e.Message);
            }
            catch(Exception ex)
            {
                logger.Fatal(ex.Message);
                return ResponseError((int)CodeStatusEnum.INTERNAL_ERROR, "Error desconocido en el sistema");
            }
        }


        /// <summary>
        /// Register a new user into module, making an unique id for any product
        /// </summary>
        /// <param name="obj">It will receive all params to register an user</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Register([FromBody]RegisterRequest obj)
        {

            try
            {

                ClienteController clientCore = new ClienteController();

                // Verify at least object arrives with data
                if(obj == null)  throw new NotEnoughAttributesException("No se ha recibido ningún parámetro");
                
                // Verify for parameters needed
                if(obj.idChannel == null || obj.idProduct == null || obj.value == null)
                    throw new NotEnoughAttributesException("No se han recibido todos los parámetros requeridos");

                // If code is setted, then codeType is requested
                if(obj.code != null)
                {
                    if(obj.codeType == null)
                    {
                        throw new NotEnoughAttributesException("No se ha determinado el tipo de código");
                    }

                    // Check codeType's value
                    string codeType = obj.codeType.ToLower();
                    if(codeType != "short" && codeType != "long")
                    {
                        throw new NotValidDataException("No existe el tipo de código proporcionado");
                    }

                }

                // Check params's integrity
                if(obj.idChannel.GetType() != typeof(int)
                    || obj.idProduct.GetType() != typeof(int)
                    || obj.value.GetType() != typeof(string)) throw new NotEnoughAttributesException("Los tipos de datos no coinciden");


                // Check idGuide (if it's setted)
                if(obj.idGuide != null)
                {

                    int Num;
                    bool isNum = int.TryParse(obj.idGuide.ToString(), out Num);

                    if(!isNum)
                    {
                        obj.idGuide = 0;
                    }
                    else
                    {
                        obj.idGuide = Convert.ToInt32(obj.idGuide);
                    }
                } else
                {
                    // If is null, set to 0
                    obj.idGuide = 0;
                }


                // Call core, general Register process
                ActionResponse action = clientCore.RegisterAction(obj);

                if(action.code == (int)CodeStatusEnum.OK) { return ResponseOk(action.data); }  // OK
                else { return ResponseError(action.code, action.message); } // NOK
            }
            catch(NotValidDataException e)
            {
                logger.Error(e.Message);
                return ResponseError((int)CodeStatusEnum.BAD_REQUEST, e.Message);
            }
            catch(NotEnoughAttributesException e)
            {
                logger.Error(e.Message);
                return ResponseError((int)CodeStatusEnum.BAD_REQUEST, e.Message);
            }
            catch(Exception ex)
            {
                logger.Fatal(ex.Message);
                return ResponseError((int)CodeStatusEnum.INTERNAL_ERROR, "Error desconocido en el sistema");
            }
        }

        /// <summary>
        /// Login for user, it will check if user exists, won't create session
        /// </summary>
        /// <param name="obj">Login request object</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Login([FromBody]LoginRequest obj)
        {
      
            try
            {

                ClienteController clientCore = new ClienteController();

                // Verify at least object arrives with data
                if(obj == null)
                {
                    throw new NotEnoughAttributesException("No se ha recibido ningún parámetro");
                }

                // Verify for parameters needed
                if(obj.idChannel == null || obj.idProduct == null || obj.value == null)
                {
                    throw new NotEnoughAttributesException("No se han recibido todos los parámetros requeridos");
                }
                    

                // Check params's integrity
                if(obj.idChannel.GetType() != typeof(int)
                    || obj.idProduct.GetType() != typeof(int)
                    || obj.value.GetType() != typeof(string))
                {
                    throw new NotValidDataException("Los tipos de datos no coinciden");
                }

                // Call core, general Register process
                ActionResponse action = clientCore.LoginAction(obj);

                if(action.code == (int)CodeStatusEnum.OK) { return ResponseOk(action.data); }  // OK
                else { return ResponseError(action.code, action.message); } // NOK
            }
            catch(NotValidDataException e)
            {
                logger.Error(e.Message);
                return ResponseError((int)CodeStatusEnum.BAD_REQUEST, e.Message);
            }
            catch(NotEnoughAttributesException e)
            {
                logger.Error(e.Message);
                return ResponseError((int)CodeStatusEnum.BAD_REQUEST, e.Message);
            }
            catch(Exception ex)
            {
                logger.Fatal(ex.Message);
                return ResponseError((int)CodeStatusEnum.INTERNAL_ERROR, "Error desconocido en el sistema");
            }
        }

        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="obj">Reset Password request object</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult ResetPassword([FromBody]ResetPasswordRequest obj)
        {
            try
            {

                ClienteController clientCore = new ClienteController();

                // Verify at least object arrives with data
                if(obj == null)
                {
                    throw new NotEnoughAttributesException("No se ha recibido ningún parámetro");
                }

                // Verify for parameters needed
                if(obj.idChannel == null || obj.idProduct == null || obj.value == null)
                {
                    throw new NotEnoughAttributesException("No se han recibido todos los parámetros requeridos");
                }


                // Check params's integrity
                if(obj.idChannel.GetType() != typeof(int)
                    || obj.idProduct.GetType() != typeof(int)
                    || obj.value.GetType() != typeof(string))
                {
                    throw new NotValidDataException("Los tipos de datos no coinciden");
                }

                // Call core, general Register process
                ActionResponse action = clientCore.ResetPasswordAction(obj);

                if(action.code == (int)CodeStatusEnum.OK) { return ResponseOk(action.data); }  // OK
                else { return ResponseError(action.code, action.message); } // NOK
            }
            catch(NotValidDataException e)
            {
                logger.Error(e.Message);
                return ResponseError((int)CodeStatusEnum.BAD_REQUEST, e.Message);
            }
            catch(NotEnoughAttributesException e)
            {
                logger.Error(e.Message);
                return ResponseError((int)CodeStatusEnum.BAD_REQUEST, e.Message);
            }
            catch(Exception ex)
            {
                logger.Fatal(ex.Message);
                return ResponseError((int)CodeStatusEnum.INTERNAL_ERROR, "Error desconocido en el sistema");
            }
        }

        /// <summary>
        /// Update password for user
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdatePassword([FromBody]UpdatePasswordRequest obj)
        {
            try
            {

                ClienteController clientCore = new ClienteController();

                // Verify at least object arrives with data
                if(obj == null)
                {
                    throw new NotEnoughAttributesException("No se ha recibido ningún parámetro");
                }

                // Verify for parameters needed
                if(obj.idClient == null || obj.idProduct == null || obj.idChannel == null || obj.oldPassword == null
                    || obj.newPassword == null || obj.newPasswordRe == null)
                {
                    throw new NotEnoughAttributesException("No se han recibido todos los parámetros requeridos");
                }


                // Check params's integrity
                if(obj.idClient.GetType() != typeof(decimal)
                    || obj.idChannel.GetType() != typeof(int)
                    || obj.idProduct.GetType() != typeof(int)
                    || obj.oldPassword.GetType() != typeof(string)
                    || obj.newPassword.GetType() != typeof(string)
                    || obj.newPasswordRe.GetType() != typeof(string))
                {
                    throw new NotValidDataException("Los tipos de datos no coinciden");
                }

                // Check if password is correct or not
                if(!obj.newPassword.Equals(obj.newPasswordRe))
                {
                    throw new NotValidDataException("La contraseña no coincide");
                }

                // Call core, general Register process
                ActionResponse action = clientCore.UpdatePasswordAction(obj);

                if(action.code == (int)CodeStatusEnum.OK) { return ResponseOk(action.data); }  // OK
                else { return ResponseError(action.code, action.message); } // NOK
            }
            catch(NotValidDataException e)
            {
                logger.Error(e.Message);
                return ResponseError((int)CodeStatusEnum.BAD_REQUEST, e.Message);
            }
            catch(NotEnoughAttributesException e)
            {
                logger.Error(e.Message);
                return ResponseError((int)CodeStatusEnum.BAD_REQUEST, e.Message);
            }
            catch(Exception ex)
            {
                logger.Fatal(ex.Message);
                return ResponseError((int)CodeStatusEnum.INTERNAL_ERROR, "Error desconocido en el sistema");
            }
        }


        /// <summary>
        /// Can set attributes for user (meta-key)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult FillUserData([FromBody]FillUserDataRequest obj)
        {
            try
            {

                ClienteParamController clienteParamController = new ClienteParamController();

                // Verify at least object arrives with data
                if(obj == null)
                {
                    throw new NotEnoughAttributesException("No se ha recibido ningún parámetro");
                }

                // Verify for parameters needed
                if(obj.idClient == null || obj.tags == null || obj.values == null)
                {
                    throw new NotEnoughAttributesException("No se han recibido todos los parámetros requeridos");
                }


                // Check params's integrity
                if(obj.idClient.GetType() != typeof(decimal)
                    || obj.tags.GetType() != typeof(string[])
                    || obj.values.GetType() != typeof(string[]))
                {
                    throw new NotValidDataException("Los tipos de datos no coinciden");
                }

                // Check if number of tags and values is correct
                int n = obj.tags.Length;
                int m = obj.values.Length;
                if(n != m)
                {
                    throw new NotValidDataException("Los valores proporcionados no coinciden con el número de tags enviados");
                }

                // Call core
                ActionResponse action = clienteParamController.FillDataUserAction(obj);

                if(action.code == (int)CodeStatusEnum.OK) { return ResponseOk(action.data); }  // OK
                else { return ResponseError(action.code, action.message); } // NOK
            }
            catch(NotValidDataException e)
            {
                logger.Error(e.Message);
                return ResponseError((int)CodeStatusEnum.BAD_REQUEST, e.Message);
            }
            catch(NotEnoughAttributesException e)
            {
                logger.Error(e.Message);
                return ResponseError((int)CodeStatusEnum.BAD_REQUEST, e.Message);
            }
            catch(Exception ex)
            {
                logger.Fatal(ex.Message);
                return ResponseError((int)CodeStatusEnum.INTERNAL_ERROR, "Error desconocido en el sistema");
            }
        }



        /// <summary>
        /// Get all channels available in module
        /// </summary>
        /// <returns>Objects of Channel entity</returns>
        [HttpGet]
        public IHttpActionResult GetChannelList()
        {
            try

            {

                ChannelController channelCore = new ChannelController();

                // Call core, general Register process
                ActionResponse action = channelCore.GetAllAction();

                if(action.code == (int)CodeStatusEnum.OK) { return ResponseOk(action.data); }  // OK
                else { return ResponseError(action.code, action.message); } // NOK
            }
            catch(Exception ex)
            {
                logger.Fatal(ex.Message);
                return ResponseError((int)CodeStatusEnum.INTERNAL_ERROR, "Error desconocido en el sistema");
            }
        }

        /// <summary>
        /// Get all params available for user identity
        /// </summary>
        /// <returns>Objects of Param entity</returns>
        [HttpGet]
        public IHttpActionResult GetParamList()
        {
            try

            {

                ParamController paramCore = new ParamController();

                // Call core
                ActionResponse action = paramCore.GetAllAction();

                if(action.code == (int)CodeStatusEnum.OK) { return ResponseOk(action.data); }  // OK
                else { return ResponseError(action.code, action.message); } // NOK
            }
            catch(Exception ex)
            {
                logger.Fatal(ex.Message);
                return ResponseError((int)CodeStatusEnum.INTERNAL_ERROR, "Error desconocido en el sistema");
            }
        }


        /// <summary>
        /// Desactive user, in other words, set to inactive credential
        /// </summary>
        /// <param name="obj">Unregister request object</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult Unregister([FromBody]UnregisterRequest obj)
        {
            try
            {

                ClienteController clientCore = new ClienteController();

                // Verify at least object arrives with data
                if(obj == null)
                {
                    throw new NotEnoughAttributesException("No se ha recibido ningún parámetro");
                }

                // Verify for parameters needed
                // password is optional
                if(obj.idChannel == null || obj.idProduct == null || obj.value == null)
                {
                    throw new NotEnoughAttributesException("No se han recibido todos los parámetros requeridos");
                }


                // Check params's integrity
                if(obj.idChannel.GetType() != typeof(int)
                    || obj.idProduct.GetType() != typeof(int)
                    || obj.value.GetType() != typeof(string))
                {
                    throw new NotValidDataException("Los tipos de datos no coinciden");
                }

                // Call core
                ActionResponse action = clientCore.UnregisterAction(obj);

                if(action.code == (int)CodeStatusEnum.OK) { return ResponseOk(action.data); }  // OK
                else { return ResponseError(action.code, action.message); } // NOK
            }
            catch(NotValidDataException e)
            {
                logger.Error(e.Message);
                return ResponseError((int)CodeStatusEnum.BAD_REQUEST, e.Message);
            }
            catch(NotEnoughAttributesException e)
            {
                logger.Error(e.Message);
                return ResponseError((int)CodeStatusEnum.BAD_REQUEST, e.Message);
            }
            catch(Exception ex)
            {
                logger.Fatal(ex.Message);
                return ResponseError((int)CodeStatusEnum.INTERNAL_ERROR, "Error desconocido en el sistema");
            }
        }




        // --------------------------------------------- PRIVATE METHODS ---------------------------------------------



        /// <summary>
        /// OK Response for service
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        private IHttpActionResult ResponseOk(object res)
        {
            JsonResponseOk result = new JsonResponseOk();
            result.data = res;

            return Ok(result);
        }

        /// <summary>
        /// Error Response
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private IHttpActionResult ResponseError(int code, string message)
        {
            JsonResponseErrorParams resultParams = new JsonResponseErrorParams();
            resultParams.code = code;
            resultParams.message = message;

            JsonResponseError result = new JsonResponseError();
            result.error = resultParams;

            //return Content(functions.GetHttpStatusCode(resultParams.code), result);
            return Ok(result);
        }

    }
}
