using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace tetris
{
    public partial class MainWindow : Form
    {
        const bool DEBUG = false;
        const int TABLE_WIDTH = 15; // Число должно быть нечетным
        const int TABLE_HEIGHT = 10;

        Bitmap bitmap;
        Graphics g;

        Bitmap bitmap2;
        Graphics g2;

        byte[,] table = new byte[TABLE_WIDTH, TABLE_HEIGHT];
        byte[,] shape = new byte[2, 4];

        int min_x = 0, min_y = 0;
        bool can_flip = true;
        int shape_colorid = 0;
        bool fall_direction = false;

        Stopwatch stopwatch = new Stopwatch();
        Thread thread;

        int deleted_lines = 0;
        int fallen_shape = 0;
        
        int next_shape_id = -1;
        int next_color_id = 4;


        public MainWindow()
        {
            this.KeyPreview = true;
            InitializeComponent();
            
            UserDataManager.LoadUserData();

            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            bitmap2 = new Bitmap(pbNextShape.Width, pbNextShape.Height);
            g = Graphics.FromImage(bitmap);
            g2 = Graphics.FromImage(bitmap2);

            float scale_k = (float)pictureBox1.Width / 400;
            g.ScaleTransform(scale_k, scale_k);
            g2.ScaleTransform(scale_k, scale_k);

            this.FormClosing += (s, e) => { if (thread != null) thread.Abort(); };
            btnFillField.ButtonClick += (s, e) => FillAll();
            btnFillField.Visible = DEBUG;

            btnStartGame.ButtonClick += (s, e) =>
            {
                if (btnStartGame.Label_text == "Начать игру")
                {
                    StartGame();
                    btnStartGame.Label_text = "Прекратить игру";
                }
                else
                    AbortGame();
            };

            btnPause.ButtonClick += (s, e) =>
            {
                if (btnStartGame.Label_text == "Прекратить игру")
                    if (thread.IsAlive)
                    {
                        thread.Abort();
                        stopwatch.Stop();
                        btnPause.Label_text = "Продолжить";
                    }
                    else
                    {
                        StartGame();
                        btnPause.Label_text = "Пауза";
                    }
            };

            GenShape();
            FillField();
            RefreshStats();
        }

        void StartGame()
        {
            stopwatch.Start();
            
            thread = new Thread(() =>
            {
                while (true)
                {
                    if (stopwatch.ElapsedMilliseconds >= 700)
                    {
                        if (fall_direction)
                            LeftAngle();
                        else
                            RightAngle();

                        if (FindMistake())
                        {
                            if (fall_direction)
                                LeftAngleBack();
                            else
                                RightAngleBack();
                            Down();
                        }
                        FillField();
                        stopwatch.Restart();
                    }
                }
            });
            thread.Start();
        }

        void AbortGame()
        {
            btnStartGame.Invoke(new Action(() => btnStartGame.Label_text = "Начать игру"));
            btnPause.Invoke(new Action(() => btnPause.Label_text = "Пауза"));

            deleted_lines = 0;
            fallen_shape = 0;

            for (int i = 0; i < TABLE_HEIGHT; i++)
                for (int k = 0; k < TABLE_WIDTH; k++)
                    table[k, i] = 0;

            RefreshStats();
            GenShape();
            FillField();
            if (thread != null && thread.IsAlive) thread.Abort();
        }

        void RefreshStats()
        {
            Action action = new Action(() =>
                lblStats.Text =
                $"Линий заполнено:\n{deleted_lines}  ★{UserDataManager.recordDeletedLines}\n\n" +
                $"Фигур упало:\n{fallen_shape}  ★{UserDataManager.recordFallenShape}"
            );

            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(action));
            else action();
        }

        byte[,] GetShape(int x)
        {
            switch (x)
            {
                case 0: return new byte[,] { { 4, 5, 6, 7 }, { 0, 0, 1, 0 } }; // L
                case 1: return new byte[,] { { 4, 5, 6, 7 }, { 0, 0, 0, 0 } }; // S
                case 2: return new byte[,] { { 5, 6, 7, 6 }, { 0, 0, 0, 1 } }; // квадрат
                case 3: return new byte[,] { { 4, 5, 6, 6 }, { 0, 0, 0, 1 } }; // |-
                case 4: return new byte[,] { { 5, 7, 6, 7 }, { 0, 1, 1, 1 } }; // |   x4 y1
                case 5: return new byte[,] { { 6, 5, 6, 5 }, { 1, 1, 0, 0 } }; // S   x4 y1 (перев.)
                case 6: return new byte[,] { { 6, 7, 5, 6 }, { 0, 0, 1, 1 } }; // L   x4 y1 (перев.)
                default: return new byte[,] { { 7, 7, 5, 6 }, { 0, 0, 1, 1 } }; // |   x4 y1 (перев.)
            }
        }

        void GenShape()
        {
            Random x = new Random(DateTime.Now.Millisecond);
            int num = next_shape_id == -1 ? x.Next(7) : next_shape_id;

            shape = GetShape(num);

            min_x = 4;
            min_y = (num >= 4) ? 1 : 0;
            can_flip = (num != 2);
            shape_colorid = next_color_id;

            next_color_id = x.Next(1, 8);
            next_shape_id = x.Next(7);

            // ---  Следующая фигура ---

            g2.Clear(Color.White);

            byte[,] temp_shape = GetShape(next_shape_id);

            for (int i = 0; i < 4; i++)
                temp_shape[0, i] -= 4;

            for (int i = 0; i < 4; i++)
                FillPixel(temp_shape[0, i], temp_shape[1, i], Colors.GetColor(next_color_id), g2);

            pbNextShape.Image = bitmap2;
        }

        void FillAll()
        {
            for (int i = 0; i < TABLE_WIDTH; i++)
                for (int k = 0; k < TABLE_HEIGHT; k++)
                    FillPixel(i, k, (i % 2 == 0 ? Brushes.LightSeaGreen : Brushes.IndianRed), g);

            pictureBox1.Image = bitmap;
        }

        void FillField()
        {
            try
            {
                g.Clear(Color.White);

                for (int i = 0; i < TABLE_WIDTH; i++)
                    for (int k = 0; k < TABLE_HEIGHT; k++)

                        if (table[i, k] >= 1)
                            FillPixel(i, k, DEBUG ? (i % 2 == 0 ? Brushes.LightSeaGreen : Brushes.IndianRed) : Colors.GetColor(table[i, k]), g);

                for (int i = 0; i < 4; i++)
                    FillPixel(shape[0, i], shape[1, i], DEBUG ? (shape[0, i] % 2 == 0 ? Brushes.LightSeaGreen : Brushes.IndianRed) : Colors.GetColor(shape_colorid), g);

                // ------ Рамка -----

                for (int i = 0; i < TABLE_HEIGHT - 1; i++)  // Левая граница
                    g.FillPolygon(Colors.BORDER_COLOR, new Point[]
                    {
                        new Point(0, 25 + 50 * i),
                        new Point(25, 50 + 50 * i),
                        new Point(0, 75 + 50 * i)
                    });

                for (int i = 0; i < TABLE_HEIGHT - 1; i++) // Правая граница
                    g.FillPolygon(Colors.BORDER_COLOR, new Point[]
                    {
                        new Point((TABLE_WIDTH / 2)*50 + 50, 25 + 50 * i),
                        new Point((TABLE_WIDTH / 2)*50 + 25, 50 + 50 * i),
                        new Point((TABLE_WIDTH / 2)*50 + 50, 75 + 50 * i)
                    });

                for (int i = 0; i < 6; i++)
                    g.FillPolygon(Colors.BORDER_COLOR, new Point[]
                    {
                        new Point(75 + 50 * i, 50 * TABLE_HEIGHT),
                        new Point(100 + 50 * i, 50 * TABLE_HEIGHT + 25),
                        new Point(50 + 50 * i, 50 * TABLE_HEIGHT + 25)
                    });

                g.FillPolygon(Colors.BORDER_COLOR, new Point[]
                {
                    new Point(0, 50 * TABLE_HEIGHT - 25),
                    new Point(50, 50 * TABLE_HEIGHT + 25),
                    new Point(0, 50 * TABLE_HEIGHT + 25)
                });
                g.FillPolygon(Colors.BORDER_COLOR, new Point[]
                {
                    new Point(50 * (TABLE_WIDTH / 2) + 50, 50 * TABLE_HEIGHT - 25),
                    new Point(50 * (TABLE_WIDTH / 2), 50 * TABLE_HEIGHT + 25),
                    new Point(50 * (TABLE_WIDTH / 2) + 50, 50 * TABLE_HEIGHT + 25)
                });
                pictureBox1.Image = bitmap;
            }
            catch
            {

            }
        }

        void FillPixel(int x, int y, Brush color, Graphics g)
        {
            if (x % 2 == 0)
            {
                DrawRhomb(
                    50 * (x / 2),
                    50 * y,
                    50 * ((x / 2) + 1),
                    50 * (y + 1), color, g);
            }
            else
            {
                DrawRhomb(
                    50 * ((x + 1) / 2) - 25,
                    50 * y + 25,
                    50 * (((x + 1) / 2) + 1) - 25,
                    50 * (y + 1) + 25, color, g);
            }
        }

        void DrawRhomb(int a_x, int a_y, int b_x, int b_y, Brush b, Graphics g)
        {
            Color x = ((SolidBrush)b).Color;
            Brush y = new SolidBrush(Color.FromArgb(
                x.R + 50 > 255 ? 255 : x.R + 50,
                x.G + 50 > 255 ? 255 : x.G + 50,
                x.B + 50 > 255 ? 255 : x.B + 50));

            int f_x = (b_x - a_x) / 2 + a_x;
            int f_y = (b_y - a_y) / 2 + a_y;

            Point[] pts = {
                new Point(f_x, a_y),
                new Point(a_x, f_y),
                new Point(f_x, b_y),
                new Point(b_x, f_y)
            };
            Point[] pts2 = {
                new Point(f_x, a_y + 5),
                new Point(a_x + 5, f_y),
                new Point(f_x, b_y - 5),
                new Point(b_x - 5, f_y)
            };
            g.FillPolygon(y, pts);
            g.FillPolygon(b, pts2);
        }

        void FlipShape(bool flag)
        {
            if (min_x % 2 != 0)
            {
                LeftAngleBack();
                FlipShape(true);
                return;
            }

            for (int i = 0; i < 4; i++)
            {
                int x = shape[0, i] - min_x;
                int y = shape[1, i] - min_y;

                if (x == 0 && y == 0) { shape[0, i] += 2; shape[1, i]++; }
                if (x == 1 && y == 0) { shape[0, i] += 2; }
                if (x == 3 && y == 0) {                   shape[1, i]--; }
                if (x == 4 && y == 0) { shape[0, i] -= 2; shape[1, i]--; }

                if (x == 2 && y ==  1) { shape[0, i] += 2; shape[1, i]--; }
                if (x == 1 && y == -1) {                   shape[1, i]++; }
                if (x == 2 && y == -1) { shape[0, i] -= 2; shape[1, i]++; }
                if (x == 3 && y == -1) { shape[0, i] -= 2; }
            }
            if (flag) LeftAngle();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (!DEBUG && (thread == null || !thread.IsAlive)) return;

            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (!can_flip) return;

                    var shapeT = new byte[2, 4];
                    Array.Copy(shape, shapeT, shape.Length);

                    FlipShape(false);

                    if (FindMistake())
                        Array.Copy(shapeT, shape, shape.Length);
                    break;

                case Keys.Down: 
                    Down(); 
                    break;
                
                case Keys.Left:
                    fall_direction = true;

                    LeftAngle();
                    if (FindMistake())
                        LeftAngleBack();
                    else
                        stopwatch.Restart();
                    break;

                case Keys.Right:
                    fall_direction = false;

                    RightAngle();
                    if (FindMistake())
                        RightAngleBack();
                    else
                        stopwatch.Restart();
                    break;
            }
            FillField();
        }



        void LeftAngleBack()
        {
            for (int i = 0; i < 4; i++)
                if (shape[0, i] % 2 == 0)
                {
                    shape[0, i] += 1;
                    shape[1, i] -= 1;
                }
                else
                    shape[0, i] += 1;

            min_x++;
            if (min_x % 2 == 0) min_y--;
        }

        void LeftAngle()
        {
            for (int i = 0; i < 4; i++)
                if (shape[0, i] % 2 == 0)
                    shape[0, i] -= 1;
                else
                {
                    shape[0, i] -= 1;
                    shape[1, i] += 1;
                }

            min_x--;
            if (min_x % 2 != 0) min_y++;
        }

        void RightAngleBack()
        {
            for (int i = 0; i < 4; i++)
                if (shape[0, i] % 2 == 0)
                {
                    shape[0, i] -= 1;
                    shape[1, i] -= 1;
                }
                else
                    shape[0, i] -= 1;

            min_x--;
            if (min_x % 2 == 0) min_y--;
        }

        void RightAngle()
        {
            for (int i = 0; i < 4; i++)
                if (shape[0, i] % 2 == 0)
                    shape[0, i] += 1;
                else
                {
                    shape[0, i] += 1;
                    shape[1, i] += 1;
                }

            min_x++;
            if (min_x % 2 != 0) min_y++;
        }

        void Down()
        {
            for (int i = 0; i < 4; i++)
                shape[1, i]++;

            if (FindMistake())
            {
                for (int i = 0; i < 4; i++)
                    table[shape[0, i], --shape[1, i]] = (byte)shape_colorid;

                for (int i = TABLE_HEIGHT - 1; i >= 0; i--)
                {
                    int cross = 0;
                    for (int k = 0; k < TABLE_WIDTH; k++) if (table[k, i] >= 1) cross++;

                    if (cross >= (UserDataManager.hard_mode ? TABLE_WIDTH : 14 ) )
                    {
                        for (int k = i; k > 0; k--)
                            for (int j = 0; j < TABLE_WIDTH; j++)
                                table[j, k] = table[j, k - 1];

                        if (++deleted_lines > UserDataManager.recordDeletedLines)
                        {
                            UserDataManager.recordDeletedLines = deleted_lines;
                            UserDataManager.SaveUserData();
                        }
                    }
                }

                if (++fallen_shape > UserDataManager.recordFallenShape)
                {
                    UserDataManager.recordFallenShape = fallen_shape;
                    UserDataManager.SaveUserData();
                }
                GenShape();
                RefreshStats();

                if (FindMistake()) AbortGame();
            }
            else
            {
                min_y++;
                stopwatch.Restart();
            }
        }

        bool FindMistake()
        {
            for (int i = 0; i < 4; i++)
                if (shape[0, i] >= TABLE_WIDTH || shape[1, i] >= TABLE_HEIGHT || table[shape[0, i], shape[1, i]] >= 1)
                    return true;

            return false;
        }


    }
}
