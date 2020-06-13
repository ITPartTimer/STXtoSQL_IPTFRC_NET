using System;
using System.Data.Odbc;
using System.Collections.Generic;
using System.Text;
using STXtoSQL.Models;

namespace STXtoSQL.DataAccess
{
    public class ODBCData : Helpers
    {
        public List<IPTFRC> Get_IPTFRC()
        {

            List<IPTFRC> lstIPTFRC = new List<IPTFRC>();

            OdbcConnection conn = new OdbcConnection(ODBCDataConnString);

            try
            {
                conn.Open();

                // Try to split with verbatim literal
                OdbcCommand cmd = conn.CreateCommand();

                cmd.CommandText = @"select frc_job_no,frc_cons_ln_no,frc_itm_ctl_no,frc_tag_no,frc_nbr_stp,frc_arb_pos,frc_asgn_bal,frc_slit_typ
                                    from iptfrc_rec
                                    where frc_coil_no = 1 
                                    and frc_job_no in
                                    (select psh_job_no
                                    from iptpsh_rec s
                                    inner join iptjob_rec j
                                    on j.job_job_no = s.psh_job_no
                                    where s.psh_whs = 'SW'
                                    and psh_sch_seq_no <> 99999999
                                    and (job_job_sts = 0 or job_job_sts = 1)
                                    and (job_prs_cl = 'SL' or job_prs_cl = 'CL' or job_prs_cl = 'MB'))";

                OdbcDataReader rdr = cmd.ExecuteReader();

                using (rdr)
                {
                    while (rdr.Read())
                    {
                        IPTFRC b = new IPTFRC();

                        b.job_no = Convert.ToInt32(rdr["frc_job_no"]);
                        b.ln_no = Convert.ToInt32(rdr["frc_cons_ln_no"]);
                        b.ctl_no = Convert.ToInt32(rdr["frc_itm_ctl_no"]);
                        b.tag = rdr["frc_tag_no"].ToString();                     
                        b.nbr_stp = Convert.ToInt32(rdr["frc_nbr_stp"]);
                        b.arb_pos = Convert.ToInt32(rdr["frc_arb_pos"]);
                        b.asgn_bal= rdr["frc_asgn_bal"].ToString();
                        b.slit_typ = rdr["frc_slit_typ"].ToString();

                        lstIPTFRC.Add(b);
                    }
                }
            }
            catch (OdbcException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return lstIPTFRC;
        }
    }
}
