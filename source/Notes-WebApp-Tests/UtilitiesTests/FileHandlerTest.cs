using Microsoft.AspNetCore.Http;
using Notes_WebApp_Boomtown.Models;
using Notes_WebApp_Boomtown.Src.Notes;
using Notes_WebApp_Boomtown.Src.Utilities;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace Notes_WebApp_Tests
{
    public class FileHandlerTest
    {

        [Test]
        public void WriteReadJsonToFile()
        {
            Dictionary<string, string> testDict = new Dictionary<string, string> { ["KEY1"] = "VALUE1", ["KEY2"] = "VALUE2" };
            string json = FileHandler.GetJsonFromObject(testDict);
            FileHandler.WriteToFile("jsonSerializationTest.json", json);
            Dictionary<string, string> loadedDict = FileHandler.LoadJsonFile<Dictionary<string, string>>("jsonSerializationTest.json");

            foreach (KeyValuePair<string, string> entry in loadedDict)
            {
                Assert.AreEqual(testDict[entry.Key], entry.Value);
            }

        }

        [Test]
        public void WriteReadFile()
        {
            string content = "TEST";
            FileHandler.WriteToFile("testFile.txt", content);

            string contentLoaded = FileHandler.GetFileContent("testFile.txt");
            Assert.IsTrue(content.Equals(contentLoaded));
        }

        [Test]
        public void ReadFileFail()
        {
            bool passed;
            try
            {
                FileHandler.GetFileContent("FileDoesnotexist.txt");
                passed = false;
            }
            catch(FileNotFoundException)
            {
                passed = true;
            }
            Assert.IsTrue(passed);
        }


        [Test]
        public void ReadJsonFileFile()
        {
            bool passed;
            try
            {
                FileHandler.LoadJsonFile<Dictionary<string, string>>("FileDoesnotexist.txt");
                passed = false;
            }
            catch (FileNotFoundException)
            {
                passed = true;
            }
            Assert.IsTrue(passed);
        }
    }
}