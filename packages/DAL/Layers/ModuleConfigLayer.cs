using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks; 

namespace DAL.DataLayer
{
    public class SettingsLayer
    {
        public Task<AllUserModules> GetModuleList(string userName, SqlConnection cn)
        {
            {

                var allUserModules = new AllUserModules()
                {
                    userModuleAvailable = new List<UserModule>(),
                    userModuleConfigurable = new List<UserModule>()
                };
                SqlCommand modulesqlComm = new SqlCommand();

                modulesqlComm.Connection = cn;
                modulesqlComm.CommandText = "getMyModules ";

                modulesqlComm.CommandType = CommandType.StoredProcedure;
                modulesqlComm.Parameters.AddWithValue("@username", userName);
                SqlDataAdapter getMyModulesAdapter = new System.Data.SqlClient.SqlDataAdapter(modulesqlComm);
                DataSet module_ds = new DataSet();
                try
                {
                    getMyModulesAdapter.Fill(module_ds);
                }
                catch (Exception ex) { }
                DataTable moduleEditable = module_ds.Tables[0];

                if (moduleEditable.Rows.Count > 0)
                {
                    List<UserModule> ml = new List<UserModule>();
                    foreach (DataRow dr in moduleEditable.Rows)
                    {
                        UserModule um = new UserModule();
                        if (dr["plus_minus"] == "minus")
                        {
                            um.module_active = true;
                        }
                        else
                        {
                            um.module_active = false;
                        }
                        try
                        {
                            int module_order = 0;
                            um.module_function = dr["moduleFunction"].ToString();
                            int.TryParse((dr["controlorder"].ToString()), out module_order);
                            um.module_order = module_order;
                            um.module_title = dr["modulename"].ToString();
                            um.module_width = dr["moduleWidth"].ToString();
                            ml.Add(um);

                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    allUserModules.userModuleConfigurable = ml;
                }
                DataTable moduleRequired = module_ds.Tables[1];

                if (moduleRequired.Rows.Count > 0)
                {
                    List<UserModule> ml = new List<UserModule>();
                    foreach (DataRow dr in moduleRequired.Rows)
                    {
                        UserModule um = new UserModule();
                        if (dr["plus_minus"] == "minus")
                        {
                            um.module_active = true;
                        }
                        else
                        {
                            um.module_active = false;
                        }
                        try
                        {
                            int module_order = 0;
                            um.module_function = dr["moduleFunction"].ToString();
                            int.TryParse((dr["controlorder"].ToString()), out module_order);
                            um.module_order = module_order;
                            um.module_title = dr["modulename"].ToString();
                            um.module_width = dr["moduleWidth"].ToString();
                            ml.Add(um);

                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    allUserModules.userModuleAvailable = ml;
                }
                return Task.Run(() =>
                {
                    return allUserModules;
                });
            }


        }

        public Task<List<AvailableTableColumns>> GetUserCollums(string userName, SqlConnection sqlCon)
        {
            var columns = new List<AvailableTableColumns>();
            {
                SqlCommand sqlComm = new SqlCommand("getAvilableCollumnList");
                sqlComm.Connection = sqlCon;
                sqlComm.CommandType = CommandType.StoredProcedure;

                sqlComm.Parameters.AddWithValue("@userName", userName);
                try
                {
                    SqlDataReader reader = sqlComm.ExecuteReader();

                    AvailableTableColumns record = new AvailableTableColumns();
                    while (reader.Read())
                    {
                        record = new AvailableTableColumns()
                        {
                            id = reader.GetValue(reader.GetOrdinal("id")).ToString(),
                            value = reader.GetValue(reader.GetOrdinal("column_name")).ToString(),
                            @checked = bool.Parse(reader.GetValue(reader.GetOrdinal("default")).ToString()),
                            searchable = false,
                            sortableValue = false,

                        };
                        columns.Add(record);
                    }
                    reader.Close();


                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return Task.Run(() => { return columns; });
            }

        }
    }


    public class AllUserModules
    {
        public List<UserModule> userModuleAvailable { get; set; }
        public List<UserModule> userModuleConfigurable { get; set; }
    }


}