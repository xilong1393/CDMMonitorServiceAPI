using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MonitorAPI.Service
{
    public class BaseService
    {
        protected static DataTable CreateDataTable<T>(IEnumerable<T> enumerable)
        {
            System.Reflection.PropertyInfo[] pis = typeof(T).GetProperties();

            //create data table
            DataTable table = new DataTable();
            foreach (System.Reflection.PropertyInfo pi in pis)
            {
                if (pi.PropertyType == typeof(System.Nullable<Boolean>))
                {
                    table.Columns.Add(pi.Name, typeof(Boolean));
                }
                else if (pi.PropertyType == typeof(System.Nullable<Int16>))
                {
                    table.Columns.Add(pi.Name, typeof(Int16));
                }
                else if (pi.PropertyType == typeof(System.Nullable<Int32>))
                {
                    table.Columns.Add(pi.Name, typeof(Int32));
                }
                else if (pi.PropertyType == typeof(System.Nullable<Int64>))
                {
                    table.Columns.Add(pi.Name, typeof(Int64));
                }
                else if (pi.PropertyType == typeof(System.Nullable<Char>))
                {
                    table.Columns.Add(pi.Name, typeof(Char));
                }
                else if (pi.PropertyType == typeof(System.Nullable<DateTime>))
                {
                    table.Columns.Add(pi.Name, typeof(DateTime));
                }
                else
                {
                    table.Columns.Add(pi.Name, pi.PropertyType);
                }
            }

            //add rows to DataTable
            foreach (T elem in enumerable)
            {
                DataRow row = table.NewRow();

                foreach (System.Reflection.PropertyInfo pi in pis)
                {
                    if (pi.GetValue(elem, null) == null)
                    {
                        row[pi.Name] = DBNull.Value;
                    }
                    else
                    {
                        row[pi.Name] = pi.GetValue(elem, null);
                    }
                }
                table.Rows.Add(row);
            }
            return table;
        }
    }
}