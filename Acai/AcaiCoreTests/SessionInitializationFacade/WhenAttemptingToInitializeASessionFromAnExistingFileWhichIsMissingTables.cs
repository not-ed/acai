﻿using AcaiCore;
using Microsoft.Data.Sqlite;
using Moq;
using NUnit.Framework;

namespace AcaiCoreTests.SessionInitializationFacade
{
    [TestFixture]
    public class WhenAttemptingToInitializeASessionFromAnExistingFileWhichIsMissingTables
    {
        private readonly string _journalFilePath = "existing-journal-file.sqlite";
        private AcaiCore.SessionInitializationFacade _subject;
        private bool _result;
        private Mock<IJournalTableSchema> _mockTable;
        private Mock<SqliteCommand> _mockCommand;

        [OneTimeSetUp]
        public void Setup()
        {
            File.Create(_journalFilePath).Close();

            _mockTable = new Mock<IJournalTableSchema>();
            _mockTable.Setup(x => x.PresentInConnection(It.IsAny<SqliteConnection>()))
                .Returns(false);
            _mockTable.Setup(x => x.GetSQLTableCreationQuery())
                .Returns("Mock Table Creation Query");
            
            var tableSchemas = new List<IJournalTableSchema>()
            {
                _mockTable.Object
            };

            _mockCommand = new Mock<SqliteCommand>();
            _mockCommand.SetupProperty(x => x.CommandText);
            
            var mockConnection = new Mock<SqliteConnection>();
            mockConnection.Setup(x => x.CreateCommand())
                .Returns(_mockCommand.Object);
            
            var mockConnectionFactory = new Mock<ISqliteConnectionFactory>();
            mockConnectionFactory.Setup(x => x.CreateOpenConnection())
                .Returns(mockConnection.Object);
            
            _subject = new AcaiCore.SessionInitializationFacade(tableSchemas, mockConnectionFactory.Object);
            _result = _subject.InitializeSessionFromExistingJournalFileAtPath(_journalFilePath);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            if (File.Exists(_journalFilePath))
            {
                File.Delete(_journalFilePath);
            }
        }

        [Test]
        public void ThenTheCreationQueryOfTheMissingTableIsExecuted()
        {
            _mockTable.Verify(x => x.PresentInConnection(It.IsAny<SqliteConnection>()), Times.Once);
            _mockCommand.Verify(x => x.ExecuteNonQuery(), Times.Once);
            Assert.That(_mockCommand.Object.CommandText, Is.EqualTo("Mock Table Creation Query"));
        }
        
        [Test]
        public void ThenTheInitializationIsSuccessful()
        {
            Assert.That(_result, Is.True);
        }

        [Test]
        public void ThenNoFailureReasonIsGiven()
        {
            Assert.That(_subject.GetInitializationFailureReason, Is.EqualTo(AcaiCore.SessionInitializationFailureReason.NONE));
        }

        [Test]
        public void ThenASessionIsReturned()
        {
            Assert.That(_subject.GetSession(), Is.Not.Null);
        }
    }
}
