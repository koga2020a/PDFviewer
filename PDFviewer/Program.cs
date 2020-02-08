using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDFviewer
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            Form1 tmp_form1 = Form1.get_リサイクル_Form_obj();


            if (args.Length == 0)
                goto FINISH;

            if (args[0].ToUpper().EndsWith(".PDF"))
                tmp_form1.set_filepath(args[0]);
            else
                tmp_form1.set_folder_path(args[0]);

            //tmp_form1.set_filepath(@"c:\temp\p15dame.pdf");
            tmp_form1.ShowDialog();


        FINISH:
            Application.Exit();
        }
    }
}
