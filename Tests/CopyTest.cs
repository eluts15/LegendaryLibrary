using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Library
{
  [Collection("Library")]
  public class CopyTest : IDisposable
  {
    public CopyTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb; Initial Catalog=library_test; Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
     //Arrange, Act
     int result = Copy.GetAll().Count;

     //Assert
     Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Override_ObjectsAreEqual()
    {
      //Arrange, Act
      Copy Copy1 = new Copy("Moby Dick'd");
      Copy Copy2 = new Copy("Moby Dick'd");
      //Assert
      Assert.Equal(Copy1, Copy2);
    }

    [Fact]
    public void Test_Save_SavesToDatabase()
    {
     //Arrange
    Copy testCopy = new Copy("The Old Man and the Sea");

     //Act
     testCopy.Save();
     List<Copy> result = Copy.GetAll();
     List<Copy> testList = new List<Copy>{testCopy};

     //Assert
     Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Find_FindCopyInDatabase()
    {
      //Arrange
      Copy testCopy = new Copy("The Hobbit");
      testCopy.Save();

      //Act
      Copy foundCopy = Copy.Find(testCopy.GetId());

      //Assert
      Assert.Equal(testCopy, foundCopy);
    }

    [Fact]
    public void Test_Update_UpdatesCopyInDatabase()
    {
      //Arrange
      Copy testCopy = new Copy("The Art of Deal");
      testCopy.Save();
      string newName = "Please don't read this.... please.";
      //Act
      testCopy.Update(newName);
     string result = testCopy.GetName();

      //Assert
      Assert.Equal(newName, result);
    }

//these methods utilize our join tables, and so we have to build our patron object first.

    [Fact]
    public void GetPatrons_ReturnsAllCopyPatrons_PatronList()
    {
      //Arrange
      Copy testCopy = new Copy("The C Programming Language");
      testCopy.Save();

      Patron testPatrons1 = new Patron("B-r0n Whitwicky");
      testPatrons1.Save();

      Patron testPatrons2 = new Patron("Mitch Mitchell");
      testPatrons2.Save();

      //Act
      testCopy.AddPatron(testPatrons1);
      List<Patron> result = testCopy.GetPatrons();
      List<Patron> testList = new List<Patron> {testPatrons1};

      //Assert
      Assert.Equal(testList, result);
    }
    [Fact]
    public void AddPatron_AddsPatronsToCopy_PatronsList()
    {
      //Arrange
      Copy testCopy = new Copy("The Go Programming Language");
      testCopy.Save();

      Patron testPatrons = new Patron("Alan Alda");
      testPatrons.Save();

      //Act
      testCopy.AddPatron(testPatrons);

      List<Patron> result = testCopy.GetPatrons();
      List<Patron> testList = new List<Patron>{testPatrons};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Delete_DeletesCopysAssociationsFromDatabase_CopysList()
    {
      //Arrange
      Patron testPatron = new Patron("Kevin Bevins");
      testPatron.Save();

      Copy testCopys = new Copy("It");
      testCopys.Save();

      //Act
      testCopys.AddPatron(testPatron);
      testCopys.Delete();

      List<Copy> resultPatronCopys = testPatron.GetCopies();
      List<Copy> testPatronCopys = new List<Copy> {};

      //Assert
      Assert.Equal(testPatronCopys, resultPatronCopys);
    }

    public void Dispose()
    {
      Copy.DeleteAll();
      Author.DeleteAll();
      Copy.DeleteAll();
      Patron.DeleteAll();
    }
  }
}
