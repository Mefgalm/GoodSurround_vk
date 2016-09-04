using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodSurround.DataModels
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Photo50 { get; set; }        

        public string AccessToken { get; set; }

        public int AlbumsCount { get; set; }

        public int AudiosCount { get; set; }

        #region application token

        public Guid Token { get; set; }
        public DateTime ExpiredAt { get; set; }

        #endregion
    }
}
