using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;



namespace Ado.Net._6.HW2.SQLMetal
{
    class Program
    {
        private static string _connectionString = @"Data source = DESKTOP-PG10UGI\SQLEXPRESS; initial catalog = MCS; user id = sa; password = Mc123456;";
        private static Model1 db = new Model1(_connectionString);
        static void Main(string[] args)
        {
            Task1();
            //Task2();
            //Task3();
            //Task4();
            //Task6();
            //Task7();
        }

        public static void Task1()
        {
            var query = db.NewEquipment.Join(db.TableEquipmentHistory, i => i.IntEquipmentID, h => h.IntEquipmentID, (i, h) => 
            new {
                    i.IntGarageRoom,
                    i.StrSerialNo,
                    h.DStartDate,
                    h.DEndDate,
                    h.IntTypeHistory,
                    h.IntDaysCount,
                });

            foreach (var item in query)
            {
                Console.WriteLine(item.IntGarageRoom + " " + item.StrSerialNo + " " + item.IntTypeHistory + " " + item.DStartDate + " " + item.DEndDate + " " + item.IntDaysCount);
            }

        }

        public class NewTable
        {

            public string IntGarageRoom;

            public string StrSerialNo;

            public int? IntTypeHistory;

            public DateTime? DStartDate;

            public DateTime? DEndDate;

            public int? IntDaysCount;


        }
        public static void Task2()
        {
            var query = db.NewEquipment
                .Join(db.TableEquipmentHistory, i => i.IntEquipmentID, h => h.IntEquipmentID, (i, h) =>
            new {
                    i.IntGarageRoom,
                    i.StrSerialNo,
                    h.IntTypeHistory,
                    h.DStartDate,
                    h.DEndDate,
                    h.IntDaysCount,
                }).ToList();

            List<NewTable> newtables = new List<NewTable>();
            foreach (var item in query)
            {
                NewTable tab = new NewTable()
                {
                    IntGarageRoom = item.IntGarageRoom,
                    StrSerialNo = item.StrSerialNo,
                    IntTypeHistory = item.IntTypeHistory,
                    DStartDate = item.DStartDate,
                    DEndDate = item.DEndDate,
                    IntDaysCount = item.IntDaysCount,
                };
                newtables.Add(tab);
            }
        }

        public static void Task3()
        {
            
           db.Connection.Open();
            DbCommand cmd = db.Connection.CreateCommand();
            cmd.CommandText =
                "INSERT into TablesManufacturer (strManufacturerChecklistId, strName) VALUES(null,'Audi');" +
                "INSERT into TablesManufacturer (strManufacturerChecklistId, strName) VALUES(null,'BMW');" +
                "INSERT into TablesManufacturer (strManufacturerChecklistId, strName) VALUES(null,'KIA');" +
                "INSERT into TablesManufacturer (strManufacturerChecklistId, strName) VALUES(null,'JEEP')";
            cmd.ExecuteNonQuery();
            db.Connection.Close();
        }


        public static void Task4()
        {

            Table<NewEquipment> equips = db.GetTable<NewEquipment>();
            var query = from eq in equips select 
                new  {
                            equipmentID = eq.IntEquipmentID,
                            garageRoom = eq.IntGarageRoom,
                        };

            Console.WriteLine("EquipmentID \t GarageRoom");
            foreach (var item in query)
            {
                Console.WriteLine(item.equipmentID + "\t\t" + item.garageRoom );
            }

        }


        public static void Task6()
        {
          var array = db.TablesModel.Where(w => w.NewEquipment.Count()>10).Select(s => s.IntModelID).ToArray();
          var query = db.NewEquipment.Where(w => array.Contains(w.IntModelID)).OrderBy(o => o.IntModelID);
          foreach (var eq in query)
            {
                Console.WriteLine(eq.IntEquipmentID + "\t\t" + eq.IntGarageRoom+ "\t\t" + eq.IntManufacturerID);
            }
        }

        public static void Task7()
        {
           // (ret) ни какие цыклы.
            List<TableEquipmentHistory> query = db.TableEquipmentHistory.Where(t => t.DEndDate == null).ToList();
            db.TableEquipmentHistory.DeleteAllOnSubmit(query);
            db.SubmitChanges();
        }


    }
}
