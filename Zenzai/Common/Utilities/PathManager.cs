using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenzai.Common.Utilities
{
    public class PathManager
    {
        #region アプリケーションフォルダの取得
        /// <summary>
        /// アプリケーションフォルダの取得
        /// </summary>
        /// <returns>アプリケーションフォルダパス</returns>
        public static string GetApplicationFolder()
        {
            var fv = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetEntryAssembly()!.Location);
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), fv.CompanyName!, fv.ProductName!);
        }
        #endregion

        #region ディレクトリを再帰的に作成する
        /// <summary>
        /// ディレクトリを再帰的に作成する
        /// </summary>
        /// <param name="path"></param>
        public static void CreateDirectory(string dir_path)
        {
            if (!Directory.Exists(dir_path))
            {
                string parent = Directory.GetParent(dir_path)!.FullName;
                CreateDirectory(parent);
                Directory.CreateDirectory(dir_path);
            }
        }
        #endregion

        #region ファイルのカレントディレクトリを作成する
        /// <summary>
        /// ファイルのカレントディレクトリを作成する
        /// </summary>
        /// <param name="file_path">ファイルパス</param>
        public static void CreateCurrentDirectory(string file_path)
        {
            string parent = Directory.GetParent(file_path)!.FullName;
            if (!Directory.Exists(parent))
            {
                CreateDirectory(parent);
            }
        }
        #endregion

        #region カレントディレクトリパスを取得する
        /// <summary>
        /// カレントディレクトリパスを取得する
        /// </summary>
        /// <param name="file_path">ファイルパス</param>
        /// <returns>カレントディレクトリパス</returns>
        public static string GetCurrentDirectory(string file_path)
        {
            return Directory.GetParent(file_path)!.FullName;
        }
        #endregion

        #region 最後の文字列が指定した文字と同じなら最後の文字列を削除する
        /// <summary>
        /// 最後の文字列が指定した文字と同じなら最後の文字列を削除する
        /// </summary>
        /// <param name="text">対象文字列</param>
        /// <param name="lasttext">最後も文字列</param>
        /// <returns>削除後の文字列</returns>
        public static string TrimLastText(string text, string lasttext)
        {
            if (text.Length >= lasttext.Length)
            {
                var last = text.Substring(text.Length - lasttext.Length);

                if (last.Equals(lasttext))
                {
                    return text.Substring(0, text.Length - lasttext.Length);
                }
            }
            return text;
        }
        #endregion

        /// <summary>
        /// ディレクトリをコピーする。
        /// </summary>
        /// <param name="sourceDirName">コピーするディレクトリ</param>
        /// <param name="destDirName">コピー先のディレクトリ</param>
        public static void CopyDirectory(string sourceDirName, string destDirName)
        {
            // コピー先のディレクトリがないかどうか判定する
            if (!Directory.Exists(destDirName))
            {
                // コピー先のディレクトリを作成する
                Directory.CreateDirectory(destDirName);
            }

            // コピー元のディレクトリの属性をコピー先のディレクトリに反映する
            File.SetAttributes(destDirName, File.GetAttributes(sourceDirName));

            // ディレクトリパスの末尾が「\」でないかどうかを判定する
            if (!destDirName.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                // コピー先のディレクトリ名の末尾に「\」を付加する
                destDirName = destDirName + Path.DirectorySeparatorChar;
            }

            // コピー元のディレクトリ内のファイルを取得する
            string[] files = Directory.GetFiles(sourceDirName);
            foreach (string file in files)
            {
                // コピー元のディレクトリにあるファイルをコピー先のディレクトリにコピーする
                File.Copy(file, destDirName + Path.GetFileName(file), true);
            }

            // コピー元のディレクトリのサブディレクトリを取得する
            string[] dirs = Directory.GetDirectories(sourceDirName);
            foreach (string dir in dirs)
            {
                // コピー元のディレクトリのサブディレクトリで自メソッド（CopyDirectory）を再帰的に呼び出す
                CopyDirectory(dir, destDirName + Path.GetFileName(dir));
            }
        }
    }
}
