using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Library
{
  [Collection("Library")]
  public class AuthorTest : IDisposable
  {
    public AuthorTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb; Initial Catalog=library_test; Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_AuthorEmptyAtFirst()
    {
      //Arrange, Act
      int result = Author.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Override_ObjectsAreEqual()
    {
      //Arrange, Act
      Author author1 = new Author("Herman Melville");
      Author author2 = new Author("Herman Melville");
      //Assert
      Assert.Equal(author1, author2);
    }

    [Fact]
    public void Test_Save_SaveAuthorToDatabase()
    {
      //Arrange
      Author testAuthor = new Author("J.R.R. Tolkien");
      testAuthor.Save();

      //Act
      List<Author> result = Author.GetAll();
      List<Author> testList = new List<Author>{testAuthor};

      //Assert
      Assert.Equal(testList, result);
    }
    //
    // [Fact]
    // public void Test_Find_FindsAuthorInDatabase()
    // {
    //   //Arrange
    //   Author testAuthor = new Author("Beat Poetry", "Lit501", "Yes", "B");
    //   testAuthor.Save();
    //   //Act
    //   Author foundAuthor = Author.Find(testAuthor.GetId());
    //   //Assert
    //   Assert.Equal(testAuthor, foundAuthor);
    // }
    //
    // [Fact]
    // public void Test_Update_UpdatesAuthorInDatabase()
    // {
    //   //Arrange
    //   Author testAuthor = new Author("Sleepology", "SL101", "No", "F");
    //   testAuthor.Save();
    //   string newGrade = "B+";
    //   //Act
    //   testAuthor.Update("Sleepology", "SL101", "No", "B+");
    //   string result =testAuthor.GetGrade();
    //
    //   //Assert
    //   Assert.Equal(newGrade, result);
    // }
    //
    // [Fact]
    // public void Test_AddBook_AddsBookToAuthor()
    // {
    //   //Arrange
    //   Author testAuthor = new Author("Sleepology", "SL101", "No", "F");
    //   testAuthor.Save();
    //
    //   Book testBook = new Book("Expandrew", new DateTime(2016, 10, 20), "Game Art & Design");
    //   testBook.Save();
    //
    //   Book testBook2 = new Book("Kimlan", new DateTime(2017, 02, 28), "Software Engineering");
    //   testBook2.Save();
    //
    //   //Act
    //   testAuthor.AddBook(testBook);
    //   testAuthor.AddBook(testBook2);
    //
    //   List<Book> result = testAuthor.GetBook();
    //   List<Book> testList = new List<Book>{testBook, testBook2};
    //
    //   //Assert
    //   Assert.Equal(testList, result);
    // }
    //
    // [Fact]
    // public void GetBook_ReturnsAllAuthorBook_BookList()
    // {
    //  //Arrange
    //  Author testAuthor = new Author("Underwater Basketweaving", "UB107", "No", "N/A");
    //  testAuthor.Save();
    //
    //  Book testBook1 = new Book("MaryAnne", new DateTime(2015, 05, 14), "Marine Biology");
    //  testBook1.Save();
    //
    //  Book testBook2 = new Book("Garth", new DateTime(2014, 09, 05), "Literature");
    //  testBook2.Save();
    //
    //  //Act
    //  testAuthor.AddBook(testBook1);
    //  List<Book> savedBook = testAuthor.GetBook();
    //  List<Book> testList = new List<Book> {testBook1};
    //
    //  //Assert
    //  Assert.Equal(testList, savedBook);
    // }
    //
    // [Fact]
    // public void Delete_DeletesAuthorAssociationsFromDatabase_AuthorList()
    // {
    //   //Arrange
    //   Book testBook = new Book("Expandrew", new DateTime(2016, 10, 20), "Game Art & Design");
    //   testBook.Save();
    //
    //   Author testAuthor = new Author("Sleepology", "SL101", "No", "F");
    //   testAuthor.Save();
    //
    //   //Act
    //   testAuthor.AddBook(testBook);
    //   testAuthor.Delete();
    //
    //   List<Author> resultBookAuthor = testBook.GetAuthors();
    //   List<Author> testBookAuthor = new List<Author> {};
    //
    //   //Assert
    //   Assert.Equal(testBookAuthor, resultBookAuthor);
    // }


    public void Dispose()
    {
      Book.DeleteAll();
      Author.DeleteAll();
    }

  }
}
