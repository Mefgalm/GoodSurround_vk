using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodSurround.VkModels
{
    public class AccessToken
    {
        public string access_token { get; set; }
        public long expires_in { get; set; }
        public int user_id { get; set; }

        #region errors

        public string error { get; set; }
        public string error_description { get; set; }

        #endregion
    }
}
