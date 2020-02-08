using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using iTextSharp.text.pdf;




namespace PDFviewer
{
    public class FilePos管理class
    {
        private class PosInFile
        {
            public bool is_File設定中 = false;
            public bool is_FileExist
            {
                get
                {
                    if (is_File設定中 == false)
                        return false;
                    if (File.Exists(FilePath) == false)
                        return false;
                    return true;
                }
            }
            public string FilePath = "";
            public string DIR_name = "";
            private int _pos_no_at_fileNameList = -1;
            public int pos_no_at_fileNameList
            {
                get { return _pos_no_at_fileNameList; }
            }


            int _全ページ数 = 1;
            public int 全ページ数 { get { return _全ページ数; } }
            int _ページ番号 = 1;
            public int ページ番号 { get { return _ページ番号; } }

            public bool move_pagePOS_最初ページ()
            {
                if (_ページ番号 == 1)
                    return false;

                _ページ番号 = 1;
                return true;
            }
            public bool move_pagePOS_最終ページ()
            {
                if (_ページ番号 == _全ページ数)
                    return false;

                _ページ番号 = _全ページ数;
                return true;
            }
            public bool move_pagePOS_指定ページ(int in_指定ページ番号)
            {
                if (_ページ番号 == in_指定ページ番号 ||
                    in_指定ページ番号 < 1 ||
                    in_指定ページ番号 > _全ページ数)
                    return false;

                _ページ番号 = in_指定ページ番号;
                return true;
            }
            public bool move_at_page数(int in_ページ数)
            {
                bool is_他ファイル移動 = false;
                int back_ページ番号 = _ページ番号;

                _ページ番号 += in_ページ数;
                if (_ページ番号 > _全ページ数)
                {
                    is_他ファイル移動 = true;
                    _ページ番号 = 1;
                }
                if (_ページ番号 <1)
                {
                    is_他ファイル移動 = true;
                    _ページ番号 = 1;
                }

                if(is_他ファイル移動==false)
                {
                    return (back_ページ番号 != _ページ番号);
                }



                int back_no = _pos_no_at_fileNameList;
                return move_at_file数((in_ページ数 > 0) ? 1 : -1);
            }
            /// <summary>引数1が負のケースでは、最終ページ。正のケースでは1ページめとなります。</summary>
            public bool move_at_file数(int in_ファイル数)
            {
                int back_pos_no = _pos_no_at_fileNameList;

                _pos_no_at_fileNameList += in_ファイル数;
                if (_pos_no_at_fileNameList < 0)
                    _pos_no_at_fileNameList = 0;
                if (_pos_no_at_fileNameList > filePath_atDIR_list.Count - 1)
                    _pos_no_at_fileNameList = filePath_atDIR_list.Count - 1;

                if (back_pos_no == _pos_no_at_fileNameList)
                    return false;

                //
                // ファイル移動したケース
                //
                FilePath = filePath_atDIR_list[_pos_no_at_fileNameList].filepath;
                _全ページ数 = get_全ページ数(FilePath);
                _ページ番号 = (in_ファイル数 < 0) ? _全ページ数 : 1;

                return true;
            }           
            public bool move_at_file先頭ファイル先頭ページ()
            {
                if (_ページ番号 == 1 && _pos_no_at_fileNameList == 0)    // 現在が、先頭ファイル先頭ページ のケース
                    return false;

                _ページ番号 = 1;
                _pos_no_at_fileNameList = 0;

                //
                // ファイル移動したケース
                //
                FilePath = filePath_atDIR_list[_pos_no_at_fileNameList].filepath;
                _全ページ数 = get_全ページ数(FilePath);

                return true;
            }
            public bool move_at_file最終ファイル末尾ページ()
            {
                if (_ページ番号 == _全ページ数 && _pos_no_at_fileNameList == filePath_atDIR_list.Count - 1)   // 現在が、最終ファイル末尾ページ のケース
                    return false;

                _pos_no_at_fileNameList = filePath_atDIR_list.Count - 1;
                _ページ番号 = get_全ページ数(filePath_atDIR_list[_pos_no_at_fileNameList].filepath);

                //
                // ファイル移動したケース
                //
                FilePath = filePath_atDIR_list[_pos_no_at_fileNameList].filepath;
                _全ページ数 = get_全ページ数(FilePath);

                return true;
            }
            /// <summary>指定ファイルのPDFなどで、全ページ数を返します。</summary>
            private int get_全ページ数(string in_filepath)
            {
                using (PdfReader tmp_PDFreader_for_inPDF = new PdfReader(in_filepath))
                {
                    int ret_ページ数_for_inPDF = tmp_PDFreader_for_inPDF.NumberOfPages;
                    return ret_ページ数_for_inPDF;
                }
            }

