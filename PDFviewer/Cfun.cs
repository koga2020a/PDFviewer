using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;



namespace PDFviewer
{
    public class TSXBIN_SYM
    {
        public class IntStringKeys_pair
        {
            public IntStringKeys_pair(int in_val, string in_text = null, Keys in_Keys = Keys.None)
            {
                this.int_val = in_val;
                this.string_text = in_text;
                this.keys = in_Keys;
            }

            public int int_val = -1;
            public string string_text = "";
            public Keys keys = Keys.None;
        }
    }

    class Cfun
    {
        #region JOB登録のキーマクロ動作用

        static private List<char> JOB登録_キーマクロ動作_char_list = new List<char>();
        /// <summary>ユーザメニュー内のJOB登録の文字列を新規にセットします。</summary>
        static public void JOB登録_clear_キーマクロ部_buffer()
        {
            JOB登録_Log_char_list("JOB登録_clear_キーマクロ部_buffer");
            JOB登録_キーマクロ動作_char_list = new List<char>();
        }
        /// <summary>ユーザメニュー内のJOB登録の文字列を新規にセットします。
        /// ※最初の文字から半角SPまでは無視されます。</summary>
        static public void JOB登録_set_キーマクロ部(string in_text)
        {
            JOB登録_clear_キーマクロ部_buffer();

            string tmp_header = in_text.Split(' ')[0];
            string tmp_set_text = in_text.Substring(tmp_header.Length);
            JOB登録_set_キーマクロ部_proc_with半角SP詰める(tmp_set_text);
        }
        /// <summary>ユーザメニュー内のJOB登録の文字列を新規にセットします。</summary>
        static public void JOB登録_set_キーマクロ部_for_コマンドライン引数(string in_text)
        {
            JOB登録_clear_キーマクロ部_buffer();

            string tmp_set_text = in_text;
            JOB登録_set_キーマクロ部_proc_with半角SP詰める(tmp_set_text);
        }
        static private void JOB登録_set_キーマクロ部_proc_with半角SP詰める(string in_text)
        {
            if (string.IsNullOrEmpty(in_text))
                return;

            string tmp_set_text = in_text.Replace(" ", "");     // 半角SPを詰めます。   JOBのキーマクロ部は「NENT 2 TOGETU! % F12」もOKで「NENT 2 TOGETU!%F12」と同じ動作。そうなるようにした。 2016.10.16
            foreach (char tmp_char in tmp_set_text)
            {
                JOB登録_キーマクロ動作_char_list.Add(tmp_char);
            }
            JOB登録_Log_char_list("JOB登録_set_キーマクロ部_proc_with半角SP詰める[" + in_text + "]");
        }

