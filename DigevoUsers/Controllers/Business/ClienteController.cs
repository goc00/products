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
using System.Net.Mail;
using System.Security.Cryptography;
using System.Web.Mvc;

namespace DigevoUsers.Controllers.Business
{
    public class ClienteController : Controller
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();
        CommonTasks functions = new CommonTasks();


        /// <summary>
        /// Registration over any channel defined in module. Credential concept is implicated too.
        /// </summary>
        /// <param name="obj">Request object</param>
        /// <returns>ID user if it's ok or null when it's error</returns>
        public ActionResponse RegisterAction(RegisterRequest obj)
        {
            ActionResponse output = new ActionResponse();

            try
            {

                // OPERATOR registration
                bool asOperatorRegistration = false;
                if(obj.code != null) { asOperatorRegistration = true; }

                // Code doesn't exist, will enter by normal registration flow
                int idProduct = obj.idProduct.Value;
                int idChannel = obj.idChannel.Value;
                string passReq = null;

                if(!String.IsNullOrEmpty(obj.password))
                {
                    passReq = obj.password.Trim();
                }

                // STEP 0: Need to verify if product and channel exist or not
                ProductData prodData = new ProductData();
                Product oProduct = prodData.GetProductById(idProduct);
                if(oProduct == null)
                {
                    return functions.Response((int)CodeStatusEnum.NO_CONTENT, "El producto no existe en el sistema", null);
                }

                ChannelData channelData = new ChannelData();
                Channel oChannel = channelData.GetChannelById(idChannel);
                if(oChannel == null)
                {
                    return functions.Response((int)CodeStatusEnum.NO_CONTENT, "El canal no existe en el sistema", null);
                }


                // We need to check if code is setted
                // If code exists, will check for this first (custody)
                CustodyController custodyController = new CustodyController();
                Custody oCustody = new Custody();
                if(asOperatorRegistration)
                {

                    // Find Custody object linked to code received
                    oCustody = custodyController.FindCustodyByCode(idProduct, obj.code, obj.codeType.ToLower());
                    if(oCustody == null)
                    {
                        return functions.Response((int)CodeStatusEnum.NO_CONTENT, "No hay ninguna custodia vinculada al código proporcionado", null);
                    }

                }

                // Normal flow for registration. Distinct to get code or not, it will do same actions
                // Firstly, will check consistency value in function of channel
                switch(obj.idChannel.Value)
                {
                    case (int)ChannelEnum.EMAIL:
                        try
                        {
                            MailAddress m = new MailAddress(obj.value);
                            break;
                        }
                        catch(FormatException)
                        {
                            return functions.Response((int)CodeStatusEnum.BAD_REQUEST, "El formato del email es incorrecto", null);
                        }

                    case (int)ChannelEnum.ANI:

                        int l = 11;
                        string prefix = "569";

                        bool error = true;
                        string val = obj.value;
                        // Check for prefix (569) considerating solution for Chile (need to be dynamic as soon as possible)
                        if(val.Contains(prefix))
                        {
                            // Check for length (11) adding prefix
                            if(val.Length == l)
                            {
                                error = false;
                            }
                        }

                        if(error)
                        {
                            return functions.Response((int)CodeStatusEnum.BAD_REQUEST, "El formato del ani es incorrecto", null);
                        }

                        break;
                    case (int)ChannelEnum.FACEBOOK:

                        // Will check if ID is a numeric number
                        break;
                }

                // STEP 1: Check if user identify already exists
                // If UserIdentify doesn't exist, it won't check credential because of it doesn't exist too obviously
                // If UserIdentify exists, we need to check for credentials vinculated.
                UserIdentifyData uiData = new UserIdentifyData();

                UserIdentify ui = uiData.FindByIdChannelAndValue(idChannel, obj.value);
                string passMD5 = null;
                if(ui.id_cliente != null)
                {

                    // STEP 2: Check if credential already exists
                    CredentialData cdData = new CredentialData();

                    List<Credential> lstCredential = cdData.FindByProductAndUserIdentify(idProduct, ui.idUserIdentify.Value);

                    if(lstCredential.Count > 0)
                    {
                        return functions.Response((int)CodeStatusEnum.CONFLICT, "La credencial del usuario ya existe en el sistema", null);
                    }
 
                }


                // If entire data is ok, try to create user
                // Will return ID user or null (transaction)
                ClienteData u = new ClienteData();
                // string ani, string email, string usuario, decimal id_operador
                int? res = 0;

                // Create a random value for ani, it must to be fixed to correct logic with registration operator (custody)
                int max = Int32.Parse(functions.ConfigItem("MAX_RANDOM_ANI_USER"));
                string rndString = "user_" + functions.GetUniqueKey(max);

                // If password is setted, it becomes as MD5
                if(!String.IsNullOrEmpty(passReq))
                {
                    // Will check password integrity
                    int minLengthPass = Int32.Parse(functions.ConfigItem("MIN_PASS_LENGTH"));
                    if(passReq.Length < minLengthPass)
                    {
                        return functions.Response((int)CodeStatusEnum.BAD_REQUEST, "La contraseña debe tener un mínimo de " + minLengthPass + " caracteres", null);
                    }

                    using(MD5 md5Hash = MD5.Create())
                    {
                        passMD5 = functions.GetMd5Hash(md5Hash, passReq);
                    }

                }
                else
                {
                    // Without password
                    passMD5 = passReq;  
                }


                // Will create user, receiving UserIdentify or making it within other actions
                if(ui.idUserIdentify == null)
                {

                    // Operator registration always won´t have UserIdentify associated (will be created later)
                    // UserIdentify unknown
                    res = u.CreateUser(idChannel, idProduct, obj.value, passMD5, 0, rndString, "", "", 0, false, 0, obj.idGuide.Value);
                }
                else
                {
                    // UserIdentify already known
                    res = u.CreateUser(idChannel, idProduct, obj.value, passMD5, 0, rndString, "", "", 0, true, ui.idUserIdentify.Value, obj.idGuide.Value);
                }

                // Sp's response
                if(res == null)
                {
                    return functions.Response((int)CodeStatusEnum.CONFLICT, "No se pudo registrar al usuario en la plataforma", res);
                }

                // OK (will return ID cliente generated -last insert-)
                RegisterResponse response = new RegisterResponse();
                response.idClient = (decimal)res;
                // Normal or custody registration
                if(asOperatorRegistration) {

                    // Reserved value into response
                    response.reservedValue = oCustody.value;

                    // Try to close custody
                    int idCustody = oCustody.idCustody.Value;
                    int idClient = (int)oCustody.id_cliente.Value;

                    // Need to close Custody, it will be the end of this process
                    // Will check custody is valid or not
                    ActionResponse resX = custodyController.CloseCustodyByIdAction(idCustody);
                    if(resX.code != (int)CodeStatusEnum.OK)
                    {
                        logger.Error(resX.message);
                    }

                }

                return functions.Response((int)CodeStatusEnum.OK, "OK", response);

            }
            catch(Exception e)
            {
                logger.Fatal(e.Message);
                return functions.Response((int)CodeStatusEnum.INTERNAL_ERROR, e.Message, null);
            }
            
        }



