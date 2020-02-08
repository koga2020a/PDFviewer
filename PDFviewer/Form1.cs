using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Reflection;




namespace PDFviewer
{
    public partial class Form1 : Form
    {
        #region Formオブジェクトのリサイクル用の処理です。ShowDialogではFormメモリが解放されない仕様なので、再利用します。
        //*************************************************************************************************************
        // Formオブジェクトのリサイクル用の処理です。ShowDialogではFormメモリが解放されない仕様なので、再利用します。
        //*************************************************************************************************************
        static private List<Form1> _Form_obj_list = new List<Form1>();
        static public Form1 get_リサイクル_Form_obj()
        {
            Func<Form1> get_空きForm_from_list = () =>
            {
                foreach (Form1 tmp_form in _Form_obj_list)
                {
                    if (tmp_form.is_使用中_forForms == false)
                        return tmp_form;
                }
                return null;
            };

            Form1 ret_form = get_空きForm_from_list();
            if (ret_form == null)
            {
                ret_form = new Form1();
                _Form_obj_list.Add(ret_form);
            }

            return ret_form;
        }
        #endregion



        private bool is_使用中_forForms = false;
        private bool is_closing = false;
        public string _return_mode = "";

        private FilePos管理class FilePos管理_obj = new FilePos管理class();

        private void プロパティ初期化()
        {
            is_使用中_forForms = true;

            is_closing = false;
            _return_mode = "";
        }

