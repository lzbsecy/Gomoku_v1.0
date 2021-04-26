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
                MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
                cmd = new MySqlCommand(sql, sqlConnection);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }

        public bool Login(MainPack pack, MySqlConnection sqlConnection)
        {
            string username = pack.LoginPack.Username;
            string password = pack.LoginPack.Password;

            string sql = "SELECT * FROM userdata WHERE username='"+username+"' AND password='"+password+"'";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            MySqlDataReader read = cmd.ExecuteReader();
            bool result = read.HasRows;
            if (result)
            {
                Console.WriteLine(username + " 登陆成功!");     
            }
            else
            {
                Console.WriteLine(username + " 登陆失败!");
            }
            read.Close();
            return result;
        }

    }
}
