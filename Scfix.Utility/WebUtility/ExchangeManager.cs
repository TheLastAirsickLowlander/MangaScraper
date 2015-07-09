using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scfix.Utility.WebUtility
{
   public class ExchangeManager
    {
       protected ExchangeService _Service = null;
       public ExchangeService Service
       {
           get
           {
               return _Service;
           }
           private set
           {
               _Service = value;
           }
       }

       public ExchangeManager(String EmailAddress, String Password)
       {
           _Service = GetBinding(EmailAddress, Password);
       }

       #region Server Connection
       static public ExchangeService GetBinding(String EmailAddress, String Password) 
        {
            ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010_SP1);
            service.Credentials =  new WebCredentials(EmailAddress, Password);
            try
            {
                service.AutodiscoverUrl(EmailAddress, RedirectionUrlValidationCallback);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return service;
        }

       private static bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            return (redirectionUrl == "https://autodiscover-s.outlook.com/autodiscover/autodiscover.xml");
        }
       #endregion
    }
}