            public List<filePDF_class> filePath_atDIR_list = new List<filePDF_class>();
            public void set_filepath(string in_filepath)
            {
                DIR_name = Path.GetDirectoryName(in_filepath);
                FilePath = in_filepath;
                set_DIR(DIR_name);

                if (Directory.Exists(DIR_name) == false)
                {
                    is_File設定中 = false;
                    return;
                }

                if (File.Exists(in_filepath) == false)
                {
                    is_File設定中 = false;
                    return;
                }

                _pos_no_at_fileNameList = get_filePosNo_atDir(in_filepath);
                is_File設定中 = (_pos_no_at_fileNameList < 0) ? false : true;
            }
            private void set_DIR(string in_pathName)
            {
                filePath_atDIR_list = new List<filePDF_class>();
                string[] tmp_filepath_list = Directory.GetFiles(in_pathName, "*.PDF");
                foreach(string in_line in tmp_filepath_list)
                {
                    filePath_atDIR_list.Add(new filePDF_class(in_line));
                }

            }
            int get_filePosNo_atDir(string in_filepath)
            {
                for (int i = 0; i < filePath_atDIR_list.Count; i++)
                {
                    if (Cfun.is_same_filepath(filePath_atDIR_list[i].filepath, in_filepath))
                        return i;
                }
                return -1;
            }
        }

        PosInFile now_PosInFile_obj = new PosInFile();

        public string filepath2
        {
            get
            {
                if (now_PosInFile_obj.is_File設定中 == false)
                    return "";
                return now_PosInFile_obj.FilePath;
            }
        }
        enum status_type_enum
        {
            未初期化,
            指定のファイルがない,
            指定中,
        }
        enum status_POS_type_enum
        {
            Only1つだけ,
            先頭,
            末尾,
            複数で端ではない,
            対象ファイルが無い,
        }
        status_POS_type_enum status_POS_type_at_File;
        status_POS_type_enum status_POS_type_at_Page;
        status_type_enum status_type = status_type_enum.未初期化;

        public string get_status_POS_type_at_File()
        {
            return status_POS_type_at_File.ToString();
        }
        public string get_status_POS_type_at_Page()
        {
            return status_POS_type_at_Page.ToString();
        }
        public string get_status_type()
        {
            return status_type.ToString();
        }
        public int get_now_ページ番号()
        {
            return now_PosInFile_obj.ページ番号;
        }
        public int get_now_全ページ数()
        {
            return now_PosInFile_obj.全ページ数;
        }
        class filePDF_class
        {
            public string filepath;
            public filePDF_class(string in_filepath)
            {
                filepath = in_filepath;
            }
        }


