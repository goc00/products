using DigevoUsers.Models.Dto;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DigevoUsers.Models.Data
{

    public class CredentialData : Connection
    {

        const string TABLE = "Credential";

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public CredentialData() : base() { }

        /// <summary>
        /// Get credential in function of product, useridentify and password
        /// </summary>
        /// <param name="idProduct">ID Product</param>
        /// <param name="idUserIdentify">ID UserIdentify (id_cliente + idChannel + value)</param>
        /// <param name="password">Password related to credential</param>
        /// <returns></returns>
        public Credential FindByProductAndUserIdentifyAndPass(int idProduct, int idUserIdentify, string password)
        {
            Credential output = new Credential();

            List<Credential> res = FindByProductAndUserIdentify(idProduct, idUserIdentify);

            if(res.Count > 0)
            {
                foreach(Credential o in res)
                {
                    if(o.password == password)
                    {
                        output = o;
                        break;
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// Get credential in function of idUserIdentify and idProduct
        /// </summary>
        /// <param name="idProduct">ID Product</param>
        /// <param name="idUserIdentify">ID User Identify related to credential</param>
        /// <returns></returns>
        public List<Credential> FindByProductAndUserIdentify(int idProduct, int idUserIdentify)
        {

            List<Credential> output = new List<Credential>();

            Dictionary<string, object> attrs = new Dictionary<string, object>();
            attrs.Add("idProduct", idProduct);
            attrs.Add("idUserIdentify", idUserIdentify);

            output = SelectSP<Credential>("SpFindCredentialsByProductAndUserIdentify", attrs);
            
            return output;
        }


        /// <summary>
        /// Update credential's password
        /// </summary>
        /// <param name="idCredential">ID Credential</param>
        /// <param name="password">New Password</param>
        /// <param name="idState">(optional) ID State</param>
        /// <returns></returns>
        public bool UpdatePassword(int idCredential, string password, int idState = 0)
        {
            try
            {
                Dictionary<string, object> attrs = new Dictionary<string, object>();
                attrs.Add("idCredential", idCredential);
                attrs.Add("newPass", password);
                attrs.Add("idState", idState);
  
                return UpdateSP("SpUpdatePasswordCredential", attrs) > 0;
            }
            catch(SqlException e)
            {
                logger.Error("Error SQL: " + e.Message);
            }
            catch(Exception e)
            {
                logger.Error("Error desconocido en ClienteData: " + e.Message);
            }

            return false;
        }

        /// <summary>
        /// Create Credential
        /// </summary>
        /// <param name="idProduct">ID Product</param>
        /// <param name="idUserIdentify">ID UserIdentify (channel + value)</param>
        /// <param name="idState">ID State</param>
        /// <param name="password">(optional) Credential's Password</param>
        /// <returns>ID Credential</returns>
        public int CreateCredential(int idProduct, int idUserIdentify, int idState, string password)
        {
            int result = 0;

            try
            {
                Dictionary<string, object> attrsCredential = new Dictionary<string, object>();
                attrsCredential.Add("idProduct", idProduct);
                attrsCredential.Add("idUserIdentify", idUserIdentify);
                attrsCredential.Add("idState", idState);
                attrsCredential.Add("password", password);

                return InsertSP("SpCreateCredential", attrsCredential);
            }
            catch(SqlException e)
            {
                logger.Error("Error SQL: " + e.Message);
            }
            catch(Exception e)
            {
                logger.Error("Error desconocido en ClienteData: " + e.Message);
            }

            return result;
        }

    }
}