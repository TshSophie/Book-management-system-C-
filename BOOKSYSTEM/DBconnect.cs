using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace BOOKSYSTEM
{
    class DBconnect
    {
        public static SqlConnection BooksystemCon()
        {
            return new SqlConnection("server=.;database=BOOK_MANAGE_SYSTEM;Trusted_Connection=SSPI");
        }
    }
}