        public FilePos管理class()
        {
        }
        public bool set_folder_path(string in_path)
        {
            Func<string> get_filepath = () =>
             {
                 if (in_path == "")
                     return "";
                 string[] tmp_dir_files = Directory.GetFiles(in_path, "*.pdf");
                 if (tmp_dir_files.Length == 0)
                     return "";
                 return tmp_dir_files[0];
             };

            string tmp_filepath = get_filepath();


            now_PosInFile_obj.set_filepath(tmp_filepath);

            if (File.Exists(tmp_filepath) == false)
            {
                status_type = status_type_enum.指定のファイルがない;
                status_POS_type_at_File = status_POS_type_enum.対象ファイルが無い;
                now_PosInFile_obj.is_File設定中 = false;

                set_status_pos_at_DIR(tmp_filepath);

                return false;
            }

            //
            // ファイルがあるケース
            //
            status_type = status_type_enum.指定中;
            now_PosInFile_obj.is_File設定中 = true;

            set_status_pos_at_DIR(tmp_filepath);

            return true;
        }
        public bool set_filepath(string in_filepath)
        {
            now_PosInFile_obj.set_filepath(in_filepath);

            if (File.Exists(in_filepath) == false)
            {
                status_type = status_type_enum.指定のファイルがない;
                status_POS_type_at_File = status_POS_type_enum.対象ファイルが無い;
                now_PosInFile_obj.is_File設定中 = false;

                set_status_pos_at_DIR(in_filepath);

                return false;
            }

            //
            // ファイルがあるケース
            //
            status_type = status_type_enum.指定中;
            now_PosInFile_obj.is_File設定中 = true;

            set_status_pos_at_DIR(in_filepath);

            return true;
        }
        void set_status_pos_at_DIR(string in_filepath)
        {
            now_PosInFile_obj.set_filepath(in_filepath);

            Func<bool> set_at_FILE_proc=() =>
            {
                if (now_PosInFile_obj.filePath_atDIR_list.Count == 1)
                {
                    status_POS_type_at_File = status_POS_type_enum.Only1つだけ;
                    return true;
                }
                if (Cfun.is_same_filepath(now_PosInFile_obj.filePath_atDIR_list[0].filepath, in_filepath))
                {
                    status_POS_type_at_File = status_POS_type_enum.先頭;
                    return true;
                }
                if (Cfun.is_same_filepath(now_PosInFile_obj.filePath_atDIR_list[now_PosInFile_obj.filePath_atDIR_list.Count - 1].filepath, in_filepath))
                {
                    status_POS_type_at_File = status_POS_type_enum.末尾;
                    return true;
                }
                status_POS_type_at_File = status_POS_type_enum.複数で端ではない;
                return true;
            };
            set_at_FILE_proc();

            Func<bool> set_at_PAGE_proc = () =>
            {
                if (now_PosInFile_obj.全ページ数 == 1)
                {
                    status_POS_type_at_Page = status_POS_type_enum.Only1つだけ;
                    return true;
                }
                if (now_PosInFile_obj.ページ番号 == 1)
                {
                    status_POS_type_at_Page = status_POS_type_enum.先頭;
                    return true;
                }
                if (now_PosInFile_obj.ページ番号 == now_PosInFile_obj.全ページ数)
                {
                    status_POS_type_at_Page = status_POS_type_enum.末尾;
                    return true;
                }
                status_POS_type_at_Page = status_POS_type_enum.複数で端ではない;
                return true;
            };
            set_at_PAGE_proc();
        }

        public bool move_FilePOS_先頭ファイル先頭ページ()
        {
            bool is_移動した = now_PosInFile_obj.move_at_file先頭ファイル先頭ページ(); ;
            if (is_移動した == false)
                return false;

            //
            // ファイル移動をしたケース
            set_status_pos_at_DIR(now_PosInFile_obj.FilePath);

            return true;
        }
        public bool move_FilePOS_最終ファイル末尾ページ()
        {
            bool is_移動した = now_PosInFile_obj.move_at_file最終ファイル末尾ページ(); ;
            if (is_移動した == false)
                return false;

            //
            // ファイル移動をしたケース
            set_status_pos_at_DIR(now_PosInFile_obj.FilePath);

            return true;
        }
        /// <summary>ファイルPOSを移動します。正の数で末尾方向、負の数で先頭方向です。</summary> 
        public bool move_pagePOS_最初ページ()
        {
            bool is_移動した = now_PosInFile_obj.move_pagePOS_最初ページ();
            if (is_移動した == false)
                return false;

            //
            // ファイル移動をしたケース
            set_status_pos_at_DIR(now_PosInFile_obj.FilePath);

            return true;
        }
        public bool move_pagePOS_最終ページ()
        {
            bool is_移動した = now_PosInFile_obj.move_pagePOS_最終ページ();
            if (is_移動した == false)
                return false;

            //
            // ファイル移動をしたケース
            set_status_pos_at_DIR(now_PosInFile_obj.FilePath);

            return true;
        }
        public bool move_pagePOS(int in_数 = 1)
        {
            if (in_数 == 0)
                return false;

            bool is_移動した = now_PosInFile_obj.move_at_page数(in_数);
            if (is_移動した == false)
                return false;

            //
            // ファイル移動をしたケース
            set_status_pos_at_DIR(now_PosInFile_obj.FilePath);

            return true;
        }
        /// <summary>ファイルPOSを移動します。正の数で末尾方向、負の数で先頭方向です。</summary> 
        public bool move_filePOS(int in_数 = 1)
        {
            if (in_数 == 0)
                return false;

            bool is_移動した = now_PosInFile_obj.move_at_file数(in_数);
            if (is_移動した == false)
                return false;

            //
            // ファイル移動をしたケース
            set_status_pos_at_DIR(now_PosInFile_obj.FilePath);

            return true;
        }
    }
}
