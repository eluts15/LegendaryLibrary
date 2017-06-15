using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Library
{
  [Collection("Library")]
  public class PatronTest : IDisposable
  {
    public PatronTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb; Initial Catalog=library_test; Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_PatronEmptyAtFirst()
    {
      //Arrange, Act
      int result = Patron.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Override_ObjectsAreEqual()
    {
      //Arrange, Act
      Patron Patron1 = new Patron("Andrew Dalton");
      Patron Patron2 = new Patron("Andrew Dalton");
      //Assert
      Assert.Equal(Patron1, Patron2);
    }

    [Fact]
    public void Test_Save_SavePatronToDatabase()
    {
      //Arrange
      Patron testPatron = new Patron("Ethan Luts");
      testPatron.Save();

      //Act
      List<Patron> result = Patron.GetAll();
      List<Patron> testList = new List<Patron>{testPatron};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Find_FindsPatronInDatabase()
    {
      //Arrange
      Patron testPatron = new Patron("Nicholas Wise");
      testPatron.Save();
      //Act
      Patron foundPatron = Patron.Find(testPatron.GetId());
      //Assert
      Assert.Equal(testPatron, foundPatron);
    }

    [Fact]
    public void Test_Update_UpdatesPatronInDatabase()
    {
      //Arrange
      Patron testPatron = new Patron("Prince");
      testPatron.Save();
      string newName = "The Artist Formerly Known As Prince";
      //Act
      testPatron.Update(newName);
      string result = testPatron.GetName();

      //Assert
      Assert.Equal(newName, result);
    }

    [Fact]
    public void GetCopies_ReturnsAllPatronCopy_CopyList()
    {
     //Arrange
     Patron testPatron = new Patron("Charles McMahon");
     testPatron.Save();

     Copy testCopy1 = new Copy("A Wrinkle In Time");
     testCopy1.Save();

     Copy testCopy2 = new Copy("A Wizard of EarthSea");
     testCopy2.Save();

     //Act
     testPatron.AddCopy(testCopy1);
     List<Copy> savedCopy = testPatron.GetCopies();
     List<Copy> testList = new List<Copy> {testCopy1};

     //Assert
     Assert.Equal(testList, savedCopy);
    }

    [Fact]
    public void Test_AddCopy_AddsCopyToPatron()
    {
      //Arrange
      Patron testPatron = new Patron("George Beasely");
      testPatron.Save();

      Copy testCopy = new Copy("Slaughterhouse Five");
      testCopy.Save();

      Copy testCopy2 = new Copy("Breakfast of Champions");
      testCopy2.Save();

      //Act
      testPatron.AddCopy(testCopy);
      testPatron.AddCopy(testCopy2);
      List<Copy> result = testPatron.GetCopies();
      List<Copy> testList = new List<Copy>{testCopy, testCopy2};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Delete_DeletesPatronAssociationsFromDatabase_PatronList()
    {
      //Arrange
      Copy testCopy = new Copy("The Unbearable Lightness of Being");
      testCopy.Save();

      Patron testPatron = new Patron("Dan Rather");
      testPatron.Save();

      //Act
      testPatron.AddCopy(testCopy);
      testPatron.Delete();

      List<Patron> resultCopyPatron = testCopy.GetPatrons();
      List<Patron> testCopyPatron = new List<Patron> {};

      //Assert
      Assert.Equal(testCopyPatron, resultCopyPatron);
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
