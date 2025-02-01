using DryIoc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Zenzai.Common.Utilities;

namespace Zenzai.Models.A1111
{
    public class WebUIBaseModel : BindableBase
    {
        internal const int CTRL_C_EVENT = 0;
        [DllImport("kernel32.dll")]
        internal static extern bool GenerateConsoleCtrlEvent(uint dwCtrlEvent, uint dwProcessGroupId);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool AttachConsole(uint dwProcessId);
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern bool FreeConsole();
        [DllImport("kernel32.dll")]
        static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate? HandlerRoutine, bool Add);
        // Delegate type to be used as the Handler Routine for SCCH
        delegate Boolean ConsoleCtrlDelegate(uint CtrlType);

        #region A1111 Request[Request]プロパティ
        /// <summary>
        /// A1111 Request[Request]プロパティ用変数
        /// </summary>
        RequestModel _Request = new RequestModel();
        /// <summary>
        /// A1111 Request[Request]プロパティ
        /// </summary>
        public RequestModel Request
        {
            get
            {
                return _Request;
            }
            set
            {
                if (_Request == null || !_Request.Equals(value))
                {
                    _Request = value;
                    RaisePropertyChanged("Request");
                }
            }
        }
        #endregion

        /// <summary>
        /// A1111用プロセス
        /// </summary>
        public Process? A1111Proc { get; set; }

        #region リダイレクトメッセージ[RedirectMessage]プロパティ
        /// <summary>
        /// リダイレクトメッセージ[RedirectMessage]プロパティ用変数
        /// </summary>
        RedirectMessageModel _RedirectMessage = new RedirectMessageModel();
        /// <summary>
        /// リダイレクトメッセージ[RedirectMessage]プロパティ
        /// </summary>
        public RedirectMessageModel RedirectMessage
        {
            get
            {
                return _RedirectMessage;
            }
            set
            {
                if (_RedirectMessage == null || !_RedirectMessage.Equals(value))
                {
                    _RedirectMessage = value;
                    RaisePropertyChanged("RedirectMessage");
                }
            }
        }
        #endregion

        #region A111のプロセスを実行する
        /// <summary>
        /// A111のプロセスを実行する
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> RunCommand(string curr_dir_path)
        {

            this.A1111Proc = new Process();
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "cmd.exe";
            //info.Arguments = "/c " + $"python {curr_dir_path}\\launch.py --nowebui --xformers";//引数
            info.RedirectStandardInput = true;
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;
            info.CreateNoWindow = true; // コンソール・ウィンドウを開かない
            this.A1111Proc.StartInfo = info;
            this.A1111Proc.Start();

            using (StreamWriter sw = this.A1111Proc.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine("cd {0}", curr_dir_path);
                    sw.WriteLine("python launch.py --nowebui --xformers");
                }
            }
            string line;

            // プロセス実行中は常にコンソールの値を読み込み続ける
            while ((line = this.A1111Proc.StandardOutput.ReadLine()!) != null)
            {
                yield return line;
            }
        }
        #endregion

        #region WebUIの起動処理
        /// <summary>
        /// WebUIの起動処理
        /// </summary>
        /// <param name="curr_dir_path">実行ファイルが置かれている場所</param>
        public void ExecuteWebUI(string curr_dir_path)
        {
            Task.Run(() =>
            {
                try
                {
                    foreach (var msg in RunCommand(curr_dir_path))
                    {
                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(() =>
                        {
                            if (!string.IsNullOrEmpty(msg))
                            {
                                this.RedirectMessage.Add(msg);
                            }
                        }));
                    }
                }
                catch
                {
                }
            });
        }
        #endregion


        #region WebUI A1111の実行
        /// <summary>
        /// WebUI A1111の実行
        /// </summary>
        public void WebUIExecute(string current_dir)
        {
            try
            {
                string file_path = Path.Combine(current_dir, "launch.py");

                // 未設定の場合
                if (string.IsNullOrEmpty(current_dir))
                {
                    return; // エラーを出さずに抜ける
                }

                // ファイルパスが見つからない場合
                if (!File.Exists(file_path))
                {
                    ShowMessage.ShowNoticeOK($"Not found {file_path}", "Notice");   // エラーを出して抜ける
                    return;
                }

                Task.Run(() =>
                {
                    try
                    {
                        foreach (var msg in RunCommand(current_dir))
                        {
                            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                            new Action(() =>
                            {
                                if (!string.IsNullOrEmpty(msg))
                                {
                                    this.RedirectMessage.Add(msg);
                                }
                            }));
                        }
                    }
                    catch
                    {
                        //ExecuteProcessF = false;
                    }
                });
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion


        #region A1111プロセスの終了処理
        /// <summary>
        /// A1111プロセスの終了処理
        /// </summary>
        public void WebUIProcessEnd()
        {
            Process? p = this.A1111Proc;

            if (p == null)
            {
                return;
            }

            if (AttachConsole((uint)p.Id))
            {
                SetConsoleCtrlHandler(null, true);
                try
                {
                    if (!GenerateConsoleCtrlEvent(CTRL_C_EVENT, 0))
                        return;
                    p.WaitForExit();
                }
                finally
                {
                    SetConsoleCtrlHandler(null, false);
                    FreeConsole();
                }
            }
        }
        #endregion
    }
}
