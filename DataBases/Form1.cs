using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Linq;

namespace DataBases
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Kd_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (double.TryParse(Kd.Text, out double kdValue))
            {
                Kr.Text = (kdValue / 2).ToString();
            }
        }

        private void u_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            var uToT1Items = new Dictionary<string, string[]>
            {
                { "1", new[] { "47", "93", "135", "180" } },
                { "1,25", new[] { "61", "95", "122", "190" } },
                { "1,4", new[] { "50", "73", "101", "145" } },
                { "1,6", new[] { "41", "55", "113", "229" } },
                { "2", new[] { "49", "69", "99", "141" } },
                { "2,5", new[] { "71", "118", "157" } },
                { "3,15", new[] { "105", "152" } },
                { "4", new[] { "145", "207" } }
            };

            if (uToT1Items.TryGetValue(u.Text, out string[] items))
            {
                T1.Items.Clear();
                T1.Items.AddRange(items);
            }
        }

        private void Kbee_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(Kbee.Text, out double Kbe) && double.TryParse(u.Text, out double uu))
            {
                double Shirr = (Kbe * uu) / (2.0 - Kbe);

                if (Shirr < 0.3)
                    Shirr = 0.2;
                else if (Shirr < 0.5)
                    Shirr = 0.4;
                else if (Shirr < 0.7)
                    Shirr = 0.6;
                else if (Shirr < 0.9)
                    Shirr = 0.8;
                else
                    Shirr = 1;

                Shir.Text = Shirr.ToString();
            }
        }

        private void Zv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (double.TryParse(Shir.Text, out double Shirr))
            {
                Khb.Text = GetKhbValue(Op.Text, Td.Text, Zv.Text, Shirr);
            }
        }

        private string GetKhbValue(string opText, string tdText, string zvText, double shirr)
        {
            if (opText == "Шариковые")
            {
                if (tdText == "HB > 350")
                {
                    if (zvText == "Прямые и тангенциальные")
                    {
                        if (shirr == 0.2) return "1,16";
                        else if (shirr == 0.4) return "1,37";
                        else if (shirr == 0.6) return "1,58";
                        else if (shirr == 0.8) return "1,80";
                        else if (shirr == 1) return "0";
                    }
                    else if (zvText == "Круглые")
                    {
                        if (shirr == 0.2) return "1,08";
                        else if (shirr == 0.4) return "1,18";
                        else if (shirr == 0.6) return "1,29";
                        else if (shirr == 0.8) return "1,40";
                        else if (shirr == 1) return "0";
                    }
                }
                else if (tdText == "HB =< 350")
                {
                    if (zvText == "Прямые и тангенциальные")
                    {
                        if (shirr == 0.2) return "1,07";
                        else if (shirr == 0.4) return "1,14";
                        else if (shirr == 0.6) return "1,23";
                        else if (shirr == 0.8) return "1,34";
                        else if (shirr == 1) return "1,0";
                    }
                    else if (zvText == "Круглые")
                    {
                        return "1";
                    }
                }
            }
            else if (opText == "Роликовые")
            {
                if (tdText == "HB > 350")
                {
                    if (zvText == "Прямые и тангенциальные")
                    {
                        if (shirr == 0.2) return "1,08";
                        else if (shirr == 0.4) return "1,20";
                        else if (shirr == 0.6) return "1,32";
                        else if (shirr == 0.8) return "1,44";
                        else if (shirr == 1) return "1,55";
                    }
                    else if (zvText == "Круглые")
                    {
                        if (shirr == 0.2) return "1,04";
                        else if (shirr == 0.4) return "1,10";
                        else if (shirr == 0.6) return "1,15";
                        else if (shirr == 0.8) return "1,22";
                        else if (shirr == 1) return "1,28";
                    }
                }
                else if (tdText == "HB =< 350")
                {
                    if (zvText == "Прямые и тангенциальные")
                    {
                        if (shirr == 0.2) return "1,04";
                        else if (shirr == 0.4) return "1,08";
                        else if (shirr == 0.6) return "1,13";
                        else if (shirr == 0.8) return "1,18";
                        else if (shirr == 1) return "1,23";
                    }
                    else if (zvText == "Круглые")
                    {
                        return "1";
                    }
                }
            }
            return "0";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (double.TryParse(Kr.Text, out double Krr) &&
                double.TryParse(sigmaHP.Text, out double sigma) &&
                double.TryParse(u.Text, out double uu) &&
                double.TryParse(T1.Text, out double T11) &&
                double.TryParse(Khb.Text, out double Kh) &&
                double.TryParse(Kbee.Text, out double Kb))
            {
                double pow = Math.Pow(T11 * Kh / ((1.0 - Kb) * Kb * uu * sigma), 1.0 / 3.0);
                double sqr = Math.Sqrt(uu * uu + 1.0);

                double Ree = Krr * sqr * pow;

                Re.Text = Ree.ToString();

                double.TryParse(Kd.Text, out double kdValue);
                double.TryParse(Shir.Text, out double Shirr);

                AddData(Kh, Ree, kdValue, Krr, uu, T11, Kb,sigma,Op.Text,Td.Text,Zv.Text, Shirr);
            }
            else
            {
                MessageBox.Show("Ошибка ввода данных");
            }
        }
        public void AddData(double Kh, double Ree,
                    double kdValue, double Krr,
                    double uu, double T11, double Kb,
                    double sigma, string opText,
                    string tdText, string zvText, double shirr)
        {
            string connectionString = "Data Source=C:\\Users\\МАРИНА\\Documents\\.универ\\3 курс\\BD\\kursach\\DataBase-solution-master\\KURSOVAYA.db;Version=3;"; // Путь к фалу .db

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Вставка в таблицу "Узел"
                        string insertNode = "INSERT INTO Assemblies (Name, TransmissionCount) VALUES (@Name, @TransmissionCount); SELECT last_insert_rowid();";
                        long nodeId;
                        using (SQLiteCommand cmd = new SQLiteCommand(insertNode, conn))
                        {
                            cmd.Parameters.AddWithValue("@Name", GenerateRandomString(10));
                            cmd.Parameters.AddWithValue("@TransmissionCount", 1);
                            nodeId = (long)cmd.ExecuteScalar(); // Получаем сгенерированный Id
                        }

                        // Вставка в таблицу "Сборочная единица"
                        string insertAssembly = "INSERT INTO Unit (Name, LoadDistributionCoefficient, ConeDistance, NodeId) VALUES (@Name, @LoadDistributionCoefficient, @ConeDistance, @NodeId); SELECT last_insert_rowid();";
                        long assemblyId;
                        using (SQLiteCommand cmd = new SQLiteCommand(insertAssembly, conn))
                        {
                            cmd.Parameters.AddWithValue("@Name", GenerateRandomString(10));
                            cmd.Parameters.AddWithValue("@LoadDistributionCoefficient", Kh);
                            cmd.Parameters.AddWithValue("@ConeDistance",Ree);
                            cmd.Parameters.AddWithValue("@NodeId", nodeId);
                            assemblyId = (long)cmd.ExecuteScalar(); // Получаем сгенерированный Id
                        }

                        // Вставка в таблицу "Деталь"
                        string insertDetail = "INSERT INTO Detail (Name, AuxiliaryCoefficient1, AuxiliaryCoefficient2, TransmissionRatio, Torque, WidthCoefficient, ContactStress, RowDefinition, SupportType, Hardness, TeethType, EquivalentWidth, AssemblyId) VALUES (@Name, @AuxiliaryCoefficient1, @AuxiliaryCoefficient2, @TransmissionRatio, @Torque, @WidthCoefficient, @ContactStress, @RowDefinition, @SupportType, @Hardness, @TeethType, @EquivalentWidth, @AssemblyId)";
                        using (SQLiteCommand cmd = new SQLiteCommand(insertDetail, conn))
                        {
                            cmd.Parameters.AddWithValue("@Name", GenerateRandomString(10));
                            cmd.Parameters.AddWithValue("@AuxiliaryCoefficient1", kdValue);
                            cmd.Parameters.AddWithValue("@AuxiliaryCoefficient2", Krr);
                            cmd.Parameters.AddWithValue("@TransmissionRatio", uu);
                            cmd.Parameters.AddWithValue("@Torque", T11);
                            cmd.Parameters.AddWithValue("@WidthCoefficient", Kb);
                            cmd.Parameters.AddWithValue("@ContactStress", sigma);
                            cmd.Parameters.AddWithValue("@RowDefinition", uu.ToString());
                            cmd.Parameters.AddWithValue("@SupportType", opText);
                            cmd.Parameters.AddWithValue("@Hardness", tdText);
                            cmd.Parameters.AddWithValue("@TeethType", zvText);
                            cmd.Parameters.AddWithValue("@EquivalentWidth", shirr);
                            cmd.Parameters.AddWithValue("@AssemblyId", assemblyId);
                            cmd.ExecuteNonQuery();
                        }

                        // Завершаем транзакцию
                        transaction.Commit();
                        Console.WriteLine("Данные успешно добавлены в три таблицы.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Ошибка при добавлении данных: {ex.Message}");
                    }
                }

                conn.Close();
            }
        }

        private static readonly Random random = new Random();
        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
                                         .Select(s => s[random.Next(s.Length)])
                                         .ToArray());
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
