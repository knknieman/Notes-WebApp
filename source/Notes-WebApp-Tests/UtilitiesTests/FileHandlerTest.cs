using Microsoft.AspNetCore.Http;
using Notes_WebApp_Boomtown.Models;
using Notes_WebApp_Boomtown.Src.Notes;
using Notes_WebApp_Boomtown.Src.Utilities;
using NUnit.Framework;
using System.Collections.Generic;

namespace Notes_WebApp_Tests
{
    public class FileHandlerTest
    {

        [SetUp]
        public void Setup()
        {

        }

        [TearDown]
        public void Cleanup()
        {

        }

        [Test]
        public void WriteReadJsonToFile()
        {
            Dictionary<string, string> testDict = new Dictionary<string, string> { ["KEY1"] = "VALUE1", ["KEY2"] = "VALUE2" };
            string json = FileHandler.GetJsonFromObject(testDict);
            FileHandler.WriteToFile("jsonSerializationTest.json", json);
            //"\\TestData\\jsonSerializationTest.json"
            Dictionary<string, string> loadedDict = FileHandler.LoadJsonFile<Dictionary<string, string>>("jsonSerializationTest.json");

            foreach (KeyValuePair<string, string> entry in loadedDict)
            {
                Assert.AreEqual(testDict[entry.Key], entry.Value);
            }

        }
    }
}