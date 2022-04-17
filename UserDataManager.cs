using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace tetris
{
    class UserDataManager
    {
        public static int recordDeletedLines = 0;
        public static int recordScore = 0;
        public static int recordFallenShape = 0;
        public static bool hard_mode = false;

        private static string writePath = "data.txt";


        public static void LoadUserData()
        {
            if (File.Exists(writePath))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(writePath, Encoding.Default))
                    {
                        int i = 0;
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            switch (i)
                            {
                                case 0:
                                    {
                                        recordDeletedLines = Convert.ToInt32(line);
                                        break;
                                    }
                                case 1:
                                    {
                                        recordFallenShape = Convert.ToInt32(line);
                                        break;
                                    }
                                case 2:
                                    {
                                        hard_mode = line.ToLower() == "true";
                                        break;
                                    }
                            }
                            i++;
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            else
            {
                SaveUserData();
            }
        }

        public static void SaveUserData()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(writePath, false, Encoding.Default))
                {
                    sw.WriteLine(recordDeletedLines);
                    sw.WriteLine(recordFallenShape);
                    sw.WriteLine(hard_mode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
