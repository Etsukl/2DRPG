using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using IOManagerAssembly;
using System;
using System.IO;
using System.Linq;
/*
 * ファイルが存在する場合、ファイルを読み取るメソッドが正しく動作することを確認する
ファイルが存在しない場合、ファイルを読み取るメソッドが適切な例外をスローすることを確認する
ファイルが存在する場合、ファイルに書き込むメソッドが正しく動作することを確認する
ファイルが存在しない場合、ファイルに書き込むメソッドが新しいファイルを作成し、正しく書き込むことを確認する
*/
namespace Editor
{
    [TestFixture]
    public class FileIOTests
    {
        private string existFilePath = @".\test";
        private string existFilePath_WithExtension = @".\test.txt";
        private string existFilePath_Full = @"C:\Users\otonade\Documents\Unity\2DRPG\Assets\Scripts\Managers\test.txt";
        
        private string notexistFilePath = @".\not";
        private string notexistFilePath_WithExtension = @".\not.txt";
        private string notexistFilePath_Full = @"C:\Users\otonade\Documents\Unity\2DRPG\Assets\Scripts\Managers\not.txt";

        private string data =
            @"
            aaaaaaaaaaaaaaaaaaaaaaaaa
            bbbbbbbbbbbbbbbbbbbbbbbbb
            ccccccccccccccccccccccccc

            ddddddddddddddddddddddddd
            eeeeeeeeeeeeeeeeeeeeeeeee
            ";

        private IOManagerAssembly.IDataIO fileIO;
        [SetUp]
        public void SetUp()
        {
            File.WriteAllText(Path.Combine(Application.streamingAssetsPath,"test.txt"),data);
        }
        //
        [Test]
        public void Read_ファイルが存在するとき正常に読み込めるか()
        {
            fileIO = new FileIO("test.txt");
            Assert.AreEqual(data,fileIO.ReadData());
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(existFilePath)) File.Delete(existFilePath);
            if (File.Exists(notexistFilePath)) File.Delete(notexistFilePath);
        }
    }
}