        public Form1()
        {
            InitializeComponent();

            this.Location = new Point(0, 0);
            this.panel1.Location = new Point(0, 0);

            this.Size = new Size(1280, 960);
            this.panel1.Size = new Size(1280, 960);
            this.panel1.MouseMove += Panel1_MouseMove;
            this.pictureBox1.Paint += PictureBox1_Paint1;
            this.pictureBox1.MouseClick += PictureBox1_MouseClick;
            this.pictureBox1.MouseMove += PictureBox1_MouseMove;
            this.pictureBox1.Location = new Point(0, 0);
            this.pictureBox1.Size = new Size(panel1.Width, panel1.Size.Height);


            this.StartPosition = FormStartPosition.Manual;

            this.Shown += Form1_Shown;
            this.FormClosed += Form1_FormClosed;

            プロパティ初期化();

            PictureBox1_ZOOM_obj = new PictureBox_ZOOM_class(pictureBox1);
        }

        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            set_BOX_location(e.X, e.Y);
        }
        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            set_BOX_location(e.X, e.Y);
        }

        private void PictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            PictureBox1_ZOOM_obj.change倍率();

            pictureBox1.Refresh();

            string ZOOMtext = string.Format("ZOOMtype{0}   ←の上で、倍率{1}", PictureBox1_ZOOM_obj.ZOOMtype.ToString(), PictureBox1_ZOOM_obj.倍率);
            label2_debug.Text = ZOOMtext;
        }

        public void set_filepath(string in_filepath)
        {
            FilePos管理_obj.set_filepath(in_filepath);

            review();
        }
        public void set_folder_path(string in_path)
        {
            FilePos管理_obj.set_folder_path(in_path);

            review();
        }
        void review()
        {
            label1_Message.Text = FilePos管理_obj.filepath2;

            label2_debug.Text = "";
            label2_debug.Text = "≪ファイルinfo≫\n";
            label2_debug.Text += "status_type: " + FilePos管理_obj.get_status_type() + "\n";
            label2_debug.Text += "status_POS_type_at_File: " + FilePos管理_obj.get_status_POS_type_at_File() + "\n";
            label2_debug.Text += "\n";
            label2_debug.Text += "≪ページinfo≫\n";
            label2_debug.Text += "status_POS_type_at_Page: " + FilePos管理_obj.get_status_POS_type_at_Page() + "\n";
            label2_debug.Text += string.Format("ページ   {0} / {1}\n", FilePos管理_obj.get_now_ページ番号(), FilePos管理_obj.get_now_全ページ数());
            //label2_debug.Text += "ページ番号: " + FilePos管理_obj.get_now_ページ番号().ToString() + "\n";
            //label2_debug.Text += "全ページ数: " + FilePos管理_obj.get_now_全ページ数().ToString() + "\n";


            if(File.Exists(FilePos管理_obj.filepath2)==false)
            {
                return;
            }

            set_image_by_filepath(pictureBox1, FilePos管理_obj.filepath2, FilePos管理_obj.get_now_ページ番号());
        }
        void set_image_by_filepath(PictureBox in_picBox, string in_filepath, int in_ページ番号)
        {
            string tmp_EXT = Path.GetExtension(in_filepath);
            string TEMP_PNG_filepath = Path.GetTempFileName();
            switch (tmp_EXT.ToUpper().TrimStart('.'))
            {
                case "PDF":
                    convert_PDF2PNG(TEMP_PNG_filepath, in_filepath, in_ページ番号);
                    PictureBox1_ZOOM_obj.set_filepath(TEMP_PNG_filepath);

                    break;
            }
        }
        private class PictureBox_ZOOM_class
        {
            /// <summary>ありそうでなかった、ほしい倍率方法を用意しました</summary>
            public class ZOOM_type_class
            {
                public enum ZOOMtype_enum
                {
                    全体表示,
                    幅ピッタリ基準,
                    縦ピッタリ基準,
                    幅or縦短い方にピッタリ基準,
                    指定倍率,
                }
                public ZOOMtype_enum _ZOOMtype;
                public ZOOMtype_enum ZOOMtype { get { return _ZOOMtype; } }
                public double _倍率;     // 全体表示、幅ピッタリ基準 の後に修正で、この倍率を適用します。
                public double 倍率 { get { return _倍率; } }
                public ZOOM_type_class(ZOOMtype_enum in_ZOOMtype, double in_倍率)
                {
                    _ZOOMtype = in_ZOOMtype;
                    _倍率 = in_倍率;
                }
            }
            private string 画像filepath;
            private Bitmap currentImage;
            private int 元画像_width;
            private int 元画像_height;
            private int 元picBox_width;
            private int 元picBox_height;
            PictureBox picBox;
            ZOOM_type_class now_ZOOM_type { get { return 倍率list[倍率list_no]; } }
            List<ZOOM_type_class> 倍率list = new List<ZOOM_type_class>() 
            {
                new ZOOM_type_class(ZOOM_type_class.ZOOMtype_enum.幅or縦短い方にピッタリ基準,1.1d),
                new ZOOM_type_class(ZOOM_type_class.ZOOMtype_enum.指定倍率,1.2d),
                new ZOOM_type_class(ZOOM_type_class.ZOOMtype_enum.全体表示,1.2d),
            };
            int 倍率list_no = 0;
            public ZOOM_type_class.ZOOMtype_enum ZOOMtype
            {
                get { return now_ZOOM_type._ZOOMtype; }
            }
            public double 倍率
            {
                get { return now_ZOOM_type._倍率; }
            }
            public void change倍率()
            {
                倍率list_no++;
                if (倍率list_no == 倍率list.Count)
                    倍率list_no = 0;
            }

            public PictureBox_ZOOM_class(PictureBox in_picBox)
            {
                picBox = in_picBox;
                元picBox_width = picBox.Width;
                元picBox_height = picBox.Height;
            }
            public void set_filepath(string in_filepath)
            {
                画像filepath = in_filepath;
                review();
            }
            public void review()
            {
                //表示する画像を読み込む
                if (currentImage != null)
                {
                    currentImage.Dispose();
                }
                if (File.Exists(画像filepath) == false)
                    return;

                try
                {
                    currentImage = new Bitmap(画像filepath);
                    元画像_width = currentImage.Width;
                    元画像_height = currentImage.Height;
                }
                catch (Exception ex)
                {
                }

                picBox.Invalidate();
            }
            public void review_at_event(object sender, PaintEventArgs e)
            {
                if (currentImage == null)
                    return;

                Rectangle tmp_drawRectangle = new Rectangle();
                PictureBox pb = (PictureBox)sender;

                Func<bool> 幅ピッタリ基準_proc = () =>
                {
                    double tmp_横倍_for収める = (double)元picBox_width / (double)currentImage.Width;
                    //double tmp_縦倍_for収める = (double)元picBox_height / (double)currentImage.Height;
                    double tmp_倍率_for全体収める = tmp_横倍_for収める;
                    double tmp_修正後倍率 = tmp_倍率_for全体収める * now_ZOOM_type.倍率;
                    tmp_drawRectangle.Width = (int)Math.Round((double)currentImage.Width * tmp_修正後倍率);
                    tmp_drawRectangle.Height = (int)Math.Round((double)currentImage.Height * tmp_修正後倍率);
                    tmp_drawRectangle.X = 0;
                    tmp_drawRectangle.Y = 0;
                    picBox.Size = new Size(tmp_drawRectangle.Width, tmp_drawRectangle.Height);
                    return true;
                };
                Func<bool> 縦ピッタリ基準_proc = () =>
                {
                    //double tmp_横倍_for収める = (double)元picBox_width / (double)currentImage.Width;
                    double tmp_縦倍_for収める = (double)元picBox_height / (double)currentImage.Height;
                    double tmp_倍率_for全体収める = tmp_縦倍_for収める;
                    double tmp_修正後倍率 = tmp_倍率_for全体収める * now_ZOOM_type.倍率;
                    tmp_drawRectangle.Width = (int)Math.Round((double)currentImage.Width * tmp_修正後倍率);
                    tmp_drawRectangle.Height = (int)Math.Round((double)currentImage.Height * tmp_修正後倍率);
                    tmp_drawRectangle.X = 0;
                    tmp_drawRectangle.Y = 0;
                    picBox.Size = new Size(tmp_drawRectangle.Width, tmp_drawRectangle.Height);
                    return true;
                };
                switch (now_ZOOM_type.ZOOMtype)
                {
                    case ZOOM_type_class.ZOOMtype_enum.全体表示:
                        {
                            double tmp_横倍_for収める = (double)元picBox_width / (double)currentImage.Width;
                            double tmp_縦倍_for収める = (double)元picBox_height / (double)currentImage.Height;
                            double tmp_倍率_for全体収める = (tmp_横倍_for収める < tmp_縦倍_for収める) ? tmp_横倍_for収める : tmp_縦倍_for収める;
                            double tmp_修正後倍率 = tmp_倍率_for全体収める * now_ZOOM_type.倍率;
                            tmp_drawRectangle.Width = (int)Math.Round((double)currentImage.Width * tmp_修正後倍率);
                            tmp_drawRectangle.Height = (int)Math.Round((double)currentImage.Height * tmp_修正後倍率);
                            tmp_drawRectangle.X = 0;
                            tmp_drawRectangle.Y = 0;
                            picBox.Size = new Size(tmp_drawRectangle.Width, tmp_drawRectangle.Height);
                            break;
                        }
                    case ZOOM_type_class.ZOOMtype_enum.幅or縦短い方にピッタリ基準:
                        {
                            if (currentImage.Width > currentImage.Height)
                            {
                                縦ピッタリ基準_proc();
                                break;
                            }

                            幅ピッタリ基準_proc();
                            break;
                        }
                    case ZOOM_type_class.ZOOMtype_enum.幅ピッタリ基準:
                        {
                            幅ピッタリ基準_proc();
                            break;
                        }
                    case ZOOM_type_class.ZOOMtype_enum.縦ピッタリ基準:
                        {
                            縦ピッタリ基準_proc();
                            break;
                        }
                    case ZOOM_type_class.ZOOMtype_enum.指定倍率:
                        {
                            double tmp_修正後倍率 = now_ZOOM_type.倍率;
                            tmp_drawRectangle.Width = (int)Math.Round((double)currentImage.Width * tmp_修正後倍率);
                            tmp_drawRectangle.Height = (int)Math.Round((double)currentImage.Height * tmp_修正後倍率);
                            tmp_drawRectangle.X = 0;
                            tmp_drawRectangle.Y = 0;
                            picBox.Size = new Size(tmp_drawRectangle.Width, tmp_drawRectangle.Height);
                            break;
                        }
                }

                e.Graphics.DrawImage(currentImage, tmp_drawRectangle);  //画像を指定された位置、サイズで描画する
            }
        }

        PictureBox_ZOOM_class PictureBox1_ZOOM_obj;

        private void set_BOX_location(int in_mouse_X, int in_mouse_Y)
        {
            //
            // ZOOMモードのケースで、マウスカーソル位置に応じてPictureBoxの表示位置を変更します。
            //
            Panel panel_Message = this.panel1;

            double tmp_MARGIN_X1 = 0.6d;
            double tmp_MARGIN_Y1 = 0.6d;

            double tmp_MAR_X2 = 1d + tmp_MARGIN_X1;
            double tmp_MAR_X3 = 1d - tmp_MARGIN_X1;
            int width_10 = (int)((double)panel_Message.Width * tmp_MARGIN_X1);

            double tmp_MAR_Y2 = 1d + tmp_MARGIN_Y1;
            double tmp_MAR_Y3 = 1d - tmp_MARGIN_Y1;
            int height_10 = (int)((double)panel_Message.Height * tmp_MARGIN_Y1);

            int m_x = pictureBox1.Location.X * (-1) - in_mouse_X;
            int m_y = pictureBox1.Location.Y * (-1) - in_mouse_Y;
            m_x = (m_x < 0) ? m_x * (-1) : m_x;
            m_y = (m_y < 0) ? m_y * (-1) : m_y;

            m_x = (int)((double)m_x * tmp_MAR_X2);
            m_x = m_x - width_10 / 2;
            m_x = (m_x < 0) ? 0 : m_x;
            m_x = (m_x > panel_Message.Width) ? panel_Message.Width : m_x;

            m_y = (int)((double)m_y * tmp_MAR_Y2);
            m_y = m_y - height_10 / 2;
            m_y = (m_y < 0) ? 0 : m_y;
            m_y = (m_y > panel_Message.Height) ? panel_Message.Height : m_y;



            double tmp_割合_X = (double)m_x / (double)panel_Message.Width;
            double tmp_割合_Y = (double)m_y / (double)panel_Message.Height;
            int target_x = (int)(tmp_割合_X * (double)(pictureBox1.Width - panel_Message.Width));
            int target_y = (int)(tmp_割合_Y * (double)(pictureBox1.Height - panel_Message.Height));
            int tmp_x = target_x;
            int tmp_y = target_y;
            pictureBox1.Location = new Point(-tmp_x, -tmp_y);

            
            //
            // 拡大箇所を示す枠の位置
            //
            int 枠width = (int)((double)panel_Message.Width / PictureBox1_ZOOM_obj.倍率);
            int 枠height = (int)((double)panel_Message.Height / PictureBox1_ZOOM_obj.倍率);
            int 枠target_x = (int)(tmp_割合_X * (double)(panel_Message.Width - 枠width));
            int 枠target_y = (int)(tmp_割合_Y * (double)(panel_Message.Height - 枠height));
            box枠_locate(枠target_x, 枠target_y, 枠target_x + 枠width, 枠target_y + 枠height);

            box枠_表示();
        }


        //PictureBox1のPaintイベントハンドラ
        private void PictureBox1_Paint1(object sender, PaintEventArgs e)
        {
            PictureBox1_ZOOM_obj.review_at_event(sender, e);
        }
        /// <summary>現状、変換の成功・失敗そのものは取得していません。</summary>
        bool convert_PDF2PNG(string in_OUT_filepath, string in_IN_filepath,int in_page番号)
        {
            // ●プロセス起動情報の構築
            ProcessStartInfo process_Info = new ProcessStartInfo();
            string tmp_実行_path = Path.GetDirectoryName(in_IN_filepath);     //Assembly.GetEntryAssembly().Location;  // この実行EXEの実行パス
            string TEMP_ヘッダName = "TMP_PNGheader_" + Path.GetFileName(Path.GetTempFileName());
            string tmp_変換後のFilePath = string.Format("{0}\\{1}-{2:D6}.png", Path.GetDirectoryName(in_IN_filepath), TEMP_ヘッダName, in_page番号);

            process_Info.WorkingDirectory = tmp_実行_path;
            process_Info.FileName = tmp_実行_path + @"\pdftopng.exe";
            process_Info.Arguments = string.Format("-r 200 -f {0} -l {0} \"{1}\" \"{2}\"", in_page番号, in_IN_filepath, TEMP_ヘッダName);

            process_Info.CreateNoWindow = true; // コンソール・ウィンドウを開かない
            process_Info.UseShellExecute = false; // シェル機能を使用しない

            process_Info.RedirectStandardOutput = false;                  // 標準出力をリダイレクトしない

            try
            {
                using (Process proc = Process.Start(process_Info))            // 別プロセスとして起動
                {
                    proc.WaitForExit();                      // 処理が終了するまで待ちます。

                    if (File.Exists(in_OUT_filepath))
                        File.Delete(in_OUT_filepath);
                    File.Copy(tmp_変換後のFilePath, in_OUT_filepath);
                    File.Delete(tmp_変換後のFilePath);

                    int ret_code = proc.ExitCode;            // Exeの終了コードを取得します。
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void box枠_非表示()
        {
            line_1.Visible = false;
            line_2.Visible = false;
            line_3.Visible = false;
            line_4.Visible = false;
        }
        private void box枠_表示()
        {
            if (PictureBox1_ZOOM_obj.倍率 == 1d)
            {
                box枠_非表示();
                return;
            }

            line_1.Visible = true;
            line_2.Visible = true;
            line_3.Visible = true;
            line_4.Visible = true;
        }
        private void box枠_locate(int in_X1, int in_Y1, int in_X2, int in_Y2)
        {
            int tmp_width = in_X2 - in_X1;
            int tmp_height = in_Y2 - in_Y1;

            line_1.Location = new Point(in_X1, in_Y1);
            line_1.Size = new Size(tmp_width, 1);
            line_2.Location = new Point(in_X2, in_Y1);
            line_2.Size = new Size(1, tmp_height);
            line_3.Location = new Point(in_X1, in_Y2);
            line_3.Size = new Size(tmp_width, 1);
            line_4.Location = new Point(in_X1, in_Y1);
            line_4.Size = new Size(1, tmp_height);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            while (true)        // JOB登録のキーマクロの処理
            {
                TSXBIN_SYM.IntStringKeys_pair tmp_int_string_keys_pair = Cfun.JOB登録_get_nextキーマクロ();
                if (tmp_int_string_keys_pair.int_val == -1)     // キーマクロの残りが
                    break;

                if (tmp_int_string_keys_pair.int_val == 1)      // 文字はファイル名欄へ
                {
                    continue;
                }

                if (tmp_int_string_keys_pair.int_val == 2)
                {
                    keyData_proc(tmp_int_string_keys_pair.keys);
                    goto NEXT;
                }

            NEXT:
                if (is_closing)
                    break;  // 他のマクロは残したまま、終了する。
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            is_使用中_forForms = false;

            Cfun.GCたくさん();
        }

        /// <summary>Keyイベント（Key押下）</summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Alt | Keys.F4))  // Alt+F4で閉じないようにする。
                return true;

            bool ret_bool = keyData_proc(keyData);

            return ret_bool;
        }

        private bool keyData_proc(Keys in_keyData)
        {

            switch (in_keyData)
            {
                case Keys.Escape:
                    this.Close();
                    return true;

                //
                // 表示方向（回転）関係
                //
                case Keys.R:
                    //keys_R_proc();
                    return true;

                //
                // ページ・ファイル移動関係
                //
                case Keys.Control | Keys.Home:
                    keys_CTRL_help_proc();
                    return true;
                case Keys.Control | Keys.End:
                    keys_CTRL_end_proc();
                    return true;
                case Keys.Home:
                    keys_help_proc();
                    return true;
                case Keys.Help:
                case Keys.End:
                    keys_end_proc();
                    return true;
                case Keys.Left:
                    keys_left_proc();
                    return true;
                case Keys.Right:
                    keys_right_proc();
                    return true;
            }

            return false;
        }

        void keys_CTRL_help_proc()
        {
            bool tmp_移動あり = FilePos管理_obj.move_FilePOS_先頭ファイル先頭ページ();

            if (tmp_移動あり)
                review();
        }
        void keys_CTRL_end_proc()
        {
            bool tmp_移動あり = FilePos管理_obj.move_FilePOS_最終ファイル末尾ページ();

            if (tmp_移動あり)
                review();
        }
        void keys_help_proc()
        {
            bool tmp_移動あり = FilePos管理_obj.move_pagePOS_最初ページ();

            if (tmp_移動あり)
                review();
        }
        void keys_end_proc()
        {
            bool tmp_移動あり = FilePos管理_obj.move_pagePOS_最終ページ();

            if (tmp_移動あり)
                review();
        }
        void keys_left_proc()
        {
            bool tmp_移動あり = FilePos管理_obj.move_pagePOS(-1);

            if(tmp_移動あり)
                review();
        }
        void keys_right_proc()
        {
            bool tmp_移動あり = FilePos管理_obj.move_pagePOS(1);

            if (tmp_移動あり)
                review();
        }
    }
}
