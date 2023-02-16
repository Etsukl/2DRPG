using System.Collections;
using UnityEngine;
using System.IO;
using IOManagerAssembly;
using System;
using System.Security.Authentication;
using System.Security;

namespace IOManagerAssembly
{
    /*
     * 問題点
     * １．パス、ファイル名の渡し方が物凄く不便
     * ２．ディレクトリを有効なパスとして渡せてしまう。ファイル名がないため当然エラー
     * ３．非同期に対応していないのでバッチに対応したメソッドが兄弟が必要
     * ４．拡張子が重複してしまうことがある。txt.txt
     * ５．パスの渡し方について、test.txtとtestと./testが区別されてしまう。
     */
    public class FileIO:IDataIO
    {
        private string _path;
        public FileIO(string fileName)
            : this(fileName, Path.GetExtension(fileName)) { }
        public FileIO(string fileName,string extension)
        {
            try
            {
                //　引数のファイル名から絶対パスの作成
                /*
                string dirPath = Path.GetDirectoryName(fileName);
                if (string.IsNullOrEmpty(dirPath)) dirPath = Application.streamingAssetsPath;
                */
                string dirPath = Application.streamingAssetsPath;
                _path = Path.Combine(dirPath, fileName);
            }
            catch (ArgumentException e)
            {
                ArgumentExceptionHandler(e);
            }
            catch (PathTooLongException e)
            {
                PathTooLongExceptionHandler(e);
            }
            catch (UnauthorizedAccessException e)
            {
                Debug.LogError("アクセスが拒否されていません。\nErrorMessage: " + e.Message);
            }
            catch (IOException e)
            {
                IOExceptionHandler(e);
            }
        }
        // 
        public string ReadData()
        {
            string result = null;
            try
            {
                using (var sr = new StreamReader(_path)) 
                { 
                    result = sr.ReadToEnd();
                }
            }
            catch (OutOfMemoryException e)
            {
                Debug.LogError("メモリが不足しています。\nErrorMessage: " + e.Message);
                throw;
            }
            catch (ArgumentException e)
            {
                ArgumentExceptionHandler(e);
                throw;
            }
            catch (UnauthorizedAccessException e)
            {
                Debug.LogError("アクセスが拒否されていません。\nErrorMessage: " + e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                DirectoryNotFoundExceptionHander(e);
                throw;
            }
            catch (FileNotFoundException e)
            {
                Debug.LogError("指定したファイルが見つかりません。\nErrorMessage: " + e.Message);
                throw;
            }
            catch (IOException e)
            {
                IOExceptionHandler(e);
                throw;
            }
            return result;
        }

        public void WriteData(string data)
        {
            try
            {
                using (var sw = new StreamWriter(_path)) 
                {
                    sw.WriteLine(data);
                }
            }
            catch (AuthenticationException e) { Debug.LogError("このファイルは読み取り専用です。\nErrorMessage: " + e.Message); }
            catch (SecurityException e) { Debug.LogError("アクセス許可がありません。\nErrorMessage: " + e.Message);  }
            catch (ArgumentException e) { ArgumentExceptionHandler(e); }
            catch (PathTooLongException e) { PathTooLongExceptionHandler(e); }
            catch (DirectoryNotFoundException e) { DirectoryNotFoundExceptionHander(e); }
            catch(IOException e) { IOExceptionHandler(e); }
            catch (UnauthorizedAccessException e)
            {
                Debug.LogError("アクセスが拒否されていません。\nErrorMessage: " + e.Message);
            }
        }
        private void ArgumentExceptionHandler(ArgumentException e)
        {
            Debug.LogError("無効なファイル名/パス\nErrorMessage: " + e.Message);
        }
        private void IOExceptionHandler(IOException e)
        {
            Debug.LogError("入出力に失敗しました。\nErrorMessage" + e.Message);
        }
        private void DirectoryNotFoundExceptionHander(DirectoryNotFoundException e)
        {
            Debug.LogError("指定されたパスが正しくありません。\nErrorMessage: " + e.Message);
        }
        private void PathTooLongExceptionHandler(PathTooLongException e)
        {
            Debug.LogError("指定したパスがシステムの最大長を超えています。\nErrorMessage: " + e.Message);
        }
    }
}