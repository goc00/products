using DigevoUsers.Models.Dto;
using DigevoUsers.Models.Enum;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DigevoUsers.Models.Data
{

    public class ClienteData : Connection {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ClienteData() : base() { }

        /// <summary>
        /// Get User by ID
        /// </summary>
        /// <param name="id_cliente"></param>
        /// <param name="idChannel"></param>
        /// <param name="value"></param>
        /// <returns>Cliente object</returns>
        public Cliente GetUserById(decimal id_cliente)
        {
            Cliente output = new Cliente();

            Dictionary<string, object> attrs = new Dictionary<string, object>();
            attrs.Add("id_cliente", id_cliente);

            List<Cliente> res = SelectSP<Cliente>("SpFindCliente", attrs);

            if(res.Count > 0)
            {
                output = res[0];
            }
            return output;

        }

        /// <summary>
        /// Create user under transaction, "password" IS NOT "pass" platform's attribute, it will be empty
        /// because we don't need to create this user as platform does.
        /// </summary>
        /// <param name="idChannel">ID channel (email, ani, facebook, google+ and more)</param>
        /// <param name="idProduct">ID product</param>
        /// <param name="value">ID channel's value (example: if channel is email, value will be xxx@yyy.com)</param>
        /// <param name="password">Optional password for credential</param>
        /// <param name="ani">N/A</param>
        /// <param name="email">N/A</param>
        /// <param name="usuario">N/A</param>
        /// <param name="id_operador">N/A</param>
        /// <returns>ID User just created or null</returns>
        public int? CreateUser(int idChannel, int idProduct, string value, string password, int idCustody,
                            string ani, string email, string usuario, decimal id_operador,

                            bool userIdentifyKnown = false, int idUserIdentify = 0, int id_pauta = 0)
        {

            int? result = null;

            // Create database object to be used as transaction
            var db = CreateDatabase(); 
            
            // Start transaction
            using (var conn = CreateConnection(db))
            {

                var trans = CreateTransaction(conn);

                try
                {

                    // STEP 0 (optional): If detects code is setted, will try to close it before create user
                    if(idCustody > 0)
                    {
                        Dictionary<string, object> attrsCustody = new Dictionary<string, object>();
                        attrsCustody.Add("idCustody", idCustody);

                        var commandCustody = CreateDbCommand(db, "SpCloseCustody", attrsCustody);

                        int resUpdCustody = Convert.ToInt32(db.ExecuteNonQuery(commandCustody, trans));

                        if(resUpdCustody <= 0) { throw new Exception("No hay se pudo actualizar el estado de la custodia activa"); }

                    }
                    
                    // STEP 1: Create user (master), need to send these values because of SP requires them
                    // It's only for keeping older logic, it won't affect to module
                    Dictionary<string, object> attrsCliente = new Dictionary<string, object>();
                    attrsCliente.Add("ani", ani);
                    attrsCliente.Add("email", email);
                    attrsCliente.Add("usuario", usuario);
                    attrsCliente.Add("pass", "");
                    attrsCliente.Add("id_operador", id_operador);

                    var commandCliente = CreateDbCommand(db, "pa_InsCliente2", attrsCliente);

                    // Will be catch SCOPE_IDENTITY() from SP called
                    int id_cliente = Convert.ToInt32(db.ExecuteScalar(commandCliente, trans));
                    if(id_cliente <= 0) { throw new Exception("No se pudo crear el usuario en el sistema"); }


                    // Add to list entity
                    Dictionary<string, object> attrsList = new Dictionary<string, object>();
                    attrsList.Add("id_cliente", id_cliente);
                    attrsList.Add("id_product", idProduct);
                    if(id_pauta != 0) { attrsList.Add("id_pauta", id_pauta); }
                    else { attrsList.Add("id_pauta", null); }

                    var commandList = CreateDbCommand(db, "pa_InsClienteListaPautaByIdproduct", attrsList);

                    int idList = Convert.ToInt32(db.ExecuteScalar(commandList, trans));
                    if(idList <= 0) { throw new Exception("No se pudo asociar al usuario con la lista"); }

                    // STEP 2: Create UserIdentify or use it
                    if(!userIdentifyKnown)
                    {
                        Dictionary<string, object> attrsUserIdentify = new Dictionary<string, object>();
                        attrsUserIdentify.Add("id_cliente", id_cliente);
                        attrsUserIdentify.Add("idChannel", idChannel);
                        attrsUserIdentify.Add("value", value);

                        var commandUserIdentify = CreateDbCommand(db, "SpCreateUserIdentify", attrsUserIdentify);

                        idUserIdentify = Convert.ToInt32(db.ExecuteScalar(commandUserIdentify, trans));

                        if(idUserIdentify <= 0) { throw new Exception("No se pudo crear la identidad del usuario"); }
                    }

                    // STEP 3: Create Credential (default)
                    Dictionary<string, object> attrsCredential = new Dictionary<string, object>();
                    attrsCredential.Add("idProduct", idProduct);
                    attrsCredential.Add("idUserIdentify", idUserIdentify);
                    attrsCredential.Add("idState", (int)StateEnum.ACTIVE);
                    attrsCredential.Add("password", password);

                    var commandCredential = CreateDbCommand(db, "SpCreateCredential", attrsCredential);

                    int idCredential = Convert.ToInt32(db.ExecuteScalar(commandCredential, trans));

                    if(idCredential <= 0) { throw new Exception("No se pudo crear la credencial de acceso del usuario"); }

                    // OK
                    trans.Commit();
                    result = id_cliente;

                }
                catch(Exception e)
                {
                    // Any error in execution will trigger rollback
                    string message = e.Message;
                    trans.Rollback();
                }
                finally
                {
                    CloseConnection(conn);
                }

            }

            return result;
        }


        /// <summary>
        /// Find id_cliente related to ani, it's a platform action (calling internal sp)
        /// </summary>
        /// <param name="ani">ANI (mobile number)</param>
        /// <returns>Return id_cliente from cliente table</returns>
        public decimal FindIdUserFromAni(string ani)
        {
            decimal output = 0;

            Dictionary<string, object> attrs = new Dictionary<string, object>();
            attrs.Add("ani", ani);

            List<Cliente> res = SelectSP<Cliente>("pa_SelCliente", attrs);
            if(res.Count > 0)
            {
                Cliente o = res.First();
                output = o.id_cliente.Value;
            }
            return output;
        }


        public int CreateUserIntoPlatform(string ani, string email, string usuario, string pass, decimal id_operador)
        {
            // Invoke SP external in order to create a new user in platform
            Dictionary<string, object> attrs = new Dictionary<string, object>();
            attrs.Add("ani", ani);
            attrs.Add("email", email);
            attrs.Add("usuario", usuario);
            attrs.Add("pass", pass);
            attrs.Add("id_operador", id_operador);

            return InsertSP("pa_InsCliente", attrs);
        }


        public Cliente FindUserByAccess()
        {
            Cliente o = new Cliente();
            return o;
        }


        public int NewCustody(Custody o)
        {
            return 1;
        }


        public Custody GetCustodyByCode(string code)
        {
            Custody o = new Custody();

            string sql = String.Format("SELECT * FROM {0} WHERE code = '{1}'", "", code);

            List<Custody> res = Select<Custody>(sql);
            if(res.Count > 0)
            {
                o = res.First();
            }

            return o;
        }

    }
}