        static public string get_All残りJOB登録_キーマクロ_withClear()
        {
            string ret_text = "";
            foreach (char tmp_c in JOB登録_キーマクロ動作_char_list)
                ret_text += tmp_c.ToString();

            JOB登録_clear_キーマクロ部_buffer();

            return ret_text;
        }
        static public bool is_JOB登録_キーマクロ_残りあり()
        {
            if (JOB登録_キーマクロ動作_char_list.Count == 0)
                return false;

            return true;
        }
        public static bool JOB登録_Log_char_list(string in_header)
        {
            string ret_text_byChar = "";
            foreach (char tmp_c in JOB登録_キーマクロ動作_char_list)
            {
                ret_text_byChar += tmp_c.ToString();
            }
            //Cfun.Log管理.set_遷移(Log管理class.遷移log_type.PROC詳細, " JOB登録_get_nextキーマクロ  " + in_header + " 内容:" + ret_text_byChar);
            return true;
        }
        static private void JOB登録_キーマクロ動作_char_REMOVE_OLD()
        {
            JOB登録_Log_char_list("RemoveAtの直前");
            JOB登録_キーマクロ動作_char_list.RemoveAt(0);
            JOB登録_Log_char_list("RemoveAtの直後");
        }
        /// <summary>ジョブのマクロ部を１つずつ返します。intが -1:マクロ（残り）無し   1:文字   2:Keys</summary>
        static public TSXBIN_SYM.IntStringKeys_pair JOB登録_get_nextキーマクロ()
        {
        START:

            if (JOB登録_キーマクロ動作_char_list != null)
                JOB登録_Log_char_list("JOB登録_get_nextキーマクロ 起動時");
            else
            {
                JOB登録_Log_char_list("JOB登録_get_nextキーマクロ 残りキー無し！");
            }

            if (JOB登録_キーマクロ動作_char_list == null || JOB登録_キーマクロ動作_char_list.Count == 0)
                return new TSXBIN_SYM.IntStringKeys_pair(-1, null, Keys.None);

            char tmp_char = JOB登録_キーマクロ動作_char_list[0];
            JOB登録_キーマクロ動作_char_REMOVE_OLD();
            switch (tmp_char)
            {
                case ' ':
                    goto START;
                case '%':
                    if (JOB登録_キーマクロ動作_char_list.Count == 0)     // % だけ、といった書き間違えのケースで例外が出ないように。
                        goto START;
                    char tmp_F_char = JOB登録_キーマクロ動作_char_list[0];
                    JOB登録_キーマクロ動作_char_REMOVE_OLD();
                    string tmp_F_char_text = Cfun.ToUpper半(tmp_F_char.ToString());
                    bool is_with_Shift = false;
                    if (tmp_F_char_text == "+")
                    {
                        tmp_F_char = JOB登録_キーマクロ動作_char_list[0];
                        JOB登録_キーマクロ動作_char_REMOVE_OLD();
                        tmp_F_char_text = Cfun.ToUpper半(tmp_F_char.ToString());
                        is_with_Shift = true;
                    }
                    Keys tmp_is_with_Shift = (is_with_Shift == true) ? Keys.Shift : Keys.None;
                    switch (tmp_F_char_text)
                    {
                        case "F":   // ファンクションキー
                            char tmp_No_char = JOB登録_キーマクロ動作_char_list[0];
                            JOB登録_キーマクロ動作_char_REMOVE_OLD();
                            switch (tmp_No_char)
                            {
                                case '1':
                                    return new TSXBIN_SYM.IntStringKeys_pair(2, null, tmp_is_with_Shift | Keys.F1);        // 1:文字     2:Keys
                                case '2':
                                    return new TSXBIN_SYM.IntStringKeys_pair(2, null, tmp_is_with_Shift | Keys.F2);
                                case '3':
                                    return new TSXBIN_SYM.IntStringKeys_pair(2, null, tmp_is_with_Shift | Keys.F3);
                                case '4':
                                    return new TSXBIN_SYM.IntStringKeys_pair(2, null, tmp_is_with_Shift | Keys.F4);
                                case '5':
                                    return new TSXBIN_SYM.IntStringKeys_pair(2, null, tmp_is_with_Shift | Keys.F5);
                                case '6':
                                    return new TSXBIN_SYM.IntStringKeys_pair(2, null, tmp_is_with_Shift | Keys.F6);
                                case '7':
                                    return new TSXBIN_SYM.IntStringKeys_pair(2, null, tmp_is_with_Shift | Keys.F7);
                                case '8':
                                    return new TSXBIN_SYM.IntStringKeys_pair(2, null, tmp_is_with_Shift | Keys.F8);
                                case '9':
                                    return new TSXBIN_SYM.IntStringKeys_pair(2, null, tmp_is_with_Shift | Keys.F9);
                                case '0':
                                default:
                                    goto START;
                            }
                        case "S":   // 半角スペースを送ります。
                            return new TSXBIN_SYM.IntStringKeys_pair(1, " ", Keys.None);        // 1:文字     2:Keys
                        case "E":   // ESCキー
                            return new TSXBIN_SYM.IntStringKeys_pair(2, null, tmp_is_with_Shift | Keys.Escape);
                        case "D":   // 下矢印キー
                            return new TSXBIN_SYM.IntStringKeys_pair(2, null, tmp_is_with_Shift | Keys.Down);
                        case "U":   // 上矢印キー
                            return new TSXBIN_SYM.IntStringKeys_pair(2, null, tmp_is_with_Shift | Keys.Up);
                        case "L":   // 左矢印キー
                            return new TSXBIN_SYM.IntStringKeys_pair(2, null, tmp_is_with_Shift | Keys.Left);
                        case "R":   // 右矢印キー
                            return new TSXBIN_SYM.IntStringKeys_pair(2, null, tmp_is_with_Shift | Keys.Right);

                        case "B":   // BackSpaceキー
                            return new TSXBIN_SYM.IntStringKeys_pair(2, null, tmp_is_with_Shift | Keys.Back);
                        case "N":   // Enterキー
                            return new TSXBIN_SYM.IntStringKeys_pair(2, null, tmp_is_with_Shift | Keys.Enter);
                        default:
                            goto START;
                    }
                case '!':
                    return new TSXBIN_SYM.IntStringKeys_pair(2, null, Keys.Enter);           // 1:文字     2:Keys
                default:
                    JOB登録_Log_char_list("switch時");
                    return new TSXBIN_SYM.IntStringKeys_pair(1, tmp_char.ToString());        // 1:文字     2:Keys
            }
        }

        #endregion


        static public string ToUpper半(string in_text)
        {
            //return in_text.ToUpper();

            // Windows7 32bit のOSの仕様で、エクスプローラー上でも全角の「ａ.txt」と「Ａ.txt」は同一と看做される。
            return Regex.Replace(in_text, "[a-z]+", m => m.Groups[0].Value.ToUpper());

            /*
            return string.Join("",
                value.Select(chr =>
                {
                    var str = chr.ToString();
                    return Encoding.UTF8.GetByteCount(str) == 1 ?
                       str : str.ToUpper();
                }));
            */
        }

        /// <summary>stringをintにして返します。</summary>
        static public int si(string in_text)
        {
            int tmp_val;
            int.TryParse(in_text, out tmp_val);
            return tmp_val;
        }
        /// <summary>stringをfloatにして返します。</summary>
        static public float si_f(string in_text)
        {
            float tmp_val;
            float.TryParse(in_text, out tmp_val);
            return tmp_val;
        }


        static public void GCたくさん()
        {
            /*
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            */
        }
        static public bool is_same_filepath(string in_A_filepath, string in_B_filepath)
        {
            return (in_A_filepath.ToUpper() == in_B_filepath.ToUpper());
        }

    }
}
