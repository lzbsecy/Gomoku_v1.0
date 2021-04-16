using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using SocketGameProtocol;

namespace SocketMultiplayerGameServer.DAO
{
    class UserData
    {

        public bool Logon(MainPack pack,MySqlConnection sqlConnection)
        {
            string username = pack.LoginPack.Username;
            string password = pack.LoginPack.Password;
            
            try
            {
                string sql = "INSERT INTO `socketgame`.`userdata`(`username`,`password`) VALUES ('" + username + "', '" + password + "')";
                MySqlCommand comd = new MySqlCommand(sql, sqlConnection);

                comd = new MySqlCommand(sql, sqlConnection);

                comd.ExecuteNonQuery();
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }

    }
}
