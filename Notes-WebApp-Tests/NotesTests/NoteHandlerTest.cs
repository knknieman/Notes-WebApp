using Microsoft.AspNetCore.Http;
using Notes_WebApp_Boomtown.Models;
using Notes_WebApp_Boomtown.Src.Notes;
using NUnit.Framework;
using System.Collections.Generic;

namespace Notes_WebApp_Tests
{
    public class NoteHandlerTest
    {
        NoteHandler instance;
        static List<string> notesToCleanUp = new List<string>();

        [SetUp]
        public void Setup()
        {
            instance = NoteHandler.GetInstance();
        }

        [TearDown]
        public void Cleanup()
        {
            foreach(string id in notesToCleanUp)
            {
                try
                {
                    instance.DeleteEntry(id);
                }
                catch (KeyNotFoundException) { }
            }
        }

        [Test]
        public void TestSuccessfulCreateGet()
        {
            NoteModel note = Create();

            //Get NoteID to passback to GetEntry
            //Should return the same Object
            string noteID = note.NoteID;

            NoteModel retrievedNote = instance.GetEntry(noteID);
            Assert.AreEqual(note, retrievedNote);
        }

        [Test]
        public void TestSucessCreateDelete()
        {
            bool testPassed;
            NoteModel note = Create();
            string noteID = note.NoteID;
            int statusCode = instance.DeleteEntry(noteID);
            Assert.AreEqual(StatusCodes.Status200OK, statusCode);

            //Delete Entry, Get should throw Exception
            try
            {
                instance.GetEntry(noteID);
                testPassed = false;
            }
            catch (KeyNotFoundException ex)
            {
                testPassed = true;
            }
            Assert.IsTrue(testPassed);
        }

        [Test]
        public void TestSuccessCreateEdit()
        {
            NoteModel note = Create();
            string ogContent = note.NoteContent;
            string ogTitle = note.NoteName;

            string newTitle = "New Title";
            string newContent = "THIS IS NEW CONTENT";

            note.NoteName = newTitle;
            note.NoteContent = newContent;

            int status = instance.UpdateEntry(note);
            Assert.AreEqual(StatusCodes.Status200OK, status);

            NoteModel updatedNote = instance.GetEntry(note.NoteID);
            Assert.AreEqual(newTitle, updatedNote.NoteName);
            Assert.AreEqual(newContent, updatedNote.NoteContent);
        }

        [Test]
        public void TestGetNotes()
        {
            List<NoteModel> entiresToTest = new List<NoteModel>();
            for(int i =0; i< 5; i++)
            {
                entiresToTest.Add(Create());
            }

            List<NoteModel> listFromInstance = instance.GetNotes();
            foreach(NoteModel note in entiresToTest)
            {
                Assert.IsTrue(listFromInstance.Contains(note));
            }
        }

        [Test]
        public void TestFailGet()
        {
            bool testPassed;
            try
            {
                instance.GetEntry("THIS-ID-DOES-NOT-EXIST");
                testPassed = false;
            }
            catch (KeyNotFoundException ex)
            {
                testPassed = true;
            }
            Assert.IsTrue(testPassed);
        }


        [Test]
        public void TestFailEdit()
        {
            NoteModel note = new NoteModel() { NoteID = "ThisDoesNotExist" };

            //Will Return a Filed COde 
            int statusCode = instance.UpdateEntry(note);
            Assert.AreEqual(StatusCodes.Status404NotFound, statusCode);
        }

        [Test]
        public void TestFailDelete()
        {

            int statusCode = instance.DeleteEntry("ID-DOES-NOT-EXIST");
            Assert.AreEqual(StatusCodes.Status404NotFound, statusCode);
        }

        private NoteModel Create()
        {
            NoteModel note = new NoteModel();
            note.NoteName = "TestNOTE";
            note.NoteContent = "TEST CONTENT";
            int statusCode = instance.CreateEntry(note);
            notesToCleanUp.Add(note.NoteID);
            Assert.AreEqual(StatusCodes.Status200OK, statusCode);
            return note;
        }
    }
}