        /// <summary>
        /// Authentication into user module. Will check if user exists or not
        /// </summary>
        /// <param name="obj">Request object</param>
        /// <returns>User object</returns>
        public ActionResponse LoginAction(LoginRequest obj)
        {
            ActionResponse output = new ActionResponse();

            try
            {

                // STEP 0: Need to verify if product and channel exist or not
                ProductData prodData = new ProductData();
                Product oProduct = prodData.GetProductById(obj.idProduct.Value);
                if(oProduct == null)
                {
                    return functions.Response((int)CodeStatusEnum.NO_CONTENT, "El producto no existe en el sistema", null);
                }

                ChannelData channelData = new ChannelData();
                Channel oChannel = channelData.GetChannelById(obj.idChannel.Value);
                if(oChannel == null)
                {
                    return functions.Response((int)CodeStatusEnum.NO_CONTENT, "El canal no existe en el sistema", null);
                }


                // STEP 1: Check if user identify already exists
                UserIdentifyData uiData = new UserIdentifyData();

                UserIdentify ui = uiData.FindByIdChannelAndValue(obj.idChannel.Value, obj.value);
                if(ui.id_cliente == null)
                {
                    return functions.Response((int)CodeStatusEnum.NO_CONTENT, "La identidad del usuario no existe en el sistema", null);
                }

                // STEP 2: If pass is setted, we need to check credentials too
                CredentialData credData = new CredentialData();
                string tmpPass = null;

                if(obj.password != null)
                {
                    using(MD5 md5Hash = MD5.Create())
                    {
                        tmpPass = functions.GetMd5Hash(md5Hash, obj.password);
                    }
                }

                Credential cred = credData.FindByProductAndUserIdentifyAndPass(obj.idProduct.Value, ui.idUserIdentify.Value, tmpPass);
                if(cred.idCredential == null)
                {
                    return functions.Response((int)CodeStatusEnum.CONFLICT, "La contraseña no corresponde", null);
                }

                switch(cred.idState.Value)
                {
                    case (int)StateEnum.INACTIVE:
                        return functions.Response((int)CodeStatusEnum.CONFLICT, "La credencial del usuario se encuentra inactiva", null);
                    case (int)StateEnum.TEMPORAL_PASSWORD:
                        return functions.Response((int)CodeStatusEnum.CONFLICT, "No se puede acceder con una contraseña temporal. Por favor ejecutar servicio de actualización.", null);
                    case (int)StateEnum.TEMPORAL_PASSWORD_USED:
                        return functions.Response((int)CodeStatusEnum.CONFLICT, "La contraseña temporal ya no es válida.", null);
                }

                // Get User
                ClienteData clientData = new ClienteData();
                Cliente res = clientData.GetUserById(ui.id_cliente.Value);

                if(res.id_cliente == null) {
                    return functions.Response((int)CodeStatusEnum.NO_CONTENT, "No se ha podido encontrar el usuario relacionado a las credenciales proporcionadas", null);
                }

                // OK (will return ID cliente generated -last insert-)
                LoginResponse response = new LoginResponse();
                response.idClient = res.id_cliente.Value;
                return functions.Response((int)CodeStatusEnum.OK, "OK", response);

            }
            catch(Exception e)
            {
                logger.Fatal(e.Message);
                return functions.Response((int)CodeStatusEnum.INTERNAL_ERROR, e.Message, null);
            }

        }



        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="obj">Request object</param>
        /// <returns>User object</returns>
        public ActionResponse ResetPasswordAction(ResetPasswordRequest obj)
        {
            ActionResponse output = new ActionResponse();

            try
            {
                int maxTmpPass = Int32.Parse(functions.ConfigItem("MAX_TMP_PASS"));
                int idProduct = obj.idProduct.Value;
                int idChannel = obj.idChannel.Value;

                // STEP 0: Need to verify if product and channel exist or not
                ProductData prodData = new ProductData();
                Product oProduct = prodData.GetProductById(idProduct);
                if(oProduct == null)
                {
                    return functions.Response((int)CodeStatusEnum.NO_CONTENT, "El producto no existe en el sistema", null);
                }

                ChannelData channelData = new ChannelData();
                Channel oChannel = channelData.GetChannelById(idChannel);
                if(oChannel == null)
                {
                    return functions.Response((int)CodeStatusEnum.NO_CONTENT, "El canal no existe en el sistema", null);
                }

                // STEP 1: Check if user identify already exists
                UserIdentifyData uiData = new UserIdentifyData();

                UserIdentify ui = uiData.FindByIdChannelAndValue(idChannel, obj.value);
                if(ui.id_cliente == null)
                {
                    return functions.Response((int)CodeStatusEnum.NO_CONTENT, "La identidad del usuario no existe en el sistema", null);
                }

                // STEP 2: Find Credential related to UserIdentify and Product
                int idUserIdentify = ui.idUserIdentify.Value;

                CredentialData credData = new CredentialData();
                List<Credential> lstCredential = credData.FindByProductAndUserIdentify(idProduct, idUserIdentify);
                if(lstCredential.Count <= 0)
                {
                    return functions.Response((int)CodeStatusEnum.NO_CONTENT, "No existen credenciales asociadas", null);
                }

                // Generate password and create temporal credential
                string rndString = "";
                using(MD5 md5Hash = MD5.Create())
                {
                    rndString = functions.GetUniqueKey(maxTmpPass);
                    string rndStringMD5 = functions.GetMd5Hash(md5Hash, rndString);

                    int idCredentialTmp = credData.CreateCredential(idProduct, idUserIdentify, (int)StateEnum.TEMPORAL_PASSWORD, rndStringMD5);
                    if(idCredentialTmp == 0)
                    {
                        return functions.Response((int)CodeStatusEnum.INTERNAL_ERROR, "No se pudo crear la credencial temporal", null);
                    }
                }

                // Will send email with new pass generated (only if channel is email)
                if(idChannel == (int)ChannelEnum.EMAIL)
                {
                    /*string productEmailFrom = "FROM_EMAIL_" + idProduct;
                    string productEmailPass = "PASS_EMAIL_" + idProduct;

                    string emailFrom;
                    string emailPass;

                    string checkProductEmailFrom = System.Web.Configuration.WebConfigurationManager.AppSettings[productEmailFrom];
                    string checkProductEmailPass = System.Web.Configuration.WebConfigurationManager.AppSettings[productEmailPass];

                    // Send mail if exists user and pass for the product
                    // else send by default user and pass
                    if (checkProductEmailFrom != null && checkProductEmailPass != null)
                    {
                        emailFrom = functions.ConfigItem(productEmailFrom);
                        emailPass = functions.ConfigItem(productEmailPass);
                    }
                    else
                    {
                        emailFrom = functions.ConfigItem("FROM_EMAIL");
                        emailPass = functions.ConfigItem("PASS_EMAIL");
                    }

                    MailMessage mail = new MailMessage();

                    mail.From = new MailAddress(emailFrom, oProduct.name, System.Text.Encoding.UTF8);
                    mail.To.Add(new MailAddress(obj.value));
                    mail.SubjectEncoding = System.Text.Encoding.UTF8;
                    mail.Subject = oProduct.name + " | Solicitud cambio de contraseña";
                    mail.Body = "Estimado usuario, <br><br> Tu contraseña temporal es: " + rndString 
                        + ", ingresa con esta contraseña para luego generar una nueva."
                        + "<br><br><br> <i>Este correo fue enviado de forma automática, no lo respondas.</i>";
                    mail.BodyEncoding = System.Text.Encoding.UTF8;
                    mail.IsBodyHtml = true;

                    SmtpClient client = new SmtpClient();

                    client.Host                     = functions.ConfigItem("HOST_EMAIL");
                    client.Port                     = Int32.Parse(functions.ConfigItem("PORT_EMAIL"));
                    client.EnableSsl                = true;
                    client.DeliveryMethod           = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials    = false;
                    client.Credentials              = new System.Net.NetworkCredential(emailFrom, emailPass);
                    client.Send(mail);*/
                }

                // OK, return temporal password
                ResetPasswordResponse response = new ResetPasswordResponse();
                response.idClient = (int)ui.id_cliente;
                response.newPassword = rndString;
                return functions.Response((int)CodeStatusEnum.OK, "OK", response);
            }
            catch(Exception e)
            {
                logger.Fatal(e.Message);
                return functions.Response((int)CodeStatusEnum.INTERNAL_ERROR, e.Message, null);
            }
        }


