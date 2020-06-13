using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STXtoSQL.Log;
using STXtoSQL.DataAccess;
using STXtoSQL.Models;

namespace STXtoSQL_IPTFRC_NET
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.LogWrite("MSG", "Start: " + DateTime.Now.ToString());

            // Declare and defaults
            int odbcCnt = 0;
            int insertCnt = 0;
            int importCnt = 0;

            #region FromSTRATIX
            ODBCData objODBC = new ODBCData();

            List<IPTFRC> lstIPTFRC = new List<IPTFRC>();

            // Get data from Straix
            try
            {
                lstIPTFRC = objODBC.Get_IPTFRC();
            }
            catch (Exception ex)
            {
                Logger.LogWrite("EXC", ex);
                Logger.LogWrite("MSG", "Return");
                return;
            }
            #endregion

            #region ToSQL
            SQLData objSQL = new SQLData();

            // Only work in SQL database, if records were retreived from Stratix
            if (lstIPTFRC.Count != 0)
            {
                odbcCnt = lstIPTFRC.Count;

                // Put Stratix data in lstIPTFRC into IMPORT IPTFRC table
                try
                {
                    importCnt = objSQL.Write_IPTFRC_IMPORT(lstIPTFRC);
                }
                catch (Exception ex)
                {
                    Logger.LogWrite("EXC", ex);
                    Logger.LogWrite("MSG", "Return");
                    return;
                }

                // Call SP to put IMPORT IPTFRC table data into WIP IPTFRC table
                try
                {
                    insertCnt = objSQL.Write_IMPORT_to_IPTFRC();
                }
                catch (Exception ex)
                {
                    Logger.LogWrite("EXC", ex);
                    Logger.LogWrite("MSG", "Return");
                    return;
                }

                Logger.LogWrite("MSG", "ODBC/IMPORT/INSERT=" + odbcCnt.ToString() + ":" + importCnt.ToString() + ":" + insertCnt.ToString());
            }
            else
                Logger.LogWrite("MSG", "No data");

            Logger.LogWrite("MSG", "End: " + DateTime.Now.ToString());
            #endregion

            // Testing
            Console.WriteLine("Press key to exit");
            Console.ReadKey();
        }
    }
}
