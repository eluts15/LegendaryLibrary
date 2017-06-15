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
      Copy Copy1 = new Copy("Moby Dick'd", 4, 3);
      Copy Copy2 = new Copy("Moby Dick'd", 4, 3);
      //Assert
      Assert.Equal(Copy1, Copy2);
    }

    // [Fact]
    // public void Test_Save_SavesToDatabase()
    // {
    //  //Arrange
    // Copy testCopy = new Copy("The Old Man and the Sea", "Literature", new DateTime(2017, 02, 28));
    //
    //  //Act
    //  testCopy.Save();
    //  List<Copy> result =Copy.GetAll();
    //  List<Copy> testList = new List<Copy>{testCopy};
    //
    //  //Assert
    //  Assert.Equal(testList, result);
    // }
    //
    // [Fact]
    // public void Test_Find_FindCopyInDatabase()
    // {
    //   //Arrange
    //   Copy testCopy = new Copy("The Hobbit", "Fantasy", new DateTime(2017, 07, 21));
    //   testCopy.Save();
    //
    //   //Act
    //   Copy foundCopy = Copy.Find(testCopy.GetId());
    //
    //   //Assert
    //   Assert.Equal(testCopy, foundCopy);
    // }
    //
    // [Fact]
    // public void Test_Update_UpdatesCopyInDatabase()
    // {
    //   //Arrange
    //   Copy testCopy = new Copy("The Art of Deal", "Actual Garbage", new DateTime(2017, 09, 27));
    //   testCopy.Save();
    //   DateTime newDueDate = new DateTime(2017, 12, 03);
    //   //Act
    //   testCopy.Update("The Art of Deal", "Actual Garbage", new DateTime(2017, 12, 03));
    //   DateTime result = testCopy.GetDueDate();
    //
    //   //Assert
    //   Assert.Equal(newDueDate, result);
    // }
    //
    //
    // [Fact]
    // public void GetAuthors_ReturnsAllCopyAuthors_AuthorList()
    // {
    //   //Arrange
    //   Copy testCopy = new Copy("The C Programming Language", "Reference Copy", new DateTime(2018, 03, 16));
    //   testCopy.Save();
    //
    //   Author testAuthors1 = new Author("Biran W. Kernighan");
    //   testAuthors1.Save();
    //
    //   Author testAuthors2 = new Author("Dennis M. Ritchie");
    //   testAuthors2.Save();
    //
    //   //Act
    //   testCopy.AddAuthor(testAuthors1);
    //   List<Author> result = testCopy.GetAuthors();
    //   List<Author> testList = new List<Author> {testAuthors1};
    //
    //   //Assert
    //   Assert.Equal(testList, result);
    // }
    // [Fact]
    // public void AddAuthor_AddsAuthorsToCopy_AuthorsList()
    // {
    //   //Arrange
    //   Copy testCopy = new Copy("The Go Programming Language", "Reference", new DateTime(2016, 10, 27));
    //   testCopy.Save();
    //
    //   Author testAuthors = new Author("Alan A. A. Donvoan");
    //   testAuthors.Save();
    //
    //   //Act
    //   testCopy.AddAuthor(testAuthors);
    //
    //   List<Author> result = testCopy.GetAuthors();
    //   List<Author> testList = new List<Author>{testAuthors};
    //
    //   //Assert
    //   Assert.Equal(testList, result);
    // }
    //
    // [Fact]
    // public void Delete_DeletesCopysAssociationsFromDatabase_CopysList()
    // {
    //   //Arrange
    //   Author testAuthor = new Author("Stephen King");
    //   testAuthor.Save();
    //
    //   Copy testCopys = new Copy("It", "Horror", new DateTime(2015, 09, 29));
    //   testCopys.Save();
    //
    //   //Act
    //   testCopys.AddAuthor(testAuthor);
    //   testCopys.Delete();
    //
    //   List<Copy> resultAuthorCopys = testAuthor.GetCopys();
    //   List<Copy> testAuthorCopys = new List<Copy> {};
    //
    //   //Assert
    //   Assert.Equal(testAuthorCopys, resultAuthorCopys);
    // }

    public void Dispose()
    {
      Copy.DeleteAll();
      Author.DeleteAll();
      Copy.DeleteAll();
      // Patron.DeleteAll();
    }
  }
}