        /// <summary>
        /// Update Password
        /// </summary>
        /// <param name="obj">Request object</param>
        /// <returns>User object</returns>
        public ActionResponse UpdatePasswordAction(UpdatePasswordRequest obj)
        {
   
            try
            {

                int idProduct = obj.idProduct.Value;
                int idChannel = obj.idChannel.Value;
                decimal idClient = obj.idClient.Value;

                // STEP 0: Need to verify if product and channel exist or not
                ProductData prodData = new ProductData();
                Product oProduct = prodData.GetProductById(idProduct);
                if(oProduct == null)
                {
                    return functions.Response((int)CodeStatusEnum.NO_CONTENT, "El producto no existe en el sistema", null);
                }

                ChannelData channelData = new ChannelData();
                Channel oChannel = channelData.GetChannelById(idChannel);
                if(oChannel == null)
                {
                    return functions.Response((int)CodeStatusEnum.NO_CONTENT, "El canal no existe en el sistema", null);
                }

                // STEP 0.1: Verify if user exists
                ClienteData clientData = new ClienteData();
                Cliente oClient = clientData.GetUserById(idClient);
                if(oClient.id_cliente == null)
                {
                    return functions.Response((int)CodeStatusEnum.NO_CONTENT, "No existe el usuario en el sistema", null);
                }

                // STEP 1: Check if user identify already exists by id_cliente + channel
                UserIdentifyData uiData = new UserIdentifyData();

                UserIdentify ui = uiData.FindByIdUserAndIdChannel(idClient, idChannel);
                if(ui.id_cliente == null)
                {
                    return functions.Response((int)CodeStatusEnum.NO_CONTENT, "La identidad del usuario no existe en el sistema", null);
                }

                int idUserIdentify = ui.idUserIdentify.Value;

                // STEP 2: Check credential
                string oldPassAsMD5 = "";
                string newPassAsMD5 = "";
                using(MD5 md5Hash = MD5.Create())
                {

                    string oldPass = obj.oldPassword.Trim();
                    string newPass = obj.newPassword.Trim();

                    // Password integrity
                    if(String.IsNullOrEmpty(newPass))
                    {
                        return functions.Response((int)CodeStatusEnum.BAD_REQUEST, "La contraseña no puede ser una cadena vacía", null);
                    }

                    int minLengthPass = Int32.Parse(functions.ConfigItem("MIN_PASS_LENGTH"));
                    if(newPass.Length < minLengthPass)
                    {
                        return functions.Response((int)CodeStatusEnum.BAD_REQUEST, "La contraseña debe tener un mínimo de " + minLengthPass + " caracteres", null);
                    }

                    if(oldPass == newPass)
                    {
                        return functions.Response((int)CodeStatusEnum.BAD_REQUEST, "La nueva contraseña no puede ser igual a la actual", null);
                    }

                    // Passwords as MD5
                    oldPassAsMD5 = functions.GetMd5Hash(md5Hash, oldPass);
                    newPassAsMD5 = functions.GetMd5Hash(md5Hash, newPass);
                }
                CredentialData credData = new CredentialData();
                Credential oCredential = credData.FindByProductAndUserIdentifyAndPass(idProduct,
                                                                                       idUserIdentify,
                                                                                        oldPassAsMD5);
                if(oCredential.idCredential == null)
                {
                    return functions.Response((int)CodeStatusEnum.BAD_REQUEST, "La contraseña no es válida", null);
                }

                int idCredential = oCredential.idCredential.Value;

                // STEP 3: If Credential is OK, need to check if credential is NORMAL or TEMPORAL
                if(oCredential.idState.Value == (int)StateEnum.ACTIVE)
                {
                    // Normal process
                    bool updCredential = credData.UpdatePassword(idCredential, newPassAsMD5, (int)StateEnum.ACTIVE);
                    if(!updCredential)
                    {
                        return functions.Response((int)CodeStatusEnum.INTERNAL_ERROR, "No se pudo actualizar la contraseña", null);
                    }
                }
                else if(oCredential.idState.Value == (int)StateEnum.TEMPORAL_PASSWORD)
                {
                    // Will set normal credential with new password
                    // Firstly, need to find it
                    List<Credential> lstCredentials = credData.FindByProductAndUserIdentify(idProduct, idUserIdentify);
                    foreach(Credential o in lstCredentials)
                    {
                        // Finding normal credential related to product and useridentify
                        if(o.idState.Value == (int)StateEnum.ACTIVE)
                        {
                            bool updCredential = credData.UpdatePassword(o.idCredential.Value, newPassAsMD5, (int)StateEnum.ACTIVE);
                            if(!updCredential)
                            {
                                return functions.Response((int)CodeStatusEnum.INTERNAL_ERROR, "No se pudo actualizar la contraseña", null);
                            }
                        }
                    }

                    // Pass temporal credential to TEMPORAL_PASSWORD_USED
                    bool updTmpCredential = credData.UpdatePassword(idCredential, oldPassAsMD5, (int)StateEnum.TEMPORAL_PASSWORD_USED);
                    if(!updTmpCredential)
                    {
                        return functions.Response((int)CodeStatusEnum.INTERNAL_ERROR, "No se pudo actualizar la credencial temporal", null);
                    }

                }
                else
                {
                    // Desactive, I can´t do anything
                    return functions.Response((int)CodeStatusEnum.CONFLICT, "La credencial se encuentra inactiva y no se puede actualizar", null);
                }

                // OK, return true/false
                UpdatePasswordResponse response = new UpdatePasswordResponse();
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
        /// Will desactive user credential (user identify + product)
        /// </summary>
        /// <param name="obj">Unregister request object (idProduct, idChannel, value)</param>
        /// <returns></returns>
        public ActionResponse UnregisterAction(UnregisterRequest obj)
        {

            try
            {

                int idProduct = obj.idProduct.Value;
                int idChannel = obj.idChannel.Value;
                string value = obj.value; // effective value for user identify
                string password = obj.password;

                // STEP 0: Need to verify if product and channel exist or not
                ProductData prodData = new ProductData();
                Product oProduct = prodData.GetProductById(obj.idProduct.Value);
                if(oProduct == null)
                {
                    return functions.Response((int)CodeStatusEnum.NO_CONTENT, "El producto no existe en el sistema", null);
                }

                ChannelData channelData = new ChannelData();
                Channel oChannel = channelData.GetChannelById(obj.idChannel.Value);
                if(oChannel == null)
                {
                    return functions.Response((int)CodeStatusEnum.NO_CONTENT, "El canal no existe en el sistema", null);
                }

                // STEP 1: Check if user identify already exists by id_cliente + channel + value
                UserIdentifyData uiData = new UserIdentifyData();

                UserIdentify ui = uiData.FindByIdChannelAndValue(idChannel, value);
                if(ui.idUserIdentify == null)
                {
                    return functions.Response((int)CodeStatusEnum.NO_CONTENT, "La identidad del usuario no existe en el sistema", null);
                }

                // STEP 2: Check credentials
                string passwordAsMD5 = null;
                if(password != null)
                {
                    using(MD5 md5Hash = MD5.Create())
                    {
                        passwordAsMD5 = functions.GetMd5Hash(md5Hash, password.Trim());
                    }
                }

                CredentialData credData = new CredentialData();
                Credential oCredential = credData.FindByProductAndUserIdentifyAndPass(idProduct, ui.idUserIdentify.Value, passwordAsMD5);
                if(oCredential.idCredential == null)
                {
                    return functions.Response((int)CodeStatusEnum.NO_CONTENT, "No existe ninguna credencial con los datos proporcionados", null);
                }

                // STEP 3: If Credential is OK, will use same method passing actual password, the idea is use it for updating
                // the state to INACTIVE, it's not neccesary to make another method
                int idCredential = oCredential.idCredential.Value;
                bool updCredential = credData.UpdatePassword(idCredential, passwordAsMD5, (int)StateEnum.INACTIVE);
                if(!updCredential)
                {
                    return functions.Response((int)CodeStatusEnum.INTERNAL_ERROR, "No se pudo actualizar el estado de la credencial", null);
                }

                // OK, return true/false
                UnregisterResponse response = new UnregisterResponse();
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
        /// Check if emails exists
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool EmailExists(string email)
        {
            bool output = false;

            try
            {

                // Check if user already exists in function of email
                UserIdentifyData uiData = new UserIdentifyData();

                UserIdentify ui = uiData.FindByIdChannelAndValue((int)ChannelEnum.EMAIL, email);
                if(ui.id_cliente != null)
                {
                    output = true;
                }
                
            }
            catch(Exception e)
            {
                //output.message = e.Message;
            }

            return output;
        }

      
    }
}