using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using STXtoSQL.Models;

namespace STXtoSQL.DataAccess
{
    class SQLData : Helpers
    {
        // Insert list of IPTFRC from STRATIX into IMPORT
        public int Write_IPTFRC_IMPORT(List<IPTFRC> lstIPTFRC)
        {
            // Returning rows inserted into IMPORT
            int r = 0;

            SqlConnection conn = new SqlConnection(STRATIXDataConnString);

            try
            {
                conn.Open();

                SqlTransaction trans = conn.BeginTransaction();

                SqlCommand cmd = new SqlCommand();

                cmd.Transaction = trans;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                // First empty IMPORT table
                try
                {
                    cmd.CommandText = "DELETE from ST_IMPORT_tbl_IPTFRC";

                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    throw;
                }

                try
                {
                    // Change Text to Insert data into IMPORT
                    cmd.CommandText = "INSERT INTO ST_IMPORT_tbl_IPTFRC (frc_job_no,frc_cons_ln_no,frc_itm_ctl_no,frc_tag_no,frc_nbr_stp,frc_arb_pos,frc_asgn_bal,frc_slit_typ) " +
                                        "VALUES (@arg1,@arg2,@arg3,@arg4,@arg5,@arg6,@arg7,@arg8)";

                    cmd.Parameters.Add("@arg1", SqlDbType.Int);
                    cmd.Parameters.Add("@arg2", SqlDbType.Int);
                    cmd.Parameters.Add("@arg3", SqlDbType.Int);
                    cmd.Parameters.Add("@arg4", SqlDbType.VarChar);
                    cmd.Parameters.Add("@arg5", SqlDbType.Int);
                    cmd.Parameters.Add("@arg6", SqlDbType.Int);
                    cmd.Parameters.Add("@arg7", SqlDbType.VarChar);
                    cmd.Parameters.Add("@arg8", SqlDbType.VarChar);

                    foreach (IPTFRC s in lstIPTFRC)
                    {
                        cmd.Parameters[0].Value = Convert.ToInt32(s.job_no);
                        cmd.Parameters[1].Value = Convert.ToInt32(s.ln_no);
                        cmd.Parameters[2].Value = Convert.ToInt32(s.ctl_no);
                        cmd.Parameters[3].Value = s.tag;
                        cmd.Parameters[4].Value = Convert.ToInt32(s.nbr_stp);
                        cmd.Parameters[5].Value = Convert.ToInt32(s.arb_pos);
                        cmd.Parameters[6].Value = s.asgn_bal;
                        cmd.Parameters[7].Value = s.slit_typ;

                        cmd.ExecuteNonQuery();
                    }

                    trans.Commit();
                }
                catch (Exception)
                {
                    // Shouldn't lave a Transaction hanging, so rollback
                    trans.Rollback();
                    throw;
                }
                try
                {
                    // Get count of rows inserted into IMPORT
                    cmd.CommandText = "SELECT COUNT(frc_job_no) from ST_IMPORT_tbl_IPTFRC";
                    r = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                // No matter what close and dispose of the connetion
                conn.Close();
                conn.Dispose();
            }

            return r;
        }

        // Insert values from IMPORT into WIP IPTFRC
        public int Write_IMPORT_to_IPTFRC()
        {
            // Returning rows inserted into IMPORT
            int r = 0;

            SqlConnection conn = new SqlConnection(STRATIXDataConnString);

            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;

                // Call SP to copy IMPORT to IPTFRC table.  Return rows inserted.
                cmd.CommandText = "ST_proc_IMPORT_to_IPTFRC";
              
                AddParamToSQLCmd(cmd, "@rows", SqlDbType.Int, 8, ParameterDirection.Output);

                cmd.ExecuteNonQuery();

                r = Convert.ToInt32(cmd.Parameters["@rows"].Value);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                // No matter what close and dispose of the connetion
                conn.Close();
                conn.Dispose();
            }

            return r;
        } 
    }